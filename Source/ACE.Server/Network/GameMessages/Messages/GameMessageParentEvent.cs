using ACE.Entity.Enum;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageParentEvent : GameMessage
    {
        public GameMessageParentEvent(WorldObject creature, WorldObject wieldedSelectableItem, ParentLocation? overriddenParentLocation = null, Placement? overriddenPlacement = null)
            : base(GameMessageOpcode.ParentEvent, GameMessageGroup.SmartboxQueue)
        {
            Writer.WriteGuid(creature.Guid);
            Writer.WriteGuid(wieldedSelectableItem.Guid);
            Writer.Write((int)(overriddenParentLocation ?? wieldedSelectableItem.ParentLocation ?? ParentLocation.None));
            Writer.Write((int)(overriddenPlacement ?? wieldedSelectableItem.Placement ?? Placement.Default));
            Writer.Write(creature.Sequences.GetCurrentSequence(SequenceType.ObjectInstance));
            Writer.Write(wieldedSelectableItem.Sequences.GetNextSequence(SequenceType.ObjectPosition));
        }
    }
}
