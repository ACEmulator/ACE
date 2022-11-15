DELETE FROM `weenie` WHERE `class_Id` = 450578;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450578, 'daggersingularitynew2tailorr', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450578,   1,          1) /* ItemType - MeleeWeapon */
     , (450578,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450578,   5,        0) /* EncumbranceVal */
     , (450578,   8,         90) /* Mass */
     , (450578,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450578,  16,          1) /* ItemUseable - No */
     , (450578,  18,          1) /* UiEffects - Magical */
     , (450578,  19,          0) /* Value */
     , (450578,  33,          1) /* Bonded - Bonded */
     , (450578,  44,         0) /* Damage */
     , (450578,  45,          3) /* DamageType - Slash, Pierce */
     , (450578,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450578,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (450578,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450578,  49,         20) /* WeaponTime */
     , (450578,  51,          1) /* CombatUse - Melee */
     , (450578,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450578, 114,          1) /* Attuned - Attuned */
     , (450578, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450578,  22, True ) /* Inscribable */
     , (450578,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450578,   5,  -0.033) /* ManaRate */
     , (450578,  21,     0.4) /* WeaponLength */
     , (450578,  22,     0.7) /* DamageVariance */
     , (450578,  29,    1.07) /* WeaponDefense */
     , (450578,  62,    1.07) /* WeaponOffense */
     , (450578, 136,     2.5) /* CriticalMultiplier */
     , (450578, 138,     1.8) /* SlayerDamageBonus */
     , (450578, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450578,   1, 'Bound Singularity Dagger') /* Name */
     , (450578,  15, 'A dagger imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450578,   1, 0x02001107) /* Setup */
     , (450578,   3, 0x20000014) /* SoundTable */
     , (450578,   6, 0x04000BEF) /* PaletteBase */
     , (450578,   7, 0x10000310) /* ClothingBase */
     , (450578,   8, 0x060033E4) /* Icon */
     , (450578,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450578,  36, 0x0E000014) /* MutateFilter */;

