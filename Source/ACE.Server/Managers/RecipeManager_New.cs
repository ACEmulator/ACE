using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public partial class RecipeManager
    {
        public static Dictionary<uint, Dictionary<uint, uint>> Precursors;

        public static void ReadJSON()
        {
            // read recipeprecursors.json
            // tool -> target -> recipe
            var json = File.ReadAllText(@"json\recipeprecursors.json");
            var precursors = JsonConvert.DeserializeObject<List<RecipePrecursor>>(json);
            Precursors = new Dictionary<uint, Dictionary<uint, uint>>();

            foreach (var precursor in precursors)
            {
                Dictionary<uint, uint> tool = null;
                if (!Precursors.TryGetValue(precursor.Tool, out tool))
                {
                    tool = new Dictionary<uint, uint>();
                    Precursors.Add(precursor.Tool, tool);
                }
                tool[precursor.Target] = precursor.RecipeID;
            }
        }

        public static Recipe GetNewRecipe(Player player, WorldObject source, WorldObject target)
        {
            Recipe recipe = null;

            switch ((WeenieClassName)source.WeenieClassId)
            {
                case WeenieClassName.W_POTDYEDARKGREEN_CLASS:
                case WeenieClassName.W_POTDYEDARKRED_CLASS:
                case WeenieClassName.W_POTDYEDARKYELLOW_CLASS:
                case WeenieClassName.W_POTDYEWINTERBLUE_CLASS:
                case WeenieClassName.W_POTDYEWINTERGREEN_CLASS:
                case WeenieClassName.W_POTDYEWINTERSILVER_CLASS:
                case WeenieClassName.W_POTDYESPRINGBLACK_CLASS:
                case WeenieClassName.W_POTDYESPRINGBLUE_CLASS:
                case WeenieClassName.W_POTDYESPRINGPURPLE_CLASS:

                    // ensure item is armor/clothing and dyeable
                    if (target.WeenieType != WeenieType.Clothing || !(target.GetProperty(PropertyBool.Dyable) ?? false))
                        return null;

                    recipe = DatabaseManager.World.GetCachedRecipe(3844);     // base dye recipe
                    break;

                case WeenieClassName.W_DYERAREETERNALFOOLPROOFBLUE_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFBLACK_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFBOTCHED_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFDARKGREEN_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFDARKRED_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFDARKYELLOW_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFLIGHTBLUE_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFLIGHTGREEN_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFPURPLE_CLASS:
                case WeenieClassName.W_DYERAREETERNALFOOLPROOFSILVER_CLASS:

                    // ensure item is armor/clothing and dyeable
                    if (target.WeenieType != WeenieType.Clothing || !(target.GetProperty(PropertyBool.Dyable) ?? false))
                        return null;

                    recipe = DatabaseManager.World.GetCachedRecipe(9068);     // rare eternal dye recipe
                    break;

                case WeenieClassName.W_MATERIALIVORY_CLASS:
                case WeenieClassName.W_MATERIALRAREETERNALIVORY_CLASS:

                    // ensure item is ivoryable
                    if (!(target.GetProperty(PropertyBool.Ivoryable) ?? false))
                        return null;

                    var recipeId = source.WeenieClassId == (int)WeenieClassName.W_MATERIALRAREETERNALIVORY_CLASS ? 9069 : 3977;

                    recipe = DatabaseManager.World.GetCachedRecipe((uint)recipeId);

                    break;

                case WeenieClassName.W_MATERIALLEATHER_CLASS:
                case WeenieClassName.W_MATERIALRAREETERNALLEATHER_CLASS:

                    // ensure item is not already retained, and is not stackable
                    // and can either be salvaged, sold, or consumed with a mana stone
                    if (target.Retained || target is Stackable)
                        return null;

                    if (target.MaterialType == null && !target.IsSellable && target.ItemMaxMana == null)
                        return null;

                    recipeId = source.WeenieClassId == (int)WeenieClassName.W_MATERIALRAREETERNALLEATHER_CLASS ? 9070 : 4426;

                    recipe = DatabaseManager.World.GetCachedRecipe((uint)recipeId);

                    break;

                case WeenieClassName.W_MATERIALSANDSTONE_CLASS:
                case WeenieClassName.W_MATERIALSANDSTONE100_CLASS:

                    // ensure item is retained and not stackable
                    // and can either be salvaged, sold, or consumed with a mana stone
                    if (!target.Retained || target is Stackable)
                        return null;

                    if (target.MaterialType == null && !target.IsSellable && target.ItemMaxMana == null)
                        return null;

                    // use sandstone recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(8003);

                    break;

                case WeenieClassName.W_MATERIALGOLD_CLASS:

                    // ensure item has value and workmanship
                    if ((target.Value ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use gold recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3851);
                    break;

                case WeenieClassName.W_MATERIALLINEN_CLASS:

                    // ensure item has burden and workmanship
                    if ((target.EncumbranceVal ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use linen recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3854);
                    break;

                case WeenieClassName.W_MATERIALMOONSTONE_CLASS:

                    // ensure item has mana and workmanship
                    if ((target.ItemMaxMana ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use moonstone recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3978);
                    break;

                case WeenieClassName.W_MATERIALPINE_CLASS:

                    // ensure item has value and workmanship
                    if ((target.Value ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use pine recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3858);
                    break;

                case WeenieClassName.W_MATERIALIRON100_CLASS:
                case WeenieClassName.W_MATERIALIRON_CLASS:
                //case WeenieClassName.W_MATERIALGRANITE50_CLASS:
                case WeenieClassName.W_MATERIALGRANITE100_CLASS:
                case WeenieClassName.W_MATERIALGRANITE_CLASS:
                case WeenieClassName.W_MATERIALGRANITEPATHWARDEN_CLASS:
                case WeenieClassName.W_MATERIALVELVET100_CLASS:
                case WeenieClassName.W_MATERIALVELVET_CLASS:
                case WeenieClassName.W_LUCKYRABBITSFOOT_CLASS:

                    // ensure melee weapon and workmanship
                    if (target.WeenieType != WeenieType.MeleeWeapon || target.Workmanship == null)
                        return null;

                    // grab correct recipe to use as base
                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                case WeenieClassName.W_MATERIALMAHOGANY100_CLASS:
                case WeenieClassName.W_MATERIALMAHOGANY_CLASS:

                    // ensure missile weapon and workmanship
                    if (target.WeenieType != WeenieType.MissileLauncher || target.Workmanship == null)
                        return null;

                    // use mahogany recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3855);
                    break;

                case WeenieClassName.W_MATERIALOAK_CLASS:

                    // ensure melee or missile weapon, and workmanship
                    if (target.WeenieType != WeenieType.MeleeWeapon && target.WeenieType != WeenieType.MissileLauncher || target.Workmanship == null)
                        return null;

                    // use oak recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3857);
                    break;

                case WeenieClassName.W_MATERIALOPAL100_CLASS:
                case WeenieClassName.W_MATERIALOPAL_CLASS:

                    // ensure item is caster and has workmanship
                    if (target.WeenieType != WeenieType.Caster || target.Workmanship == null)
                        return null;

                    // use opal recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3979);
                    break;

                case WeenieClassName.W_MATERIALGREENGARNET100_CLASS:
                case WeenieClassName.W_MATERIALGREENGARNET_CLASS:

                    // ensure item is caster and has workmanship
                    if (target.WeenieType != WeenieType.Caster || target.Workmanship == null)
                        return null;

                    // use green garnet recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(5202);
                    break;

                case WeenieClassName.W_MATERIALBRASS100_CLASS:
                case WeenieClassName.W_MATERIALBRASS_CLASS:

                    // ensure item has workmanship
                    if (target.Workmanship == null) return null;

                    // use brass recipe as base
                    recipe = DatabaseManager.World.GetCachedRecipe(3848);
                    break;

                case WeenieClassName.W_MATERIALROSEQUARTZ_CLASS:
                case WeenieClassName.W_MATERIALREDJADE_CLASS:
                case WeenieClassName.W_MATERIALMALACHITE_CLASS:
                case WeenieClassName.W_MATERIALLAVENDERJADE_CLASS:
                case WeenieClassName.W_MATERIALHEMATITE_CLASS:
                case WeenieClassName.W_MATERIALBLOODSTONE_CLASS:
                case WeenieClassName.W_MATERIALAZURITE_CLASS:
                case WeenieClassName.W_MATERIALAGATE_CLASS:
                case WeenieClassName.W_MATERIALSMOKYQUARTZ_CLASS:
                case WeenieClassName.W_MATERIALCITRINE_CLASS:
                case WeenieClassName.W_MATERIALCARNELIAN_CLASS:

                    // ensure item is generic (jewelry), and has workmanship
                    if (target.WeenieType != WeenieType.Generic || target.Workmanship == null || target.ValidLocations == EquipMask.TrinketOne)
                        return null;

                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                //case WeenieClassName.W_MATERIALSTEEL50_CLASS:
                case WeenieClassName.W_MATERIALSTEEL100_CLASS:
                case WeenieClassName.W_MATERIALSTEEL_CLASS:
                case WeenieClassName.W_MATERIALSTEELPATHWARDEN_CLASS:
                case WeenieClassName.W_MATERIALALABASTER_CLASS:
                case WeenieClassName.W_MATERIALBRONZE_CLASS:
                case WeenieClassName.W_MATERIALMARBLE_CLASS:
                case WeenieClassName.W_MATERIALARMOREDILLOHIDE_CLASS:
                case WeenieClassName.W_MATERIALCERAMIC_CLASS:
                case WeenieClassName.W_MATERIALWOOL_CLASS:
                case WeenieClassName.W_MATERIALREEDSHARKHIDE_CLASS:
                case WeenieClassName.W_MATERIALSILVER_CLASS:
                case WeenieClassName.W_MATERIALCOPPER_CLASS:

                    // ensure loot-generated item w/ armor level
                    if (target.Workmanship == null || !target.HasArmorLevel())
                        return null;

                    var allowArmor = target.ItemType == ItemType.Armor;

                    // allow clothing that only covers an extremity
                    // this excludes some clothing like boots and robes that cover extremities + non-extremities
                    var allowClothing = target.ItemType == ItemType.Clothing && (target.ValidLocations == EquipMask.HeadWear || target.ValidLocations == EquipMask.HandWear || target.ValidLocations == EquipMask.FootWear);

                    if (!allowArmor && !allowClothing)
                        return null;

                    // TODO: replace with PropertyInt.MeleeDefenseImbuedEffectTypeCache == 1 when data is updated
                    if (source.MaterialType == MaterialType.Steel && !target.IsEnchantable)
                        return null;

                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                case WeenieClassName.W_MATERIALPERIDOT_CLASS:
                case WeenieClassName.W_MATERIALYELLOWTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALZIRCON_CLASS:

                case WeenieClassName.W_MATERIALRAREFOOLPROOFPERIDOT_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFYELLOWTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFZIRCON_CLASS:
                case WeenieClassName.W_MATERIALACE36634FOOLPROOFPERIDOT:
                case WeenieClassName.W_MATERIALACE36635FOOLPROOFYELLOWTOPAZ:
                case WeenieClassName.W_MATERIALACE36636FOOLPROOFZIRCON:

                    // can be applied to anything with AL, including shields (according to base recipe)
                    if (!target.HasArmorLevel() || target.Workmanship == null)
                        return null;

                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                // imbues - foolproof handled in regular imbue code
                case WeenieClassName.W_MATERIALAQUAMARINE100_CLASS:
                case WeenieClassName.W_MATERIALAQUAMARINE_CLASS:
                case WeenieClassName.W_MATERIALBLACKGARNET100_CLASS:
                case WeenieClassName.W_MATERIALBLACKGARNET_CLASS:
                case WeenieClassName.W_MATERIALBLACKOPAL100_CLASS:
                case WeenieClassName.W_MATERIALBLACKOPAL_CLASS:
                case WeenieClassName.W_MATERIALEMERALD100_CLASS:
                case WeenieClassName.W_MATERIALEMERALD_CLASS:
                case WeenieClassName.W_MATERIALFIREOPAL100_CLASS:
                case WeenieClassName.W_MATERIALFIREOPAL_CLASS:
                case WeenieClassName.W_MATERIALIMPERIALTOPAZ100_CLASS:
                case WeenieClassName.W_MATERIALIMPERIALTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALJET100_CLASS:
                case WeenieClassName.W_MATERIALJET_CLASS:
                case WeenieClassName.W_MATERIALREDGARNET100_CLASS:
                case WeenieClassName.W_MATERIALREDGARNET_CLASS:
                case WeenieClassName.W_MATERIALSUNSTONE100_CLASS:
                case WeenieClassName.W_MATERIALSUNSTONE_CLASS:
                case WeenieClassName.W_MATERIALWHITESAPPHIRE100_CLASS:
                case WeenieClassName.W_MATERIALWHITESAPPHIRE_CLASS:

                case WeenieClassName.W_LEFTHANDTETHER_CLASS:
                case WeenieClassName.W_LEFTHANDTETHERREMOVER_CLASS:

                case WeenieClassName.W_COREPLATINGINTEGRATOR_CLASS:
                case WeenieClassName.W_COREPLATINGDISINTEGRATOR_CLASS:

                case WeenieClassName.W_MATERIALRAREFOOLPROOFAQUAMARINE_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKGARNET_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKOPAL_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFEMERALD_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFFIREOPAL_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFIMPERIALTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFJET_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFREDGARNET_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFSUNSTONE_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFWHITESAPPHIRE_CLASS:

                case WeenieClassName.W_MATERIALACE36619FOOLPROOFAQUAMARINE:
                case WeenieClassName.W_MATERIALACE36620FOOLPROOFBLACKGARNET:
                case WeenieClassName.W_MATERIALACE36621FOOLPROOFBLACKOPAL:
                case WeenieClassName.W_MATERIALACE36622FOOLPROOFEMERALD:
                case WeenieClassName.W_MATERIALACE36623FOOLPROOFFIREOPAL:
                case WeenieClassName.W_MATERIALACE36624FOOLPROOFIMPERIALTOPAZ:
                case WeenieClassName.W_MATERIALACE36625FOOLPROOFJET:
                case WeenieClassName.W_MATERIALACE36626FOOLPROOFREDGARNET:
                case WeenieClassName.W_MATERIALACE36627FOOLPROOFSUNSTONE:
                case WeenieClassName.W_MATERIALACE36628FOOLPROOFWHITESAPPHIRE:

                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                // Society Shields
                case WeenieClassName.W_CELESTIALHANDSHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBSHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODSHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDBUCKLERSHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBBUCKLERSHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODBUCKLERSHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDCOVENANTSHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBCOVENANTSHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODCOVENANTSHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDKITESHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBKITESHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDLARGEKITESHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBLARGEKITESHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODLARGEKITESHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODKITESHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDOLTHOISHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBOLTHOISHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODOLTHOISHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDROUNDSHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBROUNDSHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDLARGEROUNDSHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBLARGEROUNDSHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODLARGEROUNDSHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODROUNDSHIELDCOVER_CLASS:
                case WeenieClassName.W_CELESTIALHANDTOWERSHIELDCOVER_CLASS:
                case WeenieClassName.W_ELDRYTCHWEBTOWERSHIELDCOVER_CLASS:
                case WeenieClassName.W_RADIANTBLOODTOWERSHIELDCOVER_CLASS:

                    // ensure target is a shield
                    if (target.WeenieType != WeenieType.Generic || target.ItemType != ItemType.Armor || !target.IsShield)
                        return null;

                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                // Slayer stones
                case WeenieClassName.W_GREATERMUKKIRSLAYERSTONE_CLASS:
                case WeenieClassName.W_BLACKSKULLOFXIKMA_CLASS:
                case WeenieClassName.W_SPECTRALSKULL_CLASS:
                case WeenieClassName.W_ANEKSHAYSLAYERSTONE_CLASS:

                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                // Paragon Weapons
                case WeenieClassName.W_LUMINOUSAMBEROFTHE1STTIERPARAGON_CLASS:

                    switch (target.WeenieType)
                    {
                        case WeenieType.Caster:
                            recipe = DatabaseManager.World.GetCachedRecipe(8700);
                            break;

                        case WeenieType.MeleeWeapon:
                            recipe = DatabaseManager.World.GetCachedRecipe(8701);
                            break;

                        case WeenieType.MissileLauncher:
                            recipe = DatabaseManager.World.GetCachedRecipe(8699);
                            break;

                        default:
                            return null;
                    }

                    break;

                case WeenieClassName.W_LUMINOUSAMBEROFTHE2NDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE3RDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE4THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE5THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE6THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE7THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE8THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE9THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE10THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE11THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE12THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE13THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE14THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE15THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE16THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE17THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE18THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE19THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE20THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE21STTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE22NDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE23RDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE24THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE25THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE26THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE27THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE28THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE29THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE30THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE31STTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE32NDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE33RDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE34THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE35THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE36THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE37THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE38THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE39THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE40THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE41STTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE42NDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE43RDTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE44THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE45THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE46THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE47THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE48THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE49THTIERPARAGON_CLASS:
                case WeenieClassName.W_LUMINOUSAMBEROFTHE50THTIERPARAGON_CLASS:
                    recipe = DatabaseManager.World.GetCachedRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                case WeenieClassName.W_UNINSCRIPTIONSTONE_CLASS:

                    /* Skip this for check in favor of second version which is closer to intent of stone i think.
                    // ensure workmanship and weenie type                    
                    if (target.Workmanship == null
                        || target.WeenieType != WeenieType.MeleeWeapon || target.WeenieType != WeenieType.MissileLauncher || target.WeenieType != WeenieType.Caster
                        || target.WeenieType != WeenieType.Clothing)
                    */

                    // check for base weenie for an inscription, if it exists, you cannot uninscribe the item.
                    string inscription = "";
                    var baseWeenie = DatabaseManager.World.GetCachedWeenie(target.WeenieClassId)?.PropertiesString?.TryGetValue(PropertyString.Inscription, out inscription);

                    if (baseWeenie == null || !string.IsNullOrWhiteSpace(inscription))
                        return null;

                    recipe = DatabaseManager.World.GetCachedRecipe(9133);

                    break;
            }

            return recipe;
        }

        public static Dictionary<WeenieClassName, uint> SourceToRecipe = new Dictionary<WeenieClassName, uint>()
        {
            { WeenieClassName.W_MATERIALIRON100_CLASS,         3853 },
            { WeenieClassName.W_MATERIALIRON_CLASS,            3853 },
            { WeenieClassName.W_MATERIALGRANITE100_CLASS,      3852 },
            { WeenieClassName.W_MATERIALGRANITE_CLASS,         3852 },
            { WeenieClassName.W_MATERIALGRANITEPATHWARDEN_CLASS, 3852 },
            { WeenieClassName.W_LUCKYRABBITSFOOT_CLASS,        8751 },

            { WeenieClassName.W_MATERIALVELVET100_CLASS,       3861 },
            { WeenieClassName.W_MATERIALVELVET_CLASS,          3861 },

            { WeenieClassName.W_MATERIALROSEQUARTZ_CLASS,      4446 },
            { WeenieClassName.W_MATERIALREDJADE_CLASS,         4442 },
            { WeenieClassName.W_MATERIALMALACHITE_CLASS,       4438 },
            { WeenieClassName.W_MATERIALLAVENDERJADE_CLASS,    4441 },
            { WeenieClassName.W_MATERIALHEMATITE_CLASS,        4440 },
            { WeenieClassName.W_MATERIALBLOODSTONE_CLASS,      4448 },
            { WeenieClassName.W_MATERIALAZURITE_CLASS,         4437 },
            { WeenieClassName.W_MATERIALAGATE_CLASS,           4445 },
            { WeenieClassName.W_MATERIALSMOKYQUARTZ_CLASS,     4447 },
            { WeenieClassName.W_MATERIALCITRINE_CLASS,         4439 },
            { WeenieClassName.W_MATERIALCARNELIAN_CLASS,       4443 },

            //{ WeenieClassName.W_MATERIALSTEEL50_CLASS,         3860 },
            { WeenieClassName.W_MATERIALSTEEL100_CLASS,        3860 },
            { WeenieClassName.W_MATERIALSTEEL_CLASS,           3860 },
            { WeenieClassName.W_MATERIALSTEELPATHWARDEN_CLASS, 3860 },

            { WeenieClassName. W_MATERIALALABASTER_CLASS,      3846 },
            { WeenieClassName.W_MATERIALBRONZE_CLASS,          3849 },
            { WeenieClassName.W_MATERIALMARBLE_CLASS,          3856 },
            { WeenieClassName.W_MATERIALARMOREDILLOHIDE_CLASS, 3847 },
            { WeenieClassName.W_MATERIALCERAMIC_CLASS,         3850 },
            { WeenieClassName.W_MATERIALWOOL_CLASS,            3862 },
            { WeenieClassName.W_MATERIALREEDSHARKHIDE_CLASS,   3859 },
            { WeenieClassName.W_MATERIALSILVER_CLASS,          4427 },
            { WeenieClassName.W_MATERIALCOPPER_CLASS,          4428 },

            { WeenieClassName.W_MATERIALPERIDOT_CLASS,         4435 },
            { WeenieClassName.W_MATERIALYELLOWTOPAZ_CLASS,     4434 },
            { WeenieClassName.W_MATERIALZIRCON_CLASS,          4433 },

            { WeenieClassName.W_MATERIALRAREFOOLPROOFPERIDOT_CLASS,     8016 },
            { WeenieClassName.W_MATERIALACE36634FOOLPROOFPERIDOT,       8016 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFYELLOWTOPAZ_CLASS, 8015 },
            { WeenieClassName.W_MATERIALACE36635FOOLPROOFYELLOWTOPAZ,   8015 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFZIRCON_CLASS,      8014 },
            { WeenieClassName.W_MATERIALACE36636FOOLPROOFZIRCON,        8014 },

            { WeenieClassName.W_MATERIALAQUAMARINE100_CLASS,              4436 },
            { WeenieClassName.W_MATERIALAQUAMARINE_CLASS,                 4436 },
            { WeenieClassName.W_MATERIALBLACKGARNET100_CLASS,             4449 },
            { WeenieClassName.W_MATERIALBLACKGARNET_CLASS,                4449 },
            { WeenieClassName.W_MATERIALBLACKOPAL100_CLASS,               3863 },
            { WeenieClassName.W_MATERIALBLACKOPAL_CLASS,                  3863 },
            { WeenieClassName.W_MATERIALEMERALD100_CLASS,                 4450 },
            { WeenieClassName.W_MATERIALEMERALD_CLASS,                    4450 },
            { WeenieClassName.W_MATERIALFIREOPAL100_CLASS,                3864 },
            { WeenieClassName.W_MATERIALFIREOPAL_CLASS,                   3864 },
            { WeenieClassName.W_MATERIALIMPERIALTOPAZ100_CLASS,           4454 },
            { WeenieClassName.W_MATERIALIMPERIALTOPAZ_CLASS,              4454 },
            { WeenieClassName.W_MATERIALJET100_CLASS,                     4451 },
            { WeenieClassName.W_MATERIALJET_CLASS,                        4451 },
            { WeenieClassName.W_MATERIALREDGARNET100_CLASS,               4452 },
            { WeenieClassName.W_MATERIALREDGARNET_CLASS,                  4452 },
            { WeenieClassName.W_MATERIALSUNSTONE100_CLASS,                3865 },
            { WeenieClassName.W_MATERIALSUNSTONE_CLASS,                   3865 },
            { WeenieClassName.W_MATERIALWHITESAPPHIRE100_CLASS,           4453 },
            { WeenieClassName.W_MATERIALWHITESAPPHIRE_CLASS,              4453 },

            { WeenieClassName.W_MATERIALRAREFOOLPROOFAQUAMARINE_CLASS,    8004 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKGARNET_CLASS,   8005 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKOPAL_CLASS,     8011 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFEMERALD_CLASS,       8006 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFFIREOPAL_CLASS,      8012 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFIMPERIALTOPAZ_CLASS, 8010 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFJET_CLASS,           8007 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFREDGARNET_CLASS,     8008 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFSUNSTONE_CLASS,      8013 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFWHITESAPPHIRE_CLASS, 8009 },

            { WeenieClassName.W_MATERIALACE36619FOOLPROOFAQUAMARINE,      8004 },
            { WeenieClassName.W_MATERIALACE36620FOOLPROOFBLACKGARNET,     8005 },
            { WeenieClassName.W_MATERIALACE36621FOOLPROOFBLACKOPAL,       8011 },
            { WeenieClassName.W_MATERIALACE36622FOOLPROOFEMERALD,         8006 },
            { WeenieClassName.W_MATERIALACE36623FOOLPROOFFIREOPAL,        8012 },
            { WeenieClassName.W_MATERIALACE36624FOOLPROOFIMPERIALTOPAZ,   8010 },
            { WeenieClassName.W_MATERIALACE36625FOOLPROOFJET,             8007 },
            { WeenieClassName.W_MATERIALACE36626FOOLPROOFREDGARNET,       8008 },
            { WeenieClassName.W_MATERIALACE36627FOOLPROOFSUNSTONE,        8013 },
            { WeenieClassName.W_MATERIALACE36628FOOLPROOFWHITESAPPHIRE,   8009 },

            { WeenieClassName.W_LEFTHANDTETHER_CLASS,                     6798 },
            { WeenieClassName.W_LEFTHANDTETHERREMOVER_CLASS,              6799 },

            { WeenieClassName.W_COREPLATINGINTEGRATOR_CLASS,              6800 },
            { WeenieClassName.W_COREPLATINGDISINTEGRATOR_CLASS,           6801 },

            { WeenieClassName.W_CELESTIALHANDSHIELDCOVER_CLASS,           8337 },
            { WeenieClassName.W_ELDRYTCHWEBSHIELDCOVER_CLASS,             8338 },
            { WeenieClassName.W_RADIANTBLOODSHIELDCOVER_CLASS,            8339 },
            { WeenieClassName.W_CELESTIALHANDBUCKLERSHIELDCOVER_CLASS,    8313 },
            { WeenieClassName.W_ELDRYTCHWEBBUCKLERSHIELDCOVER_CLASS,      8314 },
            { WeenieClassName.W_RADIANTBLOODBUCKLERSHIELDCOVER_CLASS,     8315 },
            { WeenieClassName.W_CELESTIALHANDCOVENANTSHIELDCOVER_CLASS,   8316 },
            { WeenieClassName.W_ELDRYTCHWEBCOVENANTSHIELDCOVER_CLASS,     8317 },
            { WeenieClassName.W_RADIANTBLOODCOVENANTSHIELDCOVER_CLASS,    8318 },
            { WeenieClassName.W_CELESTIALHANDKITESHIELDCOVER_CLASS,       8319 },
            { WeenieClassName.W_ELDRYTCHWEBKITESHIELDCOVER_CLASS,         8320 },
            { WeenieClassName.W_CELESTIALHANDLARGEKITESHIELDCOVER_CLASS,  8322 },
            { WeenieClassName.W_ELDRYTCHWEBLARGEKITESHIELDCOVER_CLASS,    8323 },
            { WeenieClassName.W_RADIANTBLOODLARGEKITESHIELDCOVER_CLASS,   8324 },
            { WeenieClassName.W_RADIANTBLOODKITESHIELDCOVER_CLASS,        8321 },
            { WeenieClassName.W_CELESTIALHANDOLTHOISHIELDCOVER_CLASS,     8325 },
            { WeenieClassName.W_ELDRYTCHWEBOLTHOISHIELDCOVER_CLASS,       8326 },
            { WeenieClassName.W_RADIANTBLOODOLTHOISHIELDCOVER_CLASS,      8327 },
            { WeenieClassName.W_CELESTIALHANDROUNDSHIELDCOVER_CLASS,      8328 },
            { WeenieClassName.W_ELDRYTCHWEBROUNDSHIELDCOVER_CLASS,        8329 },
            { WeenieClassName.W_CELESTIALHANDLARGEROUNDSHIELDCOVER_CLASS, 8331 },
            { WeenieClassName.W_ELDRYTCHWEBLARGEROUNDSHIELDCOVER_CLASS,   8332 },
            { WeenieClassName.W_RADIANTBLOODLARGEROUNDSHIELDCOVER_CLASS,  8333 },
            { WeenieClassName.W_RADIANTBLOODROUNDSHIELDCOVER_CLASS,       8330 },
            { WeenieClassName.W_CELESTIALHANDTOWERSHIELDCOVER_CLASS,      8334 },
            { WeenieClassName.W_ELDRYTCHWEBTOWERSHIELDCOVER_CLASS,        8335 },
            { WeenieClassName.W_RADIANTBLOODTOWERSHIELDCOVER_CLASS,       8336 },

            { WeenieClassName.W_GREATERMUKKIRSLAYERSTONE_CLASS,           8752 },
            { WeenieClassName.W_BLACKSKULLOFXIKMA_CLASS,                  8753 },
            { WeenieClassName.W_SPECTRALSKULL_CLASS,                      8754 },
            { WeenieClassName.W_ANEKSHAYSLAYERSTONE_CLASS,                8755 },

            { WeenieClassName.W_LUMINOUSAMBEROFTHE2NDTIERPARAGON_CLASS,    8702 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE3RDTIERPARAGON_CLASS,    8703 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE4THTIERPARAGON_CLASS,    8704 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE5THTIERPARAGON_CLASS,    8705 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE6THTIERPARAGON_CLASS,    8706 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE7THTIERPARAGON_CLASS,    8707 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE8THTIERPARAGON_CLASS,    8708 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE9THTIERPARAGON_CLASS,    8709 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE10THTIERPARAGON_CLASS,   8710 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE11THTIERPARAGON_CLASS,   8711 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE12THTIERPARAGON_CLASS,   8712 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE13THTIERPARAGON_CLASS,   8713 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE14THTIERPARAGON_CLASS,   8714 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE15THTIERPARAGON_CLASS,   8715 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE16THTIERPARAGON_CLASS,   8716 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE17THTIERPARAGON_CLASS,   8717 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE18THTIERPARAGON_CLASS,   8718 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE19THTIERPARAGON_CLASS,   8719 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE20THTIERPARAGON_CLASS,   8720 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE21STTIERPARAGON_CLASS,   8721 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE22NDTIERPARAGON_CLASS,   8722 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE23RDTIERPARAGON_CLASS,   8723 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE24THTIERPARAGON_CLASS,   8724 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE25THTIERPARAGON_CLASS,   8725 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE26THTIERPARAGON_CLASS,   8726 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE27THTIERPARAGON_CLASS,   8727 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE28THTIERPARAGON_CLASS,   8728 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE29THTIERPARAGON_CLASS,   8729 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE30THTIERPARAGON_CLASS,   8730 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE31STTIERPARAGON_CLASS,   8731 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE32NDTIERPARAGON_CLASS,   8732 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE33RDTIERPARAGON_CLASS,   8733 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE34THTIERPARAGON_CLASS,   8734 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE35THTIERPARAGON_CLASS,   8735 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE36THTIERPARAGON_CLASS,   8736 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE37THTIERPARAGON_CLASS,   8737 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE38THTIERPARAGON_CLASS,   8738 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE39THTIERPARAGON_CLASS,   8739 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE40THTIERPARAGON_CLASS,   8740 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE41STTIERPARAGON_CLASS,   8741 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE42NDTIERPARAGON_CLASS,   8742 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE43RDTIERPARAGON_CLASS,   8743 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE44THTIERPARAGON_CLASS,   8744 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE45THTIERPARAGON_CLASS,   8745 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE46THTIERPARAGON_CLASS,   8746 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE47THTIERPARAGON_CLASS,   8747 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE48THTIERPARAGON_CLASS,   8748 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE49THTIERPARAGON_CLASS,   8749 },
            { WeenieClassName.W_LUMINOUSAMBEROFTHE50THTIERPARAGON_CLASS,   8750 },
        };
    }
}
