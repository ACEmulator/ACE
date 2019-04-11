using System;
using System.Collections.Generic;

using ACE.Database.Models.World;

namespace ACE.Adapter.GDLE
{
    public static class GDLEConverter
    {
        /// <summary>
        /// This will not alter the Guid. To sanitize the Guid for ACE usage, you should use GDLELoader.TryLoadWorldSpawnsConverted() instead.
        /// </summary>
        public static bool TryConvert(Models.Landblock input, out List<Database.Models.World.LandblockInstance> results, out List<Database.Models.World.LandblockInstanceLink> links)
        {
            try
            {
                results = new List<LandblockInstance>();
                links = new List<LandblockInstanceLink>();

                foreach (var value in input.Value.Weenies)
                {
                    var result = new LandblockInstance();

                    result.Guid = value.Id; // Collisions and other errors can be caused by invalid input. Data should be sanitized by the running ACE server.
                    //result.Landblock = input.key; ACE uses a virtual column here of (result.ObjCellId >> 16)
                    result.WeenieClassId = value.WCID;

                    result.ObjCellId = value.Position.ObjCellId;
                    result.OriginX = (float)value.Position.Frame.Origin.X;
                    result.OriginY = (float)value.Position.Frame.Origin.Y;
                    result.OriginZ = (float)value.Position.Frame.Origin.Z;
                    result.AnglesW = (float)value.Position.Frame.Angles.W;
                    result.AnglesX = (float)value.Position.Frame.Angles.X;
                    result.AnglesY = (float)value.Position.Frame.Angles.Y;
                    result.AnglesZ = (float)value.Position.Frame.Angles.Z;

                    results.Add(result);
                }

                if (input.Value.Links != null)
                {
                    foreach (var value in input.Value.Links)
                    {
                        var result = new LandblockInstanceLink();

                        result.ParentGuid = value.Target;
                        result.ChildGuid = value.Source;

                        links.Add(result);
                    }
                }

                return true;
            }
            catch
            {
                results = null;
                links = null;
                return false;
            }
        }


        public static bool TryConvert(Models.Event input, out Database.Models.World.Event result)
        {
            try
            {
                result = new Database.Models.World.Event();

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


        public static bool TryConvert(Models.Quest input, out Database.Models.World.Quest result)
        {
            try
            {
                result = new Database.Models.World.Quest();

                //result.Id // This is an Auto Increment field in the ACE schema

                result.Name = input.Key;

                result.MinDelta = (input.Value.MinDelta <= 0) ? 0 : (uint)input.Value.MinDelta; // the jsons have values of -1 here sometimes
                result.MaxSolves = input.Value.MaxSolves;
                result.Message = input.Value.FullName;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }


        public static bool TryConvert(uint id, Models.SpellValue input, out Database.Models.World.Spell result)
        {
            try
            {
                result = new Database.Models.World.Spell();

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
                    result.PositionObjCellId = input.MetaSpell.Spell.Position.ObjCellId;

                    result.PositionOriginX = (float)input.MetaSpell.Spell.Position.Frame.Origin.X;
                    result.PositionOriginY = (float)input.MetaSpell.Spell.Position.Frame.Origin.Y;
                    result.PositionOriginZ = (float)input.MetaSpell.Spell.Position.Frame.Origin.Z;

                    result.PositionAnglesW = (float)input.MetaSpell.Spell.Position.Frame.Angles.W;
                    result.PositionAnglesX = (float)input.MetaSpell.Spell.Position.Frame.Angles.X;
                    result.PositionAnglesY = (float)input.MetaSpell.Spell.Position.Frame.Angles.Y;
                    result.PositionAnglesZ = (float)input.MetaSpell.Spell.Position.Frame.Angles.Z;
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


        public static bool TryConvert(Models.Recipe input, out Database.Models.World.Recipe result)
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

                foreach (var value in input.Requirements)
                {
                    if (value == null)
                        continue;

                    // TODO!!! GDLE only has 2 requirements, int and bool. Why do we have all of them defined?
                    // TODO!!! GDLE requirements also define properties OperationType and Unknown

                    if (value.IntRequirements != null)
                    {
                        foreach (var requirement in value.IntRequirements)
                        {
                            result.RecipeRequirementsInt.Add(new RecipeRequirementsInt
                            {
                                Stat = requirement.Stat,
                                Value = (int)requirement.Value,
                                Enum = requirement.OperationType,
                                Message = requirement.Message
                            });
                        }
                    }

                    /*if (value.DIDRequirements != null)
                    {
                        foreach (var requirement in value.DIDRequirements)
                        {
                            result.RecipeRequirementsDID.Add(new RecipeRequirementsDID
                            {
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.Enum,
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
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.Enum,
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
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.Enum,
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
                                Stat = requirement.Stat,
                                Value = requirement.Value,
                                Enum = requirement.Enum,
                                Message = requirement.Message
                            });
                        }
                    }*/

                    if (value.BoolRequirements != null)
                    {
                        foreach (var requirement in value.BoolRequirements)
                        {
                            result.RecipeRequirementsBool.Add(new RecipeRequirementsBool
                            {
                                Stat = requirement.Stat,
                                Value = (requirement.Value != 0),
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
                                Stat = mod.Stat,
                                Value = (int)mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            });
                        }
                    }

                    if (value.DidRequirements != null)
                    {
                        foreach (var mod in value.DidRequirements)
                        {
                            recipeMod.RecipeModsDID.Add(new RecipeModsDID
                            {
                                Stat = mod.Stat,
                                Value = (uint)mod.Value,
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            });
                        }
                    }

                    // TODO!!!! GDLE doesn't have this
                    /*if (value.IIDMods != null)
                    {
                        foreach (var mod in value.IIDMods)
                        {
                            recipeMod.RecipeModsIID.Add(new RecipeModsIID
                            {
                                Stat = mod.Stat,
                                Value = mod.Value,
                                Enum = mod.Enum,
                                Unknown1 = mod.Unknown1
                            });
                        }
                    }*/

                    if (value.FloatRequirements != null)
                    {
                        foreach (var mod in value.FloatRequirements)
                        {
                            recipeMod.RecipeModsFloat.Add(new RecipeModsFloat
                            {
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
                                Stat = mod.Stat,
                                Value = (mod.Value != 0),
                                Enum = mod.OperationType,
                                Source = mod.Unknown ?? 0
                            });
                        }
                    }

                    recipeMod.RecipeId = result.Id;

                    recipeMod.ExecutesOnSuccess = (i <= 3); // The first 4 are "act on success", the second 4 are "act on failure"

                    // TODO!!!
                    recipeMod.Health = value.ModifyHealth;
                    recipeMod.Stamina = value.ModifyStamina;
                    recipeMod.Mana = value.ModifyMana;
                    // TODO!!! we're missing the following RequiresHealth, RequiresStamina, RequiresMana

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

        public static bool TryConvert(Models.RecipePrecursor input, out Database.Models.World.CookBook result)
        {
            try
            {
                result = new CookBook();

                result.RecipeId = input.RecipeId;

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
    }
}
