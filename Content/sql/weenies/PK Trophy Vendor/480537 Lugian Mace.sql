DELETE FROM `weenie` WHERE `class_Id` = 480537;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480537, 'lugianmaceextremepk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480537,   1,          1) /* ItemType - MeleeWeapon */
     , (480537,   5,       0) /* EncumbranceVal */
     , (480537,   8,       2080) /* Mass */
     , (480537,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480537,  16,          1) /* ItemUseable - No */
     , (480537,  19,        20) /* Value */
     , (480537,  44,         0) /* Damage */
     , (480537,  45,          4) /* DamageType - Bludgeon */
     , (480537,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480537,  47,          4) /* AttackType - Slash */
     , (480537,  48,          5) /* WeaponSkill - Mace */
     , (480537,  49,         80) /* WeaponTime */
     , (480537,  51,          1) /* CombatUse - Melee */
     , (480537,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480537, 150,        103) /* HookPlacement - Hook */
     , (480537, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480537,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480537,  21,    1.24) /* WeaponLength */
     , (480537,  22,     0.5) /* DamageVariance */
     , (480537,  39,       2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480537,   1, 'Lugian Mace') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480537,   1, 0x0200013B) /* Setup */
     , (480537,   3, 0x20000014) /* SoundTable */
     , (480537,   8, 0x060010C4) /* Icon */
     , (480537,  22, 0x3400002B) /* PhysicsEffectTable */;
