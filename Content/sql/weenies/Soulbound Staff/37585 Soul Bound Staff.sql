DELETE FROM `weenie` WHERE `class_Id` = 37585;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (37585, 'ace37585-soulboundstaff', 35, '2022-05-17 03:47:03') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (37585,   1,      32768) /* ItemType - Caster */
     , (37585,   3,          2) /* PaletteTemplate - Blue */
     , (37585,   5,         50) /* EncumbranceVal */
     , (37585,   9,   16777216) /* ValidLocations - Held */
     , (37585,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (37585,  18,          1) /* UiEffects - Magical */
     , (37585,  19,          0) /* Value */
     , (37585,  33,          1) /* Bonded - Bonded */
     , (37585,  45,          2) /* DamageType - Pierce */
     , (37585,  46,        512) /* DefaultCombatStyle - Magic */
     , (37585,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (37585,  94,         16) /* TargetType - Creature */
     , (37585, 106,        475) /* ItemSpellcraft */
     , (37585, 107,       2700) /* ItemCurMana */
     , (37585, 108,       2700) /* ItemMaxMana */
     , (37585, 114,          1) /* Attuned - Attuned */
     , (37585, 117,        250) /* ItemManaCost */
     , (37585, 151,          2) /* HookType - Wall */
     , (37585, 158,          7) /* WieldRequirements - Level */
     , (37585, 159,          1) /* WieldSkillType - Axe */
     , (37585, 160,        160) /* WieldDifficulty */
     , (37585, 166,         77) /* SlayerCreatureType - Ghost */
     , (37585, 263,          2) /* ResistanceModifierType - Pierce */
     , (37585, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (37585,  22, True ) /* Inscribable */
     , (37585,  69, False) /* IsSellable */
     , (37585,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (37585,   5,   -0.05) /* ManaRate */
     , (37585,  29,     1.2) /* WeaponDefense */
     , (37585,  39,     0.7) /* DefaultScale */
     , (37585,  76,     0.7) /* Translucency */
     , (37585, 136,       2) /* CriticalMultiplier */
     , (37585, 138,     1.2) /* SlayerDamageBonus */
     , (37585, 144,    0.15) /* ManaConversionMod */
     , (37585, 147,     0.3) /* CriticalFrequency */
     , (37585, 152,     1.2) /* ElementalDamageMod */
     , (37585, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (37585,   1, 'Soul Bound Staff') /* Name */
     , (37585,  15, 'A ghostly blue casting staff, bound to your soul.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (37585,   1, 0x020017FF) /* Setup */
     , (37585,   3, 0x20000014) /* SoundTable */
     , (37585,   6, 0x04000BEF) /* PaletteBase */
     , (37585,   7, 0x100003C9) /* ClothingBase */
     , (37585,   8, 0x06003037) /* Icon */
     , (37585,  22, 0x3400002B) /* PhysicsEffectTable */
     , (37585,  52, 0x060067E8) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (37585,  2101,      2)  /* Aura of Cragstone's Will */
     , (37585,  2117,      2)  /* Aura of Mystic's Blessing */
     , (37585,  2534,      2)  /* Major War Magic Aptitude */
     , (37585,  2581,      2)  /* Minor Focus */
     , (37585,  2584,      2)  /* Minor Willpower */
     , (37585,  3259,      2)  /* Aura of Infected Spirit Caress */;
