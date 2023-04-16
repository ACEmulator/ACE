DELETE FROM `weenie` WHERE `class_Id` = 480540;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480540, 'ace480540-frozendaggerpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480540,   1,          1) /* ItemType - MeleeWeapon */
     , (480540,   5,        0) /* EncumbranceVal */
     , (480540,   8,         90) /* Mass */
     , (480540,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480540,  16,          1) /* ItemUseable - No */
     , (480540,  19,         20) /* Value */
     , (480540,  44,         0) /* Damage */
     , (480540,  45,          8) /* DamageType - Cold */
     , (480540,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480540,  47,          6) /* AttackType - Thrust, Slash */
     , (480540,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480540,  49,         20) /* WeaponTime */
     , (480540,  51,          1) /* CombatUse - Melee */
     , (480540,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480540, 150,        103) /* HookPlacement - Hook */
     , (480540, 151,          2) /* HookType - Wall */
     , (480540, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480540,  19, True ) /* Attackable */
     , (480540,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480540,  21,     0.5) /* WeaponLength */
     , (480540,  22,     0.5) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480540,   1, 'Frozen Dagger') /* Name */
     , (480540,  16, 'The dagger glistens with destruction as if mirroring the will of the wielder.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480540,   1, 0x02001344) /* Setup */
     , (480540,   3, 0x20000014) /* SoundTable */
     , (480540,   8, 0x06005AF3) /* Icon */
     , (480540,  22, 0x3400002B) /* PhysicsEffectTable */;
