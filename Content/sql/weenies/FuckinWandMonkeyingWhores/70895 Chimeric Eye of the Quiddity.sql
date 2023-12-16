DELETE FROM `weenie` WHERE `class_Id` = 70895;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (70895, 'ace70895-chimericeyeofthequiddity', 35, '2022-06-06 04:05:48') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (70895,   1,      32768) /* ItemType - Caster */
     , (70895,   3,         82) /* PaletteTemplate - PinkPurple */
     , (70895,   5,         50) /* EncumbranceVal */
     , (70895,   9,   16777216) /* ValidLocations - Held */
     , (70895,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (70895,  18,          1) /* UiEffects - Magical */
     , (70895,  19,          0) /* Value */
     , (70895,  33,         -1) /* Bonded - Slippery */
     , (70895,  45,          1) /* DamageType - Slash */
     , (70895,  46,        512) /* DefaultCombatStyle - Magic */
     , (70895,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (70895,  94,         16) /* TargetType - Creature */
     , (70895, 106,        475) /* ItemSpellcraft */
     , (70895, 107,       2700) /* ItemCurMana */
     , (70895, 108,       2700) /* ItemMaxMana */
     , (70895, 158,          7) /* WieldRequirements - Level */
     , (70895, 159,          1) /* WieldSkillType - Axe */
     , (70895, 160,        120) /* WieldDifficulty */
     , (70895, 263,          1) /* ResistanceModifierType - Slash */
     , (70895, 267,      10800) /* Lifespan */
     , (70895, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (70895,  22, True ) /* Inscribable */
     , (70895,  69, False) /* IsSellable */
     , (70895,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (70895,   5,   -0.05) /* ManaRate */
     , (70895,  29,     1.2) /* WeaponDefense */
     , (70895,  39,     0.8) /* DefaultScale */
     , (70895, 136,     1.7) /* CriticalMultiplier */
     , (70895, 144,    0.15) /* ManaConversionMod */
     , (70895, 147,     0.3) /* CriticalFrequency */
     , (70895, 152,     1.2) /* ElementalDamageMod */
     , (70895, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (70895,   1, 'Chimeric Eye of the Quiddity') /* Name */
     , (70895,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (70895,   1, 0x02001184) /* Setup */
     , (70895,   3, 0x20000014) /* SoundTable */
     , (70895,   6, 0x04000BEF) /* PaletteBase */
     , (70895,   7, 0x100002E7) /* ClothingBase */
     , (70895,   8, 0x060035C7) /* Icon */
     , (70895,  22, 0x3400002B) /* PhysicsEffectTable */
     , (70895,  52, 0x060065FB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (70895,  3259,      2)  /* Aura of Infected Spirit Caress */
     , (70895,  2101,      2)  /* Aura of Cragstone's Will */
     , (70895,  2117,      2)  /* Aura of Mystic's Blessing */
     , (70895,  2534,      2)  /* Major War Magic Aptitude */
     , (70895,  2581,      2)  /* Minor Focus */
     , (70895,  2584,      2)  /* Minor Willpower */;
