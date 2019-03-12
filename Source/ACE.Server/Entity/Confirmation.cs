using System;
using System.Security.Cryptography;
using System.Text;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class Confirmation
    {
        public uint ConfirmationID { get; set; }

        public ConfirmationType ConfirmationType { get; set; }

        public string Message { get; set; }

        public string Quest { get; set; }

        public DateTime CreatedDateTime { get; set; }

        // define the callback function / parameters generically?
        public Player Player { get; set; }

        public WorldObject Source { get; set; }

        public WorldObject Target { get; set; }

        public Confirmation(ConfirmationType confirmationType, string message, WorldObject source, WorldObject target, Player player = null, string quest = null)
        {
            ConfirmationID = GenerateContextId();
            ConfirmationType = confirmationType;
            Message = message;
            CreatedDateTime = DateTime.UtcNow;

            // function parameters
            Player = player;
            Source = source;
            Target = target;

            Quest = quest;
        }

        public Confirmation()
        { }

        private uint GenerateContextId()
        {
            // this seems to be a much smaller # in retail... the highest i saw was between ~400-500 in a brief search
            // these #s also seem to always increase, not sure if these are just sequence #s in a particular context?
            // sending a context id for a lower # might have the client reject the message...

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
