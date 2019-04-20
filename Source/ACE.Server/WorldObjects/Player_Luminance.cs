using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void SpendLuminance(long amount)
        {
            if (amount > AvailableLuminance.Value)
                return;

            AvailableLuminance -= amount;

            var luminance = new GameMessagePrivateUpdatePropertyInt64(this, PropertyInt64.AvailableLuminance, AvailableLuminance ?? 0);
            var message = new GameMessageSystemChat($"{amount} luminance spent.", ChatMessageType.Advancement);
            Session.Network.EnqueueSend(luminance, message);
        }
    }
}
