using System.Collections.Generic;
using System.IO;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    public class Coin : WorldObject
    {
        private const IdentifyResponseFlags idFlags = IdentifyResponseFlags.IntStatsTable;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Coin(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            //SetProperty(PropertyInt.EncumbranceVal, 0);
            //SetProperty(PropertyInt.Value, 1);
            //SetProperty(PropertyInt.StackSize, 1);

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Coin(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
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
