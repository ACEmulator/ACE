using ACE.Network.Enum;
using ACE.Network.Sequence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity
{
    public class Confirmation
    {
        public uint ConfirmationID { get; set; }

        public ConfirmationType ConfirmationType { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public uint Initiator { get; set; }

        public uint Target { get; set; }

        public Confirmation(ConfirmationType confirmationType, string message, uint initiator, uint confirmationSequence, uint target)
        {
            ConfirmationID = confirmationSequence;
            ConfirmationType = confirmationType;
            Message = message;
            Initiator = initiator;
            Target = target;
        }

        public Confirmation()
        { }

    }
}
