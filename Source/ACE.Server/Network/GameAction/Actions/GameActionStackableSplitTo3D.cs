
namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// This method handles the Game Action F7B1 - 0x0056 Stackable Split to 3D. This is sent to the server when the player splits a stack onto the ground.
    /// There are different messages for split to 3D (world) and to split to wield.
    /// We get from the client the guid of the item stack we are about to split and the amount we are trying to split out.
    /// </summary>
    public static class GameActionStackableSplitTo3D
    {
        [GameAction(GameActionType.StackableSplitTo3D)]
        public static void Handle(ClientMessage message, Session session)
        {
            // Read in the applicable data.
            uint stackId    = message.Payload.ReadUInt32();
            int amount      = message.Payload.ReadInt32();

            session.Player.HandleActionStackableSplitTo3D(stackId, amount);
        }
    }
}
