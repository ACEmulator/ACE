using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionFellowshipAssignNewLeader
    {
        [GameAction(GameActionType.FellowshipAssignNewLeader)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint newLeaderID = message.Payload.ReadUInt32();
            Player newLeader = WorldManager.GetPlayerByGuidId(newLeaderID);
            session.Player.FellowshipNewLeader(newLeader);
        }
    }
}
