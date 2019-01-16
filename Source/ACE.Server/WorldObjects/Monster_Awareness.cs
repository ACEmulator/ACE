using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;

namespace ACE.Server.WorldObjects
{
    public enum AggroType
    {
        Nearest,
        Random,
        LowestLevel,
        TopDamager
    };

    /// <summary>
    /// Determines when a monster wakes up from idle state
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// Determines when a monster will attack
        /// </summary>
        [Flags]
        public enum Tolerance
        {
            None        = 0,  // attack targets in range
            NoAttack    = 1,  // never attack
            ID          = 2,  // attack when ID'd or attacked
            Unknown     = 4,  // unused?
            Provoke     = 8,  // used in conjunction with 32
            Unknown2    = 16, // unused?
            Target      = 32, // only target original attacker
            Retaliate   = 64  // only attack after attacked
        };

        public AggroType AggroType = AggroType.Nearest;

        /// <summary>
        /// Determines when a monster wakes up from idle state
        /// </summary>
        public const float RadiusAwareness = 35.0f;
        public const float RadiusAwarenessSquared = RadiusAwareness * RadiusAwareness;

        /// <summary>
        /// Monsters wake up when players are in visual range
        /// </summary>
        public bool IsAwake = false;

        /// <summary>
        /// Transitions a monster from idle to awake state
        /// </summary>
        public void WakeUp(bool alertNearby = true)
        {
            MonsterState = State.Awake;
            IsAwake = true;
            //DoAttackStance();
            EmoteManager.OnAttack(AttackTarget as Creature);

            if (alertNearby)
                AlertFriendly();
        }

        /// <summary>
        /// Transitions a monster from awake to idle state
        /// </summary>
        public virtual void Sleep()
        {
            if (DebugMove)
                Console.WriteLine($"{Name}.Sleep()");

            AttackTarget = null;
            IsAwake = false;
            IsMoving = false;
            MonsterState = State.Idle;
        }

        public double NextFindTarget;

        public virtual void HandleFindTarget()
        {
            return;

            /*
            var currentTime = Timers.RunningTime;

            if (currentTime < NextFindTarget)
                return;

            // if nearest or random, make sure we aren't chasing target
            if (AggroType == AggroType.Nearest || AggroType == AggroType.Random)
            {
                if (IsTurning || IsMoving)
                    return;
            }
            FindNextTarget();
            */
        }

        public void SetNextTargetTime()
        {
            // rng?
            NextFindTarget = Timers.RunningTime + 10.0f;
        }

        public virtual bool FindNextTarget()
        {
            SetNextTargetTime();

            // rebuild visible objects (handle this better for monsters)
            GetVisibleObjects();

            var players = GetAttackablePlayers();
            if (players.Count == 0)
                return false;

            switch (AggroType)
            {
                case AggroType.Nearest:

                    var nearest = BuildTargetDistance(players);
                    AttackTarget = nearest[0].Target;
                    break;

                case AggroType.Random:
                    var rng = ThreadSafeRandom.Next(0, players.Count - 1);
                    AttackTarget = players[rng];
                    break;

                case AggroType.LowestLevel:

                    // should probably shuffle the list beforehand,
                    // in case a bunch of levels of same level are in a group,
                    // so the same player isn't always selected
                    var lowest = players.OrderBy(p => p.Level).FirstOrDefault();
                    AttackTarget = lowest;
                    break;

                case AggroType.TopDamager:
                    AttackTarget = DamageHistory.TopDamager;
                    break;
            }
            //Console.WriteLine($"{Name}.FindNextTarget = {AttackTarget.Name}");
            return AttackTarget != null;
        }

        /// <summary>
        /// Returns a list of attackable players in this monster's visible objects table
        /// </summary>
        public List<Creature> GetAttackablePlayers()
        {
            // TODO: this might need refreshed
            var visibleObjs = PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            var players = new List<Creature>();

            foreach (var obj in visibleObjs)
            {
                // exclude self (should hopefully not be in this list)
                if (PhysicsObj == obj) continue;

                // ensure creature
                var wo = obj.WeenieObj.WorldObject;
                var creature = wo as Creature;
                if (creature == null) continue;

                // ensure player or player's pet
                if (!(wo is Player) && !(wo is CombatPet)) continue;

                // ensure attackable
                var attackable = creature.GetProperty(PropertyBool.Attackable) ?? false;
                if (!attackable) continue;

                players.Add(creature);
            }
            return players;
        }

        /// <summary>
        /// Returns the list of potential attack targets, sorted by closest distance 
        /// </summary>
        public List<TargetDistance> BuildTargetDistance(List<Creature> targets)
        {
            var targetDistance = new List<TargetDistance>();

            var curPos = Location.ToGlobal();

            foreach (var target in targets)
                targetDistance.Add(new TargetDistance(target, Vector3.DistanceSquared(curPos, target.Location.ToGlobal())));

            return targetDistance.OrderBy(i => i.Distance).ToList();
        }

        /// <summary>
        /// Rebuilds the visible objects tables for this monster
        /// </summary>
        public void GetVisibleObjects()
        {
            PhysicsObj.ObjMaint.RemoveAllObjects();
            PhysicsObj.handle_visible_cells();
        }

        public double? VisualAwarenessRange
        {
            get => GetProperty(PropertyFloat.VisualAwarenessRange);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.VisualAwarenessRange); else SetProperty(PropertyFloat.VisualAwarenessRange, value.Value); }
        }

        public double? AuralAwarenessRange
        {
            get => GetProperty(PropertyFloat.AuralAwarenessRange);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.AuralAwarenessRange); else SetProperty(PropertyFloat.AuralAwarenessRange, value.Value); }
        }

        /// <summary>
        /// Monsters can only alert other monsters once?
        /// </summary>
        public bool Alerted = false;

        public static float AlertRadius = 12.0f;    // TODO: find alert radius from retail
        public static float AlertRadiusSq = AlertRadius * AlertRadius;

        public void AlertFriendly()
        {
            if (Alerted) return;

            var visibleObjs = PhysicsObj.ObjMaint.GetVisibleObjects(PhysicsObj.CurCell);

            foreach (var obj in visibleObjs)
            {
                var nearbyCreature = obj.WeenieObj.WorldObject as Creature;
                if (nearbyCreature == null || nearbyCreature.IsAwake/* || nearbyCreature.IsAlerted*/ || !(nearbyCreature.GetProperty(PropertyBool.Attackable) ?? false))
                    continue;

                if (CreatureType != null && CreatureType == nearbyCreature.CreatureType ||
                      FriendType != null && FriendType == nearbyCreature.CreatureType)
                {
                    // clamp radius if outdoors
                    /*if ((Location.Cell & 0xFFFF) < 0x100)
                    {
                        var distSq = Vector3.DistanceSquared(Location.ToGlobal(), nearbyCreature.Location.ToGlobal());
                        if (distSq > AlertRadiusSq)
                            continue;
                    }*/

                    var dist = Vector3.Distance(Location.ToGlobal(), nearbyCreature.Location.ToGlobal());
                    if (dist > (nearbyCreature.VisualAwarenessRange ?? AlertRadius))
                        continue;

                    Alerted = true;
                    nearbyCreature.AttackTarget = AttackTarget;
                    nearbyCreature.WakeUp(false);
                }
            }
        }
    }
}
