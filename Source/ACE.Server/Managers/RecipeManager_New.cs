using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public partial class RecipeManager
    {
        public static Dictionary<uint, Dictionary<uint, uint>> Precursors;

        public static HashSet<int> Trinkets;

        static RecipeManager()
        {
            Trinkets = LootTables.trinketItems.ToHashSet();
        }

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

            if (Trinkets.Contains((int)target.WeenieClassId))
            {
                // trinkets are ItemType.Jewelry,
                // so need to be excluded in precursors
                switch ((WeenieClassName)source.WeenieClassId)
                {
                    // allowed
                    case WeenieClassName.W_MATERIALAMBER_CLASS:
                    case WeenieClassName.W_MATERIALDIAMOND_CLASS:
                    case WeenieClassName.W_MATERIALGROMNIEHIDE_CLASS:
                    case WeenieClassName.W_MATERIALPYREAL_CLASS:
                    case WeenieClassName.W_MATERIALRUBY_CLASS:
                    case WeenieClassName.W_MATERIALSAPPHIRE_CLASS:
                        break;

                    default:
                        return null;
                }
            }

            switch ((WeenieClassName)source.WeenieClassId)
            {
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

                    // use dye recipe as base, cleared
                    recipe = DatabaseManager.World.GetRecipe(3844);
                    ClearRecipe(recipe);
                    break;

                case WeenieClassName.W_MATERIALIVORY_CLASS:
                case WeenieClassName.W_MATERIALRAREETERNALIVORY_CLASS:

                    // ensure item is ivoryable
                    if (!(target.GetProperty(PropertyBool.Ivoryable) ?? false))
                        return null;

                    // use ivory recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3977);

                    if (source.WeenieClassId == (int)WeenieClassName.W_MATERIALRAREETERNALIVORY_CLASS)
                        ClearRecipe(recipe);

                    break;

                case WeenieClassName.W_MATERIALLEATHER_CLASS:
                case WeenieClassName.W_MATERIALRAREETERNALLEATHER_CLASS:

                    // ensure item is not retained and sellable
                    if (target.Retained || !target.IsSellable)
                        return null;

                    // use leather recipe as base
                    recipe = DatabaseManager.World.GetRecipe(4426);

                    if (source.WeenieClassId == (int)WeenieClassName.W_MATERIALRAREETERNALLEATHER_CLASS)
                        ClearRecipe(recipe);

                    break;

                case WeenieClassName.W_MATERIALSANDSTONE_CLASS:

                    // ensure item is retained and sellable
                    if (!target.Retained || !target.IsSellable)
                        return null;

                    // use sandstone recipe as base
                    recipe = DatabaseManager.World.GetRecipe(8003);

                    break;

                case WeenieClassName.W_MATERIALGOLD_CLASS:

                    // ensure item has value and workmanship
                    if ((target.Value ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use gold recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3851);
                    break;

                case WeenieClassName.W_MATERIALLINEN_CLASS:

                    // ensure item has burden and workmanship
                    if ((target.EncumbranceVal ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use linen recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3854);
                    break;

                case WeenieClassName.W_MATERIALMOONSTONE_CLASS:

                    // ensure item has mana and workmanship
                    if ((target.ItemMaxMana ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use moonstone recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3978);
                    break;

                case WeenieClassName.W_MATERIALPINE_CLASS:

                    // ensure item has value and workmanship
                    if ((target.Value ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // use pine recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3858);
                    break;

                //case WeenieClassName.W_MATERIALIRON100_CLASS:
                case WeenieClassName.W_MATERIALIRON_CLASS:
                //case WeenieClassName.W_MATERIALGRANITE50_CLASS:
                case WeenieClassName.W_MATERIALGRANITE100_CLASS:
                case WeenieClassName.W_MATERIALGRANITE_CLASS:
                case WeenieClassName.W_MATERIALGRANITEPATHWARDEN_CLASS:
                //case WeenieClassName.W_MATERIALVELVET100_CLASS:
                case WeenieClassName.W_MATERIALVELVET_CLASS:

                    // ensure melee weapon and workmanship
                    if (target.WeenieType != WeenieType.MeleeWeapon || target.Workmanship == null)
                        return null;

                    // grab correct recipe to use as base
                    recipe = DatabaseManager.World.GetRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                case WeenieClassName.W_MATERIALMAHOGANY100_CLASS:
                case WeenieClassName.W_MATERIALMAHOGANY_CLASS:

                    // ensure missile weapon and workmanship
                    if (target.WeenieType != WeenieType.MissileLauncher || target.Workmanship == null)
                        return null;

                    // use mahogany recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3855);
                    break;

                case WeenieClassName.W_MATERIALOAK_CLASS:

                    // ensure melee or missile weapon, and workmanship
                    if (target.WeenieType != WeenieType.MeleeWeapon && target.WeenieType != WeenieType.MissileLauncher || target.Workmanship == null)
                        return null;

                    // use oak recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3857);
                    break;

                //case WeenieClassName.W_MATERIALOPAL100_CLASS:
                case WeenieClassName.W_MATERIALOPAL_CLASS:

                    // ensure item is caster and has workmanship
                    if (target.WeenieType != WeenieType.Caster || target.Workmanship == null)
                        return null;

                    // use opal recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3979);
                    break;

                //case WeenieClassName.W_MATERIALGREENGARNET100_CLASS:
                case WeenieClassName.W_MATERIALGREENGARNET_CLASS:

                    // ensure item is caster and has workmanship
                    if (target.WeenieType != WeenieType.Caster || target.Workmanship == null)
                        return null;

                    // use green garnet recipe as base
                    recipe = DatabaseManager.World.GetRecipe(5202);
                    break;

                //case WeenieClassName.W_MATERIALBRASS100_CLASS:
                case WeenieClassName.W_MATERIALBRASS_CLASS:

                    // ensure item has workmanship
                    if (target.Workmanship == null) return null;

                    // use brass recipe as base
                    recipe = DatabaseManager.World.GetRecipe(3848);
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
                    if (target.WeenieType != WeenieType.Generic || target.Workmanship == null)
                        return null;

                    recipe = DatabaseManager.World.GetRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
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

                    // ensure armor w/ workmanship
                    if (target.ItemType != ItemType.Armor || (target.ArmorLevel ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    // TODO: replace with PropertyInt.MeleeDefenseImbuedEffectTypeCache == 1 when data is updated
                    if (source.MaterialType == MaterialType.Steel && !target.IsEnchantable)
                        return null;

                    recipe = DatabaseManager.World.GetRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                case WeenieClassName.W_MATERIALPERIDOT_CLASS:
                case WeenieClassName.W_MATERIALYELLOWTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALZIRCON_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFPERIDOT_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFYELLOWTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFZIRCON_CLASS:

                    // ensure clothing/armor w/ AL and workmanship
                    if (target.WeenieType != WeenieType.Clothing || (target.ArmorLevel ?? 0) == 0 || target.Workmanship == null)
                        return null;

                    recipe = DatabaseManager.World.GetRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;

                case WeenieClassName.W_POTDYEDARKGREEN_CLASS:
                case WeenieClassName.W_POTDYEDARKRED_CLASS:
                case WeenieClassName.W_POTDYEDARKYELLOW_CLASS:
                case WeenieClassName.W_POTDYEWINTERBLUE_CLASS:
                case WeenieClassName.W_POTDYEWINTERGREEN_CLASS:
                case WeenieClassName.W_POTDYEWINTERSILVER_CLASS:
                case WeenieClassName.W_POTDYESPRINGBLACK_CLASS:
                case WeenieClassName.W_POTDYESPRINGBLUE_CLASS:
                case WeenieClassName.W_POTDYESPRINGPURPLE_CLASS:

                    // ensure dyeable armor/clothing
                    if (target.WeenieType != WeenieType.Clothing || !(target.GetProperty(PropertyBool.Dyable) ?? false))
                        return null;

                    recipe = DatabaseManager.World.GetRecipe(3844);
                    break;

                // imbues - foolproof handled in regular imbue code
                case WeenieClassName.W_MATERIALRAREFOOLPROOFAQUAMARINE_CLASS:
                case WeenieClassName.W_MATERIALAQUAMARINE100_CLASS:
                case WeenieClassName.W_MATERIALAQUAMARINE_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKGARNET_CLASS:
                case WeenieClassName.W_MATERIALBLACKGARNET100_CLASS:
                case WeenieClassName.W_MATERIALBLACKGARNET_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKOPAL_CLASS:
                case WeenieClassName.W_MATERIALBLACKOPAL100_CLASS:
                case WeenieClassName.W_MATERIALBLACKOPAL_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFEMERALD_CLASS:
                case WeenieClassName.W_MATERIALEMERALD100_CLASS:
                case WeenieClassName.W_MATERIALEMERALD_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFFIREOPAL_CLASS:
                case WeenieClassName.W_MATERIALFIREOPAL100_CLASS:
                case WeenieClassName.W_MATERIALFIREOPAL_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFIMPERIALTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALIMPERIALTOPAZ100_CLASS:
                case WeenieClassName.W_MATERIALIMPERIALTOPAZ_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFJET_CLASS:
                case WeenieClassName.W_MATERIALJET100_CLASS:
                case WeenieClassName.W_MATERIALJET_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFREDGARNET_CLASS:
                case WeenieClassName.W_MATERIALREDGARNET100_CLASS:
                case WeenieClassName.W_MATERIALREDGARNET_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFSUNSTONE_CLASS:
                case WeenieClassName.W_MATERIALSUNSTONE100_CLASS:
                case WeenieClassName.W_MATERIALSUNSTONE_CLASS:
                case WeenieClassName.W_MATERIALRAREFOOLPROOFWHITESAPPHIRE_CLASS:
                case WeenieClassName.W_MATERIALWHITESAPPHIRE100_CLASS:
                case WeenieClassName.W_MATERIALWHITESAPPHIRE_CLASS:
                case WeenieClassName.W_LEFTHANDTETHER_CLASS:
                case WeenieClassName.W_LEFTHANDTETHERREMOVER_CLASS:
                case WeenieClassName.W_COREPLATINGINTEGRATOR_CLASS:
                case WeenieClassName.W_COREPLATINGDISINTEGRATOR_CLASS:

                    recipe = DatabaseManager.World.GetRecipe(SourceToRecipe[(WeenieClassName)source.WeenieClassId]);
                    break;
            }

            return recipe;
        }

        public static void ClearRecipe(Recipe recipe)
        {
            recipe.Difficulty = 0;
            recipe.FailAmount = 0;
            recipe.FailDestroySourceAmount = 0;
            recipe.FailDestroySourceChance = 0;
            recipe.SuccessAmount = 0;
            recipe.SuccessDestroySourceChance = 0;
            recipe.SuccessDestroySourceChance = 0;
            recipe.SuccessWCID = 0;
            recipe.FailWCID = 0;
        }

        public static Dictionary<WeenieClassName, uint> SourceToRecipe = new Dictionary<WeenieClassName, uint>()
        {
            //{ WeenieClassName.W_MATERIALIRON100_CLASS,         3853 },
            { WeenieClassName.W_MATERIALIRON_CLASS,            3853 },
            { WeenieClassName.W_MATERIALGRANITE100_CLASS,      3852 },
            { WeenieClassName.W_MATERIALGRANITE_CLASS,         3852 },
            { WeenieClassName.W_MATERIALGRANITEPATHWARDEN_CLASS, 3852 },

            //{ WeenieClassName.W_MATERIALVELVET100_CLASS,       3861 },
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
            { WeenieClassName.W_MATERIALRAREFOOLPROOFPERIDOT_CLASS,     4435 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFYELLOWTOPAZ_CLASS, 4434 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFZIRCON_CLASS,      4433 },

            { WeenieClassName.W_MATERIALRAREFOOLPROOFAQUAMARINE_CLASS,    4436 },
            { WeenieClassName.W_MATERIALAQUAMARINE100_CLASS,              4436 },
            { WeenieClassName.W_MATERIALAQUAMARINE_CLASS,                 4436 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKGARNET_CLASS,   4449 },
            { WeenieClassName.W_MATERIALBLACKGARNET100_CLASS,             4449 },
            { WeenieClassName.W_MATERIALBLACKGARNET_CLASS,                4449 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKOPAL_CLASS,     3863 },
            { WeenieClassName.W_MATERIALBLACKOPAL100_CLASS,               3863 },
            { WeenieClassName.W_MATERIALBLACKOPAL_CLASS,                  3863 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFEMERALD_CLASS,       4450 },
            { WeenieClassName.W_MATERIALEMERALD100_CLASS,                 4450 },
            { WeenieClassName.W_MATERIALEMERALD_CLASS,                    4450 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFFIREOPAL_CLASS,      3864 },
            { WeenieClassName.W_MATERIALFIREOPAL100_CLASS,                3864 },
            { WeenieClassName.W_MATERIALFIREOPAL_CLASS,                   3864 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFIMPERIALTOPAZ_CLASS, 4454 },
            { WeenieClassName.W_MATERIALIMPERIALTOPAZ100_CLASS,           4454 },
            { WeenieClassName.W_MATERIALIMPERIALTOPAZ_CLASS,              4454 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFJET_CLASS,           4451 },
            { WeenieClassName.W_MATERIALJET100_CLASS,                     4451 },
            { WeenieClassName.W_MATERIALJET_CLASS,                        4451 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFREDGARNET_CLASS,     4452 },
            { WeenieClassName.W_MATERIALREDGARNET100_CLASS,               4452 },
            { WeenieClassName.W_MATERIALREDGARNET_CLASS,                  4452 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFSUNSTONE_CLASS,      3865 },
            { WeenieClassName.W_MATERIALSUNSTONE100_CLASS,                3865 },
            { WeenieClassName.W_MATERIALSUNSTONE_CLASS,                   3865 },
            { WeenieClassName.W_MATERIALRAREFOOLPROOFWHITESAPPHIRE_CLASS, 4453 },
            { WeenieClassName.W_MATERIALWHITESAPPHIRE100_CLASS,           4453 },
            { WeenieClassName.W_MATERIALWHITESAPPHIRE_CLASS,              4453 },
            { WeenieClassName.W_LEFTHANDTETHER_CLASS,                     6798 },
            { WeenieClassName.W_LEFTHANDTETHERREMOVER_CLASS,              6799 },
            { WeenieClassName.W_COREPLATINGINTEGRATOR_CLASS,              6800 },
            { WeenieClassName.W_COREPLATINGDISINTEGRATOR_CLASS,           6801 },
        };
    }
}
