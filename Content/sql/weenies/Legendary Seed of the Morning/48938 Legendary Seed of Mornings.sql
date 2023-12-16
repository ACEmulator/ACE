DELETE FROM `weenie` WHERE `class_Id` = 48938;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (48938, 'ace48938-legendaryseedofmornings', 35, '2022-05-17 03:47:03') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (48938,   1,      32768) /* ItemType - Caster */
     , (48938,   5,         50) /* EncumbranceVal */
     , (48938,   9,   16777216) /* ValidLocations - Held */
     , (48938,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (48938,  18,          1) /* UiEffects - Magical */
     , (48938,  19,      20000) /* Value */
     , (48938,  33,          1) /* Bonded - Bonded */
     , (48938,  46,        512) /* DefaultCombatStyle - Magic */
     , (48938,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (48938,  94,         16) /* TargetType - Creature */
     , (48938, 106,        450) /* ItemSpellcraft */
     , (48938, 107,       5000) /* ItemCurMana */
     , (48938, 108,       5000) /* ItemMaxMana */
     , (48938, 109,        300) /* ItemDifficulty */
     , (48938, 114,          1) /* Attuned - Attuned */
     , (48938, 151,          3) /* HookType - Floor, Wall */
     , (48938, 158,          2) /* WieldRequirements - RawSkill */
     , (48938, 159,         33) /* WieldSkillType - LifeMagic */
     , (48938, 160,        340) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (48938,  22, True ) /* Inscribable */
     , (48938,  23, True ) /* DestroyOnSell */
     , (48938,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (48938,   5,  -0.025) /* ManaRate */
     , (48938,  29,     1.2) /* WeaponDefense */
     , (48938,  39,     0.6) /* DefaultScale */
     , (48938, 144,     0.2) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (48938,   1, 'Legendary Seed of Mornings') /* Name */
     , (48938,  16, 'A large, glowing seed, empowered by the magics of the Light Falatacot.  This seed was retrieved from the Temple of Mornings, underneath the desert sands.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (48938,   1, 0x02001BA5) /* Setup */
     , (48938,   3, 0x20000014) /* SoundTable */
     , (48938,   6, 0x04000BEF) /* PaletteBase */
     , (48938,   8, 0x060073EA) /* Icon */
     , (48938,  22, 0x3400002B) /* PhysicsEffectTable */
	 , (48938,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (48938,  28,       2072) /* Spell - Adja's Gift */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (48938,  6086,      2)  /* Epic Hermetic Link */
     , (48938,  4582,      2)  /* Incantation of Life Magic Mastery Self */
     , (48938,  6060,      2)  /* Legendary Life Magic Aptitude */
     , (48938,  4602,      2)  /* Incantation of Mana Conversion Mastery Self */;
