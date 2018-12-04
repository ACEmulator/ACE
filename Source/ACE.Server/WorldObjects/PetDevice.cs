using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Entity;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using System.Collections.Generic;
using System;
using ACE.Server.Network.Structure;
using ACE.DatLoader;
using ACE.Database;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// For essences (ie. Blistering Moar Essence), used to summon a creature
    /// </summary>
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
            //Good PCAP example of using a PetDevice to summon a pet:
            //Asherons-Call-packets-includes-3-towers\pkt_2017-1-30_1485823896_log.pcap lines 27837 - 27843

            if (!PetDeviceToPetMapping.TryGetValue(WeenieClassId, out uint petWCID))
            {
                Console.WriteLine($"PetDevice.UseItem(): couldn't find a matching pet for essence wcid {WeenieClassId}");
                player.SendUseDoneEvent();
                return;
            }

            var pet = WorldObjectFactory.CreateNewWorldObject(petWCID) as Creature;
            if (pet == null)
            {
                // this would be a good place to send a friendly reminder to install the latest summoning updates from ACE-World-Patch
                Console.WriteLine($"PetDevice.UseItem(): failed to create pet for wcid {petWCID}");
                player.SendUseDoneEvent();
                return;
            }

            pet.SuppressGenerateEffect = true;
            pet.NoCorpse = true;
            pet.IsPet = true;
            pet.petCreationTime = DateTime.UtcNow;
            pet.Location = player.Location.InFrontOf(5f);
            pet.Name = player.Name + "'s " + pet.Name;
            pet.PetOwner = player.Guid.Full;
            pet.SetCombatMode(CombatMode.Melee);
            pet.CombatTableDID = 0x30000001;
            pet.EnterWorld();
            pet.UpdateObjectPhysics();
            pet.PetFindTarget();

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

            player.SendUseDoneEvent();
        }

        /// <summary>
        /// Maps an Essence to a WCID to be spawned
        /// </summary>
        public static Dictionary<uint, uint> PetDeviceToPetMapping = new Dictionary<uint, uint>()
        {
            { 49344, 49114 },   // Blistering Moar Essence => Moar
        };
    }
}
