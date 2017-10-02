// WeenieType.Coin

using ACE.DatLoader.FileTypes;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.Entity
{
    public class Coin : WorldObject
    {
        private const IdentifyResponseFlags idFlags = IdentifyResponseFlags.IntStatsTable;

        private ushort stackSize = 1;
        public override ushort? StackSize
        {
            get { return stackSize; }
            set
            {
                if (value != stackSize)
                {
                    base.StackSize = value;
                    stackSize = (ushort)value;
                    Value = (Weenie.Value ?? 0) * (StackSize ?? 1);
                }
            }
        }

        public Coin(AceObject aceObject)
            : base(aceObject)
        {
            CoinPropertiesInt = PropertiesInt.Where(x => x.PropertyId == (uint)PropertyInt.Value
                                                          || x.PropertyId == (uint)PropertyInt.EncumbranceVal).ToList();

            StackSize = (base.StackSize ?? 1);

            Value = (Weenie.Value ?? 0) * (StackSize ?? 1);

            if (StackSize == null)
                StackSize = 1;
        }

        private List<AceObjectPropertiesInt> CoinPropertiesInt
        {
            get;
            set;
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            WriteIdentifyObjectHeader(writer, idFlags, true); // Always succeed in assessing a coin.
            WriteIdentifyObjectIntProperties(writer, idFlags, CoinPropertiesInt);
        }
    }
}
