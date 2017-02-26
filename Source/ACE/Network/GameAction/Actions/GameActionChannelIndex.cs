
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.IndexChannels)]
    public class GameActionChannelIndex : GameActionPacket
    {
        //private GroupChatType chatChannelID;

        public GameActionChannelIndex(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        //public override void Read()
        //{
        //    chatChannelID = (GroupChatType)Fragment.Payload.ReadUInt32();
        //}

        public override void Handle()
        {
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!Session.Player.IsAdmin && !Session.Player.IsArch && !Session.Player.IsPsr)
                return;

            NetworkManager.SendWorldMessage(Session, new GameEventChannelIndex(Session));
        }
    }
}