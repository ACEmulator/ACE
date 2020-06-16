using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.World;

namespace ACE.Adapter.GDLE
{
    public static class GDLEConverter
    {
        /// <summary>
        /// Converts ACE -> GDLE quest
        /// </summary>
        public static bool TryConvert(Quest input, out Models.Quest result)
        {
            result = new Models.Quest();
            result.key = input.Name;

            var quest = new Models.QuestValue();
            quest.fullname = input.Message;
            quest.mindelta = (int)input.MinDelta;
            quest.maxsolves = input.MaxSolves;

            result.value = quest;

            return true;
        }

        public static Dictionary<uint, string> WeenieNames;
        public static Dictionary<uint, string> WeenieClassNames;

        /// <summary>
        /// Converts ACE landblock instances -> GDLE landblock instances
        /// </summary>
        public static bool TryConvert(List<LandblockInstance> input, out Models.Landblock result)
        {
            var instanceWcids = new Dictionary<uint, uint>();
            foreach (var instance in input)
                instanceWcids.Add(instance.Guid, instance.WeenieClassId);

            result = new Models.Landblock();

            if (input.Count == 0)
                return true;

            result.key = (uint)input[0].Landblock << 16;

            result.value = new Models.LandblockValue();

            result.desc = $"{result.key}({input[0].Landblock:X4})";

            foreach (var lbi in input)
            {
                if (result.value.weenies == null)
                    result.value.weenies = new List<Models.LandblockWeenie>();

                var weenie = new Models.LandblockWeenie();
                weenie.id = lbi.Guid;
                weenie.wcid = lbi.WeenieClassId;

                if (WeenieNames != null && WeenieClassNames != null)
                {
                    WeenieNames.TryGetValue(lbi.WeenieClassId, out var weenieName);
                    WeenieClassNames.TryGetValue(lbi.WeenieClassId, out var weenieClassName);

                    weenie.desc = $"{weenieName}({weenieClassName})";
                }

                // fix this ***, write it properly.
                var pos = new Models.Position();
                pos.objcell_id = lbi.ObjCellId;

                var frame = new Models.Frame();

                frame.origin = new Models.Origin();
                frame.origin.x = lbi.OriginX;
                frame.origin.y = lbi.OriginY;
                frame.origin.z = lbi.OriginZ;

                frame.angles = new Models.Angles();
                frame.angles.w = lbi.AnglesW;
                frame.angles.x = lbi.AnglesX;
                frame.angles.y = lbi.AnglesY;
                frame.angles.z = lbi.AnglesZ;

                pos.frame = frame;
                weenie.pos = pos;

                result.value.weenies.Add(weenie);

                if (lbi.LandblockInstanceLink != null)
                {
                    foreach (var link in lbi.LandblockInstanceLink)
                    {
                        if (result.value.links == null)
                            result.value.links = new List<Models.LandblockLink>();

                        var _link = new Models.LandblockLink();
                        _link.target = link.ParentGuid;
                        _link.source = link.ChildGuid;

                        if (WeenieNames != null && WeenieClassNames != null)
                        {
                            WeenieNames.TryGetValue(lbi.WeenieClassId, out var targetName);
                            WeenieClassNames.TryGetValue(lbi.WeenieClassId, out var targetClassName);

                            if (instanceWcids.TryGetValue(link.ChildGuid, out var sourceWcid))
                            {
                                WeenieNames.TryGetValue(sourceWcid, out var sourceName);
                                WeenieClassNames.TryGetValue(sourceWcid, out var sourceClassName);

                                _link.desc = $"{sourceName}({sourceClassName})(wcid: {sourceWcid}) -> {targetClassName}(wcid: {lbi.WeenieClassId})";
                            }
                        }
                        result.value.links.Add(_link);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Converts GDLE spawn map -> ACE landblock instances
        ///
        /// This will not alter the Guid. To sanitize the Guid for ACE usage, you should use GDLELoader.TryLoadWorldSpawnsConverted() instead.
        /// </summary>
        public static bool TryConvert(Models.Landblock input, out List<LandblockInstance> results, out List<LandblockInstanceLink> links)
        {
            try
            {
                results = new List<LandblockInstance>();
                links = new List<LandblockInstanceLink>();

                foreach (var value in input.value.weenies)
                {
                    var result = new LandblockInstance();

                    result.Guid = value.id; // Collisions and other errors can be caused by invalid input. Data should be sanitized by the running ACE server.
                    //result.Landblock = input.key; ACE uses a virtual column here of (result.ObjCellId >> 16)
                    result.WeenieClassId = value.wcid;

                    result.ObjCellId = value.pos.objcell_id;
                    result.OriginX = value.pos.frame.origin.x;
                    result.OriginY = value.pos.frame.origin.y;
                    result.OriginZ = value.pos.frame.origin.z;
                    result.AnglesW = value.pos.frame.angles.w;
                    result.AnglesX = value.pos.frame.angles.x;
                    result.AnglesY = value.pos.frame.angles.y;
                    result.AnglesZ = value.pos.frame.angles.z;

                    results.Add(result);
                }

                if (input.value.links != null)
                {
                    foreach (var value in input.value.links)
                    {
                        var result = new LandblockInstanceLink();

                        result.ParentGuid = value.target;
                        result.ChildGuid = value.source;

                        links.Add(result);
                    }
                }

                // reguid if needed
                var landblockId = (ushort)(input.key >> 16);
                ReGuidAndConvertLandblocks(results, links, landblockId);

                return true;
            }
            catch
            {
                results = null;
                links = null;
                return false;
            }
        }

        private static void ReGuidAndConvertLandblocks(List<LandblockInstance> instances, List<LandblockInstanceLink> links, ushort landblockId)
        {
            var firstGuid = 0x70000000 | ((uint)landblockId << 12);
            var lastGuid = firstGuid | 0xFFF;

            var nextGuid = firstGuid;

            var reguid = new Dictionary<uint, uint>();

            foreach (var instance in instances)
            {
                if (instance.Guid < firstGuid || instance.Guid > lastGuid)
                {
                    reguid.Add(instance.Guid, nextGuid);
                    instance.Guid = nextGuid++;
                }
            }

            foreach (var link in links)
            {
                if (reguid.TryGetValue(link.ParentGuid, out var newParentGuid))
                    link.ParentGuid = newParentGuid;

                if (reguid.TryGetValue(link.ChildGuid, out var newChildGuid))
                    link.ChildGuid = newChildGuid;
            }
        }


        public static bool TryConvert(Models.Event input, out Event result)
        {
            try
            {
                result = new Event();

                //result.Id // This is an Auto Increment field in the ACE schema

                result.Name = input.Key;

                result.StartTime = input.Value.StartTime;
                result.EndTime = input.Value.EndTime;
                result.State = input.Value.EventState;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }


        /// <summary>
        /// Converts GDLE -> ACE quest
        /// </summary>
        public static bool TryConvert(Models.Quest input, out Quest result)
        {
            try
            {
                result = new Quest();

                //result.Id // This is an Auto Increment field in the ACE schema

                result.Name = input.key;

                // FIXME: db schema should be int
                result.MinDelta = (uint)input.value.mindelta;
                result.MaxSolves = input.value.maxsolves;
                result.Message = input.value.fullname;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }


        public static bool TryConvert(uint id, Models.SpellValue input, out Spell result)
        {
            try
            {
                result = new Spell();

                result.Id = id;

                result.Name = input.Name;
                
                if (input.MetaSpell.Spell.StatMod != null)
                {
                    result.StatModType = input.MetaSpell.Spell.StatMod.Type;
                    result.StatModKey = input.MetaSpell.Spell.StatMod.Key;
                    result.StatModVal = (float)input.MetaSpell.Spell.StatMod.Val;
                }

                // Projectile, LifeProjectile
                result.EType = input.MetaSpell.Spell.EType;
                result.BaseIntensity = input.MetaSpell.Spell.BaseIntensity;
                result.Variance = input.MetaSpell.Spell.Variance;
                result.Wcid = input.MetaSpell.Spell.Wcid;
                result.NumProjectiles = input.MetaSpell.Spell.NumProjectiles;
                result.NumProjectilesVariance = (int?)input.MetaSpell.Spell.NumProjectilesVariance;
                result.SpreadAngle = input.MetaSpell.Spell.SpreadAngle;
                result.VerticalAngle = input.MetaSpell.Spell.VerticalAngle;
                result.DefaultLaunchAngle = input.MetaSpell.Spell.DefaultLaunchAngle;
                result.NonTracking = input.MetaSpell.Spell.NonTracking;

                if (input.MetaSpell.Spell.CreateOffset != null)
                {
                    result.CreateOffsetOriginX = (float)input.MetaSpell.Spell.CreateOffset.X;
                    result.CreateOffsetOriginY = (float)input.MetaSpell.Spell.CreateOffset.Y;
                    result.CreateOffsetOriginZ = (float)input.MetaSpell.Spell.CreateOffset.Z;
                }

                if (input.MetaSpell.Spell.Padding != null)
                {
                    result.PaddingOriginX = (float)input.MetaSpell.Spell.Padding.X;
                    result.PaddingOriginY = (float)input.MetaSpell.Spell.Padding.Y;
                    result.PaddingOriginZ = (float)input.MetaSpell.Spell.Padding.Z;
                }

                if (input.MetaSpell.Spell.Dims != null)
                {
                    result.DimsOriginX = (float)input.MetaSpell.Spell.Dims.X;
                    result.DimsOriginY = (float)input.MetaSpell.Spell.Dims.Y;
                    result.DimsOriginZ = (float)input.MetaSpell.Spell.Dims.Z;
                }

                if (input.MetaSpell.Spell.Peturbation != null)
                {
                    result.PeturbationOriginX = (float)input.MetaSpell.Spell.Peturbation.X;
                    result.PeturbationOriginY = (float)input.MetaSpell.Spell.Peturbation.Y;
                    result.PeturbationOriginZ = (float)input.MetaSpell.Spell.Peturbation.Z;
                }

                result.ImbuedEffect = input.MetaSpell.Spell.ImbuedEffect;
                result.SlayerCreatureType = input.MetaSpell.Spell.SlayerCreatureType;
                result.SlayerDamageBonus = input.MetaSpell.Spell.SlayerDamageBonus;
                result.CritFreq = input.MetaSpell.Spell.CritFreq;
                result.CritMultiplier = input.MetaSpell.Spell.CritMultiplier;
                result.IgnoreMagicResist = input.MetaSpell.Spell.IgnoreMagicResist;
                result.ElementalModifier = input.MetaSpell.Spell.ElementalModifier;

                // LifeProjectile
                result.DrainPercentage = (float?)input.MetaSpell.Spell.DrainPercentage;
                result.DamageRatio = (float?)input.MetaSpell.Spell.DamageRatio;

                // Boost, FellowBoost
                result.DamageType = input.MetaSpell.Spell.DamageType;
                result.Boost = input.MetaSpell.Spell.Boost;
                result.BoostVariance = input.MetaSpell.Spell.BoostVariance;

                // Transfer
                result.Source = input.MetaSpell.Spell.Source;
                result.Destination = input.MetaSpell.Spell.Dest;
                result.Proportion = (float?)input.MetaSpell.Spell.Proportion;
                result.LossPercent = (float?)input.MetaSpell.Spell.LossPercent;
                result.SourceLoss = input.MetaSpell.Spell.SourceLoss;
                result.TransferCap = input.MetaSpell.Spell.TransferCap;
                result.MaxBoostAllowed = input.MetaSpell.Spell.MaxBoostAllowed;
                result.TransferBitfield = input.MetaSpell.Spell.Bitfield;

                // PortalLink
                result.Index = input.MetaSpell.Spell.Index;

                // PortalSummon
                result.Link = input.MetaSpell.Spell.Link;

                // PortalSending, FellowPortalSending
                if (input.MetaSpell.Spell.Position != null)
                {
                    result.PositionObjCellId = input.MetaSpell.Spell.Position.objcell_id;

                    result.PositionOriginX = input.MetaSpell.Spell.Position.frame.origin.x;
                    result.PositionOriginY = input.MetaSpell.Spell.Position.frame.origin.y;
                    result.PositionOriginZ = input.MetaSpell.Spell.Position.frame.origin.z;

                    result.PositionAnglesW = input.MetaSpell.Spell.Position.frame.angles.w;
                    result.PositionAnglesX = input.MetaSpell.Spell.Position.frame.angles.x;
                    result.PositionAnglesY = input.MetaSpell.Spell.Position.frame.angles.y;
                    result.PositionAnglesZ = input.MetaSpell.Spell.Position.frame.angles.z;
                }

                // Dispel, FellowDispel
                result.MinPower = input.MetaSpell.Spell.MinPower;
                result.MaxPower = input.MetaSpell.Spell.MaxPower;
                result.PowerVariance = input.MetaSpell.Spell.PowerVariance;
                //result.DispelSchool = input.DispelSchool; // TODO!!!
                result.Align = input.MetaSpell.Spell.Align;
                result.Number = input.MetaSpell.Spell.Number;
                result.NumberVariance = (float?)input.MetaSpell.Spell.NumberVariance;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static int GetIndex(RecipeMod mod)
        {
            if (mod.RecipeModsBool != null && mod.RecipeModsBool.Count > 0)
                return mod.RecipeModsBool.First().Index;
            if (mod.RecipeModsDID != null && mod.RecipeModsDID.Count > 0)
                return mod.RecipeModsDID.First().Index;
            if (mod.RecipeModsFloat != null && mod.RecipeModsFloat.Count > 0)
                return mod.RecipeModsFloat.First().Index;
            if (mod.RecipeModsIID != null && mod.RecipeModsIID.Count > 0)
                return mod.RecipeModsIID.First().Index;
            if (mod.RecipeModsInt != null && mod.RecipeModsInt.Count > 0)
                return mod.RecipeModsInt.First().Index;
            if (mod.RecipeModsString != null && mod.RecipeModsString.Count > 0)
                return mod.RecipeModsString.First().Index;

            return -1;
        }

        /// <summary>
        /// Converts ACE recipe + cookbooks to GDLE recipe + precursors
        /// </summary>
        public static bool TryConvert(List<CookBook> cookbooks, out Models.RecipeCombined result)
        {
            if (cookbooks == null || cookbooks.Count == 0)
            {
                result = null;
                return false;
            }

            var recipe = cookbooks[0].Recipe;

            result = new Models.RecipeCombined();

            result.key = recipe.Id;
            result.desc = cookbooks[0].SourceWCID.ToString();   // TODO: get weenie name

            TryConvert(recipe, out var newRecipe);
            if (newRecipe != null)
                newRecipe.RecipeId = 0;

            result.recipe = newRecipe;

            result.precursors = new List<Models.RecipePrecursor>();
            foreach (var cookbook in cookbooks)
            {
                if (TryConvert(cookbook, out var precursor))
                {
                    precursor.RecipeId = null;
                    result.precursors.Add(precursor);
                }
            }
            return true;
        }

        /// <summary>
        /// Converts an ACE recipe to GDLE recipe
        /// </summary>
        public static bool TryConvert(Recipe input, out Models.Recipe result)
        {
            try
            {
                result = new Models.Recipe();

                result.RecipeId = input.Id;

                result.Unknown = (int)input.Unknown1;
                result.Skill = input.Skill;
                result.Difficulty = input.Difficulty;
                result.SkillCheckFormulaType = (int)input.SalvageType;

                result.SuccessWcid = input.SuccessWCID;
                result.SuccessAmount = input.SuccessAmount;
                result.SuccessMessage = input.SuccessMessage;

                result.FailWcid = input.FailWCID;
                result.FailAmount = input.FailAmount;
                result.FailMessage = input.FailMessage;

                result.SuccessConsumeTargetChance = input.SuccessDestroyTargetChance;
                result.SuccessConsumeTargetAmount = input.SuccessDestroyTargetAmount;
                result.SuccessConsumeTargetMessage = input.SuccessDestroyTargetMessage;

                result.SuccessConsumeToolChance = input.SuccessDestroySourceChance;
                result.SuccessConsumeToolAmount = input.SuccessDestroySourceAmount;
                result.SuccessConsumeToolMessage = input.SuccessDestroySourceMessage;

                result.FailureConsumeTargetChance = input.FailDestroyTargetChance;
                result.FailureConsumeTargetAmount = input.FailDestroyTargetAmount;
                result.FailureConsumeTargetMessage = input.FailDestroyTargetMessage;

                result.FailureConsumeToolChance = input.FailDestroySourceChance;
                result.FailureConsumeToolAmount = input.FailDestroySourceAmount;
                result.FailureConsumeToolMessage = input.FailDestroySourceMessage;

                result.DataId = input.DataId;

                // requirements
                result.Requirements = new List<Models.RecipeRequirements>();
                for (var idx = 0; idx < 3; idx++)
                    result.Requirements.Add(null);

                foreach (var intReq in input.RecipeRequirementsInt)
                {
                    var requirements = result.Requirements[intReq.Index];

                    if (requirements == null)
                    {
                        requirements = new Models.RecipeRequirements();
                        result.Requirements[intReq.Index] = requirements;
                    }

                    requirements.IntRequirements.Add(new Models.IntRequirement
                    {
                        Stat = intReq.Stat,
                        Value = intReq.Value,
                        OperationType = intReq.Enum,
                        Message = intReq.Message
                    });
                }

                foreach (var didReq in input.RecipeRequirementsDID)
                {
                    var requirements = result.Requirements[didReq.Index];

                    if (requirements == null)
                    {
                        requirements = new Models.RecipeRequirements();
                        result.Requirements[didReq.Index] = requirements;
                    }

                    requirements.DIDRequirements.Add(new Models.DIDRequirement
                    {
                        Stat = didReq.Stat,
                        Value = didReq.Value,
                        OperationType = didReq.Enum,
                        Message = didReq.Message
                    });
                }

                foreach (var iidReq in input.RecipeRequirementsIID)
                {
                    var requirements = result.Requirements[iidReq.Index];

                    if (requirements == null)
                    {
                        requirements = new Models.RecipeRequirements();
                        result.Requirements[iidReq.Index] = requirements;
                    }

                    requirements.DIDRequirements.Add(new Models.DIDRequirement
                    {
                        Stat = iidReq.Stat,
                        Value = iidReq.Value,
                        OperationType = iidReq.Enum,
                        Message = iidReq.Message
                    });
                }

                foreach (var floatReq in input.RecipeRequirementsFloat)
                {
                    var requirements = result.Requirements[floatReq.Index];

                    if (requirements == null)
                    {
                        requirements = new Models.RecipeRequirements();
                        result.Requirements[floatReq.Index] = requirements;
                   }

                    requirements.FloatRequirements.Add(new Models.FloatRequirement
                    {
                        Stat = floatReq.Stat,
                        Value = floatReq.Value,
                        OperationType = floatReq.Enum,
                        Message = floatReq.Message
                    });
                }

                foreach (var stringReq in input.RecipeRequirementsString)
                {
                    var requirements = result.Requirements[stringReq.Index];

                    if (requirements == null)
                    {
                        requirements = new Models.RecipeRequirements();
                        result.Requirements[stringReq.Index] = requirements;
                    }

                    requirements.StringRequirements.Add(new Models.StringRequirement
                    {
                        Stat = stringReq.Stat,
                        Value = stringReq.Value,
                        OperationType = stringReq.Enum,
                    });
                }

                foreach (var boolReq in input.RecipeRequirementsBool)
                {
                    var requirements = result.Requirements[boolReq.Index];

                    if (requirements == null)
                    {
                        requirements = new Models.RecipeRequirements();
                        result.Requirements[boolReq.Index] = requirements;
                    }

                    requirements.BoolRequirements.Add(new Models.BoolRequirement
                    {
                        Stat = boolReq.Stat,
                        Value = boolReq.Value,
                        OperationType = boolReq.Enum,
                    });
                }

                // modifications
                result.Mods = new List<Models.Mod>();
                for (var idx = 0; idx < 8; idx++)
                    result.Mods.Add(null);

                foreach (var recipeMod in input.RecipeMod)
                {
                    // should index be on RecipeMod, or RecipeModType?
                    var idx = GetIndex(recipeMod);
                    if (idx == -1)
                    {
                        Console.WriteLine($"Couldn't find recipe mod idx");
                        continue;
                    }

                    var mod = result.Mods[idx];
                    if (mod == null)
                    {
                        mod = new Models.Mod();
                        result.Mods[idx] = mod;
                    }

                    // base stats
                    mod.ModifyHealth = recipeMod.Health;
                    mod.ModifyStamina = recipeMod.Stamina;
                    mod.ModifyMana = recipeMod.Mana;

                    // TODO!!! we're missing the following RequiresHealth, RequiresStamina, RequiresMana

                    mod.Unknown7 = recipeMod.Unknown7;
                    mod.ModificationScriptId = recipeMod.DataId;

                    mod.Unknown9 = recipeMod.Unknown9;
                    mod.Unknown10 = recipeMod.InstanceId;

                    // type mods
                    foreach (var intMod in recipeMod.RecipeModsInt)
                    {
                        mod.IntRequirements.Add(new Models.IntRequirement
                        {
                            Stat = intMod.Stat,
                            Value = intMod.Value,
                            OperationType = intMod.Enum,
                            Unknown = intMod.Source
                        });
                    }

                    foreach (var didMod in recipeMod.RecipeModsDID)
                    {
                        mod.DIDRequirements.Add(new Models.DIDRequirement
                        {
                            Stat = didMod.Stat,
                            Value = didMod.Value,
                            OperationType = didMod.Enum,
                            Unknown = didMod.Source
                        });
                    }

                    foreach (var iidMod in recipeMod.RecipeModsIID)
                    {
                        mod.IIDRequirements.Add(new Models.IIDRequirement
                        {
                            Stat = iidMod.Stat,
                            Value = iidMod.Value,
                            OperationType = iidMod.Enum,
                            Unknown = iidMod.Source
                        });
                    }

                    foreach (var floatMod in recipeMod.RecipeModsFloat)
                    {
                        mod.FloatRequirements.Add(new Models.FloatRequirement
                        {
                            Stat = floatMod.Stat,
                            Value = floatMod.Value,
                            OperationType = floatMod.Enum,
                            Unknown = floatMod.Source
                        });
                    }

                    foreach (var stringMod in recipeMod.RecipeModsString)
                    {
                        mod.StringRequirements.Add(new Models.StringRequirement
                        {
                            Stat = stringMod.Stat,
                            Value = stringMod.Value,
                            OperationType = stringMod.Enum,
                            Unknown = stringMod.Source
                        });
                    }

                    foreach (var boolMod in recipeMod.RecipeModsBool)
                    {
                        mod.BoolRequirements.Add(new Models.BoolRequirement
                        {
                            Stat = boolMod.Stat,
                            Value = boolMod.Value,
                            OperationType = boolMod.Enum,
                            Unknown = boolMod.Source
                        });
                    }
                }

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Converts GDLE recipe + precursors to ACE recipe + cookbooks
        /// </summary>
        public static bool TryConvert(Models.RecipeCombined input, out List<CookBook> cookbooks, out Recipe recipe)
        {
            if (!TryConvert(input.recipe, out recipe))
            {
                cookbooks = null;
                return false;
            }

            recipe.Id = input.key;

            cookbooks = new List<CookBook>();

            foreach (var precursor in input.precursors)
            {
                if (!TryConvert(precursor, out var cookbook))
                    return false;

                cookbook.RecipeId = input.key;

                cookbooks.Add(cookbook);
            }
            return true;
        }


        /// <summary>
        /// Converts a GDLE recipe to an ACE recipe
        /// </summary>
        public static bool TryConvert(Models.Recipe input, out Recipe result)
        {
            try
            {
                result = new Recipe();

                result.Id = input.RecipeId;

                result.Unknown1 = (uint)input.Unknown;
                result.Skill = input.Skill;
                result.Difficulty = input.Difficulty;
                result.SalvageType = (uint)input.SkillCheckFormulaType;

                result.SuccessWCID = input.SuccessWcid;
                result.SuccessAmount = input.SuccessAmount;
                result.SuccessMessage = input.SuccessMessage;

                result.FailWCID = input.FailWcid;
                result.FailAmount = input.FailAmount;
                result.FailMessage = input.FailMessage;

                result.SuccessDestroyTargetChance = input.SuccessConsumeTargetChance;
                result.SuccessDestroyTargetAmount = input.SuccessConsumeTargetAmount;
                result.SuccessDestroyTargetMessage = input.SuccessConsumeTargetMessage;

                result.SuccessDestroySourceChance = input.SuccessConsumeToolChance;
                result.SuccessDestroySourceAmount = input.SuccessConsumeToolAmount;
                result.SuccessDestroySourceMessage = input.SuccessConsumeToolMessage;

                result.FailDestroyTargetChance = input.FailureConsumeTargetChance;
                result.FailDestroyTargetAmount = input.FailureConsumeTargetAmount;
                result.FailDestroyTargetMessage = input.FailureConsumeTargetMessage;

                result.FailDestroySourceChance = input.FailureConsumeToolChance;
                result.FailDestroySourceAmount = input.FailureConsumeToolAmount;
                result.FailDestroySourceMessage = input.FailureConsumeToolMessage;

                result.DataId = input.DataId;

                sbyte index = -1;
                foreach (var value in input.Requirements)
                {
                    index++;
                    if (value == null)
                        continue;

                    if (value.IntRequirements != null)
                    {
                        foreach (var requirement in value.IntRequirements)
                        {
                            result.RecipeRequirementsInt.Add(new RecipeRequirementsInt
                            {
                                Index = index,
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.OperationType,
                                Message = requirement.Message
                            });
                        }
                    }

                    if (value.DIDRequirements != null)
                    {
                        foreach (var requirement in value.DIDRequirements)
                        {
                            result.RecipeRequirementsDID.Add(new RecipeRequirementsDID
                            {
                                Index = index,
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.OperationType,
                                Message = requirement.Message
                            });
                        }
                    }

                    if (value.IIDRequirements != null)
                    {
                        foreach (var requirement in value.IIDRequirements)
                        {
                            result.RecipeRequirementsIID.Add(new RecipeRequirementsIID
                            {
                                Index = index,
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.OperationType,
                                Message = requirement.Message
                            });
                        }
                    }

                    if (value.FloatRequirements != null)
                    {
                        foreach (var requirement in value.FloatRequirements)
                        {
                            result.RecipeRequirementsFloat.Add(new RecipeRequirementsFloat
                            {
                                Index = index,
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.OperationType,
                                Message = requirement.Message
                            });
                        }
                    }

                    if (value.StringRequirements != null)
                    {
                        foreach (var requirement in value.StringRequirements)
                        {
                            result.RecipeRequirementsString.Add(new RecipeRequirementsString
                            {
                                Index = index,
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.OperationType,
                            });
                        }
                    }

                    if (value.BoolRequirements != null)
                    {
                        foreach (var requirement in value.BoolRequirements)
                        {
                            result.RecipeRequirementsBool.Add(new RecipeRequirementsBool
                            {
                                Index = index,
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.OperationType,
                                Message = requirement.Message
                            });
                        }
                    }
                }

                for (int i = 0; i < 8; i++) // Must be 8
                {
                    var recipeMod = new RecipeMod();

                    var value = input.Mods[i];

                    if (value == null)
                        continue;

                    if (value.IntRequirements != null)
                    {
                        foreach (var mod in value.IntRequirements)
                        {
                            recipeMod.RecipeModsInt.Add(new RecipeModsInt
                            {
                                Index = (sbyte)i,
                                Stat = mod.Stat,
                                Value = mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            });
                        }
                    }

                    if (value.DIDRequirements != null)
                    {
                        foreach (var mod in value.DIDRequirements)
                        {
                            recipeMod.RecipeModsDID.Add(new RecipeModsDID
                            {
                                Index = (sbyte)i,
                                Stat = mod.Stat,
                                Value = mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            });
                        }
                    }

                    if (value.IIDRequirements != null)
                    {
                        foreach (var mod in value.IIDRequirements)
                        {
                            recipeMod.RecipeModsIID.Add(new RecipeModsIID
                            {
                                Index = (sbyte)i,
                                Stat = mod.Stat,
                                Value = mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            }); ;
                        }
                    }

                    if (value.FloatRequirements != null)
                    {
                        foreach (var mod in value.FloatRequirements)
                        {
                            recipeMod.RecipeModsFloat.Add(new RecipeModsFloat
                            {
                                Index = (sbyte)i,
                                Stat = mod.Stat,
                                Value = mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            });
                        }
                    }

                    if (value.StringRequirements != null)
                    {
                        foreach (var mod in value.StringRequirements)
                        {
                            recipeMod.RecipeModsString.Add(new RecipeModsString
                            {
                                Index = (sbyte)i,
                                Stat = mod.Stat,
                                Value = mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown
                            });
                        }
                    }

                    if (value.BoolRequirements != null)
                    {
                        foreach (var mod in value.BoolRequirements)
                        {
                            recipeMod.RecipeModsBool.Add(new RecipeModsBool
                            {
                                Index = (sbyte)i,
                                Stat = mod.Stat,
                                Value = mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            });
                        }
                    }

                    recipeMod.RecipeId = result.Id;

                    recipeMod.ExecutesOnSuccess = (i <= 3); // The first 4 are "act on success", the second 4 are "act on failure"

                    recipeMod.Health = value.ModifyHealth;
                    recipeMod.Stamina = value.ModifyStamina;
                    recipeMod.Mana = value.ModifyMana;

                    // TODO: ACE is missing RequiresHealth, RequiresStamina, RequiresMana

                    recipeMod.Unknown7 = value.Unknown7;
                    recipeMod.DataId = value.ModificationScriptId;

                    recipeMod.Unknown9 = value.Unknown9;
                    recipeMod.InstanceId = value.Unknown10;

                    bool add = (recipeMod.Health > 0 || recipeMod.Stamina > 0 || recipeMod.Mana > 0);
                    add = (add || recipeMod.Unknown7 || recipeMod.DataId > 0 || recipeMod.Unknown9 > 0 || recipeMod.InstanceId > 0);
                    add = (add || recipeMod.RecipeModsBool.Count > 0 || recipeMod.RecipeModsDID.Count > 0 || recipeMod.RecipeModsFloat.Count > 0 || recipeMod.RecipeModsIID.Count > 0 || recipeMod.RecipeModsInt.Count > 0 || recipeMod.RecipeModsString.Count > 0);

                    if (add)
                        result.RecipeMod.Add(recipeMod);
                }

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Converts ACE cookbook to GDLE precursor
        /// </summary>
        public static bool TryConvert(CookBook input, out Models.RecipePrecursor result)
        {
            try
            {
                result = new Models.RecipePrecursor();

                result.RecipeId = input.RecipeId;

                result.Tool = input.SourceWCID;
                result.Target = input.TargetWCID;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Converts a GDLE precursor to ACE cookbook
        /// </summary>
        public static bool TryConvert(Models.RecipePrecursor input, out CookBook result)
        {
            try
            {
                result = new CookBook();

                result.RecipeId = input.RecipeId ?? 0;

                result.SourceWCID = input.Tool;
                result.TargetWCID = input.Target;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Converts GDLE -> ACE wielded treasure table
        /// </summary>
        public static bool TryConvert(Models.WieldedTreasureTable input, out List<TreasureWielded> results)
        {
            try
            {
                results = new List<TreasureWielded>();

                foreach (var entry in input.Value)
                {
                    var result = new TreasureWielded();

                    result.TreasureType = input.Key;

                    result.ContinuesPreviousSet = entry.ContinuesPreviousSet;
                    result.HasSubSet = entry.HasSubSet;
                    result.PaletteId = entry.PaletteId;
                    result.Probability = entry.Probability;
                    result.SetStart = entry.SetStart;
                    result.Shade = entry.Shade;
                    result.StackSize = entry.StackSize;
                    result.StackSizeVariance = entry.StackSizeVariance;
                    result.Unknown1 = entry.Unknown1;
                    result.Unknown10 = entry.Unknown10;
                    result.Unknown11 = entry.Unknown11;
                    result.Unknown12 = entry.Unknown12;
                    result.Unknown3 = entry.Unknown3;
                    result.Unknown4 = entry.Unknown4;
                    result.Unknown5 = entry.Unknown5;
                    result.Unknown9 = entry.Unknown9;
                    result.WeenieClassId = entry.WeenieClassId;

                    results.Add(result);
                }

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }
    }
}
