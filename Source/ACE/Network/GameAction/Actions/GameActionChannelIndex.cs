﻿using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionChannelIndex
    {
        [GameAction(GameActionType.IndexChannels)]
        public static void Handle(ClientMessage message, Session session)
        {
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!session.Player.IsAdmin && !session.Player.IsArch && !session.Player.IsPsr)
                return;

            session.EnqueueSend(new GameEventChannelIndex(session));
        }
    }
}