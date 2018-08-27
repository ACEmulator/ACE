using System;
using ACE.Entity.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Helper class for arrows / bolts / thrown weapons
    /// outside of the WorldObject heirarchy
    /// </summary>
    public class Projectile
    {
        public WorldObject WorldObject;

        public PhysicsObj PhysicsObj { get => WorldObject.PhysicsObj; }

        public WorldObject ProjectileSource { get => WorldObject.ProjectileSource; }
        public WorldObject ProjectileTarget { get => WorldObject.ProjectileTarget; }

        public Projectile() { }

        public Projectile(WorldObject worldObject)
        {
            WorldObject = worldObject;
        }

        public void OnCollideObject(WorldObject target)
        {
            if (!PhysicsObj.is_active()) return;

            //Console.WriteLine(string.Format("Projectile.OnCollideObject({0} - {1} || {2} - {3})", Guid.Full.ToString("X8"), Name, target.Guid.Full.ToString("X8"), target.Name));

            if (ProjectileTarget == null || !ProjectileTarget.Equals(target))
            {
                //Console.WriteLine("Unintended projectile target! (should be " + ProjectileTarget.Guid.Full.ToString("X8") + " - " + ProjectileTarget.Name + ")");
                OnCollideEnvironment();
                return;
            }

            // take damage
            var player = ProjectileSource as Player;
            var creatureTarget = target as Creature;
            if (player != null && creatureTarget != null)
            {
                var damage = player.DamageTarget(creatureTarget, WorldObject);

                if (damage > 0)
                    player.Session.Network.EnqueueSend(new GameMessageSound(WorldObject.Guid, Sound.Collision, 1.0f));    // todo: landblock broadcast?
            }

            WorldObject.CurrentLandblock?.RemoveWorldObject(WorldObject.Guid, false);
            PhysicsObj.set_active(false);
        }

        public void OnCollideEnvironment()
        {
            if (!PhysicsObj.is_active()) return;

            //Console.WriteLine("Projectile.OnCollideEnvironment(" + Guid.Full.ToString("X8") + ")");

            WorldObject.CurrentLandblock?.RemoveWorldObject(WorldObject.Guid, false);
            PhysicsObj.set_active(false);

            var player = ProjectileSource as Player;
            if (player != null)
                player.Session.Network.EnqueueSend(new GameMessageSystemChat("Your missile attack hit the environment.", ChatMessageType.Broadcast));
        }
    }
}
