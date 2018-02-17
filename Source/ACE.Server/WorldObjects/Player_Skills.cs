using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;

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

        public BiotaPropertiesSkill GetSkillProperty(Skill skill)
        {
            var result = Biota.BiotaPropertiesSkill.FirstOrDefault(x => x.Type == (uint)skill);

            if (result == null)
            {
                result = new BiotaPropertiesSkill { ObjectId = Biota.Id, Type = (ushort)skill, SAC = (uint)SkillStatus.Untrained };

                Biota.BiotaPropertiesSkill.Add(result);
            }

            return result;
        }

        /// <summary>
        /// Sets the skill to trained status for a character
        /// </summary>
        /// <param name="skill"></param>
        public bool TrainSkill(Skill skill, int creditsSpent)
        {
            var cs = GetSkillProperty(skill);

            if (cs.SAC != (uint)SkillStatus.Trained && cs.SAC != (uint)SkillStatus.Specialized)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    cs.SAC = (uint)SkillStatus.Trained;
                    cs.LevelFromPP = 0;
                    cs.PP = 0;
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
                bool trainNewSkill = Character.TrainSkill(skill, creditsSpent);

                // create an update to send to the client
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.AvailableSkillCredits, AvailableSkillCredits);

                // as long as the skill is sent, the train new triangle button on the client will not lock up.
                // Sending Skill.None with status untrained worked in test
                var trainSkillUpdate = new GameMessagePrivateUpdateSkill(Session, Skill.None, SkillStatus.Untrained, 0, 0, 0);
                // create a string placeholder for the correct after
                string trainSkillMessageText = "";

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
            var cs = GetSkillProperty(skill);

            if (cs.SAC == (uint)SkillStatus.Trained)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    RefundXp(cs.PP);
                    cs.SAC = (uint)SkillStatus.Specialized;
                    cs.LevelFromPP = 0;
                    cs.PP = 0;
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
            var cs = GetSkillProperty(skill);

            if (cs.SAC != (uint)SkillStatus.Trained && cs.SAC != (uint)SkillStatus.Specialized)
            {
                cs.SAC = (uint)SkillStatus.Untrained;
                cs.LevelFromPP = 0;
                cs.PP = 0;
                return true;
            }

            if (cs.SAC == (uint)SkillStatus.Trained)
            {
                RefundXp(cs.PP);
                cs.SAC = (uint)SkillStatus.Untrained;
                cs.LevelFromPP = 0;
                cs.PP = 0;
                AvailableSkillCredits += creditsSpent;
                return true;
            }

            return false;
        }








        [Obsolete]
        public Dictionary<Skill, CreatureSkill> Skills => AceObject.AceObjectPropertiesSkills;

        /// <summary>
        /// Spend xp Skill ranks
        /// </summary>
        public void SpendXp(Skill skill, uint amount)
        {
            uint baseValue = 0;
            CreatureSkill creatureSkill = Skills[skill];
            uint result = SpendSkillXp(creatureSkill, amount);

            uint ranks = creatureSkill.Ranks;
            uint newValue = creatureSkill.UnbuffedValue;
            var status = creatureSkill.Status;
            var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, Character.AvailableExperience);
            var skillUpdate = new GameMessagePrivateUpdateSkill(Session, skill, status, ranks, baseValue, result);
            var soundEvent = new GameMessageSound(this.Guid, Sound.RaiseTrait, 1f);
            string messageText = "";

            if (result > 0u)
            {
                // if the skill ranks out at the top of our xp chart
                // then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(ranks, status))
                {
                    // fireworks on rank up is 0x8D
                    PlayParticleEffect(global::ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {skill} is now {newValue} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {skill} is now {newValue}!";
                }
            }
            else
            {
                messageText = $"Your attempt to raise {skill} has failed!";
            }
            var message = new GameMessageSystemChat(messageText, ChatMessageType.Advancement);
            Session.Network.EnqueueSend(xpUpdate, skillUpdate, soundEvent, message);
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

            uint rankUps = 0u;
            uint currentRankXp = xpList[Convert.ToInt32(skill.Ranks)];
            uint rank1 = xpList[Convert.ToInt32(skill.Ranks) + 1] - currentRankXp;
            uint rank10;
            int rank10Offset = 0;

            if (skill.Ranks + 10 >= (xpList.Count))
            {
                rank10Offset = 10 - (Convert.ToInt32(skill.Ranks + 10) - (xpList.Count - 1));
                rank10 = xpList[Convert.ToInt32(skill.Ranks) + rank10Offset] - currentRankXp;
            }
            else
            {
                rank10 = xpList[Convert.ToInt32(skill.Ranks) + 10] - currentRankXp;
            }

            if (amount == rank1)
                rankUps = 1u;
            else if (amount == rank10)
            {
                if (rank10Offset > 0u)
                {
                    rankUps = Convert.ToUInt32(rank10Offset);
                }
                else
                {
                    rankUps = 10u;
                }
            }

            if (rankUps > 0)
            {
                skill.Ranks += rankUps;
                skill.ExperienceSpent += amount;
                this.Character.SpendXp(amount);
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
                    creatureSkill = Skills[Skill.CreatureEnchantment];
                    break;
                case MagicSchool.WarMagic:
                    creatureSkill = Skills[Skill.WarMagic];
                    break;
                case MagicSchool.ItemEnchantment:
                    creatureSkill = Skills[Skill.ItemEnchantment];
                    break;
                case MagicSchool.LifeMagic:
                    creatureSkill = Skills[Skill.LifeMagic];
                    break;
                case MagicSchool.VoidMagic:
                    creatureSkill = Skills[Skill.VoidMagic];
                    break;
                default:
                    // Undefined magic school, something bad happened.
                    Debug.Assert((int)school > 5 || school <= 0, "Undefined magic school?");
                    return false;
            }

            if (creatureSkill.Status >= SkillStatus.Trained && creatureSkill.ActiveValue >= (power - magicSkillCheckMargin))
                ret = true;

            return ret;
        }
    }
}
