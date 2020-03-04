using System;
using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Managers;

using Biota = ACE.Database.Models.Shard.Biota;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Summonable monsters combat AI
    /// </summary>
    public class CombatPet : Pet
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public CombatPet(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public CombatPet(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override bool Init(Player player, PetDevice petDevice)
        {
            var success = base.Init(player, petDevice);

            if (!success)
                return false;

            SetCombatMode(CombatMode.Melee);
            MonsterState = State.Awake;
            IsAwake = true;

            // copy ratings from pet device
            DamageRating = petDevice.GearDamage;
            DamageResistRating = petDevice.GearDamageResist;
            CritDamageRating = petDevice.GearCritDamage;
            CritDamageResistRating = petDevice.GearCritDamageResist;
            CritRating = petDevice.GearCrit;
            CritResistRating = petDevice.GearCritResist;

            return true;
        }

        public override void HandleFindTarget()
        {
            var creature = AttackTarget as Creature;

            if (creature == null || creature.IsDead || !IsVisibleTarget(creature))
                FindNextTarget();
        }

        public override bool FindNextTarget()
        {
            var nearbyMonsters = GetNearbyMonsters();
            if (nearbyMonsters.Count == 0)
            {
                //Console.WriteLine($"{Name}.FindNextTarget(): empty");
                return false;
            }

            // get nearest monster
            var nearest = BuildTargetDistance(nearbyMonsters);
            if (nearest[0].Distance > RadiusAwareness)
            {
                //Console.WriteLine($"{Name}.FindNextTarget(): next object out-of-range (dist: {Math.Round(Math.Sqrt(nearest[0].Distance))})");
                return false;
            }

            AttackTarget = nearest[0].Target;

            //Console.WriteLine($"{Name}.FindNextTarget(): {AttackTarget.Name}");

            return true;
        }

        /// <summary>
        /// Returns a list of attackable monsters in this pet's visible targets
        /// </summary>
        public List<Creature> GetNearbyMonsters()
        {
            var monsters = new List<Creature>();

            foreach (var creature in PhysicsObj.ObjMaint.GetVisibleTargetsValuesOfTypeCreature())
            {
                // why does this need to be in here?
                if (creature.IsDead)
                {
                    //Console.WriteLine($"{Name}.GetNearbyMonsters(): refusing to add dead creature {creature.Name} ({creature.Guid})");
                    continue;
                }
                monsters.Add(creature);
            }

            return monsters;
        }

        public override void Sleep()
        {
            // pets dont really go to sleep, per say
            // they keep scanning for new targets,
            // which is the reverse of the current ACE jurassic park model

            return;  // empty by default
        }
    }
}
