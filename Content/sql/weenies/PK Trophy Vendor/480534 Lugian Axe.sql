DELETE FROM `weenie` WHERE `class_Id` = 480534;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480534, 'lugianaxeextremepk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480534,   1,          1) /* ItemType - MeleeWeapon */
     , (480534,   5,       0) /* EncumbranceVal */
     , (480534,   8,       2560) /* Mass */
     , (480534,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480534,  16,          1) /* ItemUseable - No */
     , (480534,  19,        20) /* Value */
     , (480534,  44,         0) /* Damage */
     , (480534,  45,          1) /* DamageType - Slash */
     , (480534,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480534,  47,          4) /* AttackType - Slash */
     , (480534,  48,          1) /* WeaponSkill - Axe */
     , (480534,  49,        120) /* WeaponTime */
     , (480534,  51,          1) /* CombatUse - Melee */
     , (480534,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480534, 150,        103) /* HookPlacement - Hook */
     , (480534, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480534,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480534,  21,     1.5) /* WeaponLength */
     , (480534,  22,     0.5) /* DamageVariance */
     , (480534,  39,       2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480534,   1, 'Lugian Axe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480534,   1, 0x02000126) /* Setup */
     , (480534,   3, 0x20000014) /* SoundTable */
     , (480534,   8, 0x060010BC) /* Icon */
     , (480534,  22, 0x3400002B) /* PhysicsEffectTable */;


