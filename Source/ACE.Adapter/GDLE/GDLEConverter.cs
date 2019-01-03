using System;
using System.Collections.Generic;

using ACE.Database.Models.World;

namespace ACE.Adapter.GDLE
{
    public static class GDLEConverter
    {
        public static bool TryConvert(Models.Landblock input, out List<Database.Models.World.LandblockInstance> results, out List<Database.Models.World.LandblockInstanceLink> links)
        {
            try
            {
                results = new List<LandblockInstance>();
                links = new List<LandblockInstanceLink>();

                foreach (var value in input.Value.Weenies)
                {
                    var result = new LandblockInstance();

                    result.Guid = value.Id;     // TODO!!! I think we need to scale these to fit ACE model
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

                        result.ParentGuid = value.Source;   // TODO!!! I'm not sure about the order of these.. is source the parent, or child?
                        result.ChildGuid = value.Target;    // TODO!!! I'm not sure about the order of these.. is source the parent, or child?

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

                // result.Id // TODO!!! is this id'd by index? If so, the parent caller needs to set the id... or this function could take an argument that specifies the id.

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

                // result.Id // TODO!!! is this id'd by index? If so, the parent caller needs to set the id... or this function could take an argument that specifies the id.

                result.Name = input.Key;

                result.MinDelta = (uint)input.Value.MinDelta; // todo the jsons have values of -1 here sometimes
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

                result.Description = input.Desc;

                result.School = input.School;
                result.IconId = input.IconId;
                result.Category = input.Category; // aka Family
                result.Bitfield = input.Bitfield;
                result.Mana = input.BaseMana;
                result.RangeConstant = (float)input.BaseRangeConstant;
                result.RangeMod = (float)input.BaseRangeMod;
                result.Power = input.Power; // aka Difficulty
                result.EconomyMod = input.SpellEconomyMod;
                result.FormulaVersion = input.FormulaVersion;
                result.ComponentLoss = (float)input.ComponentLoss;

                result.MetaSpellType = input.MetaSpell.Type;
                result.MetaSpellId = input.MetaSpell.Spell.SpellId; // Just the spell id again

                if (input.Formula.Count >= 1) result.SpellFormulaComp1ComponentId = (uint)input.Formula[0]; // todo how to handle negative numbers
                if (input.Formula.Count >= 2) result.SpellFormulaComp2ComponentId = (uint)input.Formula[1]; // todo how to handle negative numbers
                if (input.Formula.Count >= 3) result.SpellFormulaComp3ComponentId = (uint)input.Formula[2]; // todo how to handle negative numbers
                if (input.Formula.Count >= 4) result.SpellFormulaComp4ComponentId = (uint)input.Formula[3]; // todo how to handle negative numbers
                if (input.Formula.Count >= 5) result.SpellFormulaComp5ComponentId = (uint)input.Formula[4]; // todo how to handle negative numbers
                if (input.Formula.Count >= 6) result.SpellFormulaComp6ComponentId = (uint)input.Formula[5]; // todo how to handle negative numbers
                if (input.Formula.Count >= 7) result.SpellFormulaComp7ComponentId = (uint)input.Formula[6]; // todo how to handle negative numbers
                if (input.Formula.Count >= 8) result.SpellFormulaComp8ComponentId = (uint)input.Formula[7]; // todo how to handle negative numbers

                result.CasterEffect = input.CasterEffect;
                result.TargetEffect = input.TargetEffect;
                result.FizzleEffect = input.FizzleEffect;
                result.RecoveryInterval = input.RecoveryInterval;
                result.RecoveryAmount = input.RecoveryAmount;
                result.DisplayOrder = input.DisplayOrder;
                result.NonComponentTargetType = input.NonComponentTargetType; // aka Target Mask
                result.ManaMod = input.ManaMod;

                // EnchantmentSpell/FellowshipEnchantmentSpells
                result.Duration = input.MetaSpell.Spell.Duration;
                result.DegradeModifier = input.MetaSpell.Spell.DegradeModifier;
                result.DegradeLimit = (float?)input.MetaSpell.Spell.DegradeLimit;
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
                result.PortalLifetime = input.MetaSpell.Spell.PortalLifetime;
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
