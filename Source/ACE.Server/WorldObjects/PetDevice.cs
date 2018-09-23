using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Entity;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using System.Collections.Generic;
using System;

namespace ACE.Server.WorldObjects
{
    public class PetDevice : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public PetDevice(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public PetDevice(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void UseItem(Player player)
        {
            Creature pet;

            PetDeviceToPetMapping.TryGetValue(this.WeenieClassId, out uint petWCID);

            pet = WorldObjectFactory.CreateNewWorldObject(petWCID) as Creature;

            if (pet !=null)
            {
                pet.NoCorpse = true;
                pet.IsPet = true;
                pet.petCreationTime = DateTime.UtcNow;
                pet.Location = ((Position)player.Location.Clone()).InFrontOf(5f);
                pet.Name = player.Name + "'s " + pet.Name;
                pet.PetOwner = player.Guid.Full;
                pet.SetCombatMode(CombatMode.Melee);
                pet.CombatTableDID = 0x30000001;
                pet.EnterWorld();
                pet.UpdateObjectPhysics();
                pet.PetFindTarget();

                player.SendUseDoneEvent();
            }
        }

        public static Dictionary<uint, uint> PetDeviceToPetMapping = new Dictionary<uint, uint>()
        {
            // Maps an Essence to a WCID to be spawned
            {49344, 49114}, // Blistering Moar Essence > Moar
        };
    }
}
