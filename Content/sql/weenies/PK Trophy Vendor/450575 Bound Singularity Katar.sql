DELETE FROM `weenie` WHERE `class_Id` = 450575;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450575, 'katarsingularitynew2tailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450575,   1,          1) /* ItemType - MeleeWeapon */
     , (450575,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450575,   5,        0) /* EncumbranceVal */
     , (450575,   8,         90) /* Mass */
     , (450575,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450575,  16,          1) /* ItemUseable - No */
     , (450575,  18,          1) /* UiEffects - Magical */
     , (450575,  19,          20) /* Value */
     , (450575,  33,          1) /* Bonded - Bonded */
     , (450575,  44,         0) /* Damage */
     , (450575,  45,          3) /* DamageType - Slash, Pierce */
     , (450575,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450575,  47,          1) /* AttackType - Punch */
     , (450575,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450575,  49,         20) /* WeaponTime */
     , (450575,  51,          1) /* CombatUse - Melee */
     , (450575,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450575, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450575,  22, True ) /* Inscribable */
     , (450575,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450575,   5,  -0.033) /* ManaRate */
     , (450575,  21,    0.52) /* WeaponLength */
     , (450575,  22,    0.71) /* DamageVariance */
     , (450575,  29,    1.07) /* WeaponDefense */
     , (450575,  62,    1.07) /* WeaponOffense */
     , (450575, 136,     2.5) /* CriticalMultiplier */
     , (450575, 138,     1.8) /* SlayerDamageBonus */
     , (450575, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450575,   1, 'Bound Singularity Katar') /* Name */
     , (450575,  15, 'A katar imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450575,   1, 0x02001108) /* Setup */
     , (450575,   3, 0x20000014) /* SoundTable */
     , (450575,   6, 0x04000BEF) /* PaletteBase */
     , (450575,   7, 0x10000311) /* ClothingBase */
     , (450575,   8, 0x060033F0) /* Icon */
     , (450575,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450575,  36, 0x0E000014) /* MutateFilter */;


