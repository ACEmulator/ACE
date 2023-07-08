DELETE FROM `weenie` WHERE `class_Id` = 480529;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480529, 'katarregalpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480529,   1,          1) /* ItemType - MeleeWeapon */
     , (480529,   5,         0) /* EncumbranceVal */
     , (480529,   8,         90) /* Mass */
     , (480529,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480529,  16,          1) /* ItemUseable - No */
     , (480529,  18,          1) /* UiEffects - Magical */
     , (480529,  19,       20) /* Value */
     , (480529,  44,         0) /* Damage */
     , (480529,  45,          3) /* DamageType - Slash, Pierce */
     , (480529,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (480529,  47,          1) /* AttackType - Punch */
     , (480529,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480529,  49,         10) /* WeaponTime */
     , (480529,  51,          1) /* CombatUse - Melee */
     , (480529,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480529, 150,        103) /* HookPlacement - Hook */
     , (480529, 151,          2) /* HookType - Wall */
     , (480529, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480529,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480529,   5,  -0.033) /* ManaRate */
     , (480529,  21,    0.52) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480529,   1, 'Regal Katar') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480529,   1, 0x0200120A) /* Setup */
     , (480529,   3, 0x20000014) /* SoundTable */
     , (480529,   8, 0x06003573) /* Icon */
     , (480529,  22, 0x3400002B) /* PhysicsEffectTable */;

