using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Creature melee combat for players and monsters
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// Returns TRUE for DualWieldCombat mode
        /// </summary>
        public bool IsDualWieldAttack { get => CurrentMotionState?.Stance == MotionStance.DualWieldCombat; }

        /// <summary>
        /// Dual wield alternate will be true if *next* attack is offhand
        /// </summary>
        public bool DualWieldAlternate;

        /// <summary>
        /// Returns TRUE for TwoHandedCombat mode
        /// </summary>
        public bool TwoHandedCombat { get => CurrentMotionState?.Stance == MotionStance.TwoHandedSwordCombat || CurrentMotionState?.Stance == MotionStance.TwoHandedStaffCombat; }

        /// <summary>
        /// Determines if a weapon can double or triple strike,
        /// and appends the appropriate multistrike prefix to a MotionCommand
        /// </summary>
        public string MultiStrike(AttackType attackType, string action)
        {
            if ((attackType & AttackType.MultiStrike) == 0)
                return action;

            var doubleStrike = action.EndsWith("Thrust") ? AttackType.DoubleThrust : AttackType.DoubleSlash;
            var tripleStrike = (AttackType)((int)doubleStrike * 2);

            if (attackType.HasFlag(tripleStrike))
                return $"Triple{action}";
            if (attackType.HasFlag(doubleStrike))
                return $"Double{action}";
            else
                return action;
        }

        /// <summary>
        /// Returns the attack types for a weapon
        /// </summary>
        public AttackType GetWeaponAttackType(WorldObject weapon)
        {
            if (weapon == null)
                return AttackType.Undef;

            return weapon.W_AttackType;
        }

        /// <summary>
        /// Returns the number of strikes for a weapon
        /// between 1-3 strikes
        /// </summary>
        public int GetNumStrikes(WorldObject weapon)
        {
            var attackType = GetWeaponAttackType(weapon);

            return GetNumStrikes(attackType);
        }

        /// <summary>
        /// Returns the number of strikes for an AttackType
        /// between 1-3 strikes
        /// </summary>
        public int GetNumStrikes(AttackType attackType)
        {
            if (CurrentMotionState.Stance == MotionStance.TwoHandedSwordCombat || CurrentMotionState.Stance == MotionStance.TwoHandedStaffCombat)
                return 2;

            if ((attackType & AttackType.MultiStrike) == 0)
                return 1;

            if (attackType.HasFlag(AttackType.TripleSlash) || attackType.HasFlag(AttackType.TripleThrust) || attackType.HasFlag(AttackType.OffhandTripleSlash) || attackType.HasFlag(AttackType.OffhandTripleThrust))
                return 3;
            else if (attackType.HasFlag(AttackType.DoubleSlash) || attackType.HasFlag(AttackType.DoubleThrust) || attackType.HasFlag(AttackType.OffhandDoubleSlash) || attackType.HasFlag(AttackType.OffhandDoubleThrust))
                return 2;
            else
                return 1;
        }

        public int DistanceComparator(PhysicsObj a, PhysicsObj b)
        {
            // use square distance to make things a bit faster
            var dist1 = Location.SquaredDistanceTo(a.WeenieObj.WorldObject.Location);
            var dist2 = Location.SquaredDistanceTo(b.WeenieObj.WorldObject.Location);

            return dist1.CompareTo(dist2);
        }

        public static readonly float CleaveRange = 5.0f;
        public static readonly float CleaveRangeSq = CleaveRange * CleaveRange;
        public static readonly float CleaveAngle = 180.0f;

        public static readonly float CleaveCylRange = 2.0f;

        /// <summary>
        /// Performs a cleaving attack for two-handed weapons
        /// </summary>
        /// <returns>The list of cleave targets to hit with this attack</returns>
        public List<Creature> GetCleaveTarget(Creature target, WorldObject weapon)
        {
            var player = this as Player;

            if (!weapon.IsCleaving) return null;

            // sort visible objects by ascending distance
            var visible = PhysicsObj.ObjMaint.GetVisibleObjectsValuesWhere(o => o.WeenieObj.WorldObject != null);
            visible.Sort(DistanceComparator);

            var cleaveTargets = new List<Creature>();
            var totalCleaves = weapon.CleaveTargets;

            foreach (var obj in visible)
            {
                // cleaving skips original target
                if (obj.ID == target.PhysicsObj.ID)
                    continue;

                // only cleave creatures
                var creature = obj.WeenieObj.WorldObject as Creature;
                if (creature == null || creature.Teleporting || creature.IsDead) continue;

                if (player != null && player.CheckPKStatusVsTarget(creature, null) != null)
                    continue;

                if (!creature.Attackable && creature.TargetingTactic == TargetingTactic.None || creature.Teleporting)
                    continue;

                if (creature is CombatPet && (player != null || this is CombatPet))
                    continue;

                // no objects in cleave range
                var cylDist = GetCylinderDistance(creature);
                if (cylDist > CleaveCylRange)
                    return cleaveTargets;

                // only cleave in front of attacker
                var angle = GetAngle(creature);
                if (Math.Abs(angle) > CleaveAngle / 2.0f)
                    continue;

                // found cleavable object
                cleaveTargets.Add(creature);
                if (cleaveTargets.Count == totalCleaves)
                    break;
            }
            return cleaveTargets;
        }
    }
}
