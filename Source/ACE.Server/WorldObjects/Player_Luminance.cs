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

            var m_amount = (long)Math.Round(amount * modifier);

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
            if (AvailableLuminance == MaximumLuminance)
                return;

            // this is similar to Player_Xp.UpdateXpAndLevel()

            var remaining = (MaximumLuminance - AvailableLuminance) ?? 0;

            var addAmount = Math.Min(amount, remaining);

            AvailableLuminance += addAmount;

            UpdateLuminance();
        }

        /// <summary>
        /// Spends the amount of luminance specified, deducting it from available luminance
        /// </summary>
        public bool SpendLuminance(long amount)
        {
            if (amount > (AvailableLuminance ?? 0))
                return false;

            AvailableLuminance -= amount;

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
