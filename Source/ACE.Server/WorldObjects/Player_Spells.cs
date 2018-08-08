using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool SpellIsKnown(uint spellId)
        {
            var result = Biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spellId);

            return (result != null);
        }

        /// <summary>
        /// Will return true if the spell was added, or false if the spell already exists.
        /// </summary>
        public bool AddKnownSpell(uint spellId)
        {
            var result = Biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spellId);

            if (result == null)
            {
                result = new BiotaPropertiesSpellBook { ObjectId = Biota.Id, Spell = (int)spellId, Object = Biota };

                Biota.BiotaPropertiesSpellBook.Add(result);
                ChangesDetected = true;

                return true;
            }

            return false;
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

            GameEventMagicUpdateSpell updateSpellEvent = new GameEventMagicUpdateSpell(Session, spellId);
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
            new ActionChain(this, () =>
            {
                var entity = Biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spellId);

                if (entity == null)
                {
                    log.Error("Invalid spellId passed to Player.RemoveSpellFromSpellBook");
                    return;
                }

                Biota.BiotaPropertiesSpellBook.Remove(entity);
                entity.Object = null;

                if (ExistsInDatabase && entity.Id != 0)
                    DatabaseManager.Shard.RemoveEntity(entity, null);

                GameEventMagicRemoveSpellId removeSpellEvent = new GameEventMagicRemoveSpellId(Session, spellId);
                Session.Network.EnqueueSend(removeSpellEvent);
            }).EnqueueChain();
        }


        /// <summary>
        /// Will return the spells in the bar, sorted by their position
        /// </summary>
        public List<SpellBarPositions> GetSpellsInSpellBar(int barId)
        {
            var spells = new List<SpellBarPositions>();

            var results = Character.CharacterPropertiesSpellBar.Where(x => x.SpellBarNumber == barId);

            foreach (var result in results)
            {
                var entity = new SpellBarPositions(result.SpellBarNumber, result.SpellBarIndex, result.SpellId);

                spells.Add(entity);
            }

            spells.Sort((a, b) => a.SpellBarPositionId.CompareTo(b.SpellBarPositionId));

            return spells;
        }

        /// <summary>
        /// This method implements player spell bar management for - adding a spell to a specific spell bar (0 based) at a specific slot (0 based).
        /// </summary>
        public void HandleActionAddSpellFavorite(uint spellId, uint spellBarPositionId, uint spellBarId)
        {
            var spells = GetSpellsInSpellBar((int)spellBarId);

            if (spellBarPositionId > spells.Count + 1)
                spellBarPositionId = (uint)(spells.Count + 1);

            // We must increment the position of existing spells in the bar that exist on or after this position
            foreach (var property in Character.CharacterPropertiesSpellBar)
            {
                if (property.SpellBarNumber == spellBarId && property.SpellBarIndex >= spellBarPositionId)
                    property.SpellBarIndex++;
            }

            var entity = new CharacterPropertiesSpellBar { CharacterId = Biota.Id, SpellBarNumber = spellBarId, SpellBarIndex = spellBarPositionId, SpellId = spellId };

            Character.CharacterPropertiesSpellBar.Add(entity);
            ChangesDetected = true;
        }

        /// <summary>
        /// This method implements player spell bar management for - removing a spell to a specific spell bar (0 based)
        /// </summary>
        public void HandleActionRemoveSpellFavorite(uint spellId, uint spellBarId)
        {
            var entity = Character.CharacterPropertiesSpellBar.FirstOrDefault(x => x.SpellBarNumber == spellBarId && x.SpellId == spellId);

            if (entity != null)
            {
                // We must decrement the position of existing spells in the bar that exist after this position
                foreach (var property in Character.CharacterPropertiesSpellBar)
                {
                    if (property.SpellBarNumber == spellBarId && property.SpellBarIndex > entity.SpellBarIndex)
                    {
                        property.SpellBarIndex--;
                        ChangesDetected = true;
                    }
                }

                Character.CharacterPropertiesSpellBar.Remove(entity);

                if (ExistsInDatabase && entity.Id != 0)
                    DatabaseManager.Shard.RemoveEntity(entity, null);
            }
        }
    }
}
