using System;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Applies luminance modifiers before adding luminance
        /// </summary>
        public void EarnLuminance(long amount, XpType xpType, ShareType shareType = ShareType.All)
        {
            // following the same model as Player_Xp

            var modifier = PropertyManager.GetDouble("luminance_modifier").Item;

            // should this be passed upstream to fellowship?
            var enchantment = GetXPAndLuminanceModifier(xpType);

            var m_amount = (long)Math.Round(amount * enchantment * modifier);

            GrantLuminance(m_amount, xpType, shareType);
        }

        /// <summary>
        /// Directly grants luminance to the player, without any additional luminance modifiers
        /// </summary>
        public void GrantLuminance(long amount, XpType xpType, ShareType shareType = ShareType.All)
        {
            if (Fellowship != null && Fellowship.ShareXP && shareType.HasFlag(ShareType.Fellowship))
            {
                // this will divy up the luminance, and re-call this function
                // with ShareType.Fellowship removed
                Fellowship.SplitLuminance((ulong)amount, xpType, shareType, this);
            }
            else
                AddLuminance(amount, xpType);
        }

        private void AddLuminance(long amount, XpType xpType)
        {
            var available = AvailableLuminance ?? 0;
            var maximum = MaximumLuminance ?? 0;

            if (available == maximum)
                return;

            // this is similar to Player_Xp.UpdateXpAndLevel()

            var remaining = maximum - available;

            var addAmount = Math.Min(amount, remaining);

            AvailableLuminance = available + addAmount;

            if (xpType == XpType.Quest)
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You've earned {amount:N0} Luminance.", ChatMessageType.Broadcast));

            UpdateLuminance();
        }

        /// <summary>
        /// Spends the amount of luminance specified, deducting it from available luminance
        /// </summary>
        public bool SpendLuminance(long amount)
        {
            var available = AvailableLuminance ?? 0;

            if (amount > available)
                return false;

            AvailableLuminance = available - amount;

            UpdateLuminance();

            return true;
        }

        /// <summary>
        /// Sends network message to update luminance
        /// </summary>
        private void UpdateLuminance()
        {
            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableLuminance, AvailableLuminance ?? 0));
        }
    }
}
