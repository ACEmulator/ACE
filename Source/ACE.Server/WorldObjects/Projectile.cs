using System;

using ACE.Entity.Enum;
using ACE.Server.Entity;
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

            if (targetCreature != null)
            {
                if (sourcePlayer != null)
                {
                    // player damage monster
                    var damageEvent = sourcePlayer.DamageTarget(targetCreature, WorldObject);

                    if (damageEvent != null && damageEvent.Damage > 0)
                        sourcePlayer.Session.Network.EnqueueSend(new GameMessageSound(WorldObject.Guid, Sound.Collision, 1.0f));    // todo: landblock broadcast?
                }
                else if (sourceCreature != null && sourceCreature.AttackTarget != null)
                {
                    var targetPlayer = sourceCreature.AttackTarget as Player;
                    var bodyPart = BodyParts.GetBodyPart(sourceCreature.AttackHeight.Value);

                    var damageEvent = DamageEvent.CalculateDamage(sourceCreature, targetCreature, WorldObject);

                    //var damage = sourceCreature.CalculateDamage(ref damageType, null, bodyPart, ref critical, ref shieldMod);

                    if (targetPlayer != null)
                    {
                        // monster damage player
                        if (damageEvent.HasDamage)
                        {
                            targetPlayer.TakeDamage(sourceCreature, damageEvent.DamageType, damageEvent.Damage, damageEvent.BodyPart, damageEvent.IsCritical);

                            // blood splatter?

                            if (damageEvent.ShieldMod != 1.0f)
                            {
                                var shieldSkill = targetPlayer.GetCreatureSkill(Skill.Shield);
                                Proficiency.OnSuccessUse(targetPlayer, shieldSkill, shieldSkill.Current);   // ??
                            }
                        }
                        else
                        {
                            if (!targetPlayer.UnderLifestoneProtection)
                                targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You evaded {sourceCreature.Name}!", ChatMessageType.CombatEnemy));

                            Proficiency.OnSuccessUse(targetPlayer, targetPlayer.GetCreatureSkill(Skill.MissileDefense), sourceCreature.GetCreatureSkill(sourceCreature.GetCurrentAttackSkill()).Current);
                        }
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
