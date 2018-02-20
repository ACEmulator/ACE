using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
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
        public bool AddKnownSpell(int spellId)
        {
            var result = Biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spellId);

            if (result == null)
            {
                result = new BiotaPropertiesSpellBook { ObjectId = Biota.Id, Spell = spellId };

                Biota.BiotaPropertiesSpellBook.Add(result);

                return true;
            }

            return false;
        }

        public void LearnSpell(uint spellId)
        {
            var spells = DatManager.PortalDat.SpellTable;

            if (!spells.Spells.ContainsKey(spellId))
            {
                GameMessageSystemChat errorMessage = new GameMessageSystemChat("SpellID not found in Spell Table", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(errorMessage);
                return;
            }

            if (SpellIsKnown(spellId))
            {
                GameMessageSystemChat errorMessage = new GameMessageSystemChat("That spell is already known", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(errorMessage);
                return;
            }

            Biota.BiotaPropertiesSpellBook.Add(new BiotaPropertiesSpellBook { ObjectId = Biota.Id, Spell = (int)spellId });

            GameEventMagicUpdateSpell updateSpellEvent = new GameEventMagicUpdateSpell(Session, spellId);
            Session.Network.EnqueueSend(updateSpellEvent);

            // Always seems to be this SkillUpPurple effect
            Session.Player.ApplyVisualEffects(ACE.Entity.Enum.PlayScript.SkillUpPurple);

            string spellName = spells.Spells[spellId].Name;
            string message = "You learn the " + spellName + " spell.\n";
            GameMessageSystemChat learnMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            Session.Network.EnqueueSend(learnMessage);
        }

        public void RemoveSpellIdGameAction(uint spellId)
        {
            var result = Biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spellId);

            if (result == null)
            {
                log.Error("Invalid spellId passed to Player.RemoveSpellFromSpellBook");
                return;
            }

            DatabaseManager.Shard.RemoveSpellBookEntry(result, null);

            Biota.BiotaPropertiesSpellBook.Remove(result);

            GameEventMagicRemoveSpellId removeSpellEvent = new GameEventMagicRemoveSpellId(Session, spellId);
            Session.Network.EnqueueSend(removeSpellEvent);
        }


        /// <summary>
        /// This method implements player spell bar management for - adding a spell to a specific spell bar (0 based) at a specific slot (0 based).
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="spellBarPositionId"></param>
        /// <param name="spellBarId"></param>
        public void AddSpellFavoriteGameAction(uint spellId, uint spellBarPositionId, uint spellBarId)
        {
            // The spell bar magic happens here.
            /*SpellsInSpellBars.Add(new AceObjectPropertiesSpellBarPositions()
            {
                AceObjectId = AceObject.AceObjectId,
                SpellId = spellId,
                SpellBarId = spellBarId,
                SpellBarPositionId = spellBarPositionId
            });*/
        }

        /// <summary>
        /// This method implements player spell bar management for - removing a spell to a specific spell bar (0 based)
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="spellBarId"></param>
        public void RemoveSpellFavoriteGameAction(uint spellId, uint spellBarId)
        {
            /*// More spell bar magic happens here.
            SpellsInSpellBars.Remove(SpellsInSpellBars.Single(x => x.SpellBarId == spellBarId && x.SpellId == spellId));
            // Now I have to reorder
            var sorted = SpellsInSpellBars.FindAll(x => x.AceObjectId == AceObject.AceObjectId && x.SpellBarId == spellBarId).OrderBy(s => s.SpellBarPositionId);
            uint newSpellBarPosition = 0;
            foreach (AceObjectPropertiesSpellBarPositions spells in sorted)
            {
                spells.SpellBarPositionId = newSpellBarPosition;
                newSpellBarPosition++;
            }*/
        }

        public List<SpellBarPositions> GetSpellsInSpellBar(int barId)
        {
            throw new System.NotImplementedException();
        }
    }
}
