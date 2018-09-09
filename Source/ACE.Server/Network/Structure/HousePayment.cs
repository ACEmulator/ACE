using System.Collections.Generic;
using System.IO;

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
            writer.Write(payments.Count);
            foreach (var payment in payments)
                writer.Write(payment);
        }
    }
}
