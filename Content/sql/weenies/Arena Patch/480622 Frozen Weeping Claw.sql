DELETE FROM `weenie` WHERE `class_Id` = 480622;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480622, 'clawweepingfreezingdb', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480622,   1,          1) /* ItemType - MeleeWeapon */
     , (480622,   3,          2) /* PaletteTemplate - Blue */
     , (480622,   5,        125) /* EncumbranceVal */
     , (480622,   8,        110) /* Mass */
     , (480622,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480622,  16,          1) /* ItemUseable - No */
     , (480622,  18,          1) /* UiEffects - Magical */
     , (480622,  19,       8000) /* Value */
     , (480622,  33,          1) /* Bonded - Bonded */
     , (480622,  36,       9999) /* ResistMagic */
     , (480622,  44,         54) /* Damage */
     , (480622,  45,          8) /* DamageType - Cold */
     , (480622,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (480622,  47,          1) /* AttackType - Punch */
     , (480622,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480622,  49,          1) /* WeaponTime */
     , (480622,  51,          1) /* CombatUse - Melee */
     , (480622,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480622, 106,        300) /* ItemSpellcraft */
     , (480622, 107,        800) /* ItemCurMana */
     , (480622, 108,        800) /* ItemMaxMana */
     , (480622, 109,         50) /* ItemDifficulty */
     , (480622, 114,          1) /* Attuned - Attuned */
     , (480622, 150,        103) /* HookPlacement - Hook */
     , (480622, 151,          2) /* HookType - Wall */
     , (480622, 158,          2) /* WieldRequirements - RawSkill */
     , (480622, 159,         44) /* WieldSkillType - HeavyWeapons */
     , (480622, 160,        325) /* WieldDifficulty */
     , (480622, 166,         31) /* SlayerCreatureType - Human */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480622,  22, True ) /* Inscribable */
     , (480622,  69, False) /* IsSellable */
     , (480622,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480622,   5,  -0.025) /* ManaRate */
     , (480622,  21,    0.55) /* WeaponLength */
     , (480622,  22,     0.5) /* DamageVariance */
     , (480622,  26,       0) /* MaximumVelocity */
     , (480622,  29,    1.18) /* WeaponDefense */
     , (480622,  39,       2) /* DefaultScale */
     , (480622,  62,    1.23) /* WeaponOffense */
     , (480622,  63,       1) /* DamageMod */
     , (480622, 138,     3.4) /* SlayerDamageBonus */
     , (480622, 151,       1) /* IgnoreShield */
     , (480622, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480622,   1, 'Frozen Weeping Claw') /* Name */
     , (480622,  15, 'A claw infused with the Heart of the Innocent.The weapon appears to be guided by a preternatural force seeking flesh and blood with great tenacity.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480622,   1, 0x02000FD5) /* Setup */
     , (480622,   3, 0x20000014) /* SoundTable */
     , (480622,   6, 0x0400161A) /* PaletteBase */
     , (480622,   7, 0x100004DA) /* ClothingBase */
     , (480622,   8, 0x06002D49) /* Icon */
     , (480622,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480622,  36, 0x0E000014) /* MutateFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (480622,  2694,      2)  /* Moderate Heavy Weapon Aptitude */;
