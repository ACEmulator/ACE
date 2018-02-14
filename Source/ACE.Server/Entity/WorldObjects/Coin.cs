using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

using AceObjectPropertiesInt = ACE.Entity.AceObjectPropertiesInt;

namespace ACE.Server.Entity.WorldObjects
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
                    Value = (Weenie.GetProperty(PropertyInt.Value) ?? 0) * (StackSize ?? 1);
                }
            }
        }

        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public Coin(Weenie weenie, Biota biota = null) : base(weenie, biota)
        {
            if (biota == null) // If no biota was passed our base will instantiate one, and we will initialize it with appropriate default values
            {
                // TODO we shouldn't be auto setting properties that come from our weenie by default

                Attackable = true; // todo use SetProperty instead

                SetObjectDescriptionBools(); // todo we shouldn't be doing these types of calls

                CoinPropertiesInt = PropertiesInt.Where(x => x.PropertyId == (uint)PropertyInt.Value || x.PropertyId == (uint)PropertyInt.EncumbranceVal).ToList();

                StackSize = (base.StackSize ?? 1); // todo use SetProperty instead

                Value = (Weenie.GetProperty(PropertyInt.Value) ?? 0) * (StackSize ?? 1); // todo use SetProperty instead

                if (StackSize == null)
                    StackSize = 1;
            }
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
