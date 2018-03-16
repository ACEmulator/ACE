using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Corpse : WorldObject
    {
        private static readonly UniversalMotion dead = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Corpse(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Corpse(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            CurrentMotionState = dead;
        }
    }
}
