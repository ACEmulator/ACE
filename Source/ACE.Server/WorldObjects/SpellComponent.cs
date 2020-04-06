using System;
using System.Collections.Generic;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Models;

namespace ACE.Server.WorldObjects
{
    public class SpellComponent : Stackable
    {
        /// <summary>
        /// The DAT file containing DualDidMapper for spell component ids => wcids
        /// </summary>
        public static uint SpellComponentDIDs = 0x27000002;

        /// <summary>
        /// The spell components table from the portal.dat
        /// </summary>
        public static SpellComponentsTable SpellComponentsTable { get => DatManager.PortalDat.SpellComponentsTable; }

        /// <summary>
        /// A lookup table of spell component wcids => component ids
        /// </summary>
        public static Dictionary<uint, uint> SpellComponentWCIDs;

        static SpellComponent()
        {
            BuildSpellComponentWCIDs();
        }

        public static void BuildSpellComponentWCIDs()
        {
            SpellComponentWCIDs = new Dictionary<uint, uint>();

            var dualDIDs = DatManager.PortalDat.ReadFromDat<DualDidMapper>(SpellComponentDIDs);

            foreach (var component_id in SpellComponentsTable.SpellComponents.Keys)
            {
                if (!dualDIDs.ClientEnumToID.TryGetValue(component_id, out var wcid))
                {
                    Console.WriteLine($"BuildSpellComponentWCIDs({component_id}): couldn't find component ID");
                    continue;
                }
                SpellComponentWCIDs.Add(wcid, component_id);
            }
        }

        /// <summary>
        /// Returns TRUE if the input wcid is a valid spell component
        /// </summary>
        public static bool IsValid(uint wcid)
        {
            return SpellComponentWCIDs.ContainsKey(wcid);
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public SpellComponent(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public SpellComponent(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }
    }
}
