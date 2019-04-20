using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Gets slumlord info, sent after getting a failed house transaction
    /// </summary>
    public static class GameActionHouseQueryLord
    {
        [GameAction(GameActionType.QueryLord)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine("Received 0x258 - House - QueryLord");

            var lord = message.Payload.ReadUInt32();    // slumlord ID to request info for
        }
    }
}
