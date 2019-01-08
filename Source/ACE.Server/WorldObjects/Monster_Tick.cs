using System;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        private static readonly float monsterTickInterval = 0.2f;

        private bool FirstUpdate = true;

        /// <summary>
        /// Primary dispatch for monster think
        /// </summary>
        private void Monster_Tick()
        {
            if (!IsAwake || IsDead) return;

            IsMonster = true;

            HandleFindTarget();

            CheckMissHome();    // tickrate?

            if (AttackTarget == null && MonsterState != State.Return)
            {
                Sleep();
                return;
            }

            var pet = this as CombatPet;
            if (pet != null && DateTime.UtcNow >= pet.ExpirationTime)
            {
                Destroy();
                return;
            }

            if (MonsterState == State.Return)
            {
                Movement();
                EnqueueNextMonsterTick();
                return;
            }

            var creatureTarget = AttackTarget as Creature;
            if (creatureTarget != null && (creatureTarget.IsDead || (pet == null && !creatureTarget.IsVisible(this))))
            {
                Sleep();
                return;
            }

            if (FirstUpdate)
            {
                if (CurrentMotionState.Stance == MotionStance.NonCombat)
                    DoAttackStance();

                if (IsAnimating)
                {
                    //PhysicsObj.ShowPendingMotions();
                    PhysicsObj.update_object();
                    EnqueueNextMonsterTick();
                    return;
                }

                FirstUpdate = false;
            }

            // select a new weapon if missile launcher is out of ammo
            var weapon = GetEquippedWeapon();
            if (weapon != null && weapon.IsAmmoLauncher)
            {
                var ammo = GetEquippedAmmo();
                if (ammo == null)
                {
                    TryDequipObjectWithBroadcasting(weapon.Guid, out _, out _);
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
                CurrentAttack = GetAttackType();
                MaxRange = GetMaxRange();
            }

            // get distance to target
            var targetDist = GetDistanceToTarget();

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
                    EnqueueNextMonsterTick();
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

            EnqueueNextMonsterTick();
        }

        public void EnqueueNextMonsterTick()
        {
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(monsterTickInterval);
            actionChain.AddAction(this, Monster_Tick);
            actionChain.EnqueueChain();
        }
    }
}
