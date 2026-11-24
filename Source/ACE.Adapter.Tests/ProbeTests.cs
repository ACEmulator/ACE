using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Adapter.GDLE;
using ACE.Adapter.Lifestoned;
using ACE.Adapter.Tests.Harness;
using ACE.Database.Models.World;
using ACE.Entity.Enum.Properties;

namespace ACE.Adapter.Tests
{
    /// <summary>
    /// One concrete assertion each, for behaviour the generic sweep cannot reach or can only
    /// see from one polarity.
    /// </summary>
    [TestClass]
    public class ProbeTests
    {
        [TestMethod]
        public void WeenieWithPartialAttributeSetIsExported()
        {
            // Any subset of the six attributes is valid; a missing one must not abort the export.
            var weenie = new Weenie { ClassId = 12345, Type = 1 };
            weenie.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Strength, InitLevel = 50 });

            using (var swallowed = new SwallowedExceptions())
            {
                var converted = LifestonedConverter.TryConvert(weenie, out var result);

                AssertConverted(converted, swallowed);
                Assert.AreEqual(50u, result.Attributes.Strength.Ranks);
            }
        }

        [TestMethod]
        public void WeenieWithPartialVitalSetIsExported()
        {
            // Doors and chests carry MaxHealth only.
            var weenie = new Weenie { ClassId = 12345, Type = 1 };
            weenie.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxHealth, InitLevel = 50 });

            using (var swallowed = new SwallowedExceptions())
            {
                var converted = LifestonedConverter.TryConvert(weenie, out var result);

                AssertConverted(converted, swallowed);
                Assert.AreEqual(50u, result.Attributes.Health.Ranks);
            }
        }

        [TestMethod]
        public void EmoteItemWithoutDestinationIsExported()
        {
            // Give / TakeItems / InqOwnsItems actions name an item without a destination.
            var before = RoundTripTests.WeeniePopulator(true).Create<Weenie>();
            var action = before.WeeniePropertiesEmote.First().WeeniePropertiesEmoteAction.First();
            action.DestinationType = null;

            var after = WeenieRoundTrip(before);

            var actionAfter = after.WeeniePropertiesEmote.First().WeeniePropertiesEmoteAction.First();
            Assert.AreEqual(action.WeenieClassId, actionAfter.WeenieClassId);
            Assert.AreEqual(action.StackSize, actionAfter.StackSize);
        }

        [TestMethod]
        public void FloatStatKeepsDoublePrecision()
        {
            // ACE stores PropertyFloat as double. The sweep covers emote MinDbl/MaxDbl/Percent
            // and skill LastUsedTime the same way.
            var before = RoundTripTests.WeeniePopulator(true).Create<Weenie>();
            before.WeeniePropertiesFloat.First().Value = 0.1;

            var after = WeenieRoundTrip(before);

            Assert.AreEqual(0.1, after.WeeniePropertiesFloat.First().Value);
        }

        [TestMethod]
        public void DoublesKeepPrecisionThroughJson()
        {
            var before = RoundTripTests.WeeniePopulator(true).Create<Weenie>();
            before.WeeniePropertiesFloat.First().Value = 0.1;
            before.WeeniePropertiesSkill.First().LastUsedTime = 123456789.123;

            var file = Path.GetTempFileName();
            try
            {
                Weenie after = null;
                using (var swallowed = new SwallowedExceptions())
                {
                    var converted = LifestonedConverter.TryConvertACEWeenieToLSDJSON(before, out var json, out _);
                    if (converted)
                        File.WriteAllText(file, json);
                    converted = converted && LifestonedLoader.TryLoadWeenieConverted(file, out after);

                    AssertConverted(converted, swallowed);
                }

                Assert.AreEqual(0.1, after.WeeniePropertiesFloat.First().Value);
                Assert.AreEqual(123456789.123, after.WeeniePropertiesSkill.First().LastUsedTime);
            }
            finally
            {
                File.Delete(file);
            }
        }

        [TestMethod]
        public void DuplicateDidStatImportsOnce()
        {
            // Every stat list skips duplicate keys on import, with or without correctForEnumShift.
            var input = new GDLE.Models.LSDWeenie
            {
                WeenieId = 12345,
                DidStats = new List<GDLE.Models.DidStat>
                {
                    new GDLE.Models.DidStat { Key = 1, Value = 2 },
                    new GDLE.Models.DidStat { Key = 1, Value = 3 },
                },
            };

            using (var swallowed = new SwallowedExceptions())
            {
                var converted = LifestonedConverter.TryConvert(input, out var result);

                AssertConverted(converted, swallowed);
                Assert.AreEqual(1, result.WeeniePropertiesDID.Count(d => d.Type == 1));
            }
        }

        [TestMethod]
        public void BoolRequirementMessageIsExported()
        {
            var recipe = new Recipe { Id = 7 };
            recipe.RecipeRequirementsBool.Add(new RecipeRequirementsBool { Index = 0, Stat = 1, Value = true, Enum = 1, Message = "You need to be bonded." });

            using (var swallowed = new SwallowedExceptions())
            {
                var converted = GDLEConverter.TryConvert(recipe, out GDLE.Models.Recipe result);

                AssertConverted(converted, swallowed);
                Assert.AreEqual("You need to be bonded.", result.Requirements[0].BoolRequirements[0].Message);
            }
        }

        [TestMethod]
        public void RecipeModWithNoTypedRowsRoundTrips()
        {
            // A mod's slot normally comes from its typed rows. Import creates mods with only
            // Health/Stamina/Mana/DataId, so export must find them a slot from ExecutesOnSuccess.
            var recipe = new Recipe { Id = 7 };
            recipe.RecipeMod.Add(new RecipeMod { ExecutesOnSuccess = true, Health = 5 });

            using (var swallowed = new SwallowedExceptions())
            {
                Recipe imported = null;
                var converted = GDLEConverter.TryConvert(recipe, out GDLE.Models.Recipe exported)
                    && GDLEConverter.TryConvert(exported, out imported);

                AssertConverted(converted, swallowed);
                Assert.AreEqual(1, imported.RecipeMod.Count);
                Assert.AreEqual(5, imported.RecipeMod.Single().Health);
            }
        }

        [TestMethod]
        public void UntypedRecipeModDoesNotTakeATypedModsSlot()
        {
            var recipe = new Recipe { Id = 7 };
            recipe.RecipeMod.Add(new RecipeMod { ExecutesOnSuccess = true, Health = 5 });
            var typed = new RecipeMod { ExecutesOnSuccess = true, Mana = 9 };
            typed.RecipeModsInt.Add(new RecipeModsInt { Index = 0, Stat = 1, Value = 2, Enum = 1 });
            recipe.RecipeMod.Add(typed);

            using (var swallowed = new SwallowedExceptions())
            {
                Recipe imported = null;
                var converted = GDLEConverter.TryConvert(recipe, out GDLE.Models.Recipe exported)
                    && GDLEConverter.TryConvert(exported, out imported);

                AssertConverted(converted, swallowed);
                Assert.AreEqual(2, imported.RecipeMod.Count);
                Assert.AreEqual(1, imported.RecipeMod.Single(m => m.Mana == 9).RecipeModsInt.Count);
                Assert.AreEqual(0, imported.RecipeMod.Single(m => m.Health == 5).RecipeModsInt.Count);
            }
        }

        [TestMethod]
        public void OutOfRangeLandblockGuidsAreReassignedAndLinksFollow()
        {
            const ushort landblock = 0xA9B4;
            const uint firstGuid = 0x70000000u | ((uint)landblock << 12);

            var input = new GDLE.Models.Landblock
            {
                key = (uint)landblock << 16,
                value = new GDLE.Models.LandblockValue
                {
                    weenies = new List<GDLE.Models.LandblockWeenie>
                    {
                        Instance(id: 5, landblock),
                        Instance(id: 9, landblock),
                    },
                    links = new List<GDLE.Models.LandblockLink>
                    {
                        new GDLE.Models.LandblockLink { target = 5, source = 9 },
                    },
                },
            };

            using (var swallowed = new SwallowedExceptions())
            {
                var converted = GDLEConverter.TryConvert(input, out var instances, out var links);

                AssertConverted(converted, swallowed);
                CollectionAssert.AreEqual(new[] { firstGuid, firstGuid + 1 }, instances.Select(i => i.Guid).ToArray());
                Assert.AreEqual(firstGuid, links.Single().ParentGuid);
                Assert.AreEqual(firstGuid + 1, links.Single().ChildGuid);
            }
        }

        [TestMethod]
        public void SwallowedExceptionsSeesCaughtExceptionsOnThisThreadOnly()
        {
            using (var swallowed = new SwallowedExceptions())
            {
                try
                {
                    throw new InvalidOperationException("caught here");
                }
                catch (InvalidOperationException)
                {
                }

                Task.Run(() =>
                {
                    try
                    {
                        throw new NotSupportedException("caught on another thread");
                    }
                    catch (NotSupportedException)
                    {
                    }
                }).Wait();

                Assert.AreEqual(1, swallowed.Exceptions.Count);
                Assert.IsInstanceOfType<InvalidOperationException>(swallowed.Exceptions[0]);
            }
        }

        private static GDLE.Models.LandblockWeenie Instance(uint id, ushort landblock)
        {
            return new GDLE.Models.LandblockWeenie
            {
                id = id,
                wcid = 1,
                pos = new GDLE.Models.Position
                {
                    LandCellId = ((uint)landblock << 16) | 1,
                    Frame = new GDLE.Models.Frame { Position = new GDLE.Models.XYZ(), Rotations = new GDLE.Models.Quaternion() },
                },
            };
        }

        private static Weenie WeenieRoundTrip(Weenie before)
        {
            using (var swallowed = new SwallowedExceptions())
            {
                Weenie after = null;
                var converted = LifestonedConverter.TryConvert(before, out var external)
                    && LifestonedConverter.TryConvert(external, out after);

                AssertConverted(converted, swallowed);
                return after;
            }
        }

        private static void AssertConverted(bool converted, SwallowedExceptions swallowed)
        {
            if (converted && swallowed.Exceptions.Count == 0)
                return;

            var detail = string.Join(Environment.NewLine, swallowed.Exceptions.Select(e => "  swallowed: " + SwallowedExceptions.Describe(e)));
            Assert.Fail($"Conversion {(converted ? "succeeded" : "FAILED")}{Environment.NewLine}{detail}");
        }
    }
}
