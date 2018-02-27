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
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Coin(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
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

        public override int? Value
        {
            get
            {
                var value = ((StackUnitValue ?? 0) * (StackSize ?? 1));

                SetProperty(PropertyInt.Value, value);

                return value;
            }
        }

        public override int? CoinValue
        {
            get
            {
                var value = Value ?? 0;

                SetProperty(PropertyInt.CoinValue, value);

                return value;
            }
        }
    }
}
