using System;

using ACE.Entity;
using ACE.Entity.Models;

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
            if (!StackSize.HasValue)
                StackSize = 1;

            if (!MaxStackSize.HasValue)
                MaxStackSize = 1;

            if (!Value.HasValue)
                Value = 0;

            if (!EncumbranceVal.HasValue)
                EncumbranceVal = 0;

            if (!StackUnitEncumbrance.HasValue)
            {
                if (StackSize > 1)
                    StackUnitEncumbrance = EncumbranceVal / StackSize;
                else
                    StackUnitEncumbrance = EncumbranceVal;
            }

            if (!StackUnitValue.HasValue)
            {
                if (StackSize > 1)
                    StackUnitValue = Value / StackSize;
                else
                    StackUnitValue = Value;
            }

            // This is needed to fix stackables that were created before we removed the [Ephemeral] attribute from Encumbranceval and Value
            // In the distance future, this can be removed. 2019-02-13 Mag-nus
            if (MaxStackSize > 1)
            {
                EncumbranceVal = (StackUnitEncumbrance ?? 0) * (StackSize ?? 1);
                Value = (StackUnitValue ?? 0) * (StackSize ?? 1);
            }
        }

        public override void ActOnUse(WorldObject wo)
        {
            // Do nothing
        }
    }
}
