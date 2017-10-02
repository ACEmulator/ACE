using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, WorldObject wo, List<WorldObject> items)
            : base(GameEventType.ApproachVendor, GameMessageGroup.Group09, session)
        {        
            Writer.Write(wo.Guid.Full); // merchant id

            // bit mask ? categories / mask may need figured out more.
            Writer.Write((uint)wo.MerchandiseItemTypes);
            Writer.Write((uint)wo.MerchandiseMinValue);
            Writer.Write((uint)wo.MerchandiseMaxValue);
            Writer.Write((uint)1); // magic
            Writer.Write((float)0.8); // buy_price
            Writer.Write((float)10); // sell_price
            Writer.Write((uint)0); // trade id .. is this a timestamp type val?
            Writer.Write((uint)0); // trade number .. is this a timestamp type val?
            Writer.WriteString16L("");
            Writer.Write((uint)items.Count); // number of items

            foreach (WorldObject obj in items)
            {
                // Serialize Stream.
                Writer.Write(0xFFFFFFFF); // old weenie
                Writer.Write(obj.Guid.Full);
                obj.SerializeGameDataOnly(Writer);
            }
            Writer.Align();
        }
    }
}
