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
    }
}
