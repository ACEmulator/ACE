DELETE FROM `weenie` WHERE `class_Id` = 10046121;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10046121, 'ace10046121-enhancedstingingatlantwohandedsword', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10046121,   1,          1) /* ItemType - MeleeWeapon */
     , (10046121,   3,          8) /* PaletteTemplate - Green */
     , (10046121,   5,        700) /* EncumbranceVal */
     , (10046121,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10046121,  16,          1) /* ItemUseable - No */
     , (10046121,  18,          1) /* UiEffects - Magical */
     , (10046121,  19,       5000) /* Value */
     , (10046121,  33,          1) /* Bonded - Bonded */
     , (10046121,  44,         64) /* Damage */
     , (10046121,  45,         32) /* DamageType - Acid */
     , (10046121,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10046121,  47,          4) /* AttackType - Slash */
     , (10046121,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (10046121,  49,         50) /* WeaponTime */
     , (10046121,  51,          5) /* CombatUse - TwoHanded */
     , (10046121,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10046121, 106,        580) /* ItemSpellcraft */
     , (10046121, 107,        400) /* ItemCurMana */
     , (10046121, 108,        400) /* ItemMaxMana */
     , (10046121, 109,        1) /* ItemDifficulty */
     -- , (10046121, 114,          1) /* Attuned - Attuned */
     , (10046121, 151,          2) /* HookType - Wall */
     , (10046121, 158,          2) /* WieldRequirements - RawSkill */
     , (10046121, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (10046121, 160,        1) /* WieldDifficulty */
     , (10046121, 166,          101) /* SlayerCreatureType - A'nekshay */
     , (10046121, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10046121, 263,         32) /* ResistanceModifierType - Acid */
     , (10046121, 292,          3) /* Cleaving */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10046121,  22, True ) /* Inscribable */
     , (10046121,  69, False) /* IsSellable */
     , (10046121,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10046121,   5,       0) /* ManaRate */
     , (10046121,  21,       1) /* WeaponLength */
     , (10046121,  22,     0.4) /* DamageVariance */
     , (10046121,  26,       0) /* MaximumVelocity */
     , (10046121,  29,     1.2) /* WeaponDefense */
     , (10046121,  39,     1.3) /* DefaultScale */
     , (10046121,  62,     1.2) /* WeaponOffense */
     , (10046121,  63,     1.3) /* DamageMod */
     , (10046121, 136,     1.8) /* CriticalMultiplier */
     , (10046121, 138,       2) /* SlayerDamageBonus */
     , (10046121, 147,    0.32) /* CriticalFrequency */
     , (10046121, 150,    1.05) /* WeaponMagicDefense */
     , (10046121, 155,       1) /* IgnoreArmor */
     , (10046121, 156,     0.5) /* ProcSpellRate */
     , (10046121, 157,    1.67) /* ResistanceModifier */
     , (10046121, 159,    0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10046121,   1, 'Enhanced Stinging Atlan Two Handed Sword') /* Name */
     , (10046121,  16, 'This weapon seems tough to master.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10046121,   1, 0x02000797) /* Setup */
     , (10046121,   3, 0x20000014) /* SoundTable */
     , (10046121,   6, 0x04000BEF) /* PaletteBase */
     , (10046121,   7, 0x100001F2) /* ClothingBase */
     , (10046121,   8, 0x060073CD) /* Icon */
     , (10046121,  22, 0x3400002B) /* PhysicsEffectTable */
          , (10046121,  55,       6189) /* ProcSpell - Searing Disc II */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10046121,  2966,      2)  /* Aura of Murderous Thirst */
     , (10046121,  6089,      2)  /* Legendary Blood Thirst */
     , (10046121,  6094,      2)  /* Legendary Heart Thirst */
     , (10046121,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (10046121,  6091,      2)  /* Legendary Defender */
     , (10046121,  6073,      2)  /* Legendary Two Handed Combat Aptitude */
     , (10046121,  6240,      2)  /* Paragon's Two Handed Combat Mastery V */
     , (10046121,  5110,      2)  /* Master Soldier's Two Handed Combat Aptitude */
     , (10046121,  5098,      2)  /* Incantation of Two Handed Combat Mastery Other */
     , (10046121,  5732,      2)  /* Weave of Two Handed Combat V */
     , (10046121,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (10046121,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (10046121,  5834,      2)  /* Incantation of Recklessness Mastery Self */
     , (10046121,  5786,      2)  /* Incantation of Dirty Fighting Mastery Self */
     , (10046121,  6067,      2)  /* Legendary Recklessness Prowess */
     , (10046121,  6230,      2)  /* Paragon's Recklessness Mastery V */
     , (10046121,  5957,      2)  /* Master Soldier's Recklessness Aptitude */
     , (10046121,  6049,      2)  /* Legendary Dirty Fighting Prowess */
     , (10046121,  6255,      2)  /* Paragon's Dirty Fighting Mastery V */
     , (10046121,  5949,      2)  /* Master Soldier's Dirty Fighting Aptitude */;
