using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;

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
            // This is needed to fix stackables that were created before we removed the [Ephemeral] attribute from Encumbranceval and Value
            // In the distance future, this can be removed. 2019-02-13 Mag-nus
            if (MaxStackSize > 1)
            {
                EncumbranceVal = (StackUnitEncumbrance ?? 0) * (StackSize ?? 1);
                Value = (StackUnitValue ?? 0) * (StackSize ?? 1);
            }
        }
    }
}
