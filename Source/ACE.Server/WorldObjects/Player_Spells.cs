using System.Linq;

using ACE.Database.Models.Shard;

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
    }
}
