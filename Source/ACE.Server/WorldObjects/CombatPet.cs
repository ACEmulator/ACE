using System;
using System.Collections.Generic;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Entity;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Summonable monsters combat AI
    /// </summary>
    public class CombatPet : Creature
    {
        public DateTime ExpirationTime;

        public DamageType DamageType;

        public Player P_PetOwner;

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
            Ethereal = true;
            RadarBehavior = ACE.Entity.Enum.RadarBehavior.ShowNever;
            Usable = ACE.Entity.Enum.Usable.No;

            if (!PropertyManager.GetBool("advanced_combat_pets").Item)
                Biota.BiotaPropertiesSpellBook.Clear();

            //Biota.BiotaPropertiesCreateList.Clear();
            Biota.BiotaPropertiesEmote.Clear();
            GeneratorProfiles.Clear();            

            DeathTreasureType = null;
            WieldedTreasureType = null;

            if (Biota.WeenieType != (int)WeenieType.CombatPet) // Combat Pets are currently being made from real creatures
                Biota.WeenieType = (int)WeenieType.CombatPet;
        }

        public void Init(Player player, DamageType damageType, PetDevice petDevice)
        {
            SuppressGenerateEffect = true;
            NoCorpse = true;
            TreasureCorpse = false;
            ExpirationTime = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            Location = player.Location.InFrontOf(5f);
            Location.LandblockId = new LandblockId(Location.GetCell());
            Name = player.Name + "'s " + Name;
            P_PetOwner = player;
            PetOwner = player.Guid.Full;
            EnterWorld();
            SetCombatMode(CombatMode.Melee);
            DamageType = damageType;
            Attackable = true;
            MonsterState = State.Awake;
            IsAwake = true;
            player.CurrentActiveCombatPet = this;

            // copy ratings from pet device
            DamageRating = petDevice.GearDamage;
            DamageResistRating = petDevice.GearDamageResist;
            CritDamageRating = petDevice.GearCritDamage;
            CritDamageResistRating = petDevice.GearCritDamageResist;
            CritRating = petDevice.GearCrit;
            CritResistRating = petDevice.GearCritResist;
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
