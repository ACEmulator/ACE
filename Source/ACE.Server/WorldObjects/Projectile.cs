using System;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Helper class for arrows / bolts / thrown weapons
    /// outside of the WorldObject hierarchy
    /// </summary>
    public class Projectile
    {
        public WorldObject WorldObject;

        public PhysicsObj PhysicsObj => WorldObject.PhysicsObj;

        public WorldObject ProjectileSource => WorldObject.ProjectileSource;
        public WorldObject ProjectileTarget => WorldObject.ProjectileTarget;

        public Projectile() { }

        public Projectile(WorldObject worldObject)
        {
            WorldObject = worldObject;
        }

        public void OnCollideObject(WorldObject target)
        {
            if (!PhysicsObj.is_active()) return;

            //Console.WriteLine($"Projectile.OnCollideObject - {WorldObject.Name} ({WorldObject.Guid}) -> {target.Name} ({target.Guid})");

            if (ProjectileTarget == null || ProjectileTarget != target)
            {
                //Console.WriteLine("Unintended projectile target! (should be " + ProjectileTarget.Guid.Full.ToString("X8") + " - " + ProjectileTarget.Name + ")");
                OnCollideEnvironment();
                return;
            }

            // take damage
            var sourceCreature = ProjectileSource as Creature;
            var sourcePlayer = ProjectileSource as Player;
            var targetCreature = target as Creature;

            DamageEvent damageEvent = null;

            if (targetCreature != null)
            {
                if (sourcePlayer != null)
                {
                    // player damage monster or player
                    damageEvent = sourcePlayer.DamageTarget(targetCreature, WorldObject);

                    if (damageEvent != null && damageEvent.HasDamage)
                        WorldObject.EnqueueBroadcast(new GameMessageSound(WorldObject.Guid, Sound.Collision, 1.0f));
                }
                else if (sourceCreature != null && sourceCreature.AttackTarget != null)
                {
                    var targetPlayer = sourceCreature.AttackTarget as Player;

                    damageEvent = DamageEvent.CalculateDamage(sourceCreature, targetCreature, WorldObject);

                    if (targetPlayer != null)
                    {
                        // monster damage player
                        if (damageEvent.HasDamage)
                        {
                            targetPlayer.TakeDamage(sourceCreature, damageEvent);

                            // blood splatter?

                            if (damageEvent.ShieldMod != 1.0f)
                            {
                                var shieldSkill = targetPlayer.GetCreatureSkill(Skill.Shield);
                                Proficiency.OnSuccessUse(targetPlayer, shieldSkill, shieldSkill.Current);   // ??
                            }
                        }
                        else
                            targetPlayer.OnEvade(sourceCreature, CombatType.Missile);
                    }
                    else
                    {
                        // monster damage pet
                        if (damageEvent.HasDamage)
                        {
                            targetCreature.TakeDamage(sourceCreature, damageEvent.DamageType, damageEvent.Damage);

                            // blood splatter?
                        }
                    }
                }

                // handle target procs
                if (damageEvent != null && damageEvent.HasDamage)
                    sourceCreature?.TryProcEquippedItems(targetCreature, false);
            }

            WorldObject.CurrentLandblock?.RemoveWorldObject(WorldObject.Guid, showError: !PhysicsObj.entering_world);
            PhysicsObj.set_active(false);

            WorldObject.HitMsg = true;
        }

        public void OnCollideEnvironment()
        {
            if (!PhysicsObj.is_active()) return;

            // do not send 'Your missile attack hit the environment' messages to player,
            // if projectile is still in the process of spawning into world.
            if (PhysicsObj.entering_world)
                return;

            //Console.WriteLine($"Projectile.OnCollideEnvironment({WorldObject.Name} - {WorldObject.Guid})");

            WorldObject.CurrentLandblock?.RemoveWorldObject(WorldObject.Guid, showError: !PhysicsObj.entering_world);
            PhysicsObj.set_active(false);

            if (ProjectileSource is Player player)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat("Your missile attack hit the environment.", ChatMessageType.Broadcast));
            }
            else if (ProjectileSource is Creature creature)
            {
                creature.MonsterProjectile_OnCollideEnvironment();
            }

            WorldObject.HitMsg = true;
        }
    }
}
