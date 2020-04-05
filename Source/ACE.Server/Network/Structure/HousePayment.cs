using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Contains information about a house purchase or maintenance item
    /// </summary>
    public class HousePayment
    {
        public int Num;             // The quantity required
        public int Paid;            // The quantity paid
        public uint WeenieID;       // The item weenie class ID
        public string Name;         // The item name
        public string PluralName;   // The pluralized item name (if not specified, use <Name> followed by 's' or 'es')

        /// <summary>
        /// The quantity remaining to be paid for this item
        /// </summary>
        public int Remaining => Math.Max(0, Num - Paid);

        public HousePayment() { }

        public HousePayment(string weenieName, ushort stackSize = 1)
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(weenieName);
            Init(weenie, stackSize);
        }

        public HousePayment(Weenie weenie, ushort stackSize = 1)
        {
            Init(weenie, stackSize);
        }

        public HousePayment(WorldObject wo)
        {
            Init(wo);
        }

        public void Init(Weenie weenie, ushort stackSize = 1)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassName);
            wo.SetStackSize(stackSize);
            WeenieID = weenie.WeenieClassId;
            Init(wo);
        }

        public void Init(WorldObject wo)
        {
            WeenieID = wo.WeenieClassId;
            Name = wo.Name;
            PluralName = wo.GetPluralName();
            Num = wo.StackSize ?? 1;
        }

        /// <summary>
        /// Returns the amount of items to consume for a particular HousePayment item
        /// </summary>
        public List<WorldObjectInfo<int>> GetConsumeItems(List<WorldObject> items)
        {
            if (Remaining == 0) return new List<WorldObjectInfo<int>>();

            // filter to payment tab items that apply to this HousePayment wcid
            // consume from smallest stacks first, to clear out the clutter
            var wcidItems = items.Where(i => i.WeenieClassId == WeenieID).OrderBy(i => i.StackSize ?? 1).ToList();

            // house monetary payments have pyreal wcid, and can be also be paid with trade notes
            if (WeenieID == 273)
            {
                // append trade notes
                // consume from smallest denomination stacks first, with multiple stacks of the same denomination sorted by stack size

                // note that this does not produce an optimal solution for minimizing the amount of trade notes consumed

                // a slightly better solution would be to iteratively consume the highest denomination trade note that is <= the remaining amount,
                // but there are still some sets where even that wouldn't be optimized..

                var tradeNotes = items.Where(i => i.ItemType == ItemType.PromissoryNote).OrderBy(i => i.StackSize ?? 1).OrderBy(i => i.StackUnitValue).ToList();

                var coinsAndTradeNotes = wcidItems.Concat(tradeNotes).ToList();

                return GetConsumeItems_Inner(coinsAndTradeNotes);
            }
            else
            {
                return GetConsumeItems_Inner(wcidItems);
            }
        }

        /// <summary>
        /// Returns the amount of items to consume for a particular HousePayment item
        /// </summary>
        public List<WorldObjectInfo<int>> GetConsumeItems_Inner(List<WorldObject> items)
        {
            var consumeItems = new List<WorldObjectInfo<int>>();

            var remaining = Remaining;

            foreach (var item in items)
            {
                var stackSize = item.StackSize ?? 1;

                var stackValue = item.Value ?? 0;
                var baseValue = item.StackUnitValue ?? 0;

                var amount = item.IsTradeNote ? stackValue : stackSize;

                if (amount <= remaining)
                {
                    consumeItems.Add(new WorldObjectInfo<int>(item, stackSize));
                    remaining -= amount;
                }
                else
                {
                    var consumeAmount = item.IsTradeNote ? (int)Math.Ceiling((float)remaining / baseValue) : remaining;

                    consumeItems.Add(new WorldObjectInfo<int>(item, consumeAmount));
                    remaining = 0;
                }

                if (remaining <= 0)
                    break;
            }

            return consumeItems;
        }
    }

    public static class HousePaymentExtensions
    {
        public static void Write(this BinaryWriter writer, HousePayment payment)
        {
            writer.Write(payment.Num);
            writer.Write(payment.Paid);
            writer.Write(payment.WeenieID);
            writer.WriteString16L(payment.Name);
            writer.WriteString16L(payment.PluralName);
        }

        public static void Write(this BinaryWriter writer, List<HousePayment> payments)
        {
            if (payments == null)
                payments = new List<HousePayment>();

            writer.Write(payments.Count);
            foreach (var payment in payments)
                writer.Write(payment);
        }
    }
}
