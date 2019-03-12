using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Determines when a monster wakes up from idle state
    /// </summary>
    partial class Creature
    {
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
            //SelectTargetingTactic();

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

        /// <summary>
        /// This list of possible targeting tactics for this monster
        /// </summary>
        public TargetingTactic TargetingTactic
        {
            get => (TargetingTactic)(GetProperty(PropertyInt.TargetingTactic) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.TargetingTactic); else SetProperty(PropertyInt.TargetingTactic, (int)TargetingTactic); }
        }

        /// <summary>
        /// The current targeting tactic for this monster
        /// </summary>
        public TargetingTactic CurrentTargetingTactic;

        public void SelectTargetingTactic()
        {
            // monsters have multiple targeting tactics, ex. Focused | Random

            // when should this function be called?
            // when a monster spawns in, does it choose 1 TargetingTactic?

            // or do they randomly select a TargetingTactic from their list of possible tactics,
            // each time they go to find a new target?

            //Console.WriteLine($"{Name}.TargetingTactics: {TargetingTactic}");

            var possibleTactics = EnumHelper.GetFlags(TargetingTactic);
            var rng = ThreadSafeRandom.Next(1, possibleTactics.Count - 1);

            if (TargetingTactic == 0)
                rng = 0;

            CurrentTargetingTactic = (TargetingTactic)possibleTactics[rng];

            //Console.WriteLine($"{Name}.TargetingTactic: {CurrentTargetingTactic}");
        }

        public double NextFindTarget;

        public virtual void HandleFindTarget()
        {
            if (Timers.RunningTime < NextFindTarget)
                return;

            FindNextTarget();
        }

        public void SetNextTargetTime()
        {
            // use rng?

            //var rng = ThreadSafeRandom.Next(5.0f, 10.0f);
            var rng = 5.0f;

            NextFindTarget = Timers.RunningTime + rng;
        }

        public virtual bool FindNextTarget()
        {
            // rebuild visible objects (handle this better for monsters)
            GetVisibleObjects();

            var players = GetAttackablePlayers();
            if (players.Count == 0)
                return false;

            // Generally, a creature chooses whom to attack based on:
            //  - who it was last attacking,
            //  - who attacked it last,
            //  - or who caused it damage last.

            // When players first enter the creature's detection radius, however, none of these things are useful yet,
            // so the creature chooses a target randomly, weighted by distance.

            // Players within the creature's detection sphere are weighted by how close they are to the creature --
            // the closer you are, the more chance you have to be selected to be attacked.

            SelectTargetingTactic();
            SetNextTargetTime();

            switch (CurrentTargetingTactic)
            {
                case TargetingTactic.None:

                    Console.WriteLine($"{Name}.FindNextTarget(): TargetingTactic.None");
                    break;  // same as focused?

                case TargetingTactic.Random:

                    // this is a very common tactic with monsters,
                    // although it is not truly random, it is weighted by distance
                    var targetDistances = BuildTargetDistance(players);
                    AttackTarget = SelectWeightedDistance(targetDistances);
                    break;

                case TargetingTactic.Focused:

                    break;  // always stick with original target?

                case TargetingTactic.LastDamager:

                    var lastDamager = DamageHistory.LastDamager;
                    if (lastDamager != null)
                        AttackTarget = lastDamager;
                    break;

                case TargetingTactic.TopDamager:

                    var topDamager = DamageHistory.TopDamager;
                    if (topDamager != null)
                        AttackTarget = topDamager;
                    break;

                // these below don't seem to be used in PY16 yet...

                case TargetingTactic.Weakest:

                    // should probably shuffle the list beforehand,
                    // in case a bunch of levels of same level are in a group,
                    // so the same player isn't always selected
                    var lowestLevel = players.OrderBy(p => p.Level).FirstOrDefault();
                    AttackTarget = lowestLevel;
                    break;

                case TargetingTactic.Strongest:

                    var highestLevel = players.OrderByDescending(p => p.Level).FirstOrDefault();
                    AttackTarget = highestLevel;
                    break;

                case TargetingTactic.Nearest:

                    var nearest = BuildTargetDistance(players);
                    AttackTarget = nearest[0].Target;
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

                // ensure player or player's pet
                var wo = obj.WeenieObj.WorldObject;
                if (!(wo is Player) && !(wo is CombatPet)) continue;
                var creature = wo as Creature;

                // ensure attackable
                var attackable = creature.GetProperty(PropertyBool.Attackable) ?? false;
                if (!attackable) continue;

                // ensure within 'detection radius' ?
                if (Location.SquaredDistanceTo(creature.Location) >= RadiusAwarenessSquared)
                    continue;

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

            foreach (var target in targets)
                targetDistance.Add(new TargetDistance(target, Location.DistanceTo(target.Location)));

            return targetDistance.OrderBy(i => i.Distance).ToList();
        }

        /// <summary>
        /// Uses weighted RNG selection by distance to select a target
        /// </summary>
        public Creature SelectWeightedDistance(List<TargetDistance> targetDistances)
        {
            if (targetDistances.Count == 1)
                return targetDistances[0].Target;

            // http://asheron.wikia.com/wiki/Wi_Flag

            var distSum = targetDistances.Select(i => i.Distance).Sum();

            // get the sum of the inverted ratios
            var invRatioSum = targetDistances.Count - 1;

            // roll between 0 - invRatioSum here,
            // instead of 0-1 (the source of the original wi bug)
            var rng = ThreadSafeRandom.Next(0.0f, invRatioSum);

            // walk the list
            var invRatio = 0.0f;
            foreach (var targetDistance in targetDistances)
            {
                invRatio += 1.0f - (targetDistance.Distance / distSum);

                if (rng <= invRatio)
                    return targetDistance.Target;
            }
            // precision error?
            Console.WriteLine($"{Name}.SelectWeightedDistance: couldn't find target: {string.Join(",", targetDistances.Select(i => i.Distance))}");
            return targetDistances[0].Target;
        }

        /// <summary>
        /// Rebuilds the visible objects tables for this monster
        /// </summary>
        public void GetVisibleObjects()
        {
            PhysicsObj.ObjMaint.RemoveAllObjects();
            PhysicsObj.handle_visible_cells();
        }

        /// <summary>
        /// Called when a monster is first spawning in
        /// </summary>
        public void CheckPlayers()
        {
            var attackable = Attackable ?? false;
            var tolerance = (Tolerance)(GetProperty(PropertyInt.Tolerance) ?? 0);

            if (!attackable && TargetingTactic == 0 || tolerance != Tolerance.None)
                return;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(0.75f);
            actionChain.AddAction(this, CheckPlayers_Inner);
            actionChain.EnqueueChain();
        }

        public void CheckPlayers_Inner()
        { 
            var visiblePlayers = PhysicsObj.ObjMaint.VoyeurTable.Values;

            Player closestPlayer = null;
            var closestDistSq = float.MaxValue;

            foreach (var visiblePlayer in visiblePlayers)
            {
                var player = visiblePlayer.WeenieObj.WorldObject as Player;
                if (player == null || !player.IsAttackable || (player.Hidden ?? false)) continue;

                var distSq = Location.SquaredDistanceTo(player.Location);
                if (distSq < closestDistSq)
                {
                    closestDistSq = distSq;
                    closestPlayer = player;
                }
            }
            if (closestPlayer == null || closestDistSq > RadiusAwarenessSquared)
                return;

            closestPlayer.AlertMonster(this);
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
                    var dist = Location.DistanceTo(nearbyCreature.Location);
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
