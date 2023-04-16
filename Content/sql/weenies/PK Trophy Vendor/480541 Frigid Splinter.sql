DELETE FROM `weenie` WHERE `class_Id` = 480541;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480541, 'ace480541-frigidsplinterpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480541,   1,          1) /* ItemType - MeleeWeapon */
     , (480541,   5,        0) /* EncumbranceVal */
     , (480541,   8,        140) /* Mass */
     , (480541,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480541,  16,          1) /* ItemUseable - No */
     , (480541,  19,        20) /* Value */
     , (480541,  44,         0) /* Damage */
     , (480541,  45,          8) /* DamageType - Cold */
     , (480541,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480541,  47,          2) /* AttackType - Thrust */
     , (480541,  48,         45) /* WeaponSkill - LightWeapons */
     , (480541,  49,         30) /* WeaponTime */
     , (480541,  51,          1) /* CombatUse - Melee */
     , (480541,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480541, 150,        103) /* HookPlacement - Hook */
     , (480541, 151,          2) /* HookType - Wall */
     , (480541, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480541,  13, True ) /* Ethereal */
     , (480541,  19, True ) /* Attackable */
     , (480541,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480541,  21,    1.25) /* WeaponLength */
     , (480541,  22,    0.75) /* DamageVariance */
     , (480541,  29,       1) /* WeaponDefense */
     , (480541,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480541,   1, 'Frigid Splinter') /* Name */
     , (480541,  16, 'The spear glistens with destruction as if mirroring the will of the wielder.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480541,   1, 0x02001308) /* Setup */
     , (480541,   3, 0x20000014) /* SoundTable */
     , (480541,   8, 0x06005AEE) /* Icon */
     , (480541,  22, 0x3400002B) /* PhysicsEffectTable */;
