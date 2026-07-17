DELETE FROM `weenie` WHERE `class_Id` = 10034395;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10034395, 'ace10034395-bloodscorch', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10034395,   1,          1) /* ItemType - MeleeWeapon */
     , (10034395,   3,         39) /* PaletteTemplate - Black */
     , (10034395,   5,        550) /* EncumbranceVal */
     , (10034395,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10034395,  16,          1) /* ItemUseable - No */
     , (10034395,  18,          1) /* UiEffects - Magical */
     , (10034395,  19,         250000) /* Value */
     , (10034395,  33,          1) /* Bonded - Bonded */
     , (10034395,  44,         64) /* Damage */
     , (10034395,  45,         1) /* DamageType - slash */
     , (10034395,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10034395,  47,          6) /* AttackType - Thrust, Slash */
     , (10034395,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (10034395,  49,         35) /* WeaponTime */
     , (10034395,  51,          5) /* CombatUse - TwoHanded */
     , (10034395,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10034395, 106,        580) /* ItemSpellcraft */
     , (10034395, 107,      10000) /* ItemCurMana */
     , (10034395, 108,      10000) /* ItemMaxMana */
     , (10034395, 151,          2) /* HookType - Wall */
     , (10034395, 158,          2) /* WieldRequirements - RawSkill */
     , (10034395, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (10034395, 160,        100) /* WieldDifficulty */
     , (10034395, 166,         22) /* SlayerCreatureType - Shadow */
     , (10034395, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10034395, 263,         1) /* ResistanceModifierType Slashing*/
     , (10034395, 292,          3) /* Cleaving */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10034395,  22, True ) /* Inscribable */
     , (10034395,  69, False) /* IsSellable */
     , (10034395,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10034395,   5,       0) /* ManaRate */
     , (10034395,  21,       1) /* WeaponLength */
     , (10034395,  22,     0.4) /* DamageVariance */
     , (10034395,  26,       0) /* MaximumVelocity */
     , (10034395,  29,     1.2) /* WeaponDefense */
     , (10034395,  39,     1.35) /* DefaultScale */
     , (10034395,  62,     1.2) /* WeaponOffense */
     , (10034395,  63,       1.3) /* DamageMod */
     , (10034395, 136,     1.8) /* CriticalMultiplier */
     , (10034395, 138,       2) /* SlayerDamageBonus */
     , (10034395, 147,    0.32) /* CriticalFrequency */
     , (10034395, 150,    1.05) /* WeaponMagicDefense */
     , (10034395, 155,       1) /* IgnoreArmor */
     , (10034395, 156,     0.5) /* ProcSpellRate */
     , (10034395, 157,    1.67) /* ResistanceModifier */
     , (10034395, 159,     0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10034395,   1, 'Shadowfire') /* Name */
     , (10034395,  16, 'A Perfect Isparian Two Handed Sword, infused with the power of the Shadowfire Stone.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10034395,   1, 0x02001511) /* Setup */
     , (10034395,   3, 0x20000014) /* SoundTable */
     , (10034395,   6, 0x04000BEF) /* PaletteBase */
     , (10034395,   7, 0x100003A1) /* ClothingBase */
     , (10034395,   8, 0x060073D4) /* Icon */
     , (10034395,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10034395,  55,       6190) /* ProcSpell - Horizon's Blades 2 */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10034395,  2966,      2)  /* Murderous Thirst */
     , (10034395,  6089,      2)  /* Leg Blood thirst */
     , (10034395,  6094,      2)  /* Leg Heart thirst */
     , (10034395,  6006,      2)  /* Defender 8 */
     , (10034395,  6091,      2)  /* Leg Defender */
     , (10034395,  6073,      2)  /* Leg Two handed Mastery */
     , (10034395,  6240,      2)  /* Paragon Two handed Mastery */
     , (10034395,  5110,      2)  /* Master soliders two hand*/
     , (10034395,  5098,      2)  /* incantation two hand*/
     , (10034395,  5732,      2)  /* Weave two handed mastery */
     , (10034395,  6014,      2)  /* Heart Seekr 8 */
     , (10034395,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (10034395,  5834,      2)  /*Recklessness level 8 */
     , (10034395,  5786,      2)  /*Dirty Fighting level 8 */
     -- , (10034395,  6190,      2)  /* Horizon's Blades 2*/

, (10034395,  6067,      2)  /*Legendary Recklessness*/
, (10034395,  6230,      2)  /*Paragon  Recklessness*/
, (10034395,  5957,      2)  /*Master Soldier's  Recklessness*/

, (10034395,  6049,      2)  /*Legendary Dirty Fighting*/
, (10034395,  6255,      2)  /*Paragon  Dirty Fighting*/
, (10034395,  5949,      2)  /*Master Soldier's  Dirty Fighting*/;
