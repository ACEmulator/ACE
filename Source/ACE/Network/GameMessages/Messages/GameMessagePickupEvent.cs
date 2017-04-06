using ACE.Entity;
using ACE.Network.GameEvent;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePickupEvent : GameMessage
    {
        public GameMessagePickupEvent(Session session,  WorldObject targetItem)
            : base(GameMessageOpcode.PickupEvent, GameMessageGroup.Group0A)
        {
            Writer.Write(targetItem.Guid.Full);
            Writer.Write(session.Player.Guid.Full);
            Writer.Write((uint)targetItem.PhysicsData.InstanceSequence);
            Writer.Write((uint)++targetItem.PhysicsData.PositionSequence);
        }
    }
}
