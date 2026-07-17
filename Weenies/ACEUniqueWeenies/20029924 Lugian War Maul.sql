DELETE FROM `weenie` WHERE `class_Id` = 20029924;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20029924, 'Lugianaxeregal', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20029924,   1,          1) /* ItemType - MeleeWeapon */
     , (20029924,   5,        950) /* EncumbranceVal */
     , (20029924,   8,        350) /* Mass */
     , (20029924,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (20029924,  16,          1) /* ItemUseable - No */
     , (20029924,  18,          1) /* UiEffects - Magical */
     , (20029924,  19,       6000) /* Value */
     , (20029924,  44,         70) /* Damage */
     , (20029924,  45,          2) /* DamageType - Pierce */
     , (20029924,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (20029924,  47,          4) /* AttackType - Slash */
     , (20029924,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (20029924,  49,         65) /* WeaponTime */
     , (20029924,  51,          1) /* CombatUse - Melee */
     , (20029924,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (20029924, 106,        275) /* ItemSpellcraft */
     , (20029924, 107,        800) /* ItemCurMana */
     , (20029924, 108,        800) /* ItemMaxMana */
     , (20029924, 109,        150) /* ItemDifficulty */
     , (20029924, 150,        103) /* HookPlacement - Hook */
     , (20029924, 151,          2) /* HookType - Wall */
     , (20029924, 166,         31) /* SlayerCreatureType - Human */
     , (20029924, 179,          1) /* ImbuedEffect - CriticalStrike */
     , (20029924, 263,          2) /* ResistanceModifierType - Pierce */
     , (20029924, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20029924,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20029924,   5,  -0.033) /* ManaRate */
     , (20029924,  21,    0.75) /* WeaponLength */
     , (20029924,  22,    0.55) /* DamageVariance */
     , (20029924,  29,     1.1) /* WeaponDefense */
     , (20029924,  39,       5) /* DefaultScale */
     , (20029924,  62,     1.1) /* WeaponOffense */
     , (20029924, 136,     1.8) /* CriticalMultiplier */
     , (20029924, 138,    1.75) /* SlayerDamageBonus */
     , (20029924, 147,     0.3) /* CriticalFrequency */
     , (20029924, 155,     1.3) /* IgnoreArmor */
     , (20029924, 156,     0.5) /* ProcSpellRate */
     , (20029924, 157,     1.9) /* ResistanceModifier */
     , (20029924, 159,    0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20029924,   1, 'Lugian War Maul') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20029924,   1, 0x02001211) /* Setup */
     , (20029924,   3, 0x20000014) /* SoundTable */
     , (20029924,   8, 0x0600356F) /* Icon */
     , (20029924,  22, 0x3400002B) /* PhysicsEffectTable */
     , (20029924,  30,         87) /* PhysicsScript - BreatheLightning */
     , (20029924,  55,       4485) /* ProcSpell - Incantation of Corrosion */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (20029924,  2966,      2)  /* Aura of Murderous Thirst */
     , (20029924,  6089,      2)  /* Legendary Blood Thirst */
     , (20029924,  6094,      2)  /* Legendary Heart Thirst */
     , (20029924,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (20029924,  6091,      2)  /* Legendary Defender */
     , (20029924,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (20029924,  2116,      2)  /* Aura of Atlan's Alacrity */;
