using System;
using System.IO;

using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.SQLFormatters.World
{
    public class SpellSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.Id.ToString("00000") + " " + input.Name
        /// </summary>
        public string GetDefaultFileName(Spell input)
        {
            string fileName = input.Id.ToString("00000") + " " + input.Name;
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(Spell input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `spell` WHERE `id` = {input.Id};");
        }

        public void CreateSQLINSERTStatement(Spell input, StreamWriter writer)
        {
            var spellLineHdr = "INSERT INTO `spell` (`id`, `name`";

            var spellLine = $"VALUES ({input.Id}, {GetSQLString(input.Name)}";

            if (input.StatModType.HasValue)
            {
                spellLineHdr += ", `stat_Mod_Type`";
                spellLine += $", {input.StatModType} /* {((EnchantmentTypeFlags)input.StatModType).ToString()} */";
            }
            if (input.StatModKey.HasValue)
            {
                spellLineHdr += ", `stat_Mod_Key`";
                spellLine += $", {input.StatModKey}";

                if (input.StatModType.HasValue)
                {
                    var smt = (EnchantmentTypeFlags)input.StatModType;

                    if (smt.HasFlag(EnchantmentTypeFlags.Skill))
                    {
                        if (Enum.IsDefined(typeof(Skill), (int)input.StatModKey))
                            spellLine += $" /* {Enum.GetName(typeof(Skill), input.StatModKey)} */";
                    }
                    else if (smt.HasFlag(EnchantmentTypeFlags.Attribute))
                    {
                        if (Enum.IsDefined(typeof(PropertyAttribute), (ushort)input.StatModKey))
                            spellLine += $" /* {Enum.GetName(typeof(PropertyAttribute), input.StatModKey)} */";
                    }
                    else if (smt.HasFlag(EnchantmentTypeFlags.SecondAtt))
                    {
                        if (Enum.IsDefined(typeof(PropertyAttribute2nd), (ushort)input.StatModKey))
                            spellLine += $" /* {Enum.GetName(typeof(PropertyAttribute2nd), input.StatModKey)} */";
                    }
                    else if (smt.HasFlag(EnchantmentTypeFlags.Int))
                    {
                        if (Enum.IsDefined(typeof(PropertyInt), (ushort)input.StatModKey))
                            spellLine += $" /* {Enum.GetName(typeof(PropertyInt), input.StatModKey)} */";
                    }
                    else if (smt.HasFlag(EnchantmentTypeFlags.Float))
                    {
                        if (Enum.IsDefined(typeof(PropertyFloat), (ushort)input.StatModKey))
                            spellLine += $" /* {Enum.GetName(typeof(PropertyFloat), input.StatModKey)} */";
                    }                    
                }
            }
            if (input.StatModVal.HasValue)
            {
                spellLineHdr += ", `stat_Mod_Val`";
                spellLine += $", {input.StatModVal}";
            }

            if (input.EType.HasValue)
            {
                spellLineHdr += ", `e_Type`";
                spellLine += $", {input.EType} /* {Enum.GetName(typeof(DamageType), input.EType)} */";
            }

            if (input.BaseIntensity.HasValue)
            {
                spellLineHdr += ", `base_Intensity`";
                spellLine += $", {input.BaseIntensity}";
            }

            if (input.Variance.HasValue)
            {
                spellLineHdr += ", `variance`";
                spellLine += $", {input.Variance}";
            }

            if (input.Wcid.HasValue)
            {
                spellLineHdr += ", `wcid`";

                if (WeenieNames != null && input.Wcid.Value > 0 && WeenieNames.ContainsKey(input.Wcid.Value))
                    spellLine += $", {input.Wcid} /* {WeenieNames[input.Wcid.Value]} */";
                else
                    spellLine += $", {input.Wcid}";
            }

            if (input.NumProjectiles.HasValue)
            {
                spellLineHdr += ", `num_Projectiles`";
                spellLine += $", {input.NumProjectiles}";
            }

            if (input.NumProjectilesVariance.HasValue)
            {
                spellLineHdr += ", `num_Projectiles_Variance`";
                spellLine += $", {input.NumProjectilesVariance}";
            }

            if (input.SpreadAngle.HasValue)
            {
                spellLineHdr += ", `spread_Angle`";
                spellLine += $", {input.SpreadAngle}";
            }

            if (input.VerticalAngle.HasValue)
            {
                spellLineHdr += ", `vertical_Angle`";
                spellLine += $", {input.VerticalAngle}";
            }

            if (input.DefaultLaunchAngle.HasValue)
            {
                spellLineHdr += ", `default_Launch_Angle`";
                spellLine += $", {input.DefaultLaunchAngle}";
            }

            if (input.NonTracking.HasValue)
            {
                spellLineHdr += ", `non_Tracking`";
                spellLine += $", {input.NonTracking}";
            }

            if (input.CreateOffsetOriginX.HasValue)
            {
                spellLineHdr += ", `create_Offset_Origin_X`";
                spellLine += $", {input.CreateOffsetOriginX}";
            }
            if (input.CreateOffsetOriginY.HasValue)
            {
                spellLineHdr += ", `create_Offset_Origin_Y`";
                spellLine += $", {input.CreateOffsetOriginY}";
            }
            if (input.CreateOffsetOriginZ.HasValue)
            {
                spellLineHdr += ", `create_Offset_Origin_Z`";
                spellLine += $", {input.CreateOffsetOriginZ}";
            }

            if (input.PaddingOriginX.HasValue)
            {
                spellLineHdr += ", `padding_Origin_X`";
                spellLine += $", {input.PaddingOriginX}";
            }
            if (input.PaddingOriginY.HasValue)
            {
                spellLineHdr += ", `padding_Origin_Y`";
                spellLine += $", {input.PaddingOriginY}";
            }
            if (input.PaddingOriginZ.HasValue)
            {
                spellLineHdr += ", `padding_Origin_Z`";
                spellLine += $", {input.PaddingOriginZ}";
            }

            if (input.DimsOriginX.HasValue)
            {
                spellLineHdr += ", `dims_Origin_X`";
                spellLine += $", {input.DimsOriginX}";
            }
            if (input.DimsOriginY.HasValue)
            {
                spellLineHdr += ", `dims_Origin_Y`";
                spellLine += $", {input.DimsOriginY}";
            }
            if (input.DimsOriginZ.HasValue)
            {
                spellLineHdr += ", `dims_Origin_Z`";
                spellLine += $", {input.DimsOriginZ}";
            }

            if (input.PeturbationOriginX.HasValue)
            {
                spellLineHdr += ", `peturbation_Origin_X`";
                spellLine += $", {input.PeturbationOriginX}";
            }
            if (input.PeturbationOriginY.HasValue)
            {
                spellLineHdr += ", `peturbation_Origin_Y`";
                spellLine += $", {input.PeturbationOriginY}";
            }
            if (input.PeturbationOriginZ.HasValue)
            {
                spellLineHdr += ", `peturbation_Origin_Z`";
                spellLine += $", {input.PeturbationOriginZ}";
            }

            if (input.ImbuedEffect.HasValue)
            {
                spellLineHdr += ", `imbued_Effect`";
                spellLine += $", {input.ImbuedEffect} /* {Enum.GetName(typeof(ImbuedEffectType), input.ImbuedEffect)} */";
            }

            if (input.SlayerCreatureType.HasValue)
            {
                spellLineHdr += ", `slayer_Creature_Type`";
                spellLine += $", {input.SlayerCreatureType} /* {Enum.GetName(typeof(CreatureType), input.SlayerCreatureType)} */";
            }

            if (input.SlayerDamageBonus.HasValue)
            {
                spellLineHdr += ", `slayer_Damage_Bonus`";
                spellLine += $", {input.SlayerDamageBonus}";
            }

            if (input.CritFreq.HasValue)
            {
                spellLineHdr += ", `crit_Freq`";
                spellLine += $", {input.CritFreq}";
            }

            if (input.CritMultiplier.HasValue)
            {
                spellLineHdr += ", `crit_Multiplier`";
                spellLine += $", {input.CritMultiplier}";
            }

            if (input.IgnoreMagicResist.HasValue)
            {
                spellLineHdr += ", `ignore_Magic_Resist`";
                spellLine += $", {input.IgnoreMagicResist}";
            }

            if (input.ElementalModifier.HasValue)
            {
                spellLineHdr += ", `elemental_Modifier`";
                spellLine += $", {input.ElementalModifier}";
            }

            if (input.DrainPercentage.HasValue)
            {
                spellLineHdr += ", `drain_Percentage`";
                spellLine += $", {input.DrainPercentage}";
            }

            if (input.DamageRatio.HasValue)
            {
                spellLineHdr += ", `damage_Ratio`";
                spellLine += $", {input.DamageRatio}";
            }

            if (input.DamageType.HasValue)
            {
                spellLineHdr += ", `damage_Type`";
                spellLine += $", {(int)input.DamageType} /* {Enum.GetName(typeof(DamageType), input.DamageType)} */";
            }

            if (input.Boost.HasValue)
            {
                spellLineHdr += ", `boost`";
                spellLine += $", {input.Boost}";
            }

            if (input.BoostVariance.HasValue)
            {
                spellLineHdr += ", `boost_Variance`";
                spellLine += $", {input.BoostVariance}";
            }

            if (input.Source.HasValue)
            {
                spellLineHdr += ", `source`";
                spellLine += $", {(int)input.Source} /* {Enum.GetName(typeof(PropertyAttribute2nd), input.Source)} */";
            }

            if (input.Destination.HasValue)
            {
                spellLineHdr += ", `destination`";
                spellLine += $", {(int)input.Destination} /* {Enum.GetName(typeof(PropertyAttribute2nd), input.Destination)} */";
            }

            if (input.Proportion.HasValue)
            {
                spellLineHdr += ", `proportion`";
                spellLine += $", {input.Proportion}";
            }

            if (input.LossPercent.HasValue)
            {
                spellLineHdr += ", `loss_Percent`";
                spellLine += $", {input.LossPercent}";
            }

            if (input.SourceLoss.HasValue)
            {
                spellLineHdr += ", `source_Loss`";
                spellLine += $", {input.SourceLoss}";
            }

            if (input.TransferCap.HasValue)
            {
                spellLineHdr += ", `transfer_Cap`";
                spellLine += $", {input.TransferCap}";
            }

            if (input.MaxBoostAllowed.HasValue)
            {
                spellLineHdr += ", `max_Boost_Allowed`";
                spellLine += $", {input.MaxBoostAllowed}";
            }

            if (input.TransferBitfield.HasValue)
            {
                spellLineHdr += ", `transfer_Bitfield`";
                spellLine += $", {input.TransferBitfield} /* {((TransferFlags)input.TransferBitfield).ToString()} */";
            }

            if (input.Index.HasValue)
            {
                spellLineHdr += ", `index`";
                spellLine += $", {input.Index}";
            }

            if (input.Link.HasValue)
            {
                spellLineHdr += ", `link`";
                spellLine += $", {input.Link}";
            }

            if (input.PositionObjCellId.HasValue)
            {
                spellLineHdr += ", `position_Obj_Cell_ID`";
                spellLine += $", 0x{input.PositionObjCellId:X8}";
            }
            if (input.PositionOriginX.HasValue)
            {
                spellLineHdr += ", `position_Origin_X`";
                spellLine += $", {input.PositionOriginX}";
            }
            if (input.PositionOriginY.HasValue)
            {
                spellLineHdr += ", `position_Origin_Y`";
                spellLine += $", {input.PositionOriginY}";
            }
            if (input.PositionOriginZ.HasValue)
            {
                spellLineHdr += ", `position_Origin_Z`";
                spellLine += $", {input.PositionOriginZ}";
            }
            if (input.PositionAnglesW.HasValue)
            {
                spellLineHdr += ", `position_Angles_W`";
                spellLine += $", {input.PositionAnglesW}";
            }
            if (input.PositionAnglesX.HasValue)
            {
                spellLineHdr += ", `position_Angles_X`";
                spellLine += $", {input.PositionAnglesX}";
            }
            if (input.PositionAnglesY.HasValue)
            {
                spellLineHdr += ", `position_Angles_Y`";
                spellLine += $", {input.PositionAnglesY}";
            }
            if (input.PositionAnglesZ.HasValue)
            {
                spellLineHdr += ", `position_Angles_Z`";
                spellLine += $", {input.PositionAnglesZ}";
            }

            if (input.MinPower.HasValue)
            {
                spellLineHdr += ", `min_Power`";
                spellLine += $", {input.MinPower}";
            }

            if (input.MaxPower.HasValue)
            {
                spellLineHdr += ", `max_Power`";
                spellLine += $", {input.MaxPower}";
            }

            if (input.PowerVariance.HasValue)
            {
                spellLineHdr += ", `power_Variance`";
                spellLine += $", {input.PowerVariance}";
            }

            if (input.DispelSchool.HasValue)
            {
                spellLineHdr += ", `dispel_School`";
                spellLine += $", {(int)input.DispelSchool} /* {Enum.GetName(typeof(MagicSchool), input.DispelSchool)} */";
            }

            if (input.Align.HasValue)
            {
                spellLineHdr += ", `align`";
                spellLine += $", {input.Align}";
            }

            if (input.Number.HasValue)
            {
                spellLineHdr += ", `number`";
                spellLine += $", {input.Number}";
            }

            if (input.NumberVariance.HasValue)
            {
                spellLineHdr += ", `number_Variance`";
                spellLine += $", {input.NumberVariance}";
            }

            if (input.DotDuration.HasValue)
            {
                spellLineHdr += ", `dot_Duration`";
                spellLine += $", {input.DotDuration}";
            }

            
            spellLineHdr += ", `last_Modified`";
            spellLine += $", '{input.LastModified:yyyy-MM-dd HH:mm:ss}'";
            
            spellLineHdr += ")";
            spellLine += ");";

            spellLine = FixNullFields(spellLine);

            if (input.PositionObjCellId.HasValue && input.PositionOriginX.HasValue && input.PositionOriginY.HasValue && input.PositionOriginZ.HasValue && input.PositionAnglesX.HasValue && input.PositionAnglesY.HasValue && input.PositionAnglesZ.HasValue && input.PositionAnglesW.HasValue)
            {
                spellLine += Environment.NewLine + $"/* @teleloc 0x{input.PositionObjCellId.Value:X8} [{input.PositionOriginX.Value:F6} {input.PositionOriginY.Value:F6} {input.PositionOriginZ.Value:F6}] {input.PositionAnglesW.Value:F6} {input.PositionAnglesX.Value:F6} {input.PositionAnglesY.Value:F6} {input.PositionAnglesZ.Value:F6} */";
            }

            writer.WriteLine(spellLineHdr);
            writer.WriteLine(spellLine);
        }
    }
}
