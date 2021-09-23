using ACE.Common;
using ACE.Database.Models.World;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateRandomScroll(TreasureDeath profile, TreasureRoll roll = null)
        {
            // level 8 spell components shouldn't be in here,
            // they should be associated with TreasureItemType.SpellComponent (peas)
            if (roll == null && profile.Tier >= 7)
            {
                // According to wiki, Tier 7 has a chance for level 8 spell components or level 7 spell scrolls (as does Tier 8)
                // No indication of weighting in either direction, so assuming a 50/50 split
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

                if (rng < 0.5f)
                {
                    var wcid = RollLevel8SpellComp();

                    return WorldObjectFactory.CreateNewWorldObject((uint)wcid);
                }
            }
            var spellLevel = ScrollLevelChance.Roll(profile);

            // todo: switch to SpellLevelProgression
            var spellId = SpellId.Undef;
            do
            {
                var spellIdx = ThreadSafeRandom.Next(0, ScrollSpells.Table.Length - 1);

                spellId = ScrollSpells.Table[spellIdx][spellLevel - 1];
            }
            while (spellId == SpellId.Undef);   // simple way of handling spells that start at level 3 (blasts, volleys)

            var weenie = DatabaseManager.World.GetScrollWeenie((uint)spellId);

            if (weenie == null)
            {
                log.DebugFormat("CreateRandomScroll for tier {0} and spellID of {1} returned null from the database.", profile.Tier, spellId);
                return null;
            }

            return WorldObjectFactory.CreateNewWorldObject(weenie.WeenieClassId);
        }

        private static int RollLevel8SpellComp()
        {
            var rng = ThreadSafeRandom.Next(0, LootTables.Level8SpellComps.Length - 1);

            return LootTables.Level8SpellComps[rng];
        }
    }
}
