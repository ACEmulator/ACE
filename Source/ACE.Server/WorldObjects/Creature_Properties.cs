using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

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
            get => GetProperty(PropertyFloat.ResistHealthBoost);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistHealthBoost); else SetProperty(PropertyFloat.ResistHealthBoost, value.Value); }
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

        private double GetResistanceMod(DamageType damageType, double bonusMultiplier)
        {
            var spellVuln = EnchantmentManager.GetVulnerabilityResistanceMod(damageType);
            var spellProt = EnchantmentManager.GetProtectionResistanceMod(damageType);

            if (bonusMultiplier > spellVuln)
                return bonusMultiplier * spellProt;

            return spellVuln * spellProt;
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

        public virtual double GetNaturalResistance(ResistanceType resistance)
        {
            // TODO: player override for natural resistances
            switch (resistance)
            {
                case ResistanceType.Slash:
                    return ResistSlash ?? 1.0;
                case ResistanceType.Pierce:
                    return ResistPierce ?? 1.0;
                case ResistanceType.Bludgeon:
                    return ResistBludgeon ?? 1.0;
                case ResistanceType.Fire:
                    return ResistFire ?? 1.0;
                case ResistanceType.Cold:
                    return ResistCold ?? 1.0;
                case ResistanceType.Acid:
                    return ResistAcid ?? 1.0;
                case ResistanceType.Electric:
                    return ResistElectric ?? 1.0;
                case ResistanceType.Nether:
                    return ResistNether ?? 1.0;
                case ResistanceType.HealthBoost:
                    return ResistHealthBoost ?? 1.0;
                case ResistanceType.HealthDrain:
                    return ResistHealthDrain ?? 1.0;
                case ResistanceType.StaminaBoost:
                    return ResistStaminaBoost ?? 1.0;
                case ResistanceType.StaminaDrain:
                    return ResistStaminaDrain ?? 1.0;
                case ResistanceType.ManaBoost:
                    return ResistManaBoost ?? 1.0;
                case ResistanceType.ManaDrain:
                    return ResistManaDrain ?? 1.0;
                default:
                    return 1.0;
            }
        }

        public double GetResistanceMod(ResistanceType resistance, float weaponResistanceMod = 1.0f)
        {
            switch (resistance)
            {
                case ResistanceType.Slash:
                    return (ResistSlash ?? 1.0) * GetResistanceMod(DamageType.Slash, weaponResistanceMod);
                case ResistanceType.Pierce:
                    return (ResistPierce ?? 1.0) * GetResistanceMod(DamageType.Pierce, weaponResistanceMod);
                case ResistanceType.Bludgeon:
                    return (ResistBludgeon ?? 1.0) * GetResistanceMod(DamageType.Bludgeon, weaponResistanceMod);
                case ResistanceType.Fire:
                    return (ResistFire ?? 1.0) * GetResistanceMod(DamageType.Fire, weaponResistanceMod);
                case ResistanceType.Cold:
                    return (ResistCold ?? 1.0) * GetResistanceMod(DamageType.Cold, weaponResistanceMod);
                case ResistanceType.Acid:
                    return (ResistAcid ?? 1.0) * GetResistanceMod(DamageType.Acid, weaponResistanceMod);
                case ResistanceType.Electric:
                    return (ResistElectric ?? 1.0) * GetResistanceMod(DamageType.Electric, weaponResistanceMod);
                case ResistanceType.Nether:
                    return ResistNetherMod;
                case ResistanceType.HealthBoost:
                    return ResistHealthBoostMod;
                case ResistanceType.HealthDrain:
                    return ResistHealthDrainMod;
                case ResistanceType.StaminaBoost:
                    return ResistStaminaBoostMod;
                case ResistanceType.StaminaDrain:
                    return ResistStaminaDrainMod;
                case ResistanceType.ManaBoost:
                    return ResistManaBoostMod;
                case ResistanceType.ManaDrain:
                    return ResistManaDrainMod;
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

        public int ArmorLevelMod => (ArmorLevel ?? 0) + EnchantmentManager.GetBodyArmorMod();
        public double ResistSlashMod => (ResistSlash ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Slash);
        public double ResistPierceMod => (ResistPierce ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Pierce);
        public double ResistBludgeonMod => (ResistBludgeon ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Bludgeon);
        public double ResistFireMod => (ResistFire ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Fire);
        public double ResistColdMod => (ResistCold ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Cold);
        public double ResistAcidMod => (ResistAcid ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Acid);
        public double ResistElectricMod => (ResistElectric ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Electric);
        public double ResistNetherMod => (ResistNether ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Nether);
        public double ResistHealthDrainMod => (ResistHealthDrain ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Health);
        public double ResistHealthBoostMod => (ResistHealthBoost ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Health);
        public double ResistStaminaDrainMod => (ResistStaminaDrain ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Stamina);
        public double ResistStaminaBoostMod => (ResistStaminaBoost ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Stamina);
        public double ResistManaDrainMod => (ResistManaDrain ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Mana);
        public double ResistManaBoostMod => (ResistManaBoost ?? 1.0) * EnchantmentManager.GetResistanceMod(DamageType.Mana);

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
    }
}
