using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool SpellIsKnown(uint spellId)
        {
            return Biota.SpellIsKnown((int)spellId, BiotaDatabaseLock, BiotaPropertySpells);
        }

        /// <summary>
        /// Will return true if the spell was added, or false if the spell already exists.
        /// </summary>
        public bool AddKnownSpell(uint spellId)
        {
            Biota.GetOrAddKnownSpell((int)spellId, BiotaDatabaseLock, BiotaPropertySpells, out var spellAdded);

            if (spellAdded)
                ChangesDetected = true;

            return spellAdded;
        }

        /// <summary>
        /// Removes a known spell from the player's spellbook
        /// </summary>
        public bool RemoveKnownSpell(uint spellId)
        {
            return Biota.TryRemoveKnownSpell((int)spellId, out _, BiotaDatabaseLock, BiotaPropertySpells);
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
                ApplyVisualEffects(ACE.Entity.Enum.PlayScript.SkillUpPurple);

                string message = $"You learn the {spells.Spells[spellId].Name} spell.\n";
                GameMessageSystemChat learnMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(learnMessage);
            }
        }

        public void HandleActionMagicRemoveSpellId(uint spellId)
        {
            if (!Biota.TryRemoveKnownSpell((int)spellId, BiotaDatabaseLock, BiotaPropertySpells))
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

            var spells = GetSpellSet((EquipmentSet)item.EquipmentSetId, setItems);

            // get the spells from before / without this item
            setItems.Remove(item);
            var prevSpells = GetSpellSet((EquipmentSet)item.EquipmentSetId, setItems);

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

            var spells = GetSpellSet((EquipmentSet)item.EquipmentSetId, setItems);

            // get the spells from before / with this item
            setItems.Add(item);
            var prevSpells = GetSpellSet((EquipmentSet)item.EquipmentSetId, setItems);

            EquipDequipItemFromSet(item, spells, prevSpells);
        }

        public void OnItemLevelUp(WorldObject item, int prevItemLevel)
        {
            if (!item.HasItemSet) return;

            var setItems = EquippedObjects.Values.Where(i => i.HasItemSet && i.EquipmentSetId == item.EquipmentSetId).ToList();

            var levelDiff = prevItemLevel - (item.ItemLevel ?? 0);

            var prevSpells = GetSpellSet((EquipmentSet)item.EquipmentSetId, setItems, levelDiff);

            var spells = GetSpellSet((EquipmentSet)item.EquipmentSetId, setItems);

            EquipDequipItemFromSet(item, spells, prevSpells);
        }

        public void AuditItemSpells()
        {
            // cleans up bugged chars with dangling item set spells
            // from previous bugs

            // get active item enchantments
            var enchantments = Biota.GetEnchantments(BiotaDatabaseLock).Where(i => i.Duration == -1 && i.SpellId != (int)SpellId.Vitae).ToList();

            foreach (var enchantment in enchantments)
            {
                // if this item is not equipped, remove enchantment
                if (!EquippedObjects.TryGetValue(new ObjectGuid(enchantment.CasterObjectId), out var item))
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
                var currentSpells = GetSpellSet((EquipmentSet)item.EquipmentSetId, setItems);

                // get all of the spells possible for this item set
                var possibleSpells = GetSpellSetAll((EquipmentSet)item.EquipmentSetId);

                // get the difference between them
                var inactiveSpells = possibleSpells.Except(currentSpells).ToList();

                // remove any item set spells that shouldn't be active
                foreach (var inactiveSpell in inactiveSpells)
                {
                    var removeSpells = enchantments.Where(i => i.SpellSetId == (uint)item.EquipmentSetId && i.SpellId == inactiveSpell.Id).ToList();

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
