DELETE FROM `weenie` WHERE `class_Id` = 20045449;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20045449, 'ace45449Monster-subjugator', 6, '2023-06-05 00:31:03') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20045449,   1,          1) /* ItemType - MeleeWeapon */
     , (20045449,   5,        850) /* EncumbranceVal */
     , (20045449,   8,         90) /* Mass */
     , (20045449,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (20045449,  16,          1) /* ItemUseable - No */
     , (20045449,  19,       5000) /* Value */
     , (20045449,  44,         56) /* Damage */
     , (20045449,  45,          4) /* DamageType - Bludgeon */
     , (20045449,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (20045449,  47,          4) /* AttackType - Slash */
     , (20045449,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (20045449,  49,         50) /* WeaponTime */
     , (20045449,  51,          1) /* CombatUse - Melee */
     , (20045449,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (20045449, 106,        480) /* ItemSpellcraft */
     , (20045449, 107,       2500) /* ItemCurMana */
     , (20045449, 108,       2500) /* ItemMaxMana */
     , (20045449, 109,          0) /* ItemDifficulty */
     , (20045449, 124,          2) /* Version */
     , (20045449, 151,          2) /* HookType - Wall */
     , (20045449, 166,         31) /* SlayerCreatureType - Human */
     , (20045449, 179,          1) /* ImbuedEffect - CriticalStrike */
     , (20045449, 263,          4) /* ResistanceModifierType - Bludgeon */
     , (20045449, 353,          4) /* WeaponType - Mace */;


-- INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
-- VALUES (45449,   4,          0) /* ItemTotalXp */
--      , (45449,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20045449,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20045449,   5,       0) /* ManaRate */
     , (20045449,  21,       1) /* WeaponLength */
     , (20045449,  22,     0.4) /* DamageVariance */
     , (20045449,  26,       0) /* MaximumVelocity */
     , (20045449,  29,     1.05) /* WeaponDefense */
     , (20045449,  39,       3.7) /* DefaultScale */
     , (20045449,  62,     1.2) /* WeaponOffense */
     , (20045449,  63,     1.5) /* DamageMod */
     , (20045449, 136,    1.35) /* CriticalMultiplier */
     , (20045449, 138,       2) /* SlayerDamageBonus */
     , (20045449, 147,    0.32) /* CriticalFrequency */
     , (20045449, 150,    1.05) /* WeaponMagicDefense */
     , (20045449, 155,     1.3) /* IgnoreArmor */
     , (20045449, 156,     0.5) /* ProcSpellRate */
     , (20045449, 157,     1.9) /* ResistanceModifier */
     , (20045449, 159,    0.15) /* AbsorbMagicDamage */;


INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20045449,   1, 'Lugian Subjugator') /* Name */
     , (20045449,  16, 'The Lugian Subjugator is a weapon that has made its mark upon history. The founder of the Roulean Empire, a warlord named Maleksoros, wielded this mace as his personal battle-weapon. With the Subjugator, he personally defeated the leaders of every neighboring tribe, forming the seed of the Empire that would spread out to conquer almost all the known world. Since then, the mace came to represent royal authority in all of the lands conquered by Maleksoros and his successors.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20045449,   1, 0x02001353) /* Setup */
     , (20045449,   3, 0x20000014) /* SoundTable */
     , (20045449,   6, 0x04000BEF) /* PaletteBase */
     , (20045449,   7, 0x10000860) /* ClothingBase */
     , (20045449,   8, 0x06005B95) /* Icon */
     , (20045449,  22, 0x3400002B) /* PhysicsEffectTable */
     , (20045449,  36, 0x0E000012) /* MutateFilter */
     , (20045449,  46, 0x38000032) /* TsysMutationFilter */
     , (20045449,  52, 0x06005B0C) /* IconUnderlay */
     , (20045449,  55,       4477) /* ProcSpell - Bludgeoning Vuln */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (20045449,  2966,      2)  /* Aura of Murderous Thirst */
     , (20045449,  6089,      2)  /* Legendary Blood Thirst */
     , (20045449,  6094,      2)  /* Legendary Heart Thirst */
     , (20045449,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (20045449,  6091,      2)  /* Legendary Defender */
     , (20045449,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (20045449,  2116,      2)  /* Aura of Atlan's Alacrity */
     -- , (20045449,  6073,      2)  /* Legendary Two Handed Combat Aptitude */
     -- , (20045449,  6240,      2)  /* Paragon's Two Handed Combat Mastery V */
     -- , (20045449,  5110,      2)  /* Master Soldier's Two Handed Combat Aptitude */
     -- , (20045449,  5098,      2)  /* Incantation of Two Handed Combat Mastery Other */
     -- , (20045449,  5732,      2)  /* Weave of Two Handed Combat V */

     -- , (20045449,  5834,      2)  /* Incantation of Recklessness Mastery Self */
     -- , (20045449,  5786,      2)  /* Incantation of Dirty Fighting Mastery Self */
     -- , (20045449,  6067,      2)  /* Legendary Recklessness Prowess */
     -- , (20045449,  6230,      2)  /* Paragon's Recklessness Mastery V */
     -- , (20045449,  5957,      2)  /* Master Soldier's Recklessness Aptitude */
     -- , (20045449,  6049,      2)  /* Legendary Dirty Fighting Prowess */
     -- , (20045449,  6255,      2)  /* Paragon's Dirty Fighting Mastery V */
     -- , (20045449,  5949,      2)  /* Master Soldier's Dirty Fighting Aptitude */


     ;
