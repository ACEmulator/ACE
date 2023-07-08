DELETE FROM `weenie` WHERE `class_Id` = 480536;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480536, 'lugianhammerextremepk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480536,   1,          1) /* ItemType - MeleeWeapon */
     , (480536,   5,       0) /* EncumbranceVal */
     , (480536,   8,       1840) /* Mass */
     , (480536,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480536,  16,          1) /* ItemUseable - No */
     , (480536,  19,        20) /* Value */
     , (480536,  44,         0) /* Damage */
     , (480536,  45,          4) /* DamageType - Bludgeon */
     , (480536,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480536,  47,          4) /* AttackType - Slash */
     , (480536,  48,          1) /* WeaponSkill - Axe */
     , (480536,  49,        100) /* WeaponTime */
     , (480536,  51,          1) /* CombatUse - Melee */
     , (480536,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480536, 150,        103) /* HookPlacement - Hook */
     , (480536, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480536,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480536,  21,     1.2) /* WeaponLength */
     , (480536,  22,     0.5) /* DamageVariance */
     , (480536,  39,       2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480536,   1, 'Lugian Hammer') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480536,   1, 0x0200014E) /* Setup */
     , (480536,   3, 0x20000014) /* SoundTable */
     , (480536,   8, 0x060010E3) /* Icon */
     , (480536,  22, 0x3400002B) /* PhysicsEffectTable */;
