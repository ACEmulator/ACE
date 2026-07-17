DELETE FROM `weenie` WHERE `class_Id` = 10037221;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10037221, 'EG Virindi Void Staff', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10037221,   1,      32768) /* ItemType - Caster */
     , (10037221,   3,         39) /* PaletteTemplate - Black */
     , (10037221,   5,         50) /* EncumbranceVal */
     , (10037221,   8,         50) /* Mass */
     , (10037221,   9,   16777216) /* ValidLocations - Held */
     , (10037221,  16,          1) /* ItemUseable - No */
     , (10037221,  18,          1) /* UiEffects - Magical */
     , (10037221,  19,        200) /* Value */
     , (10037221,  45,       1024) /* DamageType - Nether */
     , (10037221,  46,        512) /* DefaultCombatStyle - Magic */
     , (10037221,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10037221,  94,         16) /* TargetType - Creature */
     , (10037221, 106,        975) /* ItemSpellcraft */
     , (10037221, 107,       3000) /* ItemCurMana */
     , (10037221, 108,       3000) /* ItemMaxMana */
     , (10037221, 150,        103) /* HookPlacement - Hook */
     , (10037221, 151,          2) /* HookType - Wall */
     , (10037221, 166,         19) /* SlayerCreatureType - Virindi */
     , (10037221, 169,   84084483) /* TsysMutationData */
     , (10037221, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10037221, 240,         10) /* AugmentationResistanceSlash */
     , (10037221, 263,       1024) /* ResistanceModifierType - Nether */
     , (10037221, 353,         12) /* WeaponType - Magic */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10037221,  11, True ) /* IgnoreCollisions */
     , (10037221,  13, True ) /* Ethereal */
     , (10037221,  14, True ) /* GravityStatus */
     , (10037221,  19, True ) /* Attackable */
     , (10037221,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10037221,   5,       0) /* ManaRate */
     , (10037221,  29,     1.2) /* WeaponDefense */
     , (10037221,  39,     0.8) /* DefaultScale */
     , (10037221, 136,     1.8) /* CriticalMultiplier */
     , (10037221, 138,     1.6) /* SlayerDamageBonus */
     , (10037221, 144,     0.1) /* ManaConversionMod */
     , (10037221, 147,    0.25) /* CriticalFrequency */
     , (10037221, 150,    1.05) /* WeaponMagicDefense */
     , (10037221, 152,    1.18) /* ElementalDamageMod */
     , (10037221, 157,       1) /* ResistanceModifier */
     , (10037221, 159,     0.1) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10037221,   1, 'Nether Orb of Hatred') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10037221,   1, 0x02000EE9) /* Setup */
     , (10037221,   3, 0x20000014) /* SoundTable */
     , (10037221,   6, 0x04000BEF) /* PaletteBase */
     , (10037221,   7, 0x100003DA) /* ClothingBase */
     , (10037221,   8, 0x060035F3) /* Icon */
     , (10037221,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10037221,  36, 0x0E000016) /* MutateFilter */
     , (10037221,  46, 0x38000030) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10037221,  6022,      2)  /* Aura of Incantation of Spirit Drinker Other */
     , (10037221,  6098,      2)  /* Legendary Spirit Thirst */
     , (10037221,  6035,      2)  /* Spirit of Izexi */
     , (10037221,  3985,      2)  /* Mukkir Sense */
     , (10037221,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (10037221,  6091,      2)  /* Legendary Defender */
     , (10037221,  5988,      2)  /* Aura of Hermetic Link Other VII */
     , (10037221,  6087,      2)  /* Legendary Hermetic Link */;
