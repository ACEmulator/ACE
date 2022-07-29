using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public double? ResistSlash
        {
            get => GetProperty(PropertyFloat.ResistSlash);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistSlash); else SetProperty(PropertyFloat.ResistSlash, value.Value); }
        }

        public double? ResistPierce
        {
            get => GetProperty(PropertyFloat.ResistPierce);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistPierce); else SetProperty(PropertyFloat.ResistPierce, value.Value); }
        }

        public double? ResistBludgeon
        {
            get => GetProperty(PropertyFloat.ResistBludgeon);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistBludgeon); else SetProperty(PropertyFloat.ResistBludgeon, value.Value); }
        }

        public double? ResistFire
        {
            get => GetProperty(PropertyFloat.ResistFire);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistFire); else SetProperty(PropertyFloat.ResistFire, value.Value); }
        }

        public double? ResistCold
        {
            get => GetProperty(PropertyFloat.ResistCold);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistCold); else SetProperty(PropertyFloat.ResistCold, value.Value); }
        }

        public double? ResistAcid
        {
            get => GetProperty(PropertyFloat.ResistAcid);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistAcid); else SetProperty(PropertyFloat.ResistAcid, value.Value); }
        }

        public double? ResistElectric
        {
            get => GetProperty(PropertyFloat.ResistElectric);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistElectric); else SetProperty(PropertyFloat.ResistElectric, value.Value); }
        }

        public double? ResistHealthDrain
        {
            get => GetProperty(PropertyFloat.ResistHealthDrain);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistHealthDrain); else SetProperty(PropertyFloat.ResistHealthDrain, value.Value); }
        }

        public double? ResistHealthBoost
        {
            get => GetProperty(PropertyFloat.ResistHealthBoost);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistHealthBoost); else SetProperty(PropertyFloat.ResistHealthBoost, value.Value); }
        }

        public double? ResistStaminaDrain
        {
            get => GetProperty(PropertyFloat.ResistStaminaDrain);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistStaminaDrain); else SetProperty(PropertyFloat.ResistStaminaDrain, value.Value); }
        }

        public double? ResistStaminaBoost
        {
            get => GetProperty(PropertyFloat.ResistStaminaBoost);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistStaminaBoost); else SetProperty(PropertyFloat.ResistStaminaBoost, value.Value); }
        }

        public double? ResistManaDrain
        {
            get => GetProperty(PropertyFloat.ResistManaDrain);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistManaDrain); else SetProperty(PropertyFloat.ResistManaDrain, value.Value); }
        }

        public double? ResistManaBoost
        {
            get => GetProperty(PropertyFloat.ResistManaBoost);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistManaBoost); else SetProperty(PropertyFloat.ResistManaBoost, value.Value); }
        }

        public double? ResistNether
        {
            get => GetProperty(PropertyFloat.ResistNether);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistNether); else SetProperty(PropertyFloat.ResistNether, value.Value); }
        }

        public bool NonProjectileMagicImmune
        {
            get => GetProperty(PropertyBool.NonProjectileMagicImmune) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.NonProjectileMagicImmune); else SetProperty(PropertyBool.NonProjectileMagicImmune, value); }
        }

        public float GetResistanceMod(DamageType damageType, WorldObject attacker, WorldObject weapon, float weaponResistanceMod = 1.0f)
        {
            var ignoreMagicResist = (weapon?.IgnoreMagicResist ?? false) || (attacker?.IgnoreMagicResist ?? false);

            // hollow weapons also ignore player natural resistances
            if (ignoreMagicResist)
            {
                if (!(attacker is Player) || !(this is Player) || PropertyManager.GetDouble("ignore_magic_resist_pvp_scalar").Item == 1.0)
                    return weaponResistanceMod;
            }

            var protMod = EnchantmentManager.GetProtectionResistanceMod(damageType);
            var vulnMod = EnchantmentManager.GetVulnerabilityResistanceMod(damageType);

            var naturalResistMod = GetNaturalResistance(damageType);

            // protection mod becomes either life protection or natural resistance,
            // whichever is more powerful (more powerful = lower value here)
            if (protMod > naturalResistMod)
                protMod = naturalResistMod;

            // does this stack with natural resistance?
            if (this is Player player)
            {
                var resistAug = player.GetAugmentationResistance(damageType);
                if (resistAug > 0)
                {
                    var augFactor = Math.Min(1.0f, resistAug * 0.1f);
                    protMod *= 1.0f - augFactor;
                }
            }

            // vulnerability mod becomes either life vuln or weapon resistance mod,
            // whichever is more powerful
            if (vulnMod < weaponResistanceMod)
                vulnMod = weaponResistanceMod;

            if (ignoreMagicResist)
            {
                // convert to additive space
                var addProt = -ModToRating(protMod);
                var addVuln = ModToRating(vulnMod);

                // scale
                addProt = IgnoreMagicResistScaled(addProt);
                addVuln = IgnoreMagicResistScaled(addVuln);

                protMod = GetNegativeRatingMod(addProt);
                vulnMod = GetPositiveRatingMod(addVuln);
            }

            return protMod * vulnMod;
        }

        public virtual float GetNaturalResistance(DamageType damageType)
        {
            // overridden for players
            return 1.0f;
        }

        public double GetArmorVsType(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Slash:
                    return GetProperty(PropertyFloat.ArmorModVsSlash) ?? 1.0f;
                case DamageType.Pierce:
                    return GetProperty(PropertyFloat.ArmorModVsPierce) ?? 1.0f;
                case DamageType.Bludgeon:
                    return GetProperty(PropertyFloat.ArmorModVsBludgeon) ?? 1.0f;
                case DamageType.Fire:
                    return GetProperty(PropertyFloat.ArmorModVsFire) ?? 1.0f;
                case DamageType.Cold:
                    return GetProperty(PropertyFloat.ArmorModVsCold) ?? 1.0f;
                case DamageType.Acid:
                    return GetProperty(PropertyFloat.ArmorModVsAcid) ?? 1.0f;
                case DamageType.Electric:
                    return GetProperty(PropertyFloat.ArmorModVsElectric) ?? 1.0f;
                case DamageType.Nether:
                    return GetProperty(PropertyFloat.ArmorModVsNether) ?? 1.0f;
                default:
                    return 1.0f;
            }
        }

        public double GetResistanceMod(ResistanceType resistance, WorldObject attacker = null, WorldObject weapon = null, float weaponResistanceMod = 1.0f)
        {
            switch (resistance)
            {
                case ResistanceType.Slash:
                    return (ResistSlash ?? 1.0) * GetResistanceMod(DamageType.Slash, attacker, weapon, weaponResistanceMod);
                case ResistanceType.Pierce:
                    return (ResistPierce ?? 1.0) * GetResistanceMod(DamageType.Pierce, attacker, weapon, weaponResistanceMod);
                case ResistanceType.Bludgeon:
                    return (ResistBludgeon ?? 1.0) * GetResistanceMod(DamageType.Bludgeon, attacker, weapon, weaponResistanceMod);
                case ResistanceType.Fire:
                    return (ResistFire ?? 1.0) * GetResistanceMod(DamageType.Fire, attacker, weapon, weaponResistanceMod);
                case ResistanceType.Cold:
                    return (ResistCold ?? 1.0) * GetResistanceMod(DamageType.Cold, attacker, weapon, weaponResistanceMod);
                case ResistanceType.Acid:
                    return (ResistAcid ?? 1.0) * GetResistanceMod(DamageType.Acid, attacker, weapon, weaponResistanceMod);
                case ResistanceType.Electric:
                    return (ResistElectric ?? 1.0) * GetResistanceMod(DamageType.Electric, attacker, weapon, weaponResistanceMod);
                case ResistanceType.Nether:
                    return (ResistNether ?? 1.0) * GetResistanceMod(DamageType.Nether, attacker, weapon, weaponResistanceMod);
                case ResistanceType.HealthBoost:
                    return (ResistHealthBoost ?? 1.0) * GetHealingRatingMod();
                case ResistanceType.HealthDrain:
                    return (ResistHealthDrain ?? 1.0) * GetNaturalResistance(DamageType.Health) * GetLifeResistRatingMod();
                case ResistanceType.StaminaBoost:
                    return (ResistStaminaBoost ?? 1.0) * GetHealingRatingMod();     // does healing rating affect these?
                case ResistanceType.StaminaDrain:
                    return (ResistStaminaDrain ?? 1.0) * GetNaturalResistance(DamageType.Stamina);
                case ResistanceType.ManaBoost:
                    return (ResistManaBoost ?? 1.0) * GetHealingRatingMod();
                case ResistanceType.ManaDrain:
                    return (ResistManaDrain ?? 1.0) * GetNaturalResistance(DamageType.Mana);
                default:
                    return 1.0;
            }
        }

        public double? HealthRate
        {
            get => GetProperty(PropertyFloat.HealthRate);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.HealthRate); else SetProperty(PropertyFloat.HealthRate, value.Value); }
        }

        public double? StaminaRate
        {
            get => GetProperty(PropertyFloat.StaminaRate);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.StaminaRate); else SetProperty(PropertyFloat.StaminaRate, value.Value); }
        }

        public double ResistSlashMod => (ResistSlash ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Slash);
        public double ResistPierceMod => (ResistPierce ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Pierce);
        public double ResistBludgeonMod => (ResistBludgeon ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Bludgeon);
        public double ResistFireMod => (ResistFire ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Fire);
        public double ResistColdMod => (ResistCold ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Cold);
        public double ResistAcidMod => (ResistAcid ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Acid);
        public double ResistElectricMod => (ResistElectric ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Electric);
        public double ResistNetherMod => (ResistNether ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Nether);

        public bool NoCorpse
        {
            get => GetProperty(PropertyBool.NoCorpse) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.NoCorpse); else SetProperty(PropertyBool.NoCorpse, value); }
        }

        public bool TreasureCorpse
        {
            get => GetProperty(PropertyBool.TreasureCorpse) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.TreasureCorpse); else SetProperty(PropertyBool.TreasureCorpse, value); }
        }

        public uint? DeathTreasureType
        {
            get => GetProperty(PropertyDataId.DeathTreasureType);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DeathTreasureType); else SetProperty(PropertyDataId.DeathTreasureType, value.Value); }
        }

        public int? LuminanceAward
        {
            get => GetProperty(PropertyInt.LuminanceAward);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.LuminanceAward); else SetProperty(PropertyInt.LuminanceAward, value.Value); }
        }

        public bool AiImmobile
        {
            get => GetProperty(PropertyBool.AiImmobile) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AiImmobile); else SetProperty(PropertyBool.AiImmobile, value); }
        }

        public int? Overpower
        {
            get => GetProperty(PropertyInt.Overpower);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Overpower); else SetProperty(PropertyInt.Overpower, value.Value); }
        }

        public int? OverpowerResist
        {
            get => GetProperty(PropertyInt.OverpowerResist);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.OverpowerResist); else SetProperty(PropertyInt.OverpowerResist, value.Value); }
        }

        public string KillQuest
        {
            get => GetProperty(PropertyString.KillQuest);
            set { if (value == null) RemoveProperty(PropertyString.KillQuest); else SetProperty(PropertyString.KillQuest, value); }
        }

        public string KillQuest2
        {
            get => GetProperty(PropertyString.KillQuest2);
            set { if (value == null) RemoveProperty(PropertyString.KillQuest2); else SetProperty(PropertyString.KillQuest2, value); }
        }

        public string KillQuest3
        {
            get => GetProperty(PropertyString.KillQuest3);
            set { if (value == null) RemoveProperty(PropertyString.KillQuest3); else SetProperty(PropertyString.KillQuest3, value); }
        }

        public FactionBits? Faction1Bits
        {
            get => (FactionBits?)GetProperty(PropertyInt.Faction1Bits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Faction1Bits); else SetProperty(PropertyInt.Faction1Bits, (int)value); }
        }

        public int? Faction2Bits
        {
            get => GetProperty(PropertyInt.Faction2Bits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Faction2Bits); else SetProperty(PropertyInt.Faction2Bits, value.Value); }
        }

        public int? Faction3Bits
        {
            get => GetProperty(PropertyInt.Faction3Bits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Faction3Bits); else SetProperty(PropertyInt.Faction3Bits, value.Value); }
        }

        public int? Hatred1Bits
        {
            get => GetProperty(PropertyInt.Hatred1Bits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Hatred1Bits); else SetProperty(PropertyInt.Hatred1Bits, value.Value); }
        }

        public int? Hatred2Bits
        {
            get => GetProperty(PropertyInt.Hatred2Bits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Hatred2Bits); else SetProperty(PropertyInt.Hatred2Bits, value.Value); }
        }

        public int? Hatred3Bits
        {
            get => GetProperty(PropertyInt.Hatred3Bits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Hatred3Bits); else SetProperty(PropertyInt.Hatred3Bits, value.Value); }
        }

        public int? SocietyRankCelhan
        {
            get => GetProperty(PropertyInt.SocietyRankCelhan);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.SocietyRankCelhan); else SetProperty(PropertyInt.SocietyRankCelhan, value.Value); }
        }

        public int? SocietyRankEldweb
        {
            get => GetProperty(PropertyInt.SocietyRankEldweb);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.SocietyRankEldweb); else SetProperty(PropertyInt.SocietyRankEldweb, value.Value); }
        }

        public int? SocietyRankRadblo
        {
            get => GetProperty(PropertyInt.SocietyRankRadblo);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.SocietyRankRadblo); else SetProperty(PropertyInt.SocietyRankRadblo, value.Value); }
        }

        public FactionBits Society => Faction1Bits ?? FactionBits.None;
    }
}
