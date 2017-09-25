using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, ObjectGuid objectID, List<AceObject> items)
            : base(GameEventType.ApproachVendor, GameMessageGroup.Group09, session)
        {        
            Writer.Write(objectID.Full); // merchant id

            // bit mask ? categories / mask may need figured out more.
            // what will the vendor offer to buy
            ObjectDescriptionFlag buycatgegories;
            buycatgegories = ObjectDescriptionFlag.Food;
            Writer.Write((uint)buycatgegories);

            Writer.Write((uint)0); // min_value
            Writer.Write((uint)0); // max_value
            Writer.Write((uint)1); // magic
            Writer.Write((float)0.8); // buy_price
            Writer.Write((float)10); // sell_price
            Writer.Write((uint)0); // trade id .. is this a timestamp type val?
            Writer.Write((uint)0); // trade number .. is this a timestamp type val?
            Writer.WriteString16L("");
            Writer.Write((uint)items.Count); // number of items

            Writer.Write((uint)0xFFFFFFFF); // old weenie

            int iid = 0;
            foreach (AceObject obj in items)
            {
                //Render Stream.
                iid++;
                Writer.Write((uint)iid);
                // I need to render a bunch of stuff from game data of obj ?
            }

        }
    }
}
