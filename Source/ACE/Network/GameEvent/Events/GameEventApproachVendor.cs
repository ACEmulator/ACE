using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, WorldObject vendor, List<WorldObject> items)
            : base(GameEventType.ApproachVendor, GameMessageGroup.Group09, session)
        {        
            Writer.Write(vendor.Guid.Full); // merchant id

            // bit mask ? categories / mask may need figured out more.
            Writer.Write((uint)vendor.MerchandiseItemTypes);
            Writer.Write((uint)vendor.MerchandiseMinValue);
            Writer.Write((uint)vendor.MerchandiseMaxValue);
            Writer.Write((uint)(vendor.DealMagicalItems ?? false ? 1 : 0)); // magic
            Writer.Write((float)vendor.BuyPrice); // buy_price
            Writer.Write((float)vendor.SellPrice); // sell_price
            Writer.Write(vendor.AlternateCurrencyDID ?? 0u); // trade id .. wcid of currency vendor uses
            if (vendor.AlternateCurrencyDID > 0)
            {
                var currency = WorldObjectFactory.CreateWorldObject(Database.DatabaseManager.World.GetAceObjectByWeenie((uint)vendor.AlternateCurrencyDID));
                string fixedPlural = currency.NamePlural;
                if (fixedPlural == null)
                {
                    fixedPlural = currency.Name;
                    if (fixedPlural.EndsWith("ch") || fixedPlural.EndsWith("s") || fixedPlural.EndsWith("sh") || fixedPlural.EndsWith("x") || fixedPlural.EndsWith("z"))
                    {
                        fixedPlural += "es";
                    }
                    else
                    {
                        fixedPlural += "s";
                    }
                }
                Writer.Write((uint)0); // trade number .. current amount of that currency player has on hand, need a function to return # of items of specific wcid found in inventory
                Writer.WriteString16L(fixedPlural); // the name of that currency
            }
            else
            {
                Writer.Write((uint)0); // trade number .. current amount of that currency player has on hand, need a function to return # of items of specific wcid found in inventory
                Writer.WriteString16L(""); // the name of that currency
            }
            Writer.Write((uint)items.Count); // number of items

            foreach (WorldObject obj in items)
            {
                // Serialize Stream.
                Writer.Write(0xFFFFFFFF); // pretty sure this is either -1 (0xFFFFFFFF) or specific amount of item.. limited quanity
                obj.SerializeGameDataOnly(Writer);
            }

            Writer.Align();
        }
    }
}
