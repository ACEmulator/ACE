using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    public class Coin : Stackable
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

        public override int? CoinValue
        {
            get
            {
                var value = Value ?? 0;

                base.CoinValue = value;

                return value;
            }
        }
    }
}
