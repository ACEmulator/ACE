DELETE FROM `weenie` WHERE `class_Id` = 10046120;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10046120, 'ace10046120-enhancedsparkingatlantwohandedsword', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10046120,   1,          1) /* ItemType - MeleeWeapon */
     , (10046120,   3,         82) /* PaletteTemplate - PinkPurple */
     , (10046120,   5,        700) /* EncumbranceVal */
     , (10046120,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10046120,  16,          1) /* ItemUseable - No */
     , (10046120,  18,          1) /* UiEffects - Magical */
     , (10046120,  19,       5000) /* Value */
     , (10046120,  33,          1) /* Bonded - Bonded */
     , (10046120,  44,         64) /* Damage */
     , (10046120,  45,         64) /* DamageType - Electric */
     , (10046120,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10046120,  47,          4) /* AttackType - Slash */
     , (10046120,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (10046120,  49,         50) /* WeaponTime */
     , (10046120,  51,          5) /* CombatUse - TwoHanded */
     , (10046120,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10046120, 106,        580) /* ItemSpellcraft */
     , (10046120, 107,        400) /* ItemCurMana */
     , (10046120, 108,        400) /* ItemMaxMana */
     , (10046120, 109,          1) /* ItemDifficulty */
     , (10046120, 114,          1) /* Attuned - Attuned */
     , (10046120, 151,          2) /* HookType - Wall */
     , (10046120, 158,          2) /* WieldRequirements - RawSkill */
     , (10046120, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (10046120, 160,         10) /* WieldDifficulty */
     , (10046120, 166,          5) /* SlayerCreatureType - Lugian */
     , (10046120, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10046120, 263,         64) /* ResistanceModifierType - Electric */
     , (10046120, 292,          3) /* Cleaving */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10046120,  22, True ) /* Inscribable */
     , (10046120,  69, False) /* IsSellable */
     , (10046120,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10046120,   5,       0) /* ManaRate */
     , (10046120,  21,       1) /* WeaponLength */
     , (10046120,  22,     0.4) /* DamageVariance */
     , (10046120,  26,       0) /* MaximumVelocity */
     , (10046120,  29,     1.2) /* WeaponDefense */
     , (10046120,  39,    1.35) /* DefaultScale */
     , (10046120,  62,     1.2) /* WeaponOffense */
     , (10046120,  63,     1.3) /* DamageMod */
     , (10046120, 136,     1.8) /* CriticalMultiplier */
     , (10046120, 138,       2) /* SlayerDamageBonus */
     , (10046120, 147,    0.32) /* CriticalFrequency */
     , (10046120, 150,    1.05) /* WeaponMagicDefense */
     , (10046120, 155,       1) /* IgnoreArmor */
     , (10046120, 156,     0.5) /* ProcSpellRate */
     , (10046120, 157,    1.67) /* ResistanceModifier */
     , (10046120, 159,    0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10046120,   1, 'Enhanced Sparking Atlan Two Handed Sword') /* Name */
     , (10046120,  16, 'This weapon seems tough to master.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10046120,   1, 0x02000798) /* Setup */
     , (10046120,   3, 0x20000014) /* SoundTable */
     , (10046120,   6, 0x04000BEF) /* PaletteBase */
     , (10046120,   7, 0x100001F3) /* ClothingBase */
     , (10046120,   8, 0x060073CC) /* Icon */
     , (10046120,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10046120,  55,       6197) /* ProcSpell - Eye of the Storm II */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10046120,  2966,      2)  /* Aura of Murderous Thirst */
     , (10046120,  6089,      2)  /* Legendary Blood Thirst */
     , (10046120,  6094,      2)  /* Legendary Heart Thirst */
     , (10046120,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (10046120,  6091,      2)  /* Legendary Defender */
     , (10046120,  6073,      2)  /* Legendary Two Handed Combat Aptitude */
     , (10046120,  6240,      2)  /* Paragon's Two Handed Combat Mastery V */
     , (10046120,  5110,      2)  /* Master Soldier's Two Handed Combat Aptitude */
     , (10046120,  5098,      2)  /* Incantation of Two Handed Combat Mastery Other */
     , (10046120,  5732,      2)  /* Weave of Two Handed Combat V */
     , (10046120,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (10046120,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (10046120,  5834,      2)  /* Incantation of Recklessness Mastery Self */
     , (10046120,  5786,      2)  /* Incantation of Dirty Fighting Mastery Self */
     , (10046120,  6067,      2)  /* Legendary Recklessness Prowess */
     , (10046120,  6230,      2)  /* Paragon's Recklessness Mastery V */
     , (10046120,  5957,      2)  /* Master Soldier's Recklessness Aptitude */
     , (10046120,  6049,      2)  /* Legendary Dirty Fighting Prowess */
     , (10046120,  6255,      2)  /* Paragon's Dirty Fighting Mastery V */
     , (10046120,  5949,      2)  /* Master Soldier's Dirty Fighting Aptitude */;
