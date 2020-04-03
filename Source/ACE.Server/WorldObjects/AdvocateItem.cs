using System;

using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class AdvocateItem : GenericObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public AdvocateItem(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public AdvocateItem(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void OnWield(Creature creature)
        {
            creature.RadarColor = ACE.Entity.Enum.RadarColor.Advocate;
            creature.EnqueueBroadcast(true, new GameMessagePublicUpdatePropertyInt(creature, PropertyInt.RadarBlipColor, (int)creature.RadarColor));
            creature.SetProperty(PropertyBool.AdvocateState, true);

            base.OnWield(creature);
        }

        public override void OnUnWield(Creature creature)
        {
            creature.RadarColor = null;
            creature.EnqueueBroadcast(true, new GameMessagePublicUpdatePropertyInt(creature, PropertyInt.RadarBlipColor, 0));
            creature.SetProperty(PropertyBool.AdvocateState, false);

            base.OnUnWield(creature);
        }
    }
}
