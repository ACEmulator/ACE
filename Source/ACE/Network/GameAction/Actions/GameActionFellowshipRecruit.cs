using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionFellowshipRecruit
    {
        [GameAction(GameActionType.FellowshipRecruit)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint newMemberId = message.Payload.ReadUInt32();
            ObjectGuid newMember = new ObjectGuid(newMemberId);
            Player newPlayer = WorldManager.GetPlayerByGuidId(newMemberId);
            session.Player.FellowshipRecruit(newPlayer);
        }
    }
}
