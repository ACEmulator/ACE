using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using Biota = ACE.Database.Models.Shard.Biota;

namespace ACE.Server.WorldObjects
{
    public class Cow : Creature
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Cow(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Cow(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void ActOnUse(WorldObject activator)
        {
            if (activator is Player player)
            {
                if (CreatureType == ACE.Entity.Enum.CreatureType.Auroch)
                    player.SendMessage($"The {Name} ignores you.");
                else
                {
                    Active = false;
                    var actionChain = new ActionChain();
                    EnqueueMotion(actionChain, MotionCommand.TippedRight);
                    actionChain.AddAction(this, () => Active = true);
                    actionChain.EnqueueChain();
                }
            }
        }
    }
}
