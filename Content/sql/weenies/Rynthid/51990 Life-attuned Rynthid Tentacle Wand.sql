DELETE FROM `weenie` WHERE `class_Id` = 51990;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51990, 'ace51990-lifeattunedrynthidtentaclewand', 35, '2020-08-15 22:25:39') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51990,   1,      32768) /* ItemType - Caster */
     , (51990,   5,        150) /* EncumbranceVal */
     , (51990,   9,   16777216) /* ValidLocations - Held */
     , (51990,  16,          1) /* ItemUseable - No */
     , (51990,  18,          1) /* UiEffects - Magical */
     , (51990,  19,      10000) /* Value */
     , (51990,  33,          1) /* Bonded - Bonded */
     , (51990,  45,         16) /* DamageType - Fire */
     , (51990,  46,        512) /* DefaultCombatStyle - Magic */
     , (51990,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51990,  94,         16) /* TargetType - Creature */
     , (51990, 106,        475) /* ItemSpellcraft */
     , (51990, 107,       3000) /* ItemCurMana */
     , (51990, 108,       3000) /* ItemMaxMana */
     , (51990, 114,          1) /* Attuned - Attuned */
     , (51990, 151,          2) /* HookType - Wall */
     , (51990, 158,          2) /* WieldRequirements - RawSkill */
     , (51990, 159,         33) /* WieldSkillType - LifeMagic */
     , (51990, 160,        375) /* WieldDifficulty */
     , (51990, 166,         19) /* SlayerCreatureType - Virindi */
     , (51990, 353,         12) /* WeaponType - Magic */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51990,  11, True ) /* IgnoreCollisions */
     , (51990,  13, True ) /* Ethereal */
     , (51990,  14, True ) /* GravityStatus */
     , (51990,  19, True ) /* Attackable */
     , (51990,  22, True ) /* Inscribable */
     , (51990,  69, False) /* IsSellable */
     , (51990,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51990,   5,  -0.033) /* ManaRate */
     , (51990,  29,    1.22) /* WeaponDefense */
     , (51990, 138,       2) /* SlayerDamageBonus */
     , (51990, 144,    0.25) /* ManaConversionMod */
     , (51990, 147,     0.3) /* CriticalFrequency */
     , (51990, 152,    1.14) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51990,   1, 'Life-attuned Rynthid Tentacle Wand') /* Name */
     , (51990,  16, 'A wand crafted from enchanted obsidian and Rynthid tentacles.') /* LongDesc */
     , (51990,  33, 'TentacleWeaponPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51990,   1, 0x02001C03) /* Setup */
     , (51990,   3, 0x20000014) /* SoundTable */
     , (51990,   6, 0x04000BEF) /* PaletteBase */
     , (51990,   8, 0x060074F2) /* Icon */
     , (51990,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51990,  3964,      2)  /* Epic Focus */
     , (51990,  4227,      2)  /* Epic Willpower */
     , (51990,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (51990,  4414,      2)  /* Aura of Incantation of Spirit Drinker Self */
     , (51990,  6060,      2)  /* Legendary Life Magic Aptitude */;
