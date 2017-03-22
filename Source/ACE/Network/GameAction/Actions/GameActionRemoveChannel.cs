
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.RemoveChannel)]
    public class GameActionRemoveChannel : GameActionPacket
    {
        private GroupChatType chatChannelID;

        public GameActionRemoveChannel(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            chatChannelID = (GroupChatType)Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!Session.Player.IsAdmin && !Session.Player.IsArch && !Session.Player.IsPsr)
                return;

            // TODO: Unsubscribe from channel (chatChannelID) and save to db. Channel subscriptions are meant to persist between sessions.
        }
    }
}