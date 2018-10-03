using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Database;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The Spell class for game code
    /// A wrapper around SpellBase and Database.Spell
    /// </summary>
    public partial class Spell
    {
        /// <summary>
        /// The spell information from the client DAT
        /// </summary>
        public SpellBase _spellBase;

        /// <summary>
        /// The spell information from the server DB
        /// </summary>
        public Database.Models.World.Spell _spell;

        /// <summary>
        /// Returns TRUE if spell is missing from either the client DAT or the server spell db
        /// </summary>
        public bool NotFound { get => _spellBase == null || _spell == null; }

        /// <summary>
        /// The components required to cast the spell
        /// </summary>
        public SpellFormula Formula;

        /// <summary>
        /// Constructs a Spell from a spell ID
        /// </summary>
        /// <param name="loadDB">If FALSE, only loads the DAT info (faster)</param>
        public Spell(uint spellID, bool loadDB = true)
        {
            Init(spellID, loadDB);
        }

        /// <summary>
        /// Constructs a Spell from a spell ID
        /// </summary>
        public Spell(int spellID, bool loadDB = true)
        {
            Init((uint)spellID, loadDB);
        }

        /// <summary>
        /// Constructs a Spell from a Spell enum
        /// </summary>
        public Spell(Network.Enum.Spell spell, bool loadDB = true)
        {
            Init((uint)spell, loadDB);
        }

        /// <summary>
        /// Default initializer
        /// </summary>
        public void Init(uint spellID, bool loadDB = true)
        {
            DatManager.PortalDat.SpellTable.Spells.TryGetValue(spellID, out _spellBase);

            if (loadDB)
                _spell = DatabaseManager.World.GetCachedSpell(spellID);

            if (_spellBase != null)
                Formula = new SpellFormula(this, _formula);
        }

        /// <summary>
        /// Uses the server spell level formula,
        /// which checks the power level of the spell,
        /// and compares to the minimum power for each spell level.
        /// </summary>
        public uint Level
        {
            // this uses the server / power-based formula
            // for the client / scarab-based formula, use Formula.Level
            get
            {
                for (uint spellLevel = SpellFormula.MaxSpellLevel; spellLevel > 0; spellLevel--)
                {
                    var minPower = SpellFormula.MinPower[spellLevel];
                    if (Power >= minPower)
                        return spellLevel;
                }
                return 0;
            }
        }

        /// <summary>
        /// Returns TRUE if the spell levels are the same between the client and server formulas
        /// </summary>
        public bool LevelMatch { get => Formula.Level == Level; }

        /// <summary>
        /// Returns TRUE if this is a beneficial spell
        /// </summary>
        public bool IsBeneficial
        {
            get
            {
                // TODO: item enchantment?
                // is all of this logic even needed,
                // or can SpellFlags.Beneficial just be used?

                // All War and Void spells are harmful
                if (School == MagicSchool.WarMagic || School == MagicSchool.VoidMagic)
                    return false;

                // Life Magic spells that don't have bit three of their bitfield property set are harmful
                if (School == MagicSchool.LifeMagic && !Flags.HasFlag(SpellFlags.Beneficial))
                    return false;

                // Creature Magic spells that don't have bit three of their bitfield property set are harmful
                if (School == MagicSchool.CreatureEnchantment && !Flags.HasFlag(SpellFlags.Beneficial))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Returns TRUE if this is a hamrful spell
        /// </summary>
        public bool IsHarmful { get => !IsBeneficial; }
    }
}
