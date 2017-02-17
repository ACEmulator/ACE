using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.RemoveFriend)]
    public class GameActionRemoveFriend : GameActionPacket
    {
        private ObjectGuid friendId;

        public GameActionRemoveFriend(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            uint lowId = fragment.Payload.ReadUInt32() & 0xFFFFFF;
            friendId = new ObjectGuid(lowId, GuidType.Player);
        }

        public async override void Handle()
        {
            string result = await session.Player.RemoveFriend(friendId);

            if(!string.IsNullOrEmpty(result))
                ChatPacket.SendSystemMessage(session, "There was a problem removing that friend.");            
        }
    }
}
