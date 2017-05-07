using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Monster : Creature
    {
        private bool isMoving;

        private double stateTimerStop;

        private Position spawnPoint;

        private StateMachine combatStateMachine = new StateMachine();

        public int CombatCurrentState
        {
            get { return combatStateMachine.CurrentState; }
        }

        public Player TargetPlayer;

        public Monster(AceCreatureStaticLocation aceC)
            : base((ObjectType)aceC.CreatureData.TypeId,
                  new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.Creature),
                  aceC.CreatureData.Name,
                  aceC.WeenieClassId,
                  (ObjectDescriptionFlag)aceC.CreatureData.WdescBitField,
                  (WeenieHeaderFlag)aceC.CreatureData.WeenieFlags,
                  aceC.Position)
        {
            if (aceC.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceC.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceC.WeenieClassId - 0x8000);

            base.SetObjectData(aceC.CreatureData);
            base.SetAbilities(aceC.CreatureData);

            // Set spawnPoint for OnReturnToSpawn
            spawnPoint = aceC.Position;

            // init state machine
            combatStateMachine.Initialise(MonsterRules.GetRules(), MonsterRules.GetInitState());
            this.OnIdleEnter();
        }

        public void OnIdleEnter()
        {
            Console.WriteLine($"{this.Name} is idle now.");

            TargetPlayer = null;
            stateTimerStop = 0;
            isMoving = false;

            OnIdle(null);
        }

        public void OnIdle(List<Player> nearPlayers)
        {
            if (nearPlayers != null)
            {
                float min = 100;
                Player target = null;

                nearPlayers.ForEach(p =>
                {
                    if (p.Location.SquaredDistanceTo(this.Location) < min)
                    {
                        min = p.Location.SquaredDistanceTo(this.Location);
                        target = p;
                    }
                });
                if (target != null)
                {
                    if (combatStateMachine.ChangeState((int)MonsterStates.SensePlayer))
                    {
                        TargetPlayer = target;
                        OnSensePlayer();
                    }
                    else
                        Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.SensePlayer}");
                }
            }
        }

        public void OnSensePlayer()
        {
            Console.WriteLine($"{this.Name} senses player {TargetPlayer.Name}.");

            if (combatStateMachine.ChangeState((int)MonsterStates.EnterCombat))
                OnEnterCombat();
            else
                Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.EnterCombat}");
        }

        public void OnUnderAttack()
        {
            // TODO: Need missile or magic combat implemented first. 
            // Otherwise the monster will sense the approaching player.
        }

        public void OnEnterCombat()
        {
            Console.WriteLine($"{this.Name} enters combat with {TargetPlayer.Name}.");

            // Enter combat stance
            var combatStance = new UniversalMotion(MotionStance.UANoShieldAttack);
            var updateMotion = new GameMessageUpdateMotion(this, combatStance);
            TargetPlayer.Session.Network.EnqueueSend(updateMotion);

            if (this.Location.SquaredDistanceTo(TargetPlayer.Location) < 10)
            {
                // Attack player
                if (combatStateMachine.ChangeState((int)MonsterStates.AttackPlayer))
                    OnAttackPlayerEnter();
                else
                    Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.AttackPlayer}");
            }
            else
            {
                // Move to player
                if (combatStateMachine.ChangeState((int)MonsterStates.MoveToPlayer))
                    OnMoveToPlayerEnter();
                else
                    Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.MoveToPlayer}");
            }
        }

        public void OnMoveToPlayerEnter()
        {
            Console.WriteLine($"{this.Name} is moving towards {TargetPlayer.Name}.");

            isMoving = false;
            stateTimerStop = 0;

            OnMoveToPlayer();
        }

        public void OnMoveToPlayer()
        {
            if (stateTimerStop > WorldManager.PortalYearTicks || stateTimerStop == 0)
            {
                if (!isMoving)
                {
                    var m2p = new UniversalMotion(MotionStance.UANoShieldAttack, TargetPlayer);
                    var moveToPlayer = new GameMessageUpdateMotion(this, TargetPlayer, m2p, MovementTypes.MoveToObject, 100.0f);
                    this.Location = TargetPlayer.Location;
                    var targetPosition = new GameMessageUpdatePosition(this);
                    TargetPlayer.Session.Network.EnqueueSend(moveToPlayer, targetPosition);
                    isMoving = true;
                    stateTimerStop = WorldManager.PortalYearTicks + 10;
                }
            }
            else
            {
                float distance = this.Location.SquaredDistanceTo(TargetPlayer.Location);
                // Check if the monster is close enough yet, if not start another move to player
                if (distance < 10)
                {
                    Console.WriteLine($"{this.Name} is close enough to {TargetPlayer.Name}.");

                    if (combatStateMachine.ChangeState((int)MonsterStates.AttackPlayer))
                        OnAttackPlayerEnter();
                    else
                        Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.AttackPlayer}");
                }
                else
                {
                    if (distance > 40)
                    {
                        Console.WriteLine($"{this.Name} is too far away from {TargetPlayer.Name}.");

                        // Player moved too far away so break combat
                        if (combatStateMachine.ChangeState((int)MonsterStates.ExitCombat))
                            OnExitCombat();
                        else
                            Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.ExitCombat}");
                    }
                    else if (this.Location.SquaredDistanceTo(spawnPoint) > 60)
                    {
                        Console.WriteLine($"{this.Name} is too far away from spawn point.");

                        // Monster is too far from spawn, so break combat
                        if (combatStateMachine.ChangeState((int)MonsterStates.ExitCombat))
                            OnExitCombat();
                        else
                            Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.ExitCombat}");
                    }
                    else
                    {
                        Console.WriteLine($"{this.Name} needs to follow {TargetPlayer.Name}.");

                        // It's still worth hunting the player, so move again
                        OnMoveToPlayerEnter();
                    }
                }
            }
        }

        public void OnAttackPlayerEnter()
        {
            Console.WriteLine($"{this.Name} attacks {TargetPlayer.Name}.");

            isMoving = false;
            stateTimerStop = 0;

            OnAttackPlayer();
        }

        public void OnAttackPlayer()
        {
            if (stateTimerStop < WorldManager.PortalYearTicks)
            {
                Random random = new Random((int)WorldManager.PortalYearTicks);
                uint damageAmount = ExecuteMeleeAttack();

                if (damageAmount < TargetPlayer.Health.Current)
                {
                    Console.WriteLine($"{this.Name} has hit {TargetPlayer.Name} for {damageAmount} damage.");
                    TargetPlayer.Health.Current -= damageAmount;
                    var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(TargetPlayer.Session, Vital.Health, TargetPlayer.Health.Current);
                    TargetPlayer.Session.Network.EnqueueSend(msgHealthUpdate);
                }
                else
                {
                    Console.WriteLine($"{this.Name} has killed {TargetPlayer.Name}.");

                    TargetPlayer.Health.Current = 0;
                    var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(TargetPlayer.Session, Vital.Health, TargetPlayer.Health.Current);
                    TargetPlayer.Session.Network.EnqueueSend(msgHealthUpdate);
                    TargetPlayer.OnKill(TargetPlayer.Session);

                    stateTimerStop = 0;
                    if (combatStateMachine.ChangeState((int)MonsterStates.ExitCombat))
                        OnExitCombat();
                    else
                        Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.ExitCombat}");
                }

                stateTimerStop = WorldManager.PortalYearTicks + random.Next(2, 4);
            }
        }

        public void OnExitCombat()
        {
            Console.WriteLine($"{this.Name} stops Combat.");

            if (combatStateMachine.ChangeState((int)MonsterStates.ReturnToSpawn))
                OnReturnToSpawnEnter();
            else
                Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.ReturnToSpawn}");
        }

        public void OnReturnToSpawnEnter()
        {
            Console.WriteLine($"{this.Name} is now returning to spawn point.");

            isMoving = false;
            stateTimerStop = 0;

            OnReturnToSpawn();
        }

        public void OnReturnToSpawn()
        {
            if (stateTimerStop > WorldManager.PortalYearTicks || stateTimerStop == 0)
            {
                if (!isMoving)
                {
                    var m2p = new UniversalMotion(MotionStance.Standing, spawnPoint);
                    var moveToPosition = new GameMessageUpdateMotion(this, spawnPoint, m2p, MovementTypes.MoveToPosition, 100.0f);
                    this.Location = spawnPoint;
                    var targetPosition = new GameMessageUpdatePosition(this);
                    TargetPlayer.Session.Network.EnqueueSend(moveToPosition, targetPosition);
                    isMoving = true;
                    stateTimerStop = WorldManager.PortalYearTicks + 10;
                }
            }
            else
            {
                Console.WriteLine($"{this.Name} has reached it's spawn point.");

                isMoving = false;
                stateTimerStop = 0;
                if (combatStateMachine.ChangeState((int)MonsterStates.Idle))
                    this.OnIdleEnter();
                else
                    Console.WriteLine($"Error changing from state {combatStateMachine.CurrentState} to state {MonsterStates.Idle}");
            }
        }

        private uint ExecuteMeleeAttack()
        {
            Random random = new Random((int)WorldManager.PortalYearTicks);
            uint damageAmount = GetDamageAmount(random);
            double severity = TargetPlayer.Health.Current / damageAmount;
            bool critical = GetCritical(random);
            ulong attackConditions = 0; // TODO: take recklessness, dirty fighting, sneak attack into account

            // Attack animation
            var attackMotionItem = new MotionItem(MotionCommand.AimHigh75);
            var attackMotion = new UniversalMotion(MotionStance.UANoShieldAttack, attackMotionItem);
            attackMotion.HasTarget = true;
            attackMotion.MovementData.CurrentStyle = (ushort)MotionStance.UANoShieldAttack;
            var updateMotion = new GameMessageUpdateMotion(this, attackMotion);
            TargetPlayer.Session.Network.EnqueueSend(updateMotion);

            // SoundEvent: sound 48 (HitFlesh1), volume 0,5
            TargetPlayer.Session.Player.ActionApplySoundEffect(Sound.HitFlesh1, TargetPlayer.Guid);

            // SoundEvent: sound 14 (Wound3), volume 1
            TargetPlayer.Session.Player.ActionApplySoundEffect(Sound.Wound3, TargetPlayer.Guid);

            // Showing the hit on the player
            var effectEvent = new GameMessageScript(TargetPlayer.Guid, Network.Enum.PlayScript.SplatterMidRightFront);
            TargetPlayer.Session.Network.EnqueueSend(effectEvent);

            // Sending the melee hit message                
            var receiveDamageEvent = new GameEventReceiveMeleeDamage(TargetPlayer.Session, this.Name, DamageType.Bludgeoning, severity, damageAmount, critical, DamageLocation.Chest, attackConditions);
            TargetPlayer.Session.Network.EnqueueSend(receiveDamageEvent);

            // Test for InflictDamage when Player hits Monster
            // var inflictDamageEvent = new GameEventInflictMeleeDamage(TargetPlayer.Session, this.Name, DamageType.Bludgeoning, severity, damageAmount, critical, attackConditions);
            // TargetPlayer.Session.Network.EnqueueSend(inflictDamageEvent);

            return damageAmount;
        }

        private uint GetDamageAmount(Random random)
        {
            // TODO: some proper damage calculation

            return (uint)random.Next(1, 3);
        }

        private bool GetCritical(Random random)
        {
            // TODO: proper calculation of the critical rating once items and buffs are in
            // For now it's the base 10% chance

            if (random.Next(1, 100) < 11)
                return true;
            else
                return false;
        }
    }
}
