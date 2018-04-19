using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public bool? NoCorpse
        {
            get => GetProperty(PropertyBool.NoCorpse);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.NoCorpse); else SetProperty(PropertyBool.NoCorpse, value.Value); }
        }

        public uint? Killer
        {
            get => GetProperty(PropertyInstanceId.Killer);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Killer); else SetProperty(PropertyInstanceId.Killer, value.Value); }
        }

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

        public double GetNaturalResistence(ResistanceType resistance)
        {
            switch (resistance)
            {
                case ResistanceType.Slash:
                    return ResistSlashMod;
                case ResistanceType.Pierce:
                    return ResistPierceMod;
                case ResistanceType.Bludgeon:
                    return ResistBludgeonMod;
                case ResistanceType.Fire:
                    return ResistFireMod;
                case ResistanceType.Cold:
                    return ResistColdMod;
                case ResistanceType.Acid:
                    return ResistAcidMod;
                case ResistanceType.Electric:
                    return ResistElectricMod;
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

        public double? ManaRate
        {
            get => GetProperty(PropertyFloat.ManaRate);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ManaRate); else SetProperty(PropertyFloat.ManaRate, value.Value); }
        }

        public double? ArmorModVsSlash
        {
            get => GetProperty(PropertyFloat.ArmorModVsSlash);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsSlash); else SetProperty(PropertyFloat.ArmorModVsSlash, value.Value); }
        }
        public double? ArmorModVsPierce
        {
            get => GetProperty(PropertyFloat.ArmorModVsPierce);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsPierce); else SetProperty(PropertyFloat.ArmorModVsPierce, value.Value); }
        }
        public double? ArmorModVsBludgeon
        {
            get => GetProperty(PropertyFloat.ArmorModVsBludgeon);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsBludgeon); else SetProperty(PropertyFloat.ArmorModVsBludgeon, value.Value); }
        }
        public double? ArmorModVsCold
        {
            get => GetProperty(PropertyFloat.ArmorModVsCold);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsCold); else SetProperty(PropertyFloat.ArmorModVsCold, value.Value); }
        }
        public double? ArmorModVsFire
        {
            get => GetProperty(PropertyFloat.ArmorModVsFire);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsFire); else SetProperty(PropertyFloat.ArmorModVsFire, value.Value); }
        }
        public double? ArmorModVsAcid
        {
            get => GetProperty(PropertyFloat.ArmorModVsAcid);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsAcid); else SetProperty(PropertyFloat.ArmorModVsAcid, value.Value); }
        }
        public double? ArmorModVsElectric
        {
            get => GetProperty(PropertyFloat.ArmorModVsElectric);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsElectric); else SetProperty(PropertyFloat.ArmorModVsElectric, value.Value); }
        }

        public double? ArmorModVsNether
        {
            get => GetProperty(PropertyFloat.ArmorModVsNether);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ArmorModVsNether); else SetProperty(PropertyFloat.ArmorModVsNether, value.Value); }
        }

        public int? ArmorType
        {
            get => GetProperty(PropertyInt.ArmorType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ArmorType); else SetProperty(PropertyInt.ArmorType, value.Value); }
        }

        public int? ArmorLevel
        {
            get => GetProperty(PropertyInt.ArmorLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ArmorLevel); else SetProperty(PropertyInt.ArmorLevel, value.Value); }
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

    }
}
