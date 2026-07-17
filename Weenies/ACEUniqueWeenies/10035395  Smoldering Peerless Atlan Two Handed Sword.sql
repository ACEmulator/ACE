DELETE FROM `weenie` WHERE `class_Id` = 10035395;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10035395, 'ace10035395-FireSmoldering', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10035395,   1,          1) /* ItemType - MeleeWeapon */
     , (10035395,   3,         14) /* PaletteTemplate - Red */
     , (10035395,   5,        550) /* EncumbranceVal */
     , (10035395,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10035395,  16,          1) /* ItemUseable - No */
     , (10035395,  18,          1) /* UiEffects - Magical */
     , (10035395,  19,     250000) /* Value */
     , (10035395,  33,          1) /* Bonded - Bonded */
     , (10035395,  44,         64) /* Damage */
     , (10035395,  45,         16) /* DamageType - Fire */
     , (10035395,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10035395,  47,          6) /* AttackType - Thrust, Slash */
     , (10035395,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (10035395,  49,         35) /* WeaponTime */
     , (10035395,  51,          5) /* CombatUse - TwoHanded */
     , (10035395,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10035395, 106,        580) /* ItemSpellcraft */
     , (10035395, 107,      10000) /* ItemCurMana */
     , (10035395, 108,      10000) /* ItemMaxMana */
     , (10035395, 151,          2) /* HookType - Wall */
     , (10035395, 158,          2) /* WieldRequirements - RawSkill */
     , (10035395, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (10035395, 160,        100) /* WieldDifficulty */
     , (10035395, 166,          8) /* SlayerCreatureType - Tusker */
     , (10035395, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10035395, 263,         16) /* ResistanceModifierType - Fire */
     , (10035395, 292,          3) /* Cleaving */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10035395,  22, True ) /* Inscribable */
     , (10035395,  69, False) /* IsSellable */
     , (10035395,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10035395,   5,       0) /* ManaRate */
     , (10035395,  21,       1) /* WeaponLength */
     , (10035395,  22,     0.4) /* DamageVariance */
     , (10035395,  26,       0) /* MaximumVelocity */
     , (10035395,  29,     1.2) /* WeaponDefense */
     , (10035395,  39,     1.3) /* DefaultScale */
     , (10035395,  62,     1.2) /* WeaponOffense */
     , (10035395,  63,     1.3) /* DamageMod */
     , (10035395, 136,     1.8) /* CriticalMultiplier */
     , (10035395, 138,       2) /* SlayerDamageBonus */
     , (10035395, 147,    0.32) /* CriticalFrequency */
     , (10035395, 150,    1.05) /* WeaponMagicDefense */
     , (10035395, 155,       1) /* IgnoreArmor */
     , (10035395, 156,     0.5) /* ProcSpellRate */
     , (10035395, 157,    1.67) /* ResistanceModifier */
     , (10035395, 159,    0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10035395,   1, ' Smoldering Peerless Atlan Two Handed Sword') /* Name */
     , (10035395,  16, 'A Peerless Smoldering Atlan Weapon forgerd from the depths Crater Valley.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10035395,   1, 0x02000799) /* Setup */
     , (10035395,   3, 0x20000014) /* SoundTable */
     , (10035395,   6, 0x04000BEF) /* PaletteBase */
     , (10035395,   7, 0x100001F4) /* ClothingBase */
     , (10035395,   8, 0x060073CB) /* Icon */
     , (10035395,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10035395,  55,       6191) /* ProcSpell - Cassius' Ring of Fire II */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10035395,  2966,      2)  /* Aura of Murderous Thirst */
     , (10035395,  6089,      2)  /* Legendary Blood Thirst */
     , (10035395,  6094,      2)  /* Legendary Heart Thirst */
     , (10035395,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (10035395,  6091,      2)  /* Legendary Defender */
     , (10035395,  6073,      2)  /* Legendary Two Handed Combat Aptitude */
     , (10035395,  6240,      2)  /* Paragon's Two Handed Combat Mastery V */
     , (10035395,  5110,      2)  /* Master Soldier's Two Handed Combat Aptitude */
     , (10035395,  5098,      2)  /* Incantation of Two Handed Combat Mastery Other */
     , (10035395,  5732,      2)  /* Weave of Two Handed Combat V */
     , (10035395,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (10035395,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (10035395,  5834,      2)  /* Incantation of Recklessness Mastery Self */
     , (10035395,  5786,      2)  /* Incantation of Dirty Fighting Mastery Self */
     , (10035395,  6067,      2)  /* Legendary Recklessness Prowess */
     , (10035395,  6230,      2)  /* Paragon's Recklessness Mastery V */
     , (10035395,  5957,      2)  /* Master Soldier's Recklessness Aptitude */
     , (10035395,  6049,      2)  /* Legendary Dirty Fighting Prowess */
     , (10035395,  6255,      2)  /* Paragon's Dirty Fighting Mastery V */
     , (10035395,  5949,      2)  /* Master Soldier's Dirty Fighting Aptitude */;
