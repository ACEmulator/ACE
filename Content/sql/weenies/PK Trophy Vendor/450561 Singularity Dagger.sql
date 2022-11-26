DELETE FROM `weenie` WHERE `class_Id` = 450561;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450561, 'daggersingularitynewtailot', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450561,   1,          1) /* ItemType - MeleeWeapon */
     , (450561,   3,          2) /* PaletteTemplate - Blue */
     , (450561,   5,        0) /* EncumbranceVal */
     , (450561,   8,         90) /* Mass */
     , (450561,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450561,  16,          1) /* ItemUseable - No */
     , (450561,  18,          1) /* UiEffects - Magical */
     , (450561,  19,          20) /* Value */
     , (450561,  44,         0) /* Damage */
     , (450561,  45,          3) /* DamageType - Slash, Pierce */
     , (450561,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450561,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (450561,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450561,  49,         20) /* WeaponTime */
     , (450561,  51,          1) /* CombatUse - Melee */
     , (450561,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450561, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450561,  22, True ) /* Inscribable */
     , (450561,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450561,   5,  -0.033) /* ManaRate */
     , (450561,  21,     0.4) /* WeaponLength */
     , (450561,  22,     0.7) /* DamageVariance */
     , (450561,  29,    1.07) /* WeaponDefense */
     , (450561,  62,    1.07) /* WeaponOffense */
     , (450561, 136,     2.5) /* CriticalMultiplier */
     , (450561, 138,     1.8) /* SlayerDamageBonus */
     , (450561, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450561,   1, 'Singularity Dagger') /* Name */
     , (450561,  15, 'A dagger imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450561,   1, 0x02000B4A) /* Setup */
     , (450561,   3, 0x20000014) /* SoundTable */
     , (450561,   6, 0x04000BEF) /* PaletteBase */
     , (450561,   7, 0x10000310) /* ClothingBase */
     , (450561,   8, 0x06002459) /* Icon */
     , (450561,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450561,  36, 0x0E000014) /* MutateFilter */;

