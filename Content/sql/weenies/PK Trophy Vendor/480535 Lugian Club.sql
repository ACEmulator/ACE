DELETE FROM `weenie` WHERE `class_Id` = 480535;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480535, 'lugianclubextremepk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480535,   1,          1) /* ItemType - MeleeWeapon */
     , (480535,   5,       0) /* EncumbranceVal */
     , (480535,   8,        880) /* Mass */
     , (480535,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480535,  16,          1) /* ItemUseable - No */
     , (480535,  19,        20) /* Value */
     , (480535,  44,         0) /* Damage */
     , (480535,  45,          4) /* DamageType - Bludgeon */
     , (480535,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480535,  47,          4) /* AttackType - Slash */
     , (480535,  48,          5) /* WeaponSkill - Mace */
     , (480535,  49,         70) /* WeaponTime */
     , (480535,  51,          1) /* CombatUse - Melee */
     , (480535,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480535, 150,        103) /* HookPlacement - Hook */
     , (480535, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480535,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480535,  21,    1.36) /* WeaponLength */
     , (480535,  22,     0.5) /* DamageVariance */
     , (480535,  39,     2.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480535,   1, 'Lugian Club') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480535,   1, 0x0200012B) /* Setup */
     , (480535,   3, 0x20000014) /* SoundTable */
     , (480535,   8, 0x060010C3) /* Icon */
     , (480535,  22, 0x3400002B) /* PhysicsEffectTable */;
