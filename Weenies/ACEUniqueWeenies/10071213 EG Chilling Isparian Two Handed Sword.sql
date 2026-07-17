DELETE FROM `weenie` WHERE `class_Id` = 10071213;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10071213, 'ace10071213-enhancedchillingispariantwohandedsword', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10071213,   1,          1) /* ItemType - MeleeWeapon */
     , (10071213,   3,          2) /* PaletteTemplate - Blue */
     , (10071213,   5,        650) /* EncumbranceVal */
     , (10071213,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10071213,  16,          1) /* ItemUseable - No */
     , (10071213,  18,          1) /* UiEffects - Magical */
     , (10071213,  19,       250000) /* Value */
     , (10071213,  33,          1) /* Bonded - Bonded */
     , (10071213,  44,         64) /* Damage */
     , (10071213,  45,          8) /* DamageType - Cold */
     , (10071213,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10071213,  47,          6) /* AttackType - Slash */
     , (10071213,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (10071213,  49,         50) /* WeaponTime */
     , (10071213,  51,          5) /* CombatUse - TwoHanded */
     , (10071213,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10071213, 106,        580) /* ItemSpellcraft */
     , (10071213, 107,        750) /* ItemCurMana */
     , (10071213, 108,        750) /* ItemMaxMana */
     , (10071213, 109,        1) /* ItemDifficulty */
     -- , (10071213, 114,          1) /* Attuned - Attuned */
     , (10071213, 151,          2) /* HookType - Wall */
     , (10071213, 158,          2) /* WieldRequirements - RawSkill */
     , (10071213, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (10071213, 160,        100) /* WieldDifficulty */
     , (10071213, 166,          48) /* SlayerCreatureType - Hollow Minnion Slayer */
     , (10071213, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10071213, 263,         8) /* ResistanceModifierType - Fire */
     , (10071213, 292,          3) /* Cleaving */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10071213,  22, True ) /* Inscribable */
     , (10071213,  69, False) /* IsSellable */
     , (10071213,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10071213,   5,       0) /* ManaRate */
     , (10071213,  21,       1) /* WeaponLength */
     , (10071213,  22,     0.4) /* DamageVariance */
     , (10071213,  26,       0) /* MaximumVelocity */
     , (10071213,  29,     1.2) /* WeaponDefense */
     , (10071213,  39,     1.3) /* DefaultScale */
     , (10071213,  62,     1.2) /* WeaponOffense */
     , (10071213,  63,     1.3) /* DamageMod */
     , (10071213, 136,     1.8) /* CriticalMultiplier */
     , (10071213, 138,       2) /* SlayerDamageBonus */
     , (10071213, 147,    0.32) /* CriticalFrequency */
     , (10071213, 150,    1.05) /* WeaponMagicDefense */
     , (10071213, 155,       1) /* IgnoreArmor */
     , (10071213, 156,     0.5) /* ProcSpellRate */
     , (10071213, 157,    1.67) /* ResistanceModifier */
     , (10071213, 159,    0.15) /* AbsorbMagicDamage */;


INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10071213,   1, 'Enhanced Chilling Isparian Two Handed Sword') /* Name */
     , (10071213,  16, 'This weapon seems tough to master.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10071213,   1, 0x020007A1) /* Setup */
     , (10071213,   3, 0x20000014) /* SoundTable */
     , (10071213,   6, 0x04000BEF) /* PaletteBase */
     , (10071213,   7, 0x100003A1) /* ClothingBase */
     , (10071213,   8, 0x060073CA) /* Icon */
     , (10071213,  22, 0x3400002B) /* PhysicsEffectTable */
          , (10071213,  55,       6193) /* ProcSpell - Halo of Frost II */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10071213,  2966,      2)  /* Aura of Murderous Thirst */
     , (10071213,  6089,      2)  /* Legendary Blood Thirst */
     , (10071213,  6094,      2)  /* Legendary Heart Thirst */
     , (10071213,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (10071213,  6091,      2)  /* Legendary Defender */
     , (10071213,  6073,      2)  /* Legendary Two Handed Combat Aptitude */
     , (10071213,  6240,      2)  /* Paragon's Two Handed Combat Mastery V */
     , (10071213,  5110,      2)  /* Master Soldier's Two Handed Combat Aptitude */
     , (10071213,  5098,      2)  /* Incantation of Two Handed Combat Mastery Other */
     , (10071213,  5732,      2)  /* Weave of Two Handed Combat V */
     , (10071213,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (10071213,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (10071213,  5834,      2)  /* Incantation of Recklessness Mastery Self */
     , (10071213,  5786,      2)  /* Incantation of Dirty Fighting Mastery Self */
     , (10071213,  6067,      2)  /* Legendary Recklessness Prowess */
     , (10071213,  6230,      2)  /* Paragon's Recklessness Mastery V */
     , (10071213,  5957,      2)  /* Master Soldier's Recklessness Aptitude */
     , (10071213,  6049,      2)  /* Legendary Dirty Fighting Prowess */
     , (10071213,  6255,      2)  /* Paragon's Dirty Fighting Mastery V */
     , (10071213,  5949,      2)  /* Master Soldier's Dirty Fighting Aptitude */;
