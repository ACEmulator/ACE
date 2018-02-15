using System.Collections.Generic;
using System.IO;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Entity.WorldObjects
{
    public class Coin : WorldObject
    {
        private const IdentifyResponseFlags idFlags = IdentifyResponseFlags.IntStatsTable;

        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public Coin(Weenie weenie, Biota biota = null) : base(weenie, biota)
        {
            DescriptionFlags |= ObjectDescriptionFlag.Attackable;

            SetProperty(PropertyBool.Attackable, true);

            if (biota == null) // If no biota was passed our base will instantiate one, and we will initialize it with appropriate default values
            {
                SetProperty(PropertyInt.EncumbranceVal, 0);
                SetProperty(PropertyInt.Value, 1);
                SetProperty(PropertyInt.StackSize, 1);
            }
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            var properties = new Dictionary<PropertyInt, int>();
            properties[PropertyInt.Value] = GetProperty(PropertyInt.Value) ?? 0;
            properties[PropertyInt.EncumbranceVal] = GetProperty(PropertyInt.EncumbranceVal) ?? 0;

            WriteIdentifyObjectHeader(writer, idFlags, true); // Always succeed in assessing a coin.
            WriteIdentifyObjectProperties(writer, idFlags, properties);
        }
    }
}
