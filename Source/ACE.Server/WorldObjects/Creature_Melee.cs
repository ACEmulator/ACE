using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Physics;
using MAttackType = ACE.Entity.Enum.AttackType;

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
        public string MultiStrike(MAttackType attackType, string action)
        {
            if ((attackType & MAttackType.MultiStrike) == 0)
                return action;

            var doubleStrike = action.EndsWith("Thrust") ? MAttackType.DoubleThrust : MAttackType.DoubleSlash;
            var tripleStrike = (MAttackType)((int)doubleStrike * 2);

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
        public MAttackType GetWeaponAttackType(WorldObject weapon)
        {
            var attackType = MAttackType.Undef;     // unarmed?
            if (weapon != null)
                attackType = (MAttackType)(weapon.GetProperty(PropertyInt.AttackType) ?? 0);

            return attackType;
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
        public int GetNumStrikes(MAttackType attackType)
        {
            if (CurrentMotionState.Stance == MotionStance.TwoHandedSwordCombat || CurrentMotionState.Stance == MotionStance.TwoHandedStaffCombat)
                return 2;

            if ((attackType & MAttackType.MultiStrike) == 0)
                return 1;

            if (attackType.HasFlag(MAttackType.TripleSlash) || attackType.HasFlag(MAttackType.TripleThrust) || attackType.HasFlag(MAttackType.OffhandTripleSlash) || attackType.HasFlag(MAttackType.OffhandTripleThrust))
                return 3;
            else if (attackType.HasFlag(MAttackType.DoubleSlash) || attackType.HasFlag(MAttackType.DoubleThrust) || attackType.HasFlag(MAttackType.OffhandDoubleSlash) || attackType.HasFlag(MAttackType.OffhandDoubleThrust))
                return 2;
            else
                return 1;
        }

        private static Vector3 _globalPos;

        public int DistanceComparator(PhysicsObj a, PhysicsObj b)
        {
            // use square distance to make things a bit faster
            var globPos1 = a.WeenieObj.WorldObject.Location.ToGlobal();
            var globPos2 = b.WeenieObj.WorldObject.Location.ToGlobal();

            var dist1 = Vector3.DistanceSquared(_globalPos, globPos1);
            var dist2 = Vector3.DistanceSquared(_globalPos, globPos2);

            return dist1.CompareTo(dist2);
        }

        public static float CleaveRange = 5.0f;
        public static float CleaveRangeSq = CleaveRange * CleaveRange;
        public static float CleaveAngle = 180.0f;

        /// <summary>
        /// Performs a cleaving attack for two-handed weapons
        /// </summary>
        /// <returns>The list of cleave targets to hit with this attack</returns>
        public List<Creature> GetCleaveTarget(Creature target, WorldObject weapon)
        {
            if (!weapon.IsCleaving) return null;

            _globalPos = Location.ToGlobal();

            // sort visible objects by ascending distance
            var visible = PhysicsObj.ObjMaint.VisibleObjectTable.Values.ToList();
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
                if (creature == null)
                    continue;

                // no objects in cleave range
                var distSquared = Vector3.DistanceSquared(_globalPos, creature.Location.ToGlobal());
                if (distSquared > CleaveRangeSq)
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
