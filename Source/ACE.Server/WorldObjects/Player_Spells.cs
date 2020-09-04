using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.Structure;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool SpellIsKnown(uint spellId)
        {
            return Biota.SpellIsKnown((int)spellId, BiotaDatabaseLock);
        }

        /// <summary>
        /// Will return true if the spell was added, or false if the spell already exists.
        /// </summary>
        public bool AddKnownSpell(uint spellId)
        {
            Biota.GetOrAddKnownSpell((int)spellId, BiotaDatabaseLock, out var spellAdded);

            if (spellAdded)
                ChangesDetected = true;

            return spellAdded;
        }

        /// <summary>
        /// Removes a known spell from the player's spellbook
        /// </summary>
        public bool RemoveKnownSpell(uint spellId)
        {
            return Biota.TryRemoveKnownSpell((int)spellId, BiotaDatabaseLock);
        }

        public void LearnSpellWithNetworking(uint spellId, bool uiOutput = true)
        {
            var spells = DatManager.PortalDat.SpellTable;

            if (!spells.Spells.ContainsKey(spellId))
            {
                GameMessageSystemChat errorMessage = new GameMessageSystemChat("SpellID not found in Spell Table", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(errorMessage);
                return;
            }

            if (!AddKnownSpell(spellId))
            {
                GameMessageSystemChat errorMessage = new GameMessageSystemChat("That spell is already known", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(errorMessage);
                return;
            }

            GameEventMagicUpdateSpell updateSpellEvent = new GameEventMagicUpdateSpell(Session, (ushort)spellId);
            Session.Network.EnqueueSend(updateSpellEvent);

            //Check to see if we echo output to the client
            if (uiOutput == true)
            {
                // Always seems to be this SkillUpPurple effect
                ApplyVisualEffects(PlayScript.SkillUpPurple);

                string message = $"You learn the {spells.Spells[spellId].Name} spell.\n";
                GameMessageSystemChat learnMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(learnMessage);
            }
        }

        /// <summary>
        ///  Learns spells in bulk, without notification, filtered by school and level
        /// </summary>
        public void LearnSpellsInBulk(MagicSchool school, uint spellLevel, bool withNetworking = true)
        {
            var spellTable = DatManager.PortalDat.SpellTable;

            foreach (var spellID in PlayerSpellTable)
            {
                if (!spellTable.Spells.ContainsKey(spellID))
                {
                    Console.WriteLine($"Unknown spell ID in PlayerSpellID table: {spellID}");
                    continue;
                }
                var spell = new Spell(spellID, false);
                if (spell.School == school && spell.Formula.Level == spellLevel)
                {
                    if (withNetworking)
                        LearnSpellWithNetworking(spell.Id, false);
                    else
                        AddKnownSpell(spell.Id);
                }
            }
        }

        public void HandleActionMagicRemoveSpellId(uint spellId)
        {
            if (!Biota.TryRemoveKnownSpell((int)spellId, BiotaDatabaseLock))
            {
                log.Error("Invalid spellId passed to Player.RemoveSpellFromSpellBook");
                return;
            }

            ChangesDetected = true;

            GameEventMagicRemoveSpell removeSpellEvent = new GameEventMagicRemoveSpell(Session, (ushort)spellId);
            Session.Network.EnqueueSend(removeSpellEvent);
        }

        public void EquipItemFromSet(WorldObject item)
        {
            if (!item.HasItemSet) return;

            var setItems = EquippedObjects.Values.Where(i => i.HasItemSet && i.EquipmentSetId == item.EquipmentSetId).ToList();

            var spells = GetSpellSet(setItems);

            // get the spells from before / without this item
            setItems.Remove(item);
            var prevSpells = GetSpellSet(setItems);

            EquipDequipItemFromSet(item, spells, prevSpells);
        }

        public void EquipDequipItemFromSet(WorldObject item, List<Spell> spells, List<Spell> prevSpells)
        {
            // compare these 2 spell sets -
            // see which spells are being added, and which are being removed
            var addSpells = spells.Except(prevSpells);
            var removeSpells = prevSpells.Except(spells);

            // set spells are not affected by mana
            // if it's equipped, it's active.

            foreach (var spell in removeSpells)
                EnchantmentManager.Dispel(EnchantmentManager.GetEnchantment(spell.Id, item.EquipmentSetId.Value));

            foreach (var spell in addSpells)
                CreateItemSpell(item, spell.Id);
        }

        public void DequipItemFromSet(WorldObject item)
        {
            if (!item.HasItemSet) return;

            var setItems = EquippedObjects.Values.Where(i => i.HasItemSet && i.EquipmentSetId == item.EquipmentSetId).ToList();

            var spells = GetSpellSet(setItems);

            // get the spells from before / with this item
            setItems.Add(item);
            var prevSpells = GetSpellSet(setItems);

            EquipDequipItemFromSet(item, spells, prevSpells);
        }

        public void OnItemLevelUp(WorldObject item, int prevItemLevel)
        {
            if (!item.HasItemSet) return;

            var setItems = EquippedObjects.Values.Where(i => i.HasItemSet && i.EquipmentSetId == item.EquipmentSetId).ToList();

            var levelDiff = prevItemLevel - (item.ItemLevel ?? 0);

            var prevSpells = GetSpellSet(setItems, levelDiff);

            var spells = GetSpellSet(setItems);

            EquipDequipItemFromSet(item, spells, prevSpells);
        }

        public void CreateSentinelBuffPlayers(IEnumerable<Player> players, bool self = false, ulong maxLevel = 8)
        {
            if (!(Session.AccessLevel >= AccessLevel.Sentinel)) return;

            var SelfOrOther = self ? "Self" : "Other";

            // ensure level 8s are installed
            var maxSpellLevel = Math.Clamp(maxLevel, 1, 8);
            if (maxSpellLevel == 8 && DatabaseManager.World.GetCachedSpell((uint)SpellId.ArmorOther8) == null)
                maxSpellLevel = 7;

            var tySpell = typeof(SpellId);
            List<BuffMessage> buffMessages = new List<BuffMessage>();
            // prepare messages
            List<string> buffsNotImplementedYet = new List<string>();
            foreach (var spell in Buffs)
            {
                var spellNamPrefix = spell;
                bool isBane = false;
                if (spellNamPrefix.StartsWith("@"))
                {
                    isBane = true;
                    spellNamPrefix = spellNamPrefix.Substring(1);
                }
                string fullSpellEnumName = spellNamPrefix + ((isBane) ? string.Empty : SelfOrOther) + maxSpellLevel;
                string fullSpellEnumNameAlt = spellNamPrefix + ((isBane) ? string.Empty : ((SelfOrOther == "Self") ? "Other" : "Self")) + maxSpellLevel;
                uint spellID = (uint)Enum.Parse(tySpell, fullSpellEnumName);
                var buffMsg = BuildBuffMessage(spellID);

                if (buffMsg == null)
                {
                    spellID = (uint)Enum.Parse(tySpell, fullSpellEnumNameAlt);
                    buffMsg = BuildBuffMessage(spellID);
                }

                if (buffMsg != null)
                {
                    buffMsg.Bane = isBane;
                    buffMessages.Add(buffMsg);
                }
                else
                {
                    buffsNotImplementedYet.Add(fullSpellEnumName);
                }
            }
            // buff each player
            players.ToList().ForEach(targetPlayer =>
            {
                if (buffMessages.Any(k => !k.Bane))
                {
                    // bake player into the messages
                    buffMessages.Where(k => !k.Bane).ToList().ForEach(k => k.SetTargetPlayer(targetPlayer));
                    // update client-side enchantments
                    targetPlayer.Session.Network.EnqueueSend(buffMessages.Where(k => !k.Bane).Select(k => k.SessionMessage).ToArray());
                    // run client-side effect scripts, omitting duplicates
                    targetPlayer.EnqueueBroadcast(buffMessages.Where(k => !k.Bane).ToList().GroupBy(m => m.Spell.TargetEffect).Select(a => a.First().LandblockMessage).ToArray());
                    // update server-side enchantments

                    var buffsForPlayer = buffMessages.Where(k => !k.Bane).ToList().Select(k => k.Enchantment);

                    var lifeBuffsForPlayer = buffsForPlayer.Where(k => k.Spell.School == MagicSchool.LifeMagic).ToList();
                    var critterBuffsForPlayer = buffsForPlayer.Where(k => k.Spell.School == MagicSchool.CreatureEnchantment).ToList();
                    var itemBuffsForPlayer = buffsForPlayer.Where(k => k.Spell.School == MagicSchool.ItemEnchantment).ToList();

                    bool crit = false;
                    uint dmg = 0;
                    EnchantmentStatus ec;
                    lifeBuffsForPlayer.ForEach(spl =>
                    {
                        bool casted = targetPlayer.LifeMagic(spl.Spell, out dmg, out crit, out ec, targetPlayer, this);
                    });
                    critterBuffsForPlayer.ForEach(spl =>
                    {
                        ec = targetPlayer.CreatureMagic(targetPlayer, spl.Spell, this);
                    });
                    itemBuffsForPlayer.ForEach(spl =>
                    {
                        ec = targetPlayer.ItemMagic(targetPlayer, spl.Spell, this);
                    });
                }
                if (buffMessages.Any(k => k.Bane))
                {
                    // Impen/bane
                    var items = targetPlayer.EquippedObjects.Values.ToList();
                    var itembuffs = buffMessages.Where(k => k.Bane).ToList();
                    foreach (var itemBuff in itembuffs)
                    {
                        foreach (var item in items)
                        {
                            if ((item.WeenieType == WeenieType.Clothing || item.IsShield) && item.IsEnchantable)
                            {
                                itemBuff.SetLandblockMessage(item.Guid);
                                var enchantmentStatus = targetPlayer.ItemMagic(item, itemBuff.Spell, this);
                                targetPlayer?.EnqueueBroadcast(itemBuff.LandblockMessage);
                            }
                        }
                    }
                }
            });
        }

        // TODO: switch this over to SpellProgressionTables
        private static string[] Buffs = new string[] {
#region spells
            // @ indicates impenetrability or a bane
            "Strength",
            "Invulnerability",
            "FireProtection",
            "Armor",
            "Rejuvenation",
            "Regeneration",
            "ManaRenewal",
            "Impregnability",
            "MagicResistance",
            //"AxeMastery",    // light weapons
            "LightWeaponsMastery",
            //"DaggerMastery", // finesse weapons
            "FinesseWeaponsMastery",
            //"MaceMastery",
            //"SpearMastery",
            //"StaffMastery",
            //"SwordMastery",  // heavy weapons
            "HeavyWeaponsMastery",
            //"UnarmedCombatMastery",
            //"BowMastery",    // missile weapons
            "MissileWeaponsMastery",
            //"CrossbowMastery",
            //"ThrownWeaponMastery",
            "AcidProtection",
            "CreatureEnchantmentMastery",
            "ItemEnchantmentMastery",
            "LifeMagicMastery",
            "WarMagicMastery",
            "ManaMastery",
            "ArcaneEnlightenment",
            "ArcanumSalvaging",
            "ArmorExpertise",
            "ItemExpertise",
            "MagicItemExpertise",
            "WeaponExpertise",
            "MonsterAttunement",
            "PersonAttunement",
            "DeceptionMastery",
            "HealingMastery",
            "LeadershipMastery",
            "LockpickMastery",
            "Fealty",
            "JumpingMastery",
            "Sprint",
            "BludgeonProtection",
            "ColdProtection",
            "LightningProtection",
            "BladeProtection",
            "PiercingProtection",
            "Endurance",
            "Coordination",
            "Quickness",
            "Focus",
            "Willpower",
            "CookingMastery",
            "FletchingMastery",
            "AlchemyMastery",
            "VoidMagicMastery",
            "SummoningMastery",
            "SwiftKiller",
            "Defender",
            "BloodDrinker",
            "HeartSeeker",
            "HermeticLink",
            "SpiritDrinker",
            "DualWieldMastery",
            "TwoHandedMastery",
            "DirtyFightingMastery",
            "RecklessnessMastery",
            "SneakAttackMastery",
            "@Impenetrability",
            "@PiercingBane",
            "@BludgeonBane",
            "@BladeBane",
            "@AcidBane",
            "@FlameBane",
            "@FrostBane",
            "@LightningBane",
#endregion
            };

        private class BuffMessage
        {
            public bool Bane { get; set; } = false;
            public GameEventMagicUpdateEnchantment SessionMessage { get; set; } = null;
            public GameMessageScript LandblockMessage { get; set; } = null;
            public Spell Spell { get; set; } = null;
            public Enchantment Enchantment { get; set; } = null;
            public void SetTargetPlayer(Player p)
            {
                Enchantment.Target = p;
                SessionMessage = new GameEventMagicUpdateEnchantment(p.Session, Enchantment);
                SetLandblockMessage(p.Guid);
            }
            public void SetLandblockMessage(ObjectGuid target)
            {
                LandblockMessage = new GameMessageScript(target, Spell.TargetEffect, 1f);
            }
        }

        private static BuffMessage BuildBuffMessage(uint spellID)
        {
            BuffMessage buff = new BuffMessage();
            buff.Spell = new Spell(spellID);
            if (buff.Spell.NotFound) return null;
            buff.Enchantment = new Enchantment(null, 0, spellID, 1, (EnchantmentMask)buff.Spell.StatModType, buff.Spell.StatModVal);
            return buff;
        }

        public void HandleSpellbookFilters(SpellBookFilterOptions filters)
        {
            Character.SpellbookFilters = (uint)filters;
        }

        public void HandleSetDesiredComponentLevel(uint component_wcid, uint amount)
        {
            // ensure wcid is spell component
            if (!SpellComponent.IsValid(component_wcid))
            {
                log.Warn($"{Name}.HandleSetDesiredComponentLevel({component_wcid}, {amount}): invalid spell component wcid");
                return;
            }
            if (amount > 0)
            {
                var existing = Character.GetFillComponent(component_wcid, CharacterDatabaseLock);

                if (existing == null)
                    Character.AddFillComponent(component_wcid, amount, CharacterDatabaseLock, out bool exists);
                else
                    existing.QuantityToRebuy = (int)amount;
            }
            else
                Character.TryRemoveFillComponent(component_wcid, out var _, CharacterDatabaseLock);

            CharacterChangesDetected = true;
        }

        public static Dictionary<MagicSchool, uint> FociWCIDs = new Dictionary<MagicSchool, uint>()
        {
            { MagicSchool.CreatureEnchantment, 15268 },   // Foci of Enchantment
            { MagicSchool.ItemEnchantment,     15269 },   // Foci of Artifice
            { MagicSchool.LifeMagic,           15270 },   // Foci of Verdancy
            { MagicSchool.WarMagic,            15271 },   // Foci of Strife
            { MagicSchool.VoidMagic,           43173 },   // Foci of Shadow
        };

        public bool HasFoci(MagicSchool school)
        {
            switch (school)
            {
                case MagicSchool.CreatureEnchantment:
                    if (AugmentationInfusedCreatureMagic > 0)
                        return true;
                    break;
                case MagicSchool.ItemEnchantment:
                    if (AugmentationInfusedItemMagic > 0)
                        return true;
                    break;
                case MagicSchool.LifeMagic:
                    if (AugmentationInfusedLifeMagic > 0)
                        return true;
                    break;
                case MagicSchool.VoidMagic:
                    if (AugmentationInfusedVoidMagic > 0)
                        return true;
                    break;
                case MagicSchool.WarMagic:
                    if (AugmentationInfusedWarMagic > 0)
                        return true;
                    break;
            }

            var wcid = FociWCIDs[school];
            return Inventory.Values.FirstOrDefault(i => i.WeenieClassId == wcid) != null;
        }

        public void HandleSpellHooks(Spell spell)
        {
            HandleMaxVitalUpdate(spell);

            // unsure if spell hook was here in retail,
            // but this has the potential to take the client out of autorun mode
            // which causes them to stop if they hit a turn key afterwards
            if (PropertyManager.GetBool("runrate_add_hooks").Item)
                HandleRunRateUpdate(spell);
        }

        /// <summary>
        /// Called when an enchantment is added or removed,
        /// checks if the spell affects the max vitals,
        /// and if so, updates the client immediately
        /// </summary>
        public void HandleMaxVitalUpdate(Spell spell)
        {
            var maxVitals = spell.UpdatesMaxVitals;

            if (maxVitals.Count == 0)
                return;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(1.0f);      // client needs time for primary attribute updates
            actionChain.AddAction(this, () =>
            {
                foreach (var maxVital in maxVitals)
                {
                    var playerVital = Vitals[maxVital];

                    Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute2ndLevel(this, playerVital.ToEnum(), playerVital.Current));
                }
            });
            actionChain.EnqueueChain();
        }

        public bool HandleRunRateUpdate(Spell spell)
        {
            if (!spell.UpdatesRunRate)
                return false;

            return HandleRunRateUpdate();
        }

        public void AuditItemSpells()
        {
            // cleans up bugged chars with dangling item set spells
            // from previous bugs

            var allPossessions = GetAllPossessions().ToDictionary(i => i.Guid, i => i);

            // this is a legacy method, but is still a decent failsafe to catch any existing issues

            // get active item enchantments
            var enchantments = Biota.PropertiesEnchantmentRegistry.Clone(BiotaDatabaseLock).Where(i => i.Duration == -1 && i.SpellId != (int)SpellId.Vitae).ToList();

            foreach (var enchantment in enchantments)
            {
                var table = enchantment.HasSpellSetId ? allPossessions : EquippedObjects;

                // if this item is not equipped, remove enchantment
                if (!table.TryGetValue(new ObjectGuid(enchantment.CasterObjectId), out var item))
                {
                    var spell = new Spell(enchantment.SpellId, false);
                    log.Error($"{Name}.AuditItemSpells(): removing spell {spell.Name} from non-equipped item");

                    EnchantmentManager.Dispel(enchantment);
                    continue;
                }

                // is this item part of a set?
                if (!item.HasItemSet)
                    continue;

                // get all of the equipped items in this set
                var setItems = EquippedObjects.Values.Where(i => i.HasItemSet && i.EquipmentSetId == item.EquipmentSetId).ToList();

                // get all of the spells currently active from this set
                var currentSpells = GetSpellSet(setItems);

                // get all of the spells possible for this item set
                var possibleSpells = GetSpellSetAll((EquipmentSet)item.EquipmentSetId);

                // get the difference between them
                var inactiveSpells = possibleSpells.Except(currentSpells).ToList();

                // remove any item set spells that shouldn't be active
                foreach (var inactiveSpell in inactiveSpells)
                {
                    var removeSpells = enchantments.Where(i => i.SpellSetId == item.EquipmentSetId && i.SpellId == inactiveSpell.Id).ToList();

                    foreach (var removeSpell in removeSpells)
                    {
                        log.Error($"{Name}.AuditItemSpells(): removing spell {inactiveSpell.Name} from {item.EquipmentSetId}");

                        EnchantmentManager.Dispel(removeSpell);
                    }
                }
            }
        }
    }
}
