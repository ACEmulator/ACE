using System.Collections.Generic;
using System.Linq;

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
        /// <summary>
        /// Will return true if the spell was added, or false if the spell already exists.
        /// </summary>
        public bool AddSpell(int spellId)
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










        public bool UnknownSpell(uint spellId)
        {
            return !(AceObject.SpellIdProperties.Exists(x => x.SpellId == spellId));
        }

        public void MagicRemoveSpellId(uint spellId)
        {
            if (!AceObject.SpellIdProperties.Exists(x => x.SpellId == spellId))
            {
                log.Error("Invalid spellId passed to Player.RemoveSpellFromSpellBook");
                return;
            }

            AceObject.SpellIdProperties.RemoveAt(AceObject.SpellIdProperties.FindIndex(x => x.SpellId == spellId));
            GameEventMagicRemoveSpellId removeSpellEvent = new GameEventMagicRemoveSpellId(Session, spellId);
            Session.Network.EnqueueSend(removeSpellEvent);
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

            if (!UnknownSpell(spellId))
            {
                GameMessageSystemChat errorMessage = new GameMessageSystemChat("That spell is already known", ChatMessageType.Broadcast);
                Session.Network.EnqueueSend(errorMessage);
                return;
            }

            AceObjectPropertiesSpell newSpell = new AceObjectPropertiesSpell
            {
                AceObjectId = this.Guid.Full,
                SpellId = spellId
            };

            AceObject.SpellIdProperties.Add(newSpell);
            GameEventMagicUpdateSpell updateSpellEvent = new GameEventMagicUpdateSpell(Session, spellId);
            Session.Network.EnqueueSend(updateSpellEvent);

            // Always seems to be this SkillUpPurple effect
            Session.Player.ApplyVisualEffects(global::ACE.Entity.Enum.PlayScript.SkillUpPurple);

            string spellName = spells.Spells[spellId].Name;
            string message = "You learn the " + spellName + " spell.\n";
            GameMessageSystemChat learnMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            Session.Network.EnqueueSend(learnMessage);
        }


        /// <summary>
        /// This method implements player spell bar management for - adding a spell to a specific spell bar (0 based) at a specific slot (0 based).
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="spellBarPositionId"></param>
        /// <param name="spellBarId"></param>
        public void AddSpellToSpellBar(uint spellId, uint spellBarPositionId, uint spellBarId)
        {
            // The spell bar magic happens here.
            SpellsInSpellBars.Add(new AceObjectPropertiesSpellBarPositions()
            {
                AceObjectId = AceObject.AceObjectId,
                SpellId = spellId,
                SpellBarId = spellBarId,
                SpellBarPositionId = spellBarPositionId
            });
        }

        /// <summary>
        /// This method implements player spell bar management for - removing a spell to a specific spell bar (0 based)
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="spellBarId"></param>
        public void RemoveSpellToSpellBar(uint spellId, uint spellBarId)
        {
            // More spell bar magic happens here.
            SpellsInSpellBars.Remove(SpellsInSpellBars.Single(x => x.SpellBarId == spellBarId && x.SpellId == spellId));
            // Now I have to reorder
            var sorted = SpellsInSpellBars.FindAll(x => x.AceObjectId == AceObject.AceObjectId && x.SpellBarId == spellBarId).OrderBy(s => s.SpellBarPositionId);
            uint newSpellBarPosition = 0;
            foreach (AceObjectPropertiesSpellBarPositions spells in sorted)
            {
                spells.SpellBarPositionId = newSpellBarPosition;
                newSpellBarPosition++;
            }
        }

        public List<AceObjectPropertiesSpellBarPositions> SpellsInSpellBars
        {
            get => AceObject.SpellsInSpellBars;
            set
            {
                AceObject.SpellsInSpellBars = value;
            }
        }
    }
}
