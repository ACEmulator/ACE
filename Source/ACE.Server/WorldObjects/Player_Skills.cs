using System;
using System.Collections.Generic;
using System.Diagnostics;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public int TotalSkillCredits
        {
            get => GetProperty(PropertyInt.TotalSkillCredits) ?? 0;
            set => SetProperty(PropertyInt.TotalSkillCredits, value);
        }

        public int AvailableSkillCredits
        {
            get => GetProperty(PropertyInt.AvailableSkillCredits) ?? 0;
            set => SetProperty(PropertyInt.AvailableSkillCredits, value);
        }


        /// <summary>
        /// Sets the skill to trained status for a character
        /// </summary>
        public bool TrainSkill(Skill skill, int creditsSpent)
        {
            var cs = GetCreatureSkill(skill);

            if (cs.Status != SkillStatus.Trained && cs.Status != SkillStatus.Specialized)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    cs.Status = SkillStatus.Trained;
                    cs.Ranks = 0;
                    cs.ExperienceSpent = 0;
                    AvailableSkillCredits -= creditsSpent;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Public method for adding a new skill by spending skill credits.
        /// </summary>
        /// <remarks>
        ///  The client will throw up more then one train skill dialog and the user has the chance to spend twice.
        /// </remarks>
        public void TrainSkillGameAction(Skill skill, int creditsSpent)
        {
            if (AvailableSkillCredits >= creditsSpent)
            {
                // attempt to train the specified skill
                bool trainNewSkill = TrainSkill(skill, creditsSpent);

                // create an update to send to the client
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.AvailableSkillCredits, AvailableSkillCredits);

                // as long as the skill is sent, the train new triangle button on the client will not lock up.
                // Sending Skill.None with status untrained worked in test
                var trainSkillUpdate = new GameMessagePrivateUpdateSkill(Session, Skill.None, SkillStatus.Untrained, 0, 0, 0);
                // create a string placeholder for the correct after
                string trainSkillMessageText;

                // if the skill has already been trained or we do not have enough credits, then trainNewSkill be set false
                if (trainNewSkill)
                {
                    // replace the trainSkillUpdate message with the correct skill assignment:
                    trainSkillUpdate = new GameMessagePrivateUpdateSkill(Session, skill, SkillStatus.Trained, 0, 0, 0);
                    trainSkillMessageText = $"{skill.ToSentence()} trained. You now have {AvailableSkillCredits} credits available.";
                }
                else
                {
                    trainSkillMessageText = $"Failed to train {skill.ToSentence()}! You now have {AvailableSkillCredits} credits available.";
                }

                // create the final game message and send to the client
                var message = new GameMessageSystemChat(trainSkillMessageText, ChatMessageType.Advancement);
                Session.Network.EnqueueSend(trainSkillUpdate, currentCredits, message);
            }
        }

        /// <summary>
        /// Sets the skill to specialized status
        /// </summary>
        public bool SpecializeSkill(Skill skill, int creditsSpent)
        {
            var cs = GetCreatureSkill(skill);

            if (cs.Status == SkillStatus.Trained)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    RefundXp(cs.ExperienceSpent);
                    cs.Status = SkillStatus.Specialized;
                    cs.Ranks = 0;
                    cs.ExperienceSpent = 0;
                    AvailableSkillCredits -= creditsSpent;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the skill to untrained status
        /// </summary>
        public bool UntrainSkill(Skill skill, int creditsSpent)
        {
            var cs = GetCreatureSkill(skill);

            if (cs.Status != SkillStatus.Trained && cs.Status != SkillStatus.Specialized)
            {
                cs.Status = SkillStatus.Untrained;
                cs.Ranks = 0;
                cs.ExperienceSpent = 0;
                return true;
            }

            if (cs.Status == SkillStatus.Trained)
            {
                RefundXp(cs.ExperienceSpent);
                cs.Status = SkillStatus.Untrained;
                cs.Ranks = 0;
                cs.ExperienceSpent = 0;
                AvailableSkillCredits += creditsSpent;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Spend xp Skill ranks
        /// </summary>
        public void RaiseSkillGameAction(Skill skill, uint amount)
        {
            uint baseValue = 0;

            var creatureSkill = GetCreatureSkill(skill);

            uint result = SpendSkillXp(creatureSkill, amount);

            string messageText;

            if (result > 0u)
            {
                // if the skill ranks out at the top of our xp chart
                // then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(creatureSkill.Ranks, creatureSkill.Status))
                {
                    // fireworks on rank up is 0x8D
                    PlayParticleEffect(ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {skill} is now {creatureSkill.Base} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {skill} is now {creatureSkill.Base}!";
                }
            }
            else
            {
                messageText = $"Your attempt to raise {skill} has failed!";
            }

            var skillUpdate = new GameMessagePrivateUpdateSkill(Session, skill, creatureSkill.Status, creatureSkill.Ranks, baseValue, result);
            var soundEvent = new GameMessageSound(Guid, Sound.RaiseTrait, 1f);
            var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);
            Session.Network.EnqueueSend(skillUpdate, soundEvent, message);
        }

        /// <summary>
        /// spends the xp on this skill.
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. Not checking and accounting for XP gained from skill usage.
        /// </remarks>
        /// <returns>0 if it failed, total investment of the next rank if successful</returns>
        private uint SpendSkillXp(CreatureSkill skill, uint amount)
        {
            uint result = 0u;

            List<uint> xpList;
            var xpTable = DatManager.PortalDat.XpTable;

            if (skill.Status == SkillStatus.Trained)
                xpList = xpTable.TrainedSkillXpList;
            else if (skill.Status == SkillStatus.Specialized)
                xpList = xpTable.SpecializedSkillXpList;
            else
                return result;

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (skill.Ranks >= (xpList.Count - 1))
                return result;

            ushort rankUps = 0;
            uint currentRankXp = xpList[Convert.ToInt32(skill.Ranks)];
            uint rank1 = xpList[Convert.ToInt32(skill.Ranks) + 1] - currentRankXp;
            uint rank10;
            ushort rank10Offset = 0;

            if (skill.Ranks + 10 >= (xpList.Count))
            {
                rank10Offset = (ushort)(10 - ((skill.Ranks + 10) - (xpList.Count - 1)));
                rank10 = xpList[skill.Ranks + rank10Offset] - currentRankXp;
            }
            else
            {
                rank10 = xpList[skill.Ranks + 10] - currentRankXp;
            }

            if (amount == rank1)
                rankUps = 1;
            else if (amount == rank10)
            {
                if (rank10Offset > 0)
                    rankUps = rank10Offset;
                else
                    rankUps = 10;
            }

            if (rankUps > 0)
            {
                skill.Ranks += rankUps;
                skill.ExperienceSpent += amount;
                SpendXp(amount);
                result = skill.ExperienceSpent;
            }

            return result;
        }

        /// <summary>
        /// Check a rank against the skill charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if skill is max rank; false if skill is below max rank</returns>
        private bool IsSkillMaxRank(uint rank, SkillStatus status)
        {
            List<uint> xpList;
            var xpTable = DatManager.PortalDat.XpTable;

            if (status == SkillStatus.Trained)
                xpList = xpTable.TrainedSkillXpList;
            else if (status == SkillStatus.Specialized)
                xpList = xpTable.SpecializedSkillXpList;
            else
                throw new Exception();

            if (rank == (xpList.Count - 1))
                return true;

            return false;
        }


        private const uint magicSkillCheckMargin = 50;

        public bool CanReadScroll(MagicSchool school, uint power)
        {
            bool ret = false;
            CreatureSkill creatureSkill;

            switch (school)
            {
                case MagicSchool.CreatureEnchantment:
                    creatureSkill = GetCreatureSkill(Skill.CreatureEnchantment);
                    break;
                case MagicSchool.WarMagic:
                    creatureSkill = GetCreatureSkill(Skill.WarMagic);
                    break;
                case MagicSchool.ItemEnchantment:
                    creatureSkill = GetCreatureSkill(Skill.ItemEnchantment);
                    break;
                case MagicSchool.LifeMagic:
                    creatureSkill = GetCreatureSkill(Skill.LifeMagic);
                    break;
                case MagicSchool.VoidMagic:
                    creatureSkill = GetCreatureSkill(Skill.VoidMagic);
                    break;
                default:
                    // Undefined magic school, something bad happened.
                    Debug.Assert((int)school > 5 || school <= 0, "Undefined magic school?");
                    return false;
            }

            if (creatureSkill.Status >= SkillStatus.Trained && creatureSkill.Current >= (power - magicSkillCheckMargin))
                ret = true;

            return ret;
        }
    }
}
