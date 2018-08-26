using System.Linq;

using ACE.Database.Models.Shard;
using ACE.DatLoader;
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
                ChangesDetected = true;

                GameEventMagicRemoveSpellId removeSpellEvent = new GameEventMagicRemoveSpellId(Session, spellId);
                Session.Network.EnqueueSend(removeSpellEvent);
            }).EnqueueChain();
        }
    }
}
