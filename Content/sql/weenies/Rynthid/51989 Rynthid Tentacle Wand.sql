DELETE FROM `weenie` WHERE `class_Id` = 51989;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51989, 'ace51989-rynthidtentaclewand', 35, '2020-08-15 22:25:34') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51989,   1,      32768) /* ItemType - Caster */
     , (51989,   5,        150) /* EncumbranceVal */
     , (51989,   9,   16777216) /* ValidLocations - Held */
     , (51989,  16,          1) /* ItemUseable - No */
     , (51989,  18,          1) /* UiEffects - Magical */
     , (51989,  19,      10000) /* Value */
     , (51989,  33,          1) /* Bonded - Bonded */
     , (51989,  45,         16) /* DamageType - Fire */
     , (51989,  46,        512) /* DefaultCombatStyle - Magic */
     , (51989,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51989,  94,         16) /* TargetType - Creature */
     , (51989, 106,        475) /* ItemSpellcraft */
     , (51989, 107,       3000) /* ItemCurMana */
     , (51989, 108,       3000) /* ItemMaxMana */
     , (51989, 114,          1) /* Attuned - Attuned */
     , (51989, 151,          2) /* HookType - Wall */
     , (51989, 158,          2) /* WieldRequirements - RawSkill */
     , (51989, 159,         34) /* WieldSkillType - WarMagic */
     , (51989, 160,        375) /* WieldDifficulty */
     , (51989, 166,         19) /* SlayerCreatureType - Virindi */
     , (51989, 353,         12) /* WeaponType - Magic */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51989,  11, True ) /* IgnoreCollisions */
     , (51989,  13, True ) /* Ethereal */
     , (51989,  14, True ) /* GravityStatus */
     , (51989,  19, True ) /* Attackable */
     , (51989,  22, True ) /* Inscribable */
     , (51989,  69, False) /* IsSellable */
     , (51989,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51989,   5,  -0.033) /* ManaRate */
     , (51989,  29,     1.2) /* WeaponDefense */
     , (51989, 138,       2) /* SlayerDamageBonus */
     , (51989, 144,     0.2) /* ManaConversionMod */
     , (51989, 147,     0.3) /* CriticalFrequency */
     , (51989, 152,    1.16) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51989,   1, 'Rynthid Tentacle Wand') /* Name */
     , (51989,  16, 'A wand crafted from enchanted obsidian and Rynthid tentacles.') /* LongDesc */
     , (51989,  33, 'TentacleWeaponPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51989,   1, 0x02001C03) /* Setup */
     , (51989,   3, 0x20000014) /* SoundTable */
     , (51989,   6, 0x04000BEF) /* PaletteBase */
     , (51989,   8, 0x060074F2) /* Icon */
     , (51989,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51989,  3964,      2)  /* Epic Focus */
     , (51989,  4227,      2)  /* Epic Willpower */
     , (51989,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (51989,  4414,      2)  /* Aura of Incantation of Spirit Drinker Self */
     , (51989,  6075,      2)  /* Legendary War Magic Aptitude */;
