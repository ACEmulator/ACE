DELETE FROM `weenie` WHERE `class_Id` = 450579;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450579, 'swordsingularitynew2tailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450579,   1,          1) /* ItemType - MeleeWeapon */
     , (450579,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450579,   5,        0) /* EncumbranceVal */
     , (450579,   8,        180) /* Mass */
     , (450579,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450579,  16,          1) /* ItemUseable - No */
     , (450579,  18,          1) /* UiEffects - Magical */
     , (450579,  19,          20) /* Value */
     , (450579,  33,          1) /* Bonded - Bonded */
     , (450579,  44,         0) /* Damage */
     , (450579,  45,          3) /* DamageType - Slash, Pierce */
     , (450579,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450579,  47,          6) /* AttackType - Thrust, Slash */
     , (450579,  48,         45) /* WeaponSkill - LightWeapons */
     , (450579,  49,         40) /* WeaponTime */
     , (450579,  51,          1) /* CombatUse - Melee */
     , (450579,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450579, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450579,  22, True ) /* Inscribable */
     , (450579,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450579,   5,  -0.033) /* ManaRate */
     , (450579,  21,    0.95) /* WeaponLength */
     , (450579,  22,    0.16) /* DamageVariance */
     , (450579,  29,    1.07) /* WeaponDefense */
     , (450579,  39,     1.1) /* DefaultScale */
     , (450579,  62,    1.07) /* WeaponOffense */
     , (450579, 136,     2.5) /* CriticalMultiplier */
     , (450579, 138,     1.8) /* SlayerDamageBonus */
     , (450579, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450579,   1, 'Bound Singularity Sword') /* Name */
     , (450579,  15, 'A sword imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450579,   1, 0x0200110F) /* Setup */
     , (450579,   3, 0x20000014) /* SoundTable */
     , (450579,   6, 0x04000BEF) /* PaletteBase */
     , (450579,   7, 0x1000029F) /* ClothingBase */
     , (450579,   8, 0x060033E8) /* Icon */
     , (450579,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450579,  36, 0x0E000014) /* MutateFilter */;

