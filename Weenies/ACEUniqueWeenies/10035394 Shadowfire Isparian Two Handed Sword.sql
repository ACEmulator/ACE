DELETE FROM `weenie` WHERE `class_Id` = 10035394;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10035394, 'ace10035394-bloodscorch', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10035394,   1,          1) /* ItemType - MeleeWeapon */
     , (10035394,   3,         39) /* PaletteTemplate - Black */
     , (10035394,   5,        550) /* EncumbranceVal */
     , (10035394,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10035394,  16,          1) /* ItemUseable - No */
     , (10035394,  18,          1) /* UiEffects - Magical */
     , (10035394,  19,         250000) /* Value */
     , (10035394,  33,          1) /* Bonded - Bonded */
     , (10035394,  44,         64) /* Damage */
     , (10035394,  45,         1) /* DamageType - slash */
     , (10035394,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10035394,  47,          6) /* AttackType - Thrust, Slash */
     , (10035394,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (10035394,  49,         35) /* WeaponTime */
     , (10035394,  51,          5) /* CombatUse - TwoHanded */
     , (10035394,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10035394, 106,        580) /* ItemSpellcraft */
     , (10035394, 107,      10000) /* ItemCurMana */
     , (10035394, 108,      10000) /* ItemMaxMana */
     , (10035394, 151,          2) /* HookType - Wall */
     , (10035394, 158,          2) /* WieldRequirements - RawSkill */
     , (10035394, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (10035394, 160,        100) /* WieldDifficulty */
     , (10035394, 166,         22) /* SlayerCreatureType - Shadow */
     , (10035394, 179,          2) /* ImbuedEffect - CripplingBlow */
     , (10035394, 263,         1) /* ResistanceModifierType Slashing*/
     , (10035394, 292,          3) /* Cleaving */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10035394,  22, True ) /* Inscribable */
     , (10035394,  69, False) /* IsSellable */
     , (10035394,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10035394,   5,       0) /* ManaRate */
     , (10035394,  21,       1) /* WeaponLength */
     , (10035394,  22,     0.4) /* DamageVariance */
     , (10035394,  26,       0) /* MaximumVelocity */
     , (10035394,  29,     1.2) /* WeaponDefense */
     , (10035394,  39,     1.35) /* DefaultScale */
     , (10035394,  62,     1.2) /* WeaponOffense */
     , (10035394,  63,       1.3) /* DamageMod */
     , (10035394, 136,     1.8) /* CriticalMultiplier */
     , (10035394, 138,       2) /* SlayerDamageBonus */
     , (10035394, 147,    0.32) /* CriticalFrequency */
     , (10035394, 150,    1.05) /* WeaponMagicDefense */
     , (10035394, 155,       1) /* IgnoreArmor */
     , (10035394, 156,     0.5) /* ProcSpellRate */
     , (10035394, 157,    1.67) /* ResistanceModifier */
     , (10035394, 159,     0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10035394,   1, 'Shadowfire') /* Name */
     , (10035394,  16, 'A Perfect Isparian Two Handed Sword, infused with the power of the Shadowfire Stone.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10035394,   1, 0x02001511) /* Setup */
     , (10035394,   3, 0x20000014) /* SoundTable */
     , (10035394,   6, 0x04000BEF) /* PaletteBase */
     , (10035394,   7, 0x100003A1) /* ClothingBase */
     , (10035394,   8, 0x060073D4) /* Icon */
     , (10035394,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10035394,  55,       6190) /* ProcSpell - Horizon's Blades 2 */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10035394,  2966,      2)  /* Murderous Thirst */
     , (10035394,  6089,      2)  /* Leg Blood thirst */
     , (10035394,  6094,      2)  /* Leg Heart thirst */
     , (10035394,  6006,      2)  /* Defender 8 */
     , (10035394,  6091,      2)  /* Leg Defender */
     , (10035394,  6073,      2)  /* Leg Two handed Mastery */
     , (10035394,  6240,      2)  /* Paragon Two handed Mastery */
     , (10035394,  5110,      2)  /* Master soliders two hand*/
     , (10035394,  5098,      2)  /* incantation two hand*/
     , (10035394,  5732,      2)  /* Weave two handed mastery */
     , (10035394,  6014,      2)  /* Heart Seekr 8 */
     , (10035394,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (10035394,  5834,      2)  /*Recklessness level 8 */
     , (10035394,  5786,      2)  /*Dirty Fighting level 8 */
     -- , (10035394,  6190,      2)  /* Horizon's Blades 2*/

, (10035394,  6067,      2)  /*Legendary Recklessness*/
, (10035394,  6230,      2)  /*Paragon  Recklessness*/
, (10035394,  5957,      2)  /*Master Soldier's  Recklessness*/

, (10035394,  6049,      2)  /*Legendary Dirty Fighting*/
, (10035394,  6255,      2)  /*Paragon  Dirty Fighting*/
, (10035394,  5949,      2)  /*Master Soldier's  Dirty Fighting*/;
