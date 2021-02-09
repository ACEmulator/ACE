using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity.Enum;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// A spell component with a power level between 1-10,
    /// that determines the windup motion
    /// </summary>
    public enum Scarab
    {
        Lead     = 1,
        Iron     = 2,
        Copper   = 3,
        Silver   = 4,
        Gold     = 5,
        Pyreal   = 6,
        Diamond  = 110,
        Platinum = 112,
        Dark     = 192,
        Mana     = 193
    }

    /// <summary>
    /// The components required to cast a spell
    /// </summary>
    public class SpellFormula
    {
        /// <summary>
        /// A mapping of scarabs => their spell levels
        /// If the first component in a spell is a scarab,
        /// the client uses this to determine the spell level,
        /// for things like the spellbook filters.
        /// </summary>
        public static Dictionary<Scarab, uint> ScarabLevel = new Dictionary<Scarab, uint>()
        {
            { Scarab.Lead,     1 },
            { Scarab.Iron,     2 },
            { Scarab.Copper,   3 },
            { Scarab.Silver,   4 },
            { Scarab.Gold,     5 },
            { Scarab.Pyreal,   6 },
            { Scarab.Diamond,  6 },
            { Scarab.Platinum, 7 },
            { Scarab.Dark,     7 },
            { Scarab.Mana,     8 }
        };

        /// <summary>
        /// A mapping of scarabs => their power levels
        /// </summary>
        public static Dictionary<Scarab, uint> ScarabPower = new Dictionary<Scarab, uint>()
        {
            { Scarab.Lead,     1 },
            { Scarab.Iron,     2 },
            { Scarab.Copper,   3 },
            { Scarab.Silver,   4 },
            { Scarab.Gold,     5 },
            { Scarab.Pyreal,   6 },
            { Scarab.Diamond,  7 },
            { Scarab.Platinum, 8 },
            { Scarab.Dark,     9 },
            { Scarab.Mana,    10 }
        };

        /// <summary>
        /// Returns the spell level for a scarab
        /// </summary>
        public static uint GetLevel(Scarab scarab)
        {
            return ScarabLevel[scarab];
        }

        /// <summary>
        /// Returns the power level for a scarab
        /// </summary>
        public static uint GetPower(Scarab scarab)
        {
            return ScarabPower[scarab];
        }

        /// <summary>
        /// The maximum spell level in retail
        /// </summary>
        public static uint MaxSpellLevel = 8;

        /// <summary>
        /// A mapping of spell levels => minimum power
        /// </summary>
        public static Dictionary<uint, uint> MinPower = new Dictionary<uint, uint>()
        {
            { 1,   1 },
            { 2,  50 },
            { 3, 100 },
            { 4, 150 },
            { 5, 200 },
            { 6, 250 },
            { 7, 300 },
            { 8, 400 }
        };

        /// <summary>
        /// Returns TRUE if this spell component is a scarab
        /// </summary>
        /// <param name="componentID">The ID from the spell components table</param>
        public static bool IsScarab(uint componentID)
        {
            return Enum.IsDefined(typeof(Scarab), (int)componentID);
        }

        /// <summary>
        /// The spell for this formula
        /// </summary>
        public Spell Spell;

        /// <summary>
        /// The spell component IDs
        /// from the spell components table in portal.dat (0x0E00000F)
        /// </summary>
        public List<uint> Components;

        /// <summary>
        /// The spell components for the individual player
        /// uses a hashing algorithm based on player name
        /// </summary>
        public List<uint> PlayerFormula;

        /// <summary>
        /// The scarab + prismatic taper formula
        /// applies if player has a foci for current magic school
        /// </summary>
        public List<uint> FociFormula;

        /// <summary>
        /// The current spell formula for the player
        /// either PlayerFormula or FociFormula
        /// </summary>
        public List<uint> CurrentFormula;

        /// <summary>
        /// Constructs a SpellFormula from a list of components
        /// </summary>
        /// <param name="spell">The spell for this formula</param>
        /// <param name="components">The list of components required to cast the spell</param>
        public SpellFormula(Spell spell, List<uint> components)
        {
            Spell = spell;
            Components = components;
        }

        /// <summary>
        /// Returns a list of scarabs in the spell formula
        /// </summary>
        public List<Scarab> Scarabs
        {
            get
            {
                var scarabs = new List<Scarab>();

                foreach (var component in Components)
                    if (IsScarab(component))
                        scarabs.Add((Scarab)component);

                return scarabs;
            }
        }

        /// <summary>
        /// Uses the client spell level formula, which is used for things like spell filtering
        /// a 'rough heuristic' based on the first component of the spell, which is expected to be a scarab
        /// </summary>
        public uint Level
        {
            get
            {
                if (Components == null || Components.Count == 0)
                    return 0;

                var firstComp = Components[0];
                if (!IsScarab(firstComp))
                    return 0;

                return ScarabLevel[(Scarab)firstComp];
            }
        }

        /// <summary>
        /// Power is used to determine, among possibly thing things, the number of Prisimatic Tapers in a "Scarab Only Formula" (foci)
        /// </summary>
        public uint Power
        {
            get
            {
                if (Components == null || Components.Count == 0)
                    return 0;

                var firstComp = Components[0];
                if (!IsScarab(firstComp))
                    return 0;

                return ScarabPower[(Scarab)firstComp];
            }
        }

        /// <summary>
        /// The spell table from the portal.dat
        /// </summary>
        public static SpellTable SpellTable { get => DatManager.PortalDat.SpellTable; }

        /// <summary>
        /// The spell components table from the portal.dat
        /// </summary>
        public static SpellComponentsTable SpellComponentsTable { get => DatManager.PortalDat.SpellComponentsTable; }

        /// <summary>
        /// Builds the pseudo-randomized spell formula
        /// based on account name
        /// </summary>
        public List<uint> GetPlayerFormula(Player player)
        {
            PlayerFormula = SpellTable.GetSpellFormula(SpellTable, Spell.Id, player.Session.Account);
            FociFormula = GetFociFormula();

            GetCurrentFormula(player);

            return PlayerFormula;
        }

        /// <summary>
        /// For monsters with PropertyBool.AiUseHumanMagicAnimations
        /// </summary>
        public List<uint> GetMonsterFormula()
        {
            return PlayerFormula = SpellTable.GetSpellFormula(SpellTable, Spell.Id, "");
        }

        /// <summary>
        /// Returns the windup gesture from all the scarabs
        /// </summary>
        public List<MotionCommand> WindupGestures
        {
            get
            {
                var windupGestures = new List<MotionCommand>();

                foreach (var scarab in Scarabs)
                {
                    SpellComponentsTable.SpellComponents.TryGetValue((uint)scarab, out var component);
                    if (component == null)
                    {
                        Console.WriteLine($"SpellFormula.WindupGestures error: spell ID {Spell.Id} contains scarab {scarab} not found in components table, skipping");
                        continue;
                    }
                    windupGestures.Add((MotionCommand)component.Gesture);
                }
                return windupGestures;
            }
        }

        public bool HasWindupGestures => Scarabs.Any(i => i != Scarab.Lead);

        /// <summary>
        /// Returns the spell casting gesture, after the initial windup(s) are completed
        /// Based on the talisman (assumed to be the last spell component)
        /// </summary>
        public MotionCommand CastGesture
        {
            get
            {
                if (PlayerFormula == null || PlayerFormula.Count == 0)
                    return MotionCommand.Invalid;

                // ensure talisman
                SpellComponentsTable.SpellComponents.TryGetValue(PlayerFormula.Last(), out var talisman);
                if (talisman == null || talisman.Type != (uint)SpellComponentsTable.Type.Talisman)
                {
                    Console.WriteLine($"SpellFormula.CastGesture error: spell ID {Spell.Id} last component not talisman!");
                    return MotionCommand.Invalid;
                }
                return (MotionCommand)talisman.Gesture;
            }
        }

        /// <summary>
        /// A mapping of scarabs => PlayScript scales
        /// This determines the scale of the glowing blue/purple ball of energy during the windup motion
        /// </summary>
        public static Dictionary<Scarab, float> ScarabScale = new Dictionary<Scarab, float>()
        {
            { Scarab.Lead,     0.05f },
            { Scarab.Iron,     0.2f },
            { Scarab.Copper,   0.4f },
            { Scarab.Silver,   0.5f },
            { Scarab.Gold,     0.6f },
            { Scarab.Pyreal,   1.0f },
            { Scarab.Diamond,  1.0f },  // verify onward
            { Scarab.Platinum, 1.0f },
            { Scarab.Dark,     1.0f },
            { Scarab.Mana,     1.0f }
        };

        public Scarab FirstScarab { get => Scarabs.First(); }

        /// <summary>
        /// Returns a simple scale for the spell formula,
        /// based on the first scarab
        /// </summary>
        public float Scale { get => ScarabScale[FirstScarab]; }

        /// <summary>
        /// Returns the total casting time,
        /// based on windup + cast gestures
        /// </summary>
        public float GetCastTime(uint motionTableID, float speed, MotionCommand? weaponCastGesture = null)
        {
            var windupMotion = WindupGestures.First();
            var castMotion = weaponCastGesture ?? CastGesture;

            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(motionTableID);

            var windupTime = 0.0f;
            //var windupTime = motionTable.GetAnimationLength(MotionStance.Magic, windupMotion) / speed;
            foreach (var motion in WindupGestures)
                windupTime += motionTable.GetAnimationLength(MotionStance.Magic, motion) / speed;

            var castTime = motionTable.GetAnimationLength(MotionStance.Magic, castMotion) / speed;

            // FastCast = no windup motion
            if (Spell.Flags.HasFlag(SpellFlags.FastCast) || weaponCastGesture != null)
                return castTime;

            return windupTime + castTime;
        }

        public List<uint> GetFociFormula()
        {
            FociFormula = new List<uint>();

            // Add all the scarabs (remember, some spells have multiple scarabs, like Ring spells)
            for (var i = 0; i < Components.Count; i++)
            {
                var component = Components[i];
                if (IsScarab(component) || component == 111)    // added: chorizite, as per client
                    FociFormula.Add(component);
            }

            // Number of Prismatic Tapers is based on the spell "power"
            // See client CSpellBase::InqScarabOnlyFormula
            var numTapers = 0;
            switch (Power)
            {
                case 1:
                    numTapers = 1;
                    break;
                case 2:
                    numTapers = 2;
                    break;
                case 3:
                case 4:
                case 7:
                    numTapers = 3;
                    break;
                case 5:
                case 6:
                case 8:
                case 9:
                case 10:
                    numTapers = 4;
                    break;
            }

            for (var i = 0; i < numTapers; i++)
                FociFormula.Add(188);   // prismatic taper

            return FociFormula;
        }

        public void GetCurrentFormula(Player player)
        {
            CurrentFormula = player.HasFoci(Spell.School) ? FociFormula : PlayerFormula;
        }

        /// <summary>
        /// Returns a mapping of component wcid => number required
        /// </summary>
        public Dictionary<uint, int> GetRequiredComps()
        {
            var compsRequired = new Dictionary<uint, int>();

            foreach (var component in CurrentFormula)
            {
                var wcid = Spell.GetComponentWCID(component);

                if (compsRequired.ContainsKey(wcid))
                    compsRequired[wcid]++;
                else
                    compsRequired.Add(wcid, 1);
            }
            return compsRequired;
        }
    }
}
