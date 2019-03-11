using System;
using System.Linq;

using ACE.Common.Extensions;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// A player earns XP through natural progression, ie. kills and quests completed
        /// </summary>
        /// <param name="amount">The amount of XP being added</param>
        /// <param name="sharable">True if this XP can be shared with Fellowship</param>
        /// <param name="fixedAmount">For fellowships, is the XP bonus applied?</param>
        public void EarnXP(long amount, bool sharable = true, bool fixedAmount = false)
        {
            //Console.WriteLine($"{Name}.EarnXP({amount}, {sharable}, {fixedAmount})");

            // apply xp modifier
            var modifier = PropertyManager.GetDouble("xp_modifier").Item;
            var m_amount = (long)(amount * modifier);

            if (m_amount < 0)
            {
                log.Warn($"{Name}.EarnXP({amount}, {sharable}, {fixedAmount})");
                log.Warn($"Modifier: {modifier}, m_amount: {m_amount}");
                return;
            }

            GrantXP(m_amount, sharable, fixedAmount, false);
        }

        /// <summary>
        /// Adds XP to a player's total XP, handles triggers (vitae, level up)
        /// </summary>
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

                var xpTotalUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.TotalExperience, TotalExperience ?? 0);
                var xpAvailUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0);
                Session.Network.EnqueueSend(xpTotalUpdate, xpAvailUpdate);

                CheckForLevelup();
            }

            if (HasVitae)
                UpdateXpVitae(amount);
        }

        /// <summary>
        /// Optionally passes XP up the Allegiance tree
        /// </summary>
        private void UpdateXpAllegiance(long amount)
        {
            if (!HasAllegiance) return;

            AllegianceManager.PassXP(AllegianceNode, (ulong)amount, true);
        }

        /// <summary>
        /// Handles updating the vitae penalty through earned XP
        /// </summary>
        /// <param name="amount">The amount of XP to apply to the vitae penalty</param>
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

            if (vitaePenalty.EpsilonEquals(1.0f))
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(2.0f);
                actionChain.AddAction(this, () =>
                {
                    var vitae = EnchantmentManager.GetVitae();
                    if (vitae != null)
                    {
                        var curPenalty = vitae.StatModValue;
                        if (curPenalty.EpsilonEquals(1.0f))
                            EnchantmentManager.RemoveVitae();
                    }
                });
                actionChain.EnqueueChain();
            }
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

            if (Level >= maxLevel) return;

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
                var message = (Level == maxLevel) ? $"You have reached the maximum level of {Level}!" : $"You are now level {Level}!";

                message += (AvailableSkillCredits > 0) ? $"\nYou have {AvailableExperience:#,###0} experience points and {AvailableSkillCredits} skill credits available to raise skills and attributes." : $"\nYou have {AvailableExperience:#,###0} experience points available to raise skills and attributes.";

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
                    message += $"\nYou will earn another skill credit at level {nextLevelWithCredits}.";
                }

                if (Fellowship != null)
                    Fellowship.OnFellowLevelUp();

                Session.Network.EnqueueSend(levelUp);

                SetMaxVitals();

                // play level up effect
                PlayParticleEffect(ACE.Entity.Enum.PlayScript.LevelUp, Guid);

                Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Advancement), currentCredits);
            }
        }

        /// <summary>
        /// Spends the amount of XP specified, deducting it from available experience
        /// </summary>
        public bool SpendXP(long amount, bool sendNetworkPropertyUpdate = true)
        {
            if (AvailableExperience >= amount)
            {
                AvailableExperience -= amount;

                if (sendNetworkPropertyUpdate)
                {
                    var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableExperience, AvailableExperience ?? 0);
                    Session.Network.EnqueueSend(xpUpdate);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to spend all of the players Xp into Attributes, Vitals and Skills
        /// </summary>
        public void SpendAllXp(bool sendNetworkPropertyUpdate = true)
        {
            SpendAllAvailableAttributeXp(Strength, sendNetworkPropertyUpdate);
            SpendAllAvailableAttributeXp(Endurance, sendNetworkPropertyUpdate);
            SpendAllAvailableAttributeXp(Coordination, sendNetworkPropertyUpdate);
            SpendAllAvailableAttributeXp(Quickness, sendNetworkPropertyUpdate);
            SpendAllAvailableAttributeXp(Focus, sendNetworkPropertyUpdate);
            SpendAllAvailableAttributeXp(Self, sendNetworkPropertyUpdate);

            SpendAllAvailableVitalXp(Health, sendNetworkPropertyUpdate);
            SpendAllAvailableVitalXp(Stamina, sendNetworkPropertyUpdate);
            SpendAllAvailableVitalXp(Mana, sendNetworkPropertyUpdate);

            foreach (var skill in Skills)
                SpendAllAvailableSkillXp(skill.Value, sendNetworkPropertyUpdate);
        }

        /// <summary>
        /// Gives available XP of the amount specified, without increasing total XP
        /// </summary>
        public void RefundXP(long amount)
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

        /// <summary>
        /// Directly raises the available XP by a specified amount
        /// For debugging purposes only. Normal XP progression should use EarnXP above.
        /// </summary>
        /// <param name="amount">The amount of XP to grant to the player</param>
        /// <param name="passup">If TRUE, additional XP is passed up the allegiance chain</param>
        public void GrantXP(long amount, bool sharable = true, bool fixedAmount = false, bool message = true)
        {
            if (sharable)
            {
                if (Fellowship != null && Fellowship.ShareXP && Fellowship.SharableMembers.Contains(this))
                    Fellowship.SplitXp((ulong)amount, fixedAmount);
                else
                    UpdateXpAndLevel(amount);

                UpdateXpAllegiance(amount);
            }
            else
                UpdateXpAndLevel(amount);

            if (message)
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{amount:N0} experience granted.", ChatMessageType.Advancement));
        }

        /// <summary>
        /// Raise the available XP by a percentage of the current level XP or a maximum
        /// </summary>
        public void GrantLevelProportionalXp(double percent, ulong max)
        {
            var maxLevel = GetMaxLevel();
            if (Level >= maxLevel) return;

            var nextLevelXP = GetXPBetweenLevels(Level.Value, Level.Value + 1);
            var scaledXP = (long)Math.Min(nextLevelXP * percent, max);
            GrantXP(scaledXP);
        }

        /// <summary>
        /// Raise the available luminance by a specified amount
        /// </summary>
        public void GrantLuminance(long amount)
        {
            // apply lum modifier
            amount = (long)(amount * PropertyManager.GetDouble("luminance_modifier").Item);

            if (AvailableLuminance + amount > MaximumLuminance)
                amount = MaximumLuminance.Value - AvailableLuminance.Value;

            AvailableLuminance += amount;

            var luminance = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableLuminance, AvailableLuminance ?? 0);
            var message = new GameMessageSystemChat($"{amount:N0} luminance granted.", ChatMessageType.Advancement);
            Session.Network.EnqueueSend(luminance, message);
        }
    }
}
