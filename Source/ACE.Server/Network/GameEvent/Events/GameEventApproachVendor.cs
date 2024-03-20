using System;

using ACE.Database;
using ACE.Entity.Models;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventApproachVendor : GameEventMessage
    {
        public GameEventApproachVendor(Session session, Vendor vendor, uint altCurrencySpent)
            : base(GameEventType.ApproachVendor, GameMessageGroup.UIQueue, session, 8192) // 5,376 is the average seen in retail pcaps, 15,272 is the max seen in retail pcaps
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
                Writer.Write(0);
                Writer.WriteString16L(string.Empty);
            }

            var numItems = vendor.DefaultItemsForSale.Count + vendor.UniqueItemsForSale.Count;

            Writer.Write(numItems);

            vendor.forEachItem((obj) =>
            {
                int stackSize = obj.VendorShopCreateListStackSize ?? obj.StackSize ?? 1; // -1 = unlimited supply

                // packed value: (stackSize & 0xFFFFFF) | (pwdType << 24)
                // pwdType: flag indicating whether the new or old PublicWeenieDesc is used; -1 = PublicWeenieDesc, 1 = OldPublicWeenieDesc; -1 always used.
                Writer.Write(stackSize & 0xFFFFFF | -1 << 24);

                obj.SerializeGameDataOnly(Writer);
            });

            Writer.Align();
        }
    }
}
