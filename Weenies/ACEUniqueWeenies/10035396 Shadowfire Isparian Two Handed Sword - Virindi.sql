DELETE FROM `weenie` WHERE `class_Id` = 10035396;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10035396, 'ace10035396-bloodscorch', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10035396,   1,          1) /* ItemType - MeleeWeapon */
     , (10035396,   3,         39) /* PaletteTemplate - Black */
     , (10035396,   5,        550) /* EncumbranceVal */
     , (10035396,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10035396,  16,          1) /* ItemUseable - No */
     , (10035396,  18,          1) /* UiEffects - Magical */
     , (10035396,  19,         250000) /* Value */
     , (10035396,  33,          1) /* Bonded - Bonded */
     , (10035396,  44,         64) /* Damage */
     , (10035396,  45,         1) /* DamageType - slash */
     , (10035396,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10035396,  47,          6) /* AttackType - Thrust, Slash */
     , (10035396,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (10035396,  49,         35) /* WeaponTime */
     , (10035396,  51,          5) /* CombatUse - TwoHanded */
     , (10035396,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10035396, 106,        580) /* ItemSpellcraft */
     , (10035396, 107,      10000) /* ItemCurMana */
     , (10035396, 108,      10000) /* ItemMaxMana */
     , (10035396, 151,          2) /* HookType - Wall */
     , (10035396, 158,          2) /* WieldRequirements - RawSkill */
     , (10035396, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (10035396, 160,        100) /* WieldDifficulty */
     , (10035396, 166,         19) /* SlayerCreatureType - Shadow */
     , (10035396, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10035396, 263,         1) /* ResistanceModifierType Slashing*/
     , (10035396, 292,          3) /* Cleaving */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10035396,  22, True ) /* Inscribable */
     , (10035396,  69, False) /* IsSellable */
     , (10035396,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10035396,   5,       0) /* ManaRate */
     , (10035396,  21,       1) /* WeaponLength */
     , (10035396,  22,     0.4) /* DamageVariance */
     , (10035396,  26,       0) /* MaximumVelocity */
     , (10035396,  29,     1.2) /* WeaponDefense */
     , (10035396,  39,     1.35) /* DefaultScale */
     , (10035396,  62,     1.2) /* WeaponOffense */
     , (10035396,  63,       1.3) /* DamageMod */
     , (10035396, 136,     1.8) /* CriticalMultiplier */
     , (10035396, 138,       2) /* SlayerDamageBonus */
     , (10035396, 147,    0.32) /* CriticalFrequency */
     , (10035396, 150,    1.05) /* WeaponMagicDefense */
     , (10035396, 155,       1) /* IgnoreArmor */
     , (10035396, 156,     0.5) /* ProcSpellRate */
     , (10035396, 157,    1.67) /* ResistanceModifier */
     , (10035396, 159,     0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10035396,   1, 'Shadowfire') /* Name */
     , (10035396,  16, 'A Perfect Isparian Two Handed Sword, infused with the power of the Shadowfire Stone.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10035396,   1, 0x02001511) /* Setup */
     , (10035396,   3, 0x20000014) /* SoundTable */
     , (10035396,   6, 0x04000BEF) /* PaletteBase */
     , (10035396,   7, 0x100003A1) /* ClothingBase */
     , (10035396,   8, 0x060073D4) /* Icon */
     , (10035396,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10035396,  55,       6190) /* ProcSpell - Horizon's Blades 2 */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10035396,  2966,      2)  /* Murderous Thirst */
     , (10035396,  6089,      2)  /* Leg Blood thirst */
     , (10035396,  6094,      2)  /* Leg Heart thirst */
     , (10035396,  6006,      2)  /* Defender 8 */
     , (10035396,  6091,      2)  /* Leg Defender */
     , (10035396,  6073,      2)  /* Leg Two handed Mastery */
     , (10035396,  6240,      2)  /* Paragon Two handed Mastery */
     , (10035396,  5110,      2)  /* Master soliders two hand*/
     , (10035396,  5098,      2)  /* incantation two hand*/
     , (10035396,  5732,      2)  /* Weave two handed mastery */
     , (10035396,  6014,      2)  /* Heart Seekr 8 */
     , (10035396,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (10035396,  5834,      2)  /*Recklessness level 8 */
     , (10035396,  5786,      2)  /*Dirty Fighting level 8 */
     -- , (10035396,  6190,      2)  /* Horizon's Blades 2*/

, (10035396,  6067,      2)  /*Legendary Recklessness*/
, (10035396,  6230,      2)  /*Paragon  Recklessness*/
, (10035396,  5957,      2)  /*Master Soldier's  Recklessness*/

, (10035396,  6049,      2)  /*Legendary Dirty Fighting*/
, (10035396,  6255,      2)  /*Paragon  Dirty Fighting*/
, (10035396,  5949,      2)  /*Master Soldier's  Dirty Fighting*/;
