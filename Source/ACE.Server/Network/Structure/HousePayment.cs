using System.Collections.Generic;
using System.IO;
using ACE.Database.Models.World;
using ACE.Server.Factories;
using ACE.Database;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Contains information about a house purchase or maintenance item
    /// </summary>
    public class HousePayment
    {
        public uint Num;            // The quantity required
        public uint Paid;           // The quantity paid
        public uint WeenieID;       // The item weenie class ID
        public string Name;         // The item name
        public string PluralName;   // The pluralized item name (if not specified, use <Name> followed by 's' or 'es')

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
            WeenieID = weenie.ClassId;
            Init(wo);
        }

        public void Init(WorldObject wo)
        {
            WeenieID = wo.WeenieClassId;
            Name = wo.Name;
            PluralName = wo.NamePlural;
            Num = (uint)(wo.StackSize ?? 1);
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
