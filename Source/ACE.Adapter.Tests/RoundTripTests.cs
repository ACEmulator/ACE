using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Adapter.GDLE;
using ACE.Adapter.Lifestoned;
using ACE.Adapter.Tests.Harness;
using ACE.Database.Models.World;
using ACE.Entity.Enum.Properties;

namespace ACE.Adapter.Tests
{
    /// <summary>
    /// Generic sweep over every converter pair that exists in both directions: populate every
    /// member, convert ACE -> external -> ACE, and diff the result.
    /// </summary>
    /// <remarks>
    /// Each pair runs twice, with booleans seeded true and then false, because a dropped flag that
    /// defaults to false is invisible when the input is also false.
    ///
    /// Any difference not listed as expected fails the test, as does a swallowed exception. The
    /// expected lists are the complete record of what each round trip loses by design.
    /// </remarks>
    [TestClass]
    public class RoundTripTests
    {
        private static readonly ExpectedDifference DatabaseId =
            new ExpectedDifference("*.Id", "BY DESIGN: database surrogate key, not part of the external format");

        private static readonly ExpectedDifference LastModified =
            new ExpectedDifference("*.LastModified", "BY DESIGN: database audit timestamp, not part of the external format");

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void QuestRoundTrips(bool boolValue)
        {
            var before = new Populator(boolValue).Create<Quest>();

            RoundTrip.Run<Quest, GDLE.Models.Quest>("Quest", before, GDLEConverter.TryConvert, GDLEConverter.TryConvert)
                .AssertClean(DatabaseId, LastModified);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CookBookRoundTrips(bool boolValue)
        {
            var before = new Populator(boolValue).Create<CookBook>();

            RoundTrip.Run<CookBook, GDLE.Models.RecipePrecursor>("CookBook", before, GDLEConverter.TryConvert, GDLEConverter.TryConvert)
                .AssertClean(DatabaseId, LastModified,
                    new ExpectedDifference("CookBook.Recipe", "BY DESIGN: a precursor carries only RecipeId; the recipe travels separately (see RecipeCombinedRoundTrips)"));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void RecipeRoundTrips(bool boolValue)
        {
            var before = RecipePopulator(boolValue).Create<Recipe>();

            RoundTrip.Run<Recipe, GDLE.Models.Recipe>("Recipe", before, GDLEConverter.TryConvert, GDLEConverter.TryConvert)
                .AssertClean(RecipeExpectations("Recipe").Append(
                    new ExpectedDifference("Recipe.CookBook.Count", "BY DESIGN: cookbooks are exported as separate precursors (see RecipeCombinedRoundTrips)")).ToArray());
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void RecipeCombinedRoundTrips(bool boolValue)
        {
            var populator = RecipePopulator(boolValue);
            var recipe = populator.Create<Recipe>();
            recipe.CookBook = populator.CreateList<CookBook>();
            foreach (var cookBook in recipe.CookBook)
            {
                cookBook.Recipe = recipe;
                cookBook.RecipeId = recipe.Id;
            }
            var before = recipe.CookBook.ToList();

            RoundTrip.Run<List<CookBook>, GDLE.Models.RecipeCombined>("CookBooks", before,
                    GDLEConverter.TryConvert,
                    (GDLE.Models.RecipeCombined input, out List<CookBook> output) =>
                    {
                        // The recipe comes back as a separate out parameter; reattach it the way the
                        // database navigation would.
                        if (!GDLEConverter.TryConvert(input, out output, out var importedRecipe))
                            return false;
                        foreach (var cookBook in output)
                            cookBook.Recipe = importedRecipe;
                        return true;
                    })
                .AssertClean(RecipeExpectations("CookBooks[*].Recipe"));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void LandblockRoundTrips(bool boolValue)
        {
            const ushort landblock = 0xA9B4;

            var populator = new Populator(boolValue);
            var before = populator.CreateList<LandblockInstance>();
            for (var i = 0; i < before.Count; i++)
            {
                before[i].Guid = 0x70000000u | ((uint)landblock << 12) | (uint)i;
                before[i].ObjCellId = ((uint)landblock << 16) | (uint)(i + 1);
                before[i].Landblock = landblock;
                before[i].LandblockInstanceLink = new List<LandblockInstanceLink>();
            }

            // Instance 0 is the parent of instance 1.
            before[0].IsLinkChild = false;
            before[1].IsLinkChild = true;
            var link = populator.Create<LandblockInstanceLink>();
            link.ParentGuid = before[0].Guid;
            link.ChildGuid = before[1].Guid;
            before[0].LandblockInstanceLink.Add(link);

            RoundTrip.Run<List<LandblockInstance>, GDLE.Models.Landblock>("Instances", before,
                    GDLEConverter.TryConvert,
                    (GDLE.Models.Landblock input, out List<LandblockInstance> output) =>
                    {
                        // Links come back as a separate out parameter; attach them to their parent
                        // the way the database navigation would.
                        if (!GDLEConverter.TryConvert(input, out output, out var links))
                            return false;
                        foreach (var instance in output)
                            instance.LandblockInstanceLink = links.Where(l => l.ParentGuid == instance.Guid).ToList();
                        return true;
                    })
                .AssertClean(DatabaseId, LastModified,
                    new ExpectedDifference("Instances[*].Landblock", "BY DESIGN: computed column (ObjCellId >> 16), deliberately not assigned on import"),
                    new ExpectedDifference("Instances[*].IsLinkChild", "BY DESIGN: the converter leaves this to its caller; GDLELoader.TryConvertLandblocks sets it from the links"));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void WeenieRoundTrips(bool boolValue)
        {
            var before = WeeniePopulator(boolValue).Create<Weenie>();

            RoundTrip.Run<Weenie, GDLE.Models.LSDWeenie>("Weenie", before,
                    (Weenie input, out GDLE.Models.LSDWeenie output) => LifestonedConverter.TryConvert(input, out output),
                    (GDLE.Models.LSDWeenie input, out Weenie output) => LifestonedConverter.TryConvert(input, out output))
                .AssertClean(DatabaseId, LastModified,
                    new ExpectedDifference("Weenie.*.ObjectId", "BY DESIGN: foreign key to the weenie, assigned by the database"),
                    new ExpectedDifference("Weenie.WeeniePropertiesEmote[*].WeeniePropertiesEmoteAction[*].EmoteId", "BY DESIGN: foreign key to the emote, assigned by the database"),
                    new ExpectedDifference("Weenie.ClassName", "BY DESIGN: not in the Lifestoned format; import derives it from the WCID"),
                    new ExpectedDifference("Weenie.WeeniePropertiesAnimPart.Count", "FORMAT GAP: LSDWeenie has no anim part member"),
                    new ExpectedDifference("Weenie.WeeniePropertiesEventFilter.Count", "FORMAT GAP: LSDWeenie has no event filter member"),
                    new ExpectedDifference("Weenie.WeeniePropertiesPalette.Count", "FORMAT GAP: LSDWeenie has no palette member"),
                    new ExpectedDifference("Weenie.WeeniePropertiesTextureMap.Count", "FORMAT GAP: LSDWeenie has no texture map member"));
        }

        private static ExpectedDifference[] RecipeExpectations(string root)
        {
            return new[]
            {
                DatabaseId,
                LastModified,
                new ExpectedDifference($"{root}.*.RecipeId", "BY DESIGN: foreign key to the recipe, assigned by the database"),
                new ExpectedDifference($"{root}.RecipeMod[*].*.RecipeModId", "BY DESIGN: foreign key to the recipe mod, assigned by the database"),
                new ExpectedDifference($"{root}.RecipeRequirementsString[*].Message", "FORMAT GAP: GDLE StringRequirement has no Message member"),
            };
        }

        /// <summary>
        /// GDLE stores recipe requirements in three slots and mods in eight, so each row's Index
        /// must be in range. Every typed row on one RecipeMod shares that mod's slot, and the
        /// slot encodes ExecutesOnSuccess (0-3 success, 4-7 failure).
        /// </summary>
        internal static Populator RecipePopulator(bool boolValue)
        {
            return new Populator(boolValue)
                .With<RecipeRequirementsInt>(nameof(RecipeRequirementsInt.Index), (_, i) => (sbyte)i)
                .With<RecipeRequirementsDID>(nameof(RecipeRequirementsDID.Index), (_, i) => (sbyte)i)
                .With<RecipeRequirementsIID>(nameof(RecipeRequirementsIID.Index), (_, i) => (sbyte)i)
                .With<RecipeRequirementsFloat>(nameof(RecipeRequirementsFloat.Index), (_, i) => (sbyte)i)
                .With<RecipeRequirementsString>(nameof(RecipeRequirementsString.Index), (_, i) => (sbyte)i)
                .With<RecipeRequirementsBool>(nameof(RecipeRequirementsBool.Index), (_, i) => (sbyte)i)
                .With<RecipeMod>(nameof(RecipeMod.ExecutesOnSuccess), (_, i) => i <= 3)
                .With<RecipeMod>(nameof(RecipeMod.RecipeModsInt), (p, i) => SlotRows<RecipeModsInt>(p, i, (row, slot) => row.Index = slot))
                .With<RecipeMod>(nameof(RecipeMod.RecipeModsDID), (p, i) => SlotRows<RecipeModsDID>(p, i, (row, slot) => row.Index = slot))
                .With<RecipeMod>(nameof(RecipeMod.RecipeModsIID), (p, i) => SlotRows<RecipeModsIID>(p, i, (row, slot) => row.Index = slot))
                .With<RecipeMod>(nameof(RecipeMod.RecipeModsFloat), (p, i) => SlotRows<RecipeModsFloat>(p, i, (row, slot) => row.Index = slot))
                .With<RecipeMod>(nameof(RecipeMod.RecipeModsString), (p, i) => SlotRows<RecipeModsString>(p, i, (row, slot) => row.Index = slot))
                .With<RecipeMod>(nameof(RecipeMod.RecipeModsBool), (p, i) => SlotRows<RecipeModsBool>(p, i, (row, slot) => row.Index = slot));
        }

        private static List<T> SlotRows<T>(Populator populator, int slot, Action<T, sbyte> setIndex)
        {
            var rows = populator.CreateList<T>();
            foreach (var row in rows)
                setIndex(row, (sbyte)slot);
            return rows;
        }

        /// <summary>
        /// Attribute and vital rows are keyed by type, so the sweep supplies one complete set
        /// (partial sets are covered in <see cref="ProbeTests"/>). Book pages and emote actions
        /// are renumbered 0..n on export, so they start out numbered that way.
        /// </summary>
        internal static Populator WeeniePopulator(bool boolValue)
        {
            return new Populator(boolValue)
                .With<Weenie>(nameof(Weenie.WeeniePropertiesAttribute), (p, _) =>
                    Enumerable.Range(1, 6).Select(type =>
                    {
                        var attribute = p.Create<WeeniePropertiesAttribute>();
                        attribute.Type = (ushort)type;
                        return attribute;
                    }).ToList())
                .With<Weenie>(nameof(Weenie.WeeniePropertiesAttribute2nd), (p, _) =>
                    new[] { PropertyAttribute2nd.MaxHealth, PropertyAttribute2nd.MaxStamina, PropertyAttribute2nd.MaxMana }.Select(type =>
                    {
                        var vital = p.Create<WeeniePropertiesAttribute2nd>();
                        vital.Type = (ushort)type;
                        return vital;
                    }).ToList())
                .With<WeeniePropertiesBookPageData>(nameof(WeeniePropertiesBookPageData.PageId), (_, i) => (uint)i)
                .With<WeeniePropertiesEmoteAction>(nameof(WeeniePropertiesEmoteAction.Order), (_, i) => (uint)i);
        }
    }
}
