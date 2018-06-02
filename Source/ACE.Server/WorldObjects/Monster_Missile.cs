using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameMessages.Messages;
using System;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// The delay between missile attacks (todo: find actual value)
        /// </summary>
        public static readonly float MissileDelay = 2.5f;

        /// <summary>
        /// Returns TRUE if monster has physical ranged attacks
        /// </summary>
        public bool IsRanged
        {
            get
            {
                var weapon = GetEquippedWeapon();
                return weapon != null && weapon is MissileLauncher;
            }
        }

        /// <summary>
        /// Returns TRUE if monster is wielding a bow or crossbow
        /// </summary>
        public bool IsBow
        {
            get
            {
                var weapon = GetEquippedWeapon();
                if (weapon == null) return false;

                var combatStyle = weapon.DefaultCombatStyle;
                return combatStyle == CombatStyle.Bow || combatStyle == CombatStyle.Crossbow;
            }
        }

        /// <summary>
        /// Gives default ranged ammo if none in wielded treasure
        /// </summary>
        public void GiveAmmo()
        {
            var ammo = GetEquippedAmmo();

            if (ammo != null) return;

            ammo = WorldObjectFactory.CreateNewWorldObject(300);
            TryEquipObject(ammo, (int)EquipMask.MissileAmmo);

            SetChild(ammo, (int)ammo.CurrentWieldedLocation, out var placementId, out var parentLocation);
        }

        /// <summary>
        /// Launches a missile attack from monster to target
        /// </summary>
        public void RangeAttack()
        {
            var dist = GetDistanceToTarget();
            //Console.WriteLine("RangeAttack: " + dist);

            NextAttackTime = Timer.CurrentTime + MissileDelay;

            var weapon = GetEquippedWeapon();
            var sound = weapon.DefaultCombatStyle == CombatStyle.Crossbow ? Sound.CrossbowRelease : Sound.BowRelease;
            CurrentLandblock.EnqueueBroadcast(Location, new GameMessageSound(Guid, sound, 1.0f));

            var player = AttackTarget as Player;
            var bodyPart = GetBodyPart();

            float targetTime = 0.0f;
            var damageSource = LaunchProjectile(AttackTarget);
            var animLength = ReloadMotion();

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(targetTime);
            actionChain.AddAction(this, () =>
            {
                var critical = false;
                var damageType = DamageType.Undef;
                var damage = CalculateDamage(ref damageType, bodyPart, ref critical);

                if (damage > 0.0f)
                    player.TakeDamage(this, damageType, damage, bodyPart, critical);
                else
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You evaded {Name}!", ChatMessageType.CombatEnemy));
            });

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Returns missile base damage from a monster attack
        /// </summary>
        public Range GetMissileDamage()
        {
            var ammo = GetEquippedAmmo();

            return ammo.GetDamageMod(this);
        }
    }
}
