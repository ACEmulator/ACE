using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateAttribute : GameMessage
    {
        public GameMessagePrivateUpdateAttribute(WorldObject worldObject, CreatureAttribute creatureAttribute)
            : base(GameMessageOpcode.PrivateUpdateAttribute, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdateAttribute, creatureAttribute.Attribute));
            Writer.Write((uint)creatureAttribute.Attribute);
            Writer.Write(creatureAttribute.Ranks);
            Writer.Write(creatureAttribute.StartingValue);
            Writer.Write(creatureAttribute.ExperienceSpent);
        }
    }
}
