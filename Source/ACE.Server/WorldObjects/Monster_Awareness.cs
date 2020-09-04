using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Determines when a monster wakes up from idle state
    /// </summary>
    partial class Creature
    {
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
            EmoteManager.OnWakeUp(AttackTarget as Creature);
            EmoteManager.OnNewEnemy(AttackTarget as Creature);
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
                Console.WriteLine($"{Name} ({Guid}).Sleep()");

            SetCombatMode(CombatMode.NonCombat);

            CurrentAttack = null;
            firstUpdate = true;
            AttackTarget = null;
            IsAwake = false;
            IsMoving = false;
            MonsterState = State.Idle;

            PhysicsObj.CachedVelocity = Vector3.Zero;

            if (RetaliateTargets != null)
                RetaliateTargets.Clear();
        }

        public Tolerance Tolerance
        {
            get => (Tolerance)(GetProperty(PropertyInt.Tolerance) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.Tolerance); else SetProperty(PropertyInt.Tolerance, (int)value); }
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

            // if targeting tactic is none,
            // use the most common targeting tactic
            // TODO: ensure all monsters in the db have a targeting tactic
            var targetingTactic = TargetingTactic;
            if (targetingTactic == TargetingTactic.None)
                targetingTactic = TargetingTactic.Random | TargetingTactic.TopDamager;

            var possibleTactics = EnumHelper.GetFlags(targetingTactic);
            var rng = ThreadSafeRandom.Next(1, possibleTactics.Count - 1);

            if (targetingTactic == 0)
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
            stopwatch.Restart();

            try
            {
                SelectTargetingTactic();
                SetNextTargetTime();

                var visibleTargets = GetAttackTargets();
                if (visibleTargets.Count == 0)
                {
                    if (MonsterState != State.Return)
                        MoveToHome();

                    return false;
                }

                // Generally, a creature chooses whom to attack based on:
                //  - who it was last attacking,
                //  - who attacked it last,
                //  - or who caused it damage last.

                // When players first enter the creature's detection radius, however, none of these things are useful yet,
                // so the creature chooses a target randomly, weighted by distance.

                // Players within the creature's detection sphere are weighted by how close they are to the creature --
                // the closer you are, the more chance you have to be selected to be attacked.

                var prevAttackTarget = AttackTarget;

                switch (CurrentTargetingTactic)
                {
                    case TargetingTactic.None:

                        Console.WriteLine($"{Name}.FindNextTarget(): TargetingTactic.None");
                        break; // same as focused?

                    case TargetingTactic.Random:

                        // this is a very common tactic with monsters,
                        // although it is not truly random, it is weighted by distance
                        var targetDistances = BuildTargetDistance(visibleTargets);
                        AttackTarget = SelectWeightedDistance(targetDistances);
                        break;

                    case TargetingTactic.Focused:

                        break; // always stick with original target?

                    case TargetingTactic.LastDamager:

                        var lastDamager = DamageHistory.LastDamager?.TryGetAttacker() as Creature;
                        if (lastDamager != null)
                            AttackTarget = lastDamager;
                        break;

                    case TargetingTactic.TopDamager:

                        var topDamager = DamageHistory.TopDamager?.TryGetAttacker() as Creature;
                        if (topDamager != null)
                            AttackTarget = topDamager;
                        break;

                    // these below don't seem to be used in PY16 yet...

                    case TargetingTactic.Weakest:

                        // should probably shuffle the list beforehand,
                        // in case a bunch of levels of same level are in a group,
                        // so the same player isn't always selected
                        var lowestLevel = visibleTargets.OrderBy(p => p.Level).FirstOrDefault();
                        AttackTarget = lowestLevel;
                        break;

                    case TargetingTactic.Strongest:

                        var highestLevel = visibleTargets.OrderByDescending(p => p.Level).FirstOrDefault();
                        AttackTarget = highestLevel;
                        break;

                    case TargetingTactic.Nearest:

                        var nearest = BuildTargetDistance(visibleTargets);
                        AttackTarget = nearest[0].Target;
                        break;
                }

                //Console.WriteLine($"{Name}.FindNextTarget = {AttackTarget.Name}");

                if (AttackTarget != null && AttackTarget != prevAttackTarget)
                    EmoteManager.OnNewEnemy(AttackTarget);

                return AttackTarget != null;
            }
            finally
            {
                ServerPerformanceMonitor.AddToCumulativeEvent(ServerPerformanceMonitor.CumulativeEventHistoryType.Monster_Awareness_FindNextTarget, stopwatch.Elapsed.TotalSeconds);
            }
        }

        /// <summary>
        /// Returns a list of attackable targets currently visible to this monster
        /// </summary>
        public List<Creature> GetAttackTargets()
        {
            var visibleTargets = new List<Creature>();

            foreach (var creature in PhysicsObj.ObjMaint.GetVisibleTargetsValuesOfTypeCreature())
            {
                // ensure attackable
                if (!creature.Attackable || creature.Teleporting) continue;

                // ensure within 'detection radius' ?
                var chaseDistSq = creature == AttackTarget ? MaxChaseRangeSq : VisualAwarenessRangeSq;

                /*if (Location.SquaredDistanceTo(creature.Location) > chaseDistSq)
                    continue;*/

                if (PhysicsObj.get_distance_sq_to_object(creature.PhysicsObj, true) > chaseDistSq)
                    continue;

                // if this monster belongs to a faction,
                // ensure target does not belong to the same faction, or they have been provoked
                if (Faction1Bits != null && creature.Faction1Bits != null && (Faction1Bits & creature.Faction1Bits) != 0)
                {
                    if (RetaliateTargets != null && !RetaliateTargets.Contains(creature.Guid.Full))
                        continue;
                }

                visibleTargets.Add(creature);
            }

            return visibleTargets;
        }

        /// <summary>
        /// Returns the list of potential attack targets, sorted by closest distance 
        /// </summary>
        public List<TargetDistance> BuildTargetDistance(List<Creature> targets, bool distSq = false)
        {
            var targetDistance = new List<TargetDistance>();

            foreach (var target in targets)
                //targetDistance.Add(new TargetDistance(target, distSq ? Location.SquaredDistanceTo(target.Location) : Location.DistanceTo(target.Location)));
                targetDistance.Add(new TargetDistance(target, distSq ? (float)PhysicsObj.get_distance_sq_to_object(target.PhysicsObj, true) : (float)PhysicsObj.get_distance_to_object(target.PhysicsObj, true)));

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

                if (rng < invRatio)
                    return targetDistance.Target;
            }
            // precision error?
            Console.WriteLine($"{Name}.SelectWeightedDistance: couldn't find target: {string.Join(",", targetDistances.Select(i => i.Distance))}");
            return targetDistances[0].Target;
        }

        /// <summary>
        /// Called when a monster is first spawning in
        /// </summary>
        public void CheckTargets()
        {
            if (!Attackable && TargetingTactic == TargetingTactic.None || Tolerance != Tolerance.None)
                return;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(0.75f);
            actionChain.AddAction(this, CheckTargets_Inner);
            actionChain.EnqueueChain();
        }

        public void CheckTargets_Inner()
        {
            Creature closestTarget = null;
            var closestDistSq = float.MaxValue;

            foreach (var creature in PhysicsObj.ObjMaint.GetVisibleTargetsValuesOfTypeCreature())
            {
                if (creature is Player player && (!player.Attackable || player.Teleporting || (player.Hidden ?? false)))
                    continue;

                //var distSq = Location.SquaredDistanceTo(creature.Location);
                var distSq = PhysicsObj.get_distance_sq_to_object(creature.PhysicsObj, true);
                if (distSq < closestDistSq)
                {
                    closestDistSq = (float)distSq;
                    closestTarget = creature;
                }
            }
            if (closestTarget == null || closestDistSq > VisualAwarenessRangeSq)
                return;

            closestTarget.AlertMonster(this);
        }

        /// <summary>
        /// The most common value from retail
        /// Some other common values are in the range of 12-25
        /// </summary>
        public static readonly float VisualAwarenessRange_Default = 18.0f;

        /// <summary>
        /// The highest value found in the current database
        /// </summary>
        public static readonly float VisualAwarenessRange_Highest = 75.0f;

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

        private float? _visualAwarenessRangeSq;

        public float VisualAwarenessRangeSq
        {
            get
            {
                if (_visualAwarenessRangeSq == null)
                {
                    var visualAwarenessRange = (float)((VisualAwarenessRange ?? VisualAwarenessRange_Default) * PropertyManager.GetDouble("mob_awareness_range").Item);

                    _visualAwarenessRangeSq = visualAwarenessRange * visualAwarenessRange;
                }

                return _visualAwarenessRangeSq.Value;
            }
        }

        /// <summary>
        /// Monsters can only alert other monsters once?
        /// </summary>
        public bool Alerted;

        public void AlertFriendly()
        {
            //if (Alerted) return;

            var visibleObjs = PhysicsObj.ObjMaint.GetVisibleObjects(PhysicsObj.CurCell);

            foreach (var obj in visibleObjs)
            {
                var nearbyCreature = obj.WeenieObj.WorldObject as Creature;
                if (nearbyCreature == null || nearbyCreature.IsAwake || !nearbyCreature.Attackable)
                    continue;

                if (CreatureType != null && CreatureType == nearbyCreature.CreatureType ||
                      FriendType != null && FriendType == nearbyCreature.CreatureType)
                {
                    //var distSq = Location.SquaredDistanceTo(nearbyCreature.Location);
                    var distSq = PhysicsObj.get_distance_sq_to_object(nearbyCreature.PhysicsObj, true);
                    if (distSq > nearbyCreature.VisualAwarenessRangeSq)
                        continue;

                    if (nearbyCreature.RetaliateTargets != null)
                        nearbyCreature.RetaliateTargets.Add(AttackTarget.Guid.Full);

                    Alerted = true;
                    nearbyCreature.AttackTarget = AttackTarget;
                    nearbyCreature.WakeUp(false);
                }
            }
        }
    }
}
