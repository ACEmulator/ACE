using System;
using System.Linq;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {

        private static double XpModifier { get { return PropertyManager.GetFloat("xp_modifier"); } }
        private static double LuminanceModifier { get { return PropertyManager.GetFloat("luminance_modifier"); } }

        /// <summary>
        /// Raise the available XP by a specified amount
        /// </summary>
        /// <param name="amount">A unsigned long containing the desired XP amount to raise</param>
        public void GrantXp(long amount, bool message = true)
        {
            // apply xp modifier
            amount = (long)(amount * XpModifier);
            UpdateXpAndLevel(amount);
            if (message)
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{amount} experience granted.", ChatMessageType.Advancement));
        }

        /// <summary>
        /// Raise the available XP by a percentage of the current level XP
        /// or a maximum
        /// </summary>
        public void GrantLevelProportionalXp(double percent, ulong max)
        {
            var maxLevel = GetMaxLevel();
            if (Level >= maxLevel) return;

            var nextLevelXP = GetXPBetweenLevels(Level.Value, Level.Value + 1);
            var scaledXP = (long)Math.Min(nextLevelXP * percent, max);
            GrantXp(scaledXP);
        }

        /// <summary>
        /// Raise the available luminance by a specified amount
        /// </summary>
        public void GrantLuminance(long amount)
        {
            // apply lum modifier
            amount = (long)(amount * LuminanceModifier);

            if (AvailableLuminance + amount > MaximumLuminance)
                amount = MaximumLuminance.Value - AvailableLuminance.Value;

            AvailableLuminance += amount;

            var luminance = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableLuminance, AvailableLuminance ?? 0);
            var message = new GameMessageSystemChat($"{amount} luminance granted.", ChatMessageType.Advancement);
            Session.Network.EnqueueSend(luminance, message);
        }

        public void EarnXP(long amount, bool sharable = true, bool fixedAmount = false)
        {
            // apply xp modifier
            amount = (long)(amount * XpModifier);
            if (sharable)
            {
                if (Fellowship != null && Fellowship.ShareXP)
                    Fellowship.SplitXp((ulong) amount, fixedAmount);
                else
                    UpdateXpAndLevel(amount);
            }
            else
                UpdateXpAndLevel(amount);
        }

        private void UpdateXpAndLevel(long amount)
        {
            // until we are max level we must make sure that we send
            var xpTable = DatManager.PortalDat.XpTable;

            var maxLevel = GetMaxLevel();
            var maxLevelXp = xpTable.CharacterLevelXPList.Last();

            if (Level != maxLevel)
            {
                var amountLeftToEnd = (long)maxLevelXp - TotalExperience ?? 0;
                if (amount > amountLeftToEnd)
                    amount = amountLeftToEnd;

                AvailableExperience += amount;
                TotalExperience += amount;

                CheckForLevelup();
                var xpTotalUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.TotalExperience, TotalExperience ?? 0);
                var xpAvailUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0);
                Session.Network.EnqueueSend(xpTotalUpdate, xpAvailUpdate);
            }
            if (HasVitae) UpdateXpVitae(amount);
        }

        private void UpdateXpVitae(long amount)
        {
            var vitaePenalty = EnchantmentManager.GetVitae().StatModValue;
            var startPenalty = vitaePenalty;

            var maxPool = (int)VitaeCPPoolThreshold(vitaePenalty, DeathLevel.Value);
            var curPool = VitaeCpPool + amount;
            while (curPool >= maxPool)
            {
                curPool -= maxPool;
                vitaePenalty = EnchantmentManager.ReduceVitae();
                if (vitaePenalty == 1.0f)
                    break;
                maxPool = (int)VitaeCPPoolThreshold(vitaePenalty, DeathLevel.Value);
            }
            VitaeCpPool = (int)curPool;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.VitaeCpPool, VitaeCpPool.Value));
            if (vitaePenalty != startPenalty)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your experience has reduced your Vitae penalty!", ChatMessageType.Magic));
                EnchantmentManager.SendUpdateVitae();
            }
            if (vitaePenalty == 1.0f)
                EnchantmentManager.RemoveVitae();
        }

        /// <summary>
        /// Returns the maximum possible character level
        /// </summary>
        /// <returns></returns>
        public uint GetMaxLevel()
        {
            return (uint)DatManager.PortalDat.XpTable.CharacterLevelXPList.Count - 1; 
        }

        /// <summary>
        /// Returns the total XP required to reach a level
        /// </summary>
        public ulong GetTotalXP(int level)
        {
            var maxLevel = GetMaxLevel();
            if (level >= maxLevel)
                return 0;

            return DatManager.PortalDat.XpTable.CharacterLevelXPList[level + 1];
        }

        /// <summary>
        /// Returns the remaining XP required to the next level
        /// </summary>
        public ulong GetRemainingXP()
        {
            var maxLevel = GetMaxLevel();
            if (Level >= maxLevel)
                return 0;

            var nextLevelTotalXP = DatManager.PortalDat.XpTable.CharacterLevelXPList[Level.Value + 2];
            return nextLevelTotalXP - (ulong)TotalExperience.Value;
        }

        /// <summary>
        /// Returns the XP required to go from level A to level B
        /// </summary>
        public ulong GetXPBetweenLevels(int levelA, int levelB)
        {
            var levelA_totalXP = DatManager.PortalDat.XpTable.CharacterLevelXPList[levelA + 1];
            var levelB_totalXP = DatManager.PortalDat.XpTable.CharacterLevelXPList[levelB + 1];

            return levelB_totalXP - levelA_totalXP;
        }

        /// <summary>
        /// Determines if the player has advanced a level
        /// </summary>
        private void CheckForLevelup()
        {
            var xpTable = DatManager.PortalDat.XpTable;

            var maxLevel = GetMaxLevel();

            if (Level == maxLevel) return;

            var startingLevel = Level;
            bool creditEarned = false;

            // increases until the correct level is found
            while ((ulong)(TotalExperience ?? 0) >= xpTable.CharacterLevelXPList[(Level ?? 0) + 1])
            {
                Level++;

                // increase the skill credits if the chart allows this level to grant a credit
                if (xpTable.CharacterLevelSkillCreditList[Level ?? 0] > 0)
                {
                    AvailableSkillCredits += (int)xpTable.CharacterLevelSkillCreditList[Level ?? 0];
                    TotalSkillCredits += (int)xpTable.CharacterLevelSkillCreditList[Level ?? 0];
                    creditEarned = true;
                }

                // break if we reach max
                if (Level == maxLevel)
                {
                    PlayParticleEffect(ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    break;
                }
            }

            if (Level > startingLevel)
            {
                string levelUpMessageText = (Level == maxLevel) ? $"You have reached the maximum level of {Level}!" : $"You are now level {Level}!";
                var levelUpMessage = new GameMessageSystemChat(levelUpMessageText, ChatMessageType.Advancement);

                string xpUpdateText = (AvailableSkillCredits > 0) ? $"You have {AvailableExperience:#,###0} experience points and {AvailableSkillCredits} skill credits available to raise skills and attributes." : $"You have {AvailableExperience:#,###0} experience points available to raise skills and attributes.";
                var xpUpdateMessage = new GameMessageSystemChat(xpUpdateText, ChatMessageType.Advancement);

                var levelUp = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.Level, Level ?? 1);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.AvailableSkillCredits, AvailableSkillCredits ?? 0);

                if (Level != maxLevel && !creditEarned)
                {
                    var nextLevelWithCredits = 0;

                    for (int i = (Level ?? 0) + 1; i <= maxLevel; i++)
                    {
                        if (xpTable.CharacterLevelSkillCreditList[i] > 0)
                        {
                            nextLevelWithCredits = i;
                            break;
                        }
                    }

                    string nextCreditAtText = $"You will earn another skill credit at {nextLevelWithCredits}";
                    var nextCreditMessage = new GameMessageSystemChat(nextCreditAtText, ChatMessageType.Advancement);
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits, nextCreditMessage);
                }
                else
                {
                    Session.Network.EnqueueSend(levelUp, levelUpMessage, xpUpdateMessage, currentCredits);
                }

                // play level up effect
                PlayParticleEffect(ACE.Entity.Enum.PlayScript.LevelUp, Guid);
            }
        }

        /// <summary>
        /// spends the amount of xp specified, deducting it from avaiable experience
        /// </summary>
        public bool SpendXp(long amount)
        {
            if (AvailableExperience >= amount)
            {
                AvailableExperience -= amount;

                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0);
                Session.Network.EnqueueSend(xpUpdate);

                return true;
            }

            return false;
        }

        /// <summary>
        /// gives available xp of the amount specified without increasing total xp
        /// </summary>
        public void RefundXp(long amount)
        {
            AvailableExperience += amount;

            var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0);
            Session.Network.EnqueueSend(xpUpdate);
        }

        /// <summary>
        /// Returns the total amount of XP required to go from vitae to vitae + 0.01
        /// </summary>
        /// <param name="vitae">The current player life force, ie. 0.95f vitae = 5% penalty</param>
        /// <param name="level">The player DeathLevel, their level on last death</param>
        private double VitaeCPPoolThreshold(float vitae, int level)
        {
            return (Math.Pow(level, 2.5) * 2.5 + 20.0) * Math.Pow(vitae, 5.0) + 0.5;
        }
    }
}
