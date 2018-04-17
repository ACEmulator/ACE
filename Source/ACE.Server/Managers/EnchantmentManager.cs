using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public class EnchantmentManager
    {
        public WorldObject WorldObject;
        public ICollection<BiotaPropertiesEnchantmentRegistry> Enchantments;

        /// <summary>
        /// The amount of vitae reduced on player death
        /// </summary>
        public static readonly float VitaePenalty = 0.05f;

        /// <summary>
        /// The minimum possible vitae a player can have
        /// </summary>
        public static readonly float MinVitae = 0.60f;

        /// <summary>
        /// Returns TRUE if this object has any active enchantments in the registry
        /// </summary>
        public bool HasEnchantments
        {
            get
            {
                return WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Count() > 0;
            }
        }

        /// <summary>
        /// Returns TRUE If this object has a vitae penalty
        /// </summary>
        public bool HasVitae
        {
            get
            {
                return WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => e.SpellId == (uint)Spell.Vitae).Count() > 0;
            }
        }

        /// <summary>
        /// Constructs a new EnchantmentManager for a WorldObject
        /// </summary>
        public EnchantmentManager(WorldObject obj)
        {
            WorldObject = obj;
            Enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry;
        }

        /// <summary>
        /// Adds an enchantment to this object's registry
        /// </summary>
        public void Add(Enchantment enchantment)
        {
            // check for existing record
            // if none, add new record
            // if exists, updating existing record: ++layer, StartTime=0
            var spell = GetSpell(enchantment.Spell.SpellId);
            if (spell != null)
            {
                spell.LayerId++;
                spell.StartTime = 0;
                return;
            }
            var entry = BuildEntry(enchantment.Spell.SpellId);
            entry.LayerId = enchantment.Layer;
            WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Add(entry);
        }

        /// <summary>
        /// Writes the EnchantmentRegistry to the network stream
        /// </summary>
        public void SendRegistry(BinaryWriter writer)
        {
            var player = WorldObject as Player;
            var enchantmentRegistry = new EnchantmentRegistry(player);
            writer.Write(enchantmentRegistry);
        }

        /// <summary>
        /// Writes UpdateEnchantment vitae to the network stream
        /// </summary>
        public void SendUpdateVitae()
        {
            var player = WorldObject as Player;
            var vitae = new Enchantment(player, GetVitae());
            player.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(player.Session, vitae));
        }

        /// <summary>
        /// Called on player death
        /// </summary>
        public float UpdateVitae()
        {
            BiotaPropertiesEnchantmentRegistry vitae = null;

            if (!HasVitae)
            {
                // add entry for new vitae
                vitae = BuildEntry((uint)Spell.Vitae);
                vitae.LayerId = 0;
                vitae.StatModValue = 1.0f - VitaePenalty;

                WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Add(vitae);
            }
            else
            {
                // update existing vitae
                vitae = GetVitae();
                vitae.StatModValue -= VitaePenalty;
            }

            var player = WorldObject as Player;
            var minVitae = GetMinVitae((uint)player.Level);
            if (vitae.StatModValue < minVitae)
                vitae.StatModValue = minVitae;

            SaveDatabase();

            return vitae.StatModValue;
        }

        /// <summary>
        /// Called when player crosses the VitaeCPPool threshold
        /// </summary>
        public float ReduceVitae()
        {
            var vitae = GetVitae();
            vitae.StatModValue += 0.01f;
            //SaveDatabase();

            var player = WorldObject as Player;

            if (Math.Abs(vitae.StatModValue - 1.0f) < PhysicsGlobals.EPSILON)
                return 1.0f;
            else
                return vitae.StatModValue;
        }

        /// <summary>
        /// Removes the vitae penalty for a player
        /// </summary>
        public void RemoveVitae()
        {
            WorldObject.RemoveEnchantment((int)Spell.Vitae);
            var player = WorldObject as Player;
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(2.0f);
            actionChain.AddAction(player, () =>
            {
                player.Session.Network.EnqueueSend(new GameEventMagicRemoveEnchantment(player.Session, (ushort)Spell.Vitae, 0));
                player.Session.Network.EnqueueSend(new GameMessageSound(player.Guid, Sound.SpellExpire, 1.0f));
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Returns TRUE if registry contains spellId for this creature
        /// </summary>
        public bool HasSpell(uint spellId)
        {
            return WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => e.SpellId == spellId).Count() > 0;
        }

        public BiotaPropertiesEnchantmentRegistry GetSpell(uint spellID)
        {
            return WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(e => e.SpellId == spellID);
        }

        public BiotaPropertiesEnchantmentRegistry GetVitae()
        {
            return GetSpell((uint)Spell.Vitae);
        }

        /// <summary>
        /// Called on player death
        /// </summary>
        public void SaveDatabase()
        {
            var player = WorldObject as Player;
            var saveChain = player.GetSaveChain();
            saveChain.EnqueueChain();
        }

        /// <summary>
        /// Builds an enchantment registry entry from a spell ID
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry BuildEntry(uint spellID)
        {
            var spellBase = DatManager.PortalDat.SpellTable.Spells[spellID];
            var spell = DatabaseManager.World.GetCachedSpell(spellID);

            var entry = new BiotaPropertiesEnchantmentRegistry();

            entry.EnchantmentCategory = (uint)spell.MetaSpellType;
            var enchantmentType = (EnchantmentTypeFlags)entry.EnchantmentCategory;
            entry.ObjectId = WorldObject.Guid.Full;
            entry.Object = WorldObject.Biota;
            entry.SpellId = (int)spell.SpellId;
            entry.SpellCategory = (ushort)spell.Category;
            entry.PowerLevel = spell.Power;
            entry.Duration = spell.Duration ?? 0.0;
            entry.CasterObjectId = WorldObject.Guid.Full;   // only works for self?
            entry.DegradeModifier = spell.DegradeModifier ?? 0.0f;
            entry.DegradeLimit = spell.DegradeLimit ?? 0.0f;
            entry.StatModType = spell.StatModType ?? 0;
            entry.StatModKey = spell.StatModKey ?? 0;
            entry.StatModValue = spell.StatModVal ?? 0.0f;

            return entry;
        }

        /// <summary>
        /// Called every ~5 seconds for active object
        /// </summary>
        public void HeartBeat()
        {
            var expired = new List<BiotaPropertiesEnchantmentRegistry>();

            foreach (var enchantment in Enchantments)
            {
                enchantment.StartTime -= 5;

                // StartTime ticks backwards to -Duration
                if (enchantment.Duration > 0 && enchantment.StartTime <= -enchantment.Duration)
                    expired.Add(enchantment);
            }

            foreach (var enchantment in expired)
                Remove(enchantment);
        }

        /// <summary>
        /// Removes a spell from the enchantment registry, and
        /// sends the relevant network messages for spell removal
        /// </summary>
        public void Remove(BiotaPropertiesEnchantmentRegistry entry)
        {
            var spellID = entry.SpellId;
            var spell = DatabaseManager.World.GetCachedSpell((uint)spellID);
            WorldObject.RemoveEnchantment(spellID);

            var player = WorldObject as Player;
            var remove = new GameEventMagicRemoveEnchantment(player.Session, (ushort)entry.SpellId, entry.LayerId);
            var sound = new GameMessageSound(player.Guid, Sound.SpellExpire, 1.0f);

            player.Session.Network.EnqueueSend(remove, sound);
        }

        /// <summary>
        /// Returns the minimum vitae for a player level
        /// </summary>
        public float GetMinVitae(uint level)
        {
            var maxPenalty = (level - 1) * 3;
            if (maxPenalty < 1)
                maxPenalty = 1;
            var globalMax = 100 - (uint)Math.Round(MinVitae * 100);
            if (maxPenalty > globalMax)
                maxPenalty = globalMax;

            var minVitae = (100 - maxPenalty) / 100.0f;
            if (minVitae < MinVitae)
                minVitae = MinVitae;

            return minVitae;
        }
    }
}
