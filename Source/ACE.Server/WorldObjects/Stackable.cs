using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    public class Stackable : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Stackable(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Stackable(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override int? EncumbranceVal
        {
            get
            {
                var value = ((StackUnitEncumbrance ?? 0) * (StackSize ?? 1));

                base.EncumbranceVal = value;

                return value;
            }
        }

        public override int? Value
        {
            get
            {
                var value = ((StackUnitValue ?? 0) * (StackSize ?? 1));

                base.Value = value;

                return value;
            }
        }
    }
}
