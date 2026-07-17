DELETE FROM `weenie` WHERE `class_Id` = 20032600;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20032600, 'ace20032600-shadowfireispariansword', 6, '2022-06-06 04:05:48') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20032600,   1,          1) /* ItemType - MeleeWeapon */
     , (20032600,   3,         20) /* PaletteTemplate - Silver */
     , (20032600,   5,        450) /* EncumbranceVal */
     , (20032600,   8,        450) /* Mass */
     , (20032600,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (20032600,  16,          1) /* ItemUseable - No */
     , (20032600,  18,          1) /* UiEffects - Magical */
     , (20032600,  19,      10000) /* Value */
     , (20032600,  33,          1) /* Bonded - Bonded */
     , (20032600,  36,       9999) /* ResistMagic */
     , (20032600,  44,         48) /* Damage */
     , (20032600,  45,          1) /* DamageType - Slash */
     , (20032600,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (20032600,  47,          4) /* AttackType - Slash */
     , (20032600,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (20032600,  49,         35) /* WeaponTime */
     , (20032600,  51,          1) /* CombatUse - Melee */
     , (20032600,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (20032600, 106,        450) /* ItemSpellcraft */
     , (20032600, 107,       5000) /* ItemCurMana */
     , (20032600, 108,       5000) /* ItemMaxMana */
     , (20032600, 114,          1) /* Attuned - Attuned */
     , (20032600, 150,        103) /* HookPlacement - Hook */
     , (20032600, 151,          2) /* HookType - Wall */
     , (20032600, 158,          2) /* WieldRequirements - RawSkill */
     , (20032600, 159,         46) /* WieldSkillType - FinesseWeapons */
     , (20032600, 160,        400) /* WieldDifficulty */
     , (20032600, 166,         31) /* SlayerCreatureType - Human */
     , (20032600, 179,          1) /* ImbuedEffect - CriticalStrike */
     , (20032600, 263,          1) /* ResistanceModifierType - Slash */
     , (20032600, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20032600,  22, True ) /* Inscribable */
     , (20032600,  23, True ) /* DestroyOnSell */
     , (20032600,  69, False) /* IsSellable */
     , (20032600,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20032600,   5,       0) /* ManaRate */
     , (20032600,  21,       1) /* WeaponLength */
     , (20032600,  22,     0.4) /* DamageVariance */
     , (20032600,  26,       0) /* MaximumVelocity */
     , (20032600,  29,     1.2) /* WeaponDefense */
     , (20032600,  39,       3.6) /* DefaultScale */
     , (20032600,  62,     1.2) /* WeaponOffense */
     , (20032600,  63,     1.5) /* DamageMod */
     , (20032600, 136,    1.35) /* CriticalMultiplier */
     , (20032600, 138,       2) /* SlayerDamageBonus */
     , (20032600, 147,    0.32) /* CriticalFrequency */
     , (20032600, 150,    1.05) /* WeaponMagicDefense */
     , (20032600, 155,     1.3) /* IgnoreArmor */
     , (20032600, 156,     0.5) /* ProcSpellRate */
     , (20032600, 157,     1.9) /* ResistanceModifier */
     , (20032600, 159,    0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20032600,   1, 'Lugian Perfect Quadruple-bladed Axe') /* Name */
     , (20032600,  16, 'A Perfect Quadruple-bladed Axe, forged from the depths of Linvak Tukal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20032600,   1, 0x02000F6B) /* Setup */
     , (20032600,   3, 0x20000014) /* SoundTable */
     , (20032600,   6, 0x04000BEF) /* PaletteBase */
     , (20032600,   7, 0x10000143) /* ClothingBase */
     , (20032600,   8, 0x06002B68) /* Icon */
     , (20032600,  22, 0x3400002B) /* PhysicsEffectTable */
     , (20032600,  55,       4475) /* ProcSpell - Blade Vuln 8 */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (20032600,  2966,      2)  /* Aura of Murderous Thirst */
     , (20032600,  6089,      2)  /* Legendary Blood Thirst */
     , (20032600,  6094,      2)  /* Legendary Heart Thirst */
     , (20032600,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (20032600,  6091,      2)  /* Legendary Defender */
     , (20032600,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (20032600,  2116,      2)  /* Aura of Atlan's Alacrity */

     ;
