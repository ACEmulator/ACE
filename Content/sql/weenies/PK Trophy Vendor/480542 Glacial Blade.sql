DELETE FROM `weenie` WHERE `class_Id` = 480542;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480542, 'ace480542-glacialbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480542,   1,          1) /* ItemType - MeleeWeapon */
     , (480542,   5,        0) /* EncumbranceVal */
     , (480542,   8,        180) /* Mass */
     , (480542,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480542,  16,          1) /* ItemUseable - No */
     , (480542,  19,        20) /* Value */
     , (480542,  44,         0) /* Damage */
     , (480542,  45,          8) /* DamageType - Cold */
     , (480542,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480542,  47,          6) /* AttackType - Thrust, Slash */
     , (480542,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480542,  49,         35) /* WeaponTime */
     , (480542,  51,          1) /* CombatUse - Melee */
     , (480542,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480542, 150,        103) /* HookPlacement - Hook */
     , (480542, 151,          2) /* HookType - Wall */
     , (480542, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480542,  19, True ) /* Attackable */
     , (480542,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480542,  21,    0.95) /* WeaponLength */
     , (480542,  22,    0.45) /* DamageVariance */
     , (480542,  26,       0) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480542,   1, 'Glacial Blade') /* Name */
     , (480542,  15, 'Ruschk Weapon Group T2') /* ShortDesc */
     , (480542,  16, 'The sword glistens with destruction as if mirroring the will of the wielder.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480542,   1, 0x02001342) /* Setup */
     , (480542,   3, 0x20000014) /* SoundTable */
     , (480542,   8, 0x06005AEF) /* Icon */
     , (480542,  22, 0x3400002B) /* PhysicsEffectTable */;
