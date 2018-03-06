using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System.IO;

namespace ACE.Server.WorldObjects
{
    public class GenericObject : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public GenericObject(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public GenericObject(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            if (GetProperty(PropertyInt.ArmorType).HasValue)
                flags |= IdentifyResponseFlags.ArmorProfile;

            base.SerializeIdentifyObjectResponse(writer, success, flags);

            if (flags.HasFlag(IdentifyResponseFlags.ArmorProfile))
                WriteIdentifyObjectArmorProfile(writer, this, success);
        }
    }
}
