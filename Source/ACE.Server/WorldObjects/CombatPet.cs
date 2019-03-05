using System;
using System.Collections.Generic;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Summonable monsters combat AI
    /// </summary>
    public class CombatPet: Creature
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

        }

        public void Init(Player player, DamageType damageType, PetDevice petDevice)
        {
            SuppressGenerateEffect = true;
            NoCorpse = true;
            ExpirationTime = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            Location = player.Location.InFrontOf(5f);   // FIXME: get correct cell
            Name = player.Name + "'s " + Name;
            P_PetOwner = player;
            PetOwner = player.Guid.Full;
            SetCombatMode(CombatMode.Melee);
            EnterWorld();
            DamageType = damageType;
            Attackable = true;
            MonsterState = State.Awake;
            IsAwake = true;

            // copy ratings from pet device
            DamageRating = petDevice.GearDamage;
            DamageResistRating = petDevice.GearDamageResist;
            CritDamageRating = petDevice.GearCritDamage;
            CritDamageResistRating = petDevice.GearCritDamageResist;
            CritRating = petDevice.GearCrit;
            CritResistRating = petDevice.GearCritResist;

            /*var spellBase = DatManager.PortalDat.SpellTable.Spells[32981];
            var spell = DatabaseManager.World.GetCachedSpell(32981);

            if (spell != null && spellBase != null)
            {
                var enchantment = new Enchantment(this, player.Guid, spellBase, spellBase.Duration, 1, (uint)EnchantmentMask.Cooldown, spell.StatModType);
                player.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(player.Session, enchantment));
            }
            else
            {
                Console.WriteLine("Cooldown spell or spellBase were null");
            }
            */
        }

        public override void HandleFindTarget()
        {
            var creature = AttackTarget as Creature;

            if (creature == null || creature.IsDead || !IsVisible(creature))
                FindNextTarget();
        }

        public override bool FindNextTarget()
        {
            // rebuild visible objects (handle this better for monsters)
            GetVisibleObjects();

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
        /// Returns a list of attackable monsters in this pet's visible objects table
        /// </summary>
        public List<Creature> GetNearbyMonsters()
        {
            // TODO: this might need refreshed
            var visibleObjs = PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            var monsters = new List<Creature>();

            foreach (var obj in visibleObjs)
            {
                // exclude self (should hopefully not be in this list)
                if (PhysicsObj == obj) continue;

                // exclude players
                var wo = obj.WeenieObj.WorldObject;
                var player = wo as Player;
                if (player != null) continue;

                // ensure creature / not combat pet
                var creature = wo as Creature;
                var combatPet = wo as CombatPet;
                if (creature == null || combatPet != null || creature.IsDead) continue;

                // ensure attackable
                var attackable = creature.GetProperty(PropertyBool.Attackable) ?? false;
                if (!attackable) continue;

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
