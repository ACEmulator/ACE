using System;
using System.Collections.Generic;
using System.Diagnostics;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        //public int TotalSkillCredits
        //{
        //    get => GetProperty(PropertyInt.TotalSkillCredits) ?? 0;
        //    set => SetProperty(PropertyInt.TotalSkillCredits, value);
        //}

        //public int AvailableSkillCredits
        //{
        //    get => GetProperty(PropertyInt.AvailableSkillCredits) ?? 0;
        //    set => SetProperty(PropertyInt.AvailableSkillCredits, value);
        //}

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
        public void HandleActionTrainSkill(Skill skill, int creditsSpent)
        {
            if (AvailableSkillCredits >= creditsSpent)
            {
                // attempt to train the specified skill
                bool trainNewSkill = TrainSkill(skill, creditsSpent);

                // create an update to send to the client
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.AvailableSkillCredits, AvailableSkillCredits ?? 0);

                // as long as the skill is sent, the train new triangle button on the client will not lock up.
                // Sending Skill.None with status untrained worked in test
                var trainSkillUpdate = new GameMessagePrivateUpdateSkill(this, Skill.None, SkillStatus.Untrained, 0, 0, 0);
                // create a string placeholder for the correct after
                string trainSkillMessageText;

                // if the skill has already been trained or we do not have enough credits, then trainNewSkill be set false
                if (trainNewSkill)
                {
                    // replace the trainSkillUpdate message with the correct skill assignment:
                    trainSkillUpdate = new GameMessagePrivateUpdateSkill(this, skill, SkillStatus.Trained, 0, 0, 0);
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
                cs.Status = SkillStatus.Untrained;
                cs.Ranks = 0;
                cs.ExperienceSpent = 0;
                AvailableSkillCredits += creditsSpent;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Increases a skill by some amount of points
        /// </summary>
        public void AwardSkillPoints(Skill skill, uint amount, bool usage = false)
        {
            var creatureSkill = GetCreatureSkill(skill);

            for (var i = 0; i < amount; i++)
            {
                if (IsSkillMaxRank(creatureSkill.Ranks, creatureSkill.Status))
                    return;

                // get skill xp required for next rank
                var xpToRank = GetXpToNextRank(creatureSkill);
                if (xpToRank == uint.MaxValue)
                    return;

                RaiseSkillGameAction(skill, xpToRank, usage);
            }
        }

        /// <summary>
        /// Increases a skill from the 'Raise skill' buttons, or through natural usage
        /// </summary>
        public void RaiseSkillGameAction(Skill skill, uint amount, bool usage = false)
        {
            var creatureSkill = GetCreatureSkill(skill);

            var prevRank = creatureSkill.Ranks;
            var prevXP = creatureSkill.ExperienceSpent;

            uint result = SpendSkillXp(creatureSkill, amount, usage);

            string messageText;

            if (prevRank != creatureSkill.Ranks)
            {
                // if the skill ranks out at the top of our xp chart
                // then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(creatureSkill.Ranks, creatureSkill.Status))
                {
                    // fireworks on rank up is 0x8D
                    PlayParticleEffect(ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {skill.ToSentence()} is now {creatureSkill.Base} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {skill.ToSentence()} is now {creatureSkill.Base}!";
                }
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(this, skill, creatureSkill.Status, creatureSkill.Ranks, creatureSkill.InitLevel, result));
                Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.RaiseTrait, 1f));
                Session.Network.EnqueueSend(new GameMessageSystemChat(messageText, ChatMessageType.Advancement));
            }
            else if (prevXP != creatureSkill.ExperienceSpent)
            {
                // skill usage
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(this, skill, creatureSkill.Status, creatureSkill.Ranks, creatureSkill.InitLevel, result));
            }
            else
            {
                messageText = $"Your attempt to raise {skill} has failed!";
                Session.Network.EnqueueSend(new GameMessageSystemChat(messageText, ChatMessageType.Advancement));
            }
        }

        /// <summary>
        /// Adds experience points to a skill
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. Earned XP usage in ranks besides 1 or 10 need to be accounted for.
        /// </remarks>
        /// <returns>0 if it failed, total skill experience if successful</returns>
        private uint SpendSkillXp(CreatureSkill skill, uint amount, bool usage = false)
        {
            uint result = 0u;

            List<uint> xpList = GetXPTable(skill.Status);
            if (xpList == null) return result;

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (skill.Ranks >= (xpList.Count - 1))
                return result;

            ushort rankUps = 0;
            //uint currentRankXp = xpList[Convert.ToInt32(skill.Ranks)];
            uint currentRankXp = skill.ExperienceSpent;
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

            if (amount >= rank10)
            {
                if (rank10Offset > 0)
                    rankUps = rank10Offset;
                else
                    rankUps = 10;
            }
            else if (amount >= rank1)
                rankUps = 1;
            

            if (rankUps > 0)
                skill.Ranks += rankUps;

            if (!usage)
                SpendXP(amount);

            skill.ExperienceSpent += amount;
            result = skill.ExperienceSpent;

            return result;
        }

        /// <summary>
        /// Grants skill XP proportional to the player's skill level
        /// </summary>
        public void GrantLevelProportionalSkillXP(Skill skill, double percent, ulong max)
        {
            var creatureSkill = GetCreatureSkill(skill);
            if (IsSkillMaxRank(creatureSkill.Ranks, creatureSkill.Status))
                return;

            var nextLevelXP = GetXPBetweenSkillLevels(creatureSkill.Status, creatureSkill.Ranks, creatureSkill.Ranks + 1).Value;
            var amount = (uint)Math.Min(nextLevelXP * percent, max);

            RaiseSkillGameAction(skill, amount, true);
        }

        /// <summary>
        /// Returns the remaining XP required to the next skill level
        /// </summary>
        public uint GetXpToNextRank(CreatureSkill skill)
        {
            var xpList = GetXPTable(skill.Status);
            if (xpList != null)
                return xpList[Convert.ToInt32(skill.Ranks) + 1] - skill.ExperienceSpent;
            else
                return uint.MaxValue;
        }

        /// <summary>
        /// Returns the XP curve table based on trained or specialized skill
        /// </summary>
        public List<uint> GetXPTable(SkillStatus status)
        {
            var xpTable = DatManager.PortalDat.XpTable;
            if (status == SkillStatus.Trained)
                return xpTable.TrainedSkillXpList;
            else if (status == SkillStatus.Specialized)
                return xpTable.SpecializedSkillXpList;
            else
                return null;
        }

        /// <summary>
        /// Returns the XP required to go between skill level A and skill level B
        /// </summary>
        public ulong? GetXPBetweenSkillLevels(SkillStatus status, int levelA, int levelB)
        {
            var xpTable = GetXPTable(status);
            if (xpTable == null) return null;
            return xpTable[levelB + 1] - xpTable[levelA + 1];
        }

        /// <summary>
        /// Check a rank against the skill charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if skill is max rank; false if skill is below max rank</returns>
        private bool IsSkillMaxRank(uint rank, SkillStatus status)
        {
            var xpList = GetXPTable(status);
            if (xpList == null)
                throw new Exception();  // return false?

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

        public void AddSkillCredits(int amount, bool showText)
        {
            TotalSkillCredits += amount;
            AvailableSkillCredits += amount;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.AvailableSkillCredits, AvailableSkillCredits ?? 0));

            if (showText)
            {
                var message = string.Format("You have earned {0} skill credit{1}!", amount, amount == 1 ? "" : "s");
                Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Advancement));
                Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.RaiseTrait, 1f));
            }
        }
    }
}
