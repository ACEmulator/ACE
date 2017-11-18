using System;
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network.GameMessages.Messages;
using ACE.Entity.Actions;
using ACE.Network.Motion;
using ACE.Network.Sequence;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionGameJoin
    {
        [GameAction(GameActionType.Join)]
        public static void Handle(ClientMessage message, Session session)
        {
            var gameId = message.Payload.ReadGuid(); // object id of gameboard
            var whichTeam = message.Payload.ReadUInt32(); // expecting 0xFFFFFFFF here

            Game wo = session.Player.CurrentLandblock.GetObject(gameId) as Game;
            if (wo != null)
            {
                wo.ActOnJoin(session.Player.Guid);
            }
        }
    }
}
