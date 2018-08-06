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
            writer.WriteLine($"DELETE FROM `spell` WHERE `spell_Id` = {input.Id};");
        }

        public void CreateSQLINSERTStatement(Spell input, StreamWriter writer)
        {
            var spellLineHdr = "INSERT INTO `spell` (`id`, `name`, `description`, `school`, `icon_Id`, `category`, `bitfield`, `mana`, `range_Constant`, `range_Mod`, `power`, `economy_Mod`, `formula_Version`, `component_Loss`, `meta_Spell_Type`, `meta_Spell_Id`, `spell_Formula_Comp_1_Component_Id`, `spell_Formula_Comp_2_Component_Id`, `spell_Formula_Comp_3_Component_Id`, `spell_Formula_Comp_4_Component_Id`, `spell_Formula_Comp_5_Component_Id`, `spell_Formula_Comp_6_Component_Id`, `spell_Formula_Comp_7_Component_Id`, `spell_Formula_Comp_8_Component_Id`, `caster_Effect`, `target_Effect`, `fizzle_Effect`, `recovery_Interval`, `recovery_Amount`, `display_Order`, `non_Component_Target_Type`, `mana_Mod`";

            var spellLine = $"VALUES ({input.Id}, {GetSQLString(input.Name)}, {GetSQLString(input.Description)}, {input.School} /* {Enum.GetName(typeof(MagicSchool), input.School)} */, {input.IconId}, {input.Category}, {input.Bitfield} /* {((SpellBitfield)input.Bitfield).ToString()} */, {input.Mana}, {input.RangeConstant}, {input.RangeMod}, {input.Power}, {input.EconomyMod}, {input.FormulaVersion}, {input.ComponentLoss}, {input.MetaSpellType} /* {Enum.GetName(typeof(ACE.Entity.Enum.SpellType), input.MetaSpellType)} */, {input.MetaSpellId}, {input.SpellFormulaComp1ComponentId}, {input.SpellFormulaComp2ComponentId}, {input.SpellFormulaComp3ComponentId}, {input.SpellFormulaComp4ComponentId}, {input.SpellFormulaComp5ComponentId}, {input.SpellFormulaComp6ComponentId}, {input.SpellFormulaComp7ComponentId}, {input.SpellFormulaComp8ComponentId}, {input.CasterEffect}, {input.TargetEffect}, {input.FizzleEffect}, {input.RecoveryInterval}, {input.RecoveryAmount}, {input.DisplayOrder}, {input.NonComponentTargetType}, {input.ManaMod}";

            if (input.Duration.HasValue)
            {
                spellLineHdr += ", `duration`";
                spellLine += $", {input.Duration}";
            }

            if (input.DegradeModifier.HasValue)
            {
                spellLineHdr += ", `degrade_Modifier`";
                spellLine += $", {input.DegradeModifier}";
            }

            if (input.DegradeLimit.HasValue)
            {
                spellLineHdr += ", `degrade_Limit`";
                spellLine += $", {input.DegradeLimit}";
            }

            if (input.StatModType.HasValue)
            {
                spellLineHdr += ", `stat_Mod_Type`";
                spellLine += $", {input.StatModType} /* {((EnchantmentTypeFlags)input.StatModType).ToString()} */";
            }
            if (input.StatModKey.HasValue)
            {
                spellLineHdr += ", `stat_Mod_Key`";
                spellLine += $", {input.StatModKey}";
            }
            if (input.StatModVal.HasValue)
            {
                spellLineHdr += ", `stat_Mod_Val`";
                spellLine += $", {input.StatModVal}";
            }

            if (input.EType.HasValue)
            {
                spellLineHdr += ", `e_Type`";
                spellLine += $", {input.EType}";
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

                if (WeenieNames != null)
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
                spellLine += $", {input.TransferBitfield}";
            }

            if (input.Index.HasValue)
            {
                spellLineHdr += ", `index`";
                spellLine += $", {input.Index}";
            }

            if (input.PortalLifetime.HasValue)
            {
                spellLineHdr += ", `portal_Lifetime`";
                spellLine += $", {input.PortalLifetime}";
            }

            if (input.Link.HasValue)
            {
                spellLineHdr += ", `link`";
                spellLine += $", {input.Link}";
            }

            if (input.PositionObjCellId.HasValue)
            {
                spellLineHdr += ", `position_Obj_Cell_ID`";
                spellLine += $", {input.PositionObjCellId}";
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

            spellLineHdr += ")";
            spellLine += ");";

            spellLine = FixNullFields(spellLine);

            writer.WriteLine(spellLineHdr);
            writer.WriteLine(spellLine);
        }
    }
}
