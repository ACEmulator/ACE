using System;

using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        private const double monsterTickInterval = 0.2;

        public double NextMonsterTickTime;

        private bool firstUpdate = true;

        /// <summary>
        /// Primary dispatch for monster think
        /// </summary>
        public void Monster_Tick(double currentUnixTime)
        {
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;

            if (!IsAwake || IsDead) return;

            IsMonster = true;

            HandleFindTarget();

            CheckMissHome();    // tickrate?

            var pet = this as CombatPet;
            if (pet != null && DateTime.UtcNow >= pet.ExpirationTime)
            {
                Destroy();
                return;
            }

            if (AttackTarget == null && MonsterState != State.Return)
            {
                Sleep();
                return;
            }

            if (MonsterState == State.Return)
            {
                Movement();
                return;
            }

            var creatureTarget = AttackTarget as Creature;
            if (creatureTarget != null && (creatureTarget.IsDead || (pet == null && !creatureTarget.IsVisible(this))))
            {
                Sleep();
                return;
            }

            if (firstUpdate)
            {
                if (CurrentMotionState.Stance == MotionStance.NonCombat)
                    DoAttackStance();

                if (IsAnimating)
                {
                    //PhysicsObj.ShowPendingMotions();
                    PhysicsObj.update_object();
                    return;
                }

                firstUpdate = false;
            }

            // select a new weapon if missile launcher is out of ammo
            var weapon = GetEquippedWeapon();
            if (weapon != null && weapon.IsAmmoLauncher)
            {
                var ammo = GetEquippedAmmo();
                if (ammo == null)
                {
                    TryUnwieldObjectWithBroadcasting(weapon.Guid, out _, out _);
                    EquipInventoryItems(true);
                    DoAttackStance();
                    CurrentAttack = null;
                }
            }
            if (weapon == null && CurrentAttack != null && CurrentAttack == CombatType.Missile)
            {
                EquipInventoryItems(true);
                DoAttackStance();
                CurrentAttack = null;
            }

            // decide current type of attack
            if (CurrentAttack == null)
            {
                CurrentAttack = GetNextAttackType();
                MaxRange = GetMaxRange();

                //if (CurrentAttack == AttackType.Magic)
                //MaxRange = MaxMeleeRange;   // FIXME: server position sync
            }

            // get distance to target
            var targetDist = GetDistanceToTarget();
            //Console.WriteLine($"{Name} ({Guid}) - Dist: {targetDist}");

            if (Sticky)
                UpdatePosition();

            if (CurrentAttack != CombatType.Missile)
            {
                if (targetDist > MaxRange || (!IsFacing(AttackTarget) && !IsSelfCast()))
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

            // pets drawing aggro
            if (pet != null)
                pet.PetCheckMonsters();
        }
    }
}
