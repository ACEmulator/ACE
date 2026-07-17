DELETE FROM `weenie` WHERE `class_Id` = 10037222;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10037222, 'Ankeshen  Staff of Hatred', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10037222,   1,      32768) /* ItemType - Caster */
     , (10037222,   3,         14) /* PaletteTemplate - Red */
     , (10037222,   5,         50) /* EncumbranceVal */
     , (10037222,   8,         50) /* Mass */
     , (10037222,   9,   16777216) /* ValidLocations - Held */
     , (10037222,  16,          1) /* ItemUseable - No */
     , (10037222,  18,          1) /* UiEffects - Magical */
     , (10037222,  19,        200) /* Value */
     , (10037222,  45,         32) /* DamageType - Acid */
     , (10037222,  46,        512) /* DefaultCombatStyle - Magic */
     , (10037222,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10037222,  94,         16) /* TargetType - Creature */
     , (10037222, 105,         10) /* ItemWorkmanship */
     , (10037222, 106,        975) /* ItemSpellcraft */
     , (10037222, 107,       3000) /* ItemCurMana */
     , (10037222, 108,       3000) /* ItemMaxMana */
     , (10037222, 109,        210) /* ItemDifficulty */
     , (10037222, 131,         17) /* MaterialType - Bloodstone */
     , (10037222, 150,        103) /* HookPlacement - Hook */
     , (10037222, 151,          2) /* HookType - Wall */
     , (10037222, 166,        101) /* SlayerCreatureType - Anekshay */
     , (10037222, 169,   84084483) /* TsysMutationData */
     , (10037222, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10037222, 263,          9) /* ResistanceModifierType */
     , (10037222, 353,         12) /* WeaponType - Magic */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10037222,  11, True ) /* IgnoreCollisions */
     , (10037222,  13, True ) /* Ethereal */
     , (10037222,  14, True ) /* GravityStatus */
     , (10037222,  19, True ) /* Attackable */
     , (10037222,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10037222,   5,       0) /* ManaRate */
     , (10037222,  29,     1.2) /* WeaponDefense */
     , (10037222,  39,     0.8) /* DefaultScale */
     , (10037222, 136,     1.8) /* CriticalMultiplier */
     , (10037222, 138,     1.6) /* SlayerDamageBonus */
     , (10037222, 144,     0.1) /* ManaConversionMod */
     , (10037222, 147,    0.25) /* CriticalFrequency */
     , (10037222, 150,    1.05) /* WeaponMagicDefense */
     , (10037222, 152,    1.18) /* ElementalDamageMod */
     , (10037222, 157,       1) /* ResistanceModifier */
     , (10037222, 159,     0.1) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10037222,   1, 'Ankeshen Staff of Hatred') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10037222,   1, 0x0200184D) /* Setup */
     , (10037222,   3, 0x20000014) /* SoundTable */
     , (10037222,   6, 0x04000BEF) /* PaletteBase */
     , (10037222,   7, 0x100003DA) /* ClothingBase */
     , (10037222,   8, 0x06006855) /* Icon */
     , (10037222,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10037222,  36, 0x0E000016) /* MutateFilter */
     , (10037222,  46, 0x38000030) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10037222,  6022,      2)  /* Aura of Incantation of Spirit Drinker Other */
     , (10037222,  6098,      2)  /* Legendary Spirit Thirst */
     , (10037222,  6035,      2)  /* Spirit of Izexi */
     , (10037222,  3985,      2)  /* Mukkir Sense */
     , (10037222,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (10037222,  6091,      2)  /* Legendary Defender */
     , (10037222,  6087,      2)  /* Legendary Hermetic Link */
     , (10037222,  5989,      2)  /* Aura of Incantation of Hermetic Link Other */;
