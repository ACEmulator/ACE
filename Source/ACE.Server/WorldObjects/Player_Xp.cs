using System.Linq;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public int Level
        {
            get => GetProperty(PropertyInt.Level) ?? 1;
            set => SetProperty(PropertyInt.Level, value);
        }

        public long TotalExperience
        {
            get => GetProperty(PropertyInt64.TotalExperience) ?? 0;
            set => SetProperty(PropertyInt64.TotalExperience, value);
        }

        public long AvailableExperience
        {
            get => GetProperty(PropertyInt64.AvailableExperience) ?? 0;
            set => SetProperty(PropertyInt64.AvailableExperience, value);
        }

        /// <summary>
        /// Raise the available XP by a specified amount
        /// </summary>
        /// <param name="amount">A unsigned long containing the desired XP amount to raise</param>
        public void GrantXp(long amount)
        {
            UpdateXpAndLevel(amount);

            var message = new GameMessageSystemChat($"{amount} experience granted.", ChatMessageType.Advancement);
            Session.Network.EnqueueSend(message);
        }

        public void EarnXP(long amount, bool sharable = true, bool fixedAmount = false)
        {
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

            var maxLevel = xpTable.CharacterLevelXPList.Count;
            var maxLevelXp = xpTable.CharacterLevelXPList.Last();

            if (Level != maxLevel)
            {
                var amountLeftToEnd = (long)maxLevelXp - TotalExperience;
                if (amount > amountLeftToEnd)
                    amount = amountLeftToEnd;

                AvailableExperience += amount;
                TotalExperience += amount;

                CheckForLevelup();
                var xpTotalUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.TotalExperience, (ulong)TotalExperience);
                var xpAvailUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, (ulong)AvailableExperience);
                Session.Network.EnqueueSend(xpTotalUpdate, xpAvailUpdate);
            }
        }

        /// <summary>
        /// Determines if the player has advanced a level
        /// </summary>
        /// <remarks>
        /// Known issues:
        ///         1. XP updates from outside of the grantxp command have not been done yet.
        /// </remarks>
        private void CheckForLevelup()
        {
            // Question: Where do *we* call CheckForLevelup()? :
            //      From within the player.cs file, the options might be:
            //           GrantXp()
            //      From outside of the player.cs file, we may call CheckForLevelup() durring? :
            //           XP Updates?
            var xpTable = DatManager.PortalDat.XpTable;

            var startingLevel = Level;
            var maxLevel = xpTable.CharacterLevelXPList.Count - 1;
            bool creditEarned = false;

            if (Level == maxLevel) return;

            // increases until the correct level is found
            while (xpTable.CharacterLevelXPList[Level + 1] <= (ulong)TotalExperience)
            {
                Level++;

                // increase the skill credits if the chart allows this level to grant a credit
                if (xpTable.CharacterLevelSkillCreditList[Level] > 0)
                {
                    AvailableSkillCredits += (int)xpTable.CharacterLevelSkillCreditList[Level];
                    TotalSkillCredits += (int)xpTable.CharacterLevelSkillCreditList[Level];
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

                var levelUp = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.Level, Level);
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.AvailableSkillCredits, AvailableSkillCredits);

                if (Level != maxLevel && !creditEarned)
                {
                    var nextLevelWithCredits = 0;

                    for (int i = Level + 1; i <= maxLevel; i++)
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

                var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, (ulong)AvailableExperience);
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

            var xpUpdate = new GameMessagePrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, (ulong)AvailableExperience);
            Session.Network.EnqueueSend(xpUpdate);
        }
    }
}
