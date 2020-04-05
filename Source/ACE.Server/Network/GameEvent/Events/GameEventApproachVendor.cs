using System;
using System.Collections.Generic;

using ACE.Database;
using ACE.Entity.Models;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, Vendor vendor, List<WorldObject> items, uint altCurrencySpent)
            : base(GameEventType.ApproachVendor, GameMessageGroup.UIQueue, session)
        {        
            Writer.Write(vendor.Guid.Full);

            // the types of items vendor will purchase
            Writer.Write((uint)vendor.MerchandiseItemTypes);
            Writer.Write((uint)vendor.MerchandiseMinValue);
            Writer.Write((uint)vendor.MerchandiseMaxValue);

            Writer.Write(Convert.ToUInt32(vendor.DealMagicalItems ?? false));

            Writer.Write((float)vendor.BuyPrice);
            Writer.Write((float)vendor.SellPrice);

            // the wcid of the alternate currency
            Writer.Write(vendor.AlternateCurrency ?? 0);

            // if this vendor accepts items as alternate currency, instead of pyreals
            if (vendor.AlternateCurrency != null)
            {
                var altCurrency = DatabaseManager.World.GetCachedWeenie(vendor.AlternateCurrency.Value);
                var pluralName = altCurrency.GetPluralName();

                // the total amount of alternate currency the player currently has
                var altCurrencyInInventory = (uint)session.Player.GetNumInventoryItemsOfWCID(vendor.AlternateCurrency.Value);
                Writer.Write(altCurrencyInInventory + altCurrencySpent);

                // the plural name of alt currency
                Writer.WriteString16L(pluralName);
            }
            else
            {
                Writer.Write((uint)0);
                Writer.WriteString16L("");
            }

            Writer.Write((uint)items.Count);    

            foreach (WorldObject obj in items)
            {
                Writer.Write(-1);   // -1 = unlimited supply?
                obj.SerializeGameDataOnly(Writer);
            }

            Writer.Align();
        }
    }
}
