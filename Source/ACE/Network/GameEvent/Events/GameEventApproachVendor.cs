using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, WorldObject worldObject)
            : base(GameEventType.ApproachVendor, GameMessageGroup.Group09, session)
        {        
            Writer.WriteGuid(worldObject.Guid); // merchant id

            // Pack Categories Food & Trade Notes ?
            uint buycatgegories = 17291; 
            Writer.Write(buycatgegories); // item_types

            Writer.Write((uint)0); // min_value
            Writer.Write((ulong)100000000); // max_value
            Writer.Write((uint)0); // magic
            Writer.Write((float)10); // buy_price
            Writer.Write((float)0); // sell_price
            Writer.Write((uint)0); // trade id
            Writer.Write((uint)0); // trade number aka number of items avaliable
            Writer.WriteString16L("");

            var loot = LootGenerationFactory.CreateRandomTestWorldObject(8);
            if (loot != null)
            {
                loot.SerializeCreateObject(Writer, true);
            }
        }
    }
}