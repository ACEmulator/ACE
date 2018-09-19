using System;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        private static readonly TimeSpan monsterTickInterval = TimeSpan.FromMilliseconds(200);

        private DateTime lastMonsterTick;

        /// <summary>
        /// Primary dispatch for monster think
        /// </summary>
        private void Monster_Tick(double lastTickDuration, double currentUnixTime)
        {
            if (lastMonsterTick + monsterTickInterval > DateTime.UtcNow)
                return;

            lastMonsterTick = DateTime.UtcNow;

            if (!IsAwake || IsDead) return;

            IsMonster = true;

            if (AttackTarget != null && AttackTarget.IsDestroyed)
            {
                Sleep();
                return;
            }

            if (!AttackTarget.IsVisible(this))
            {
                Sleep();
                return;
            }

            // select a new weapon if missile launcher is out of ammo
            var weapon = GetEquippedWeapon();
            if (weapon != null && weapon.IsAmmoLauncher)
            {
                var ammo = GetEquippedAmmo();
                if (ammo == null)
                {
                    TryDequipObject(weapon.Guid);
                    EquipWieldedTreasure(true);
                    DoAttackStance();
                    CurrentAttack = null;
                }
            }
            if (weapon == null && CurrentAttack != null && CurrentAttack == AttackType.Missile)
            {
                EquipWieldedTreasure(true);
                DoAttackStance();
                CurrentAttack = null;
            }

            // decide current type of attack
            if (CurrentAttack == null)
            {
                CurrentAttack = GetAttackType();
                MaxRange = GetMaxRange();

                if (CurrentAttack == AttackType.Magic)
                    MaxRange = MaxMeleeRange;   // FIXME: server position sync
            }

            // get distance to target
            var targetDist = GetDistanceToTarget();
            //Console.WriteLine($"{Name} ({Guid}) - Dist: {targetDist}");

            if (Sticky)
                UpdatePosition();

            if (CurrentAttack != AttackType.Missile)
            {
                if (targetDist > MaxRange || !IsFacing(AttackTarget))
                {
                    // turn / move towards
                    if (!IsTurning && !IsMoving)
                        StartTurn();
                    else
                        Movement();
                }
                else
                {
                    // perform attack
                    if (AttackReady())
                        Attack();
                }
            }
            else
            {
                if (IsTurning || IsMoving)
                {
                    Movement();
                    return;
                }

                if (!IsFacing(AttackTarget))
                {
                    StartTurn();
                }
                else if (targetDist <= MaxRange)
                {
                    // perform attack
                    if (AttackReady())
                        Attack();
                }
            }
        }
    }
}
