DELETE FROM `weenie` WHERE `class_Id` = 450562;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450562, 'katarsingularitynewtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450562,   1,          1) /* ItemType - MeleeWeapon */
     , (450562,   3,          2) /* PaletteTemplate - Blue */
     , (450562,   5,        0) /* EncumbranceVal */
     , (450562,   8,         90) /* Mass */
     , (450562,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450562,  16,          1) /* ItemUseable - No */
     , (450562,  18,          1) /* UiEffects - Magical */
     , (450562,  19,          20) /* Value */
     , (450562,  44,         0) /* Damage */
     , (450562,  45,          3) /* DamageType - Slash, Pierce */
     , (450562,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450562,  47,          1) /* AttackType - Punch */
     , (450562,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450562,  49,         20) /* WeaponTime */
     , (450562,  51,          1) /* CombatUse - Melee */
     , (450562,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450562, 114,          1) /* Attuned - Attuned */
	 , (450562, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450562,  22, True ) /* Inscribable */
     , (450562,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450562,   5,  -0.033) /* ManaRate */
     , (450562,  21,    0.51) /* WeaponLength */
     , (450562,  22,    0.71) /* DamageVariance */
     , (450562,  29,    1.07) /* WeaponDefense */
     , (450562,  62,    1.07) /* WeaponOffense */
     , (450562, 136,     2.5) /* CriticalMultiplier */
     , (450562, 138,     1.8) /* SlayerDamageBonus */
     , (450562, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450562,   1, 'Singularity Katar') /* Name */
     , (450562,  15, 'A katar imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450562,   1, 0x02000B4B) /* Setup */
     , (450562,   3, 0x20000014) /* SoundTable */
     , (450562,   6, 0x04000BEF) /* PaletteBase */
     , (450562,   7, 0x10000311) /* ClothingBase */
     , (450562,   8, 0x0600245A) /* Icon */
     , (450562,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450562,  36, 0x0E000014) /* MutateFilter */;


