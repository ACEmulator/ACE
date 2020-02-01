
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class AdvocateItem : GenericObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public AdvocateItem(ACE.Entity.Models.Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public AdvocateItem(Database.Models.Shard.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public AdvocateItem(ACE.Entity.Models.Biota biota) : base(biota)
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
