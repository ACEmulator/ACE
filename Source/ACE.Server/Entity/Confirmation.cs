using System;
using System.Security.Cryptography;
using System.Text;
using ACE.Server.Network.Enum;

namespace ACE.Server.Entity
{
    public class Confirmation
    {
        public uint ConfirmationID { get; set; }

        public ConfirmationType ConfirmationType { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public uint Initiator { get; set; }

        public uint Target { get; set; }

        public Confirmation(ConfirmationType confirmationType, string message, uint initiator, uint target)
        {
            ConfirmationID = GenerateContextId();
            ConfirmationType = confirmationType;
            Message = message;
            Initiator = initiator;
            Target = target;
        }

        public Confirmation()
        { }

        private uint GenerateContextId()
        {
            char[] chars = new char[] {'1','2','3','4','5','6','7','8','9','0' };
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[9];
            crypto.GetNonZeroBytes(data);
            StringBuilder sb = new StringBuilder(9);
            foreach (byte b in data)
            {
                sb.Append(chars[b % (chars.Length)]);
            }
            return Convert.ToUInt32(sb.ToString());
        }

    }
}
