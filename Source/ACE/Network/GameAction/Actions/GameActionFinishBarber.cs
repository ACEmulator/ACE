using ACE.Common.Extensions;
using ACE.Network.Enum;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.DatLoader;
using System;
using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionFinishBarber
    {
        [GameAction(GameActionType.FinishBarber)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionFinishBarber(message);
        }
    }
}
