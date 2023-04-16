DELETE FROM `weenie` WHERE `class_Id` = 480571;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480571, 'maceguruktree3pk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480571,   1,          1) /* ItemType - MeleeWeapon */
     , (480571,   5,       0) /* EncumbranceVal */
     , (480571,   8,       2560) /* Mass */
     , (480571,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480571,  16,          1) /* ItemUseable - No */
     , (480571,  19,        20) /* Value */
     , (480571,  44,         0) /* Damage */
     , (480571,  45,          4) /* DamageType - Bludgeon */
     , (480571,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480571,  47,          4) /* AttackType - Slash */
     , (480571,  48,          1) /* WeaponSkill - Axe */
     , (480571,  49,         60) /* WeaponTime */
     , (480571,  51,          1) /* CombatUse - Melee */
     , (480571,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480571, 353,          4) /* WeaponType - Mace */
     , (480571, 150,        103) /* HookPlacement - Hook */
     , (480571, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480571,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480571,  21,     1.5) /* WeaponLength */
     , (480571,  22,     0.5) /* DamageVariance */
     , (480571,  39,     0.3) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480571,   1, 'Tree Trunk') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480571,   1, 0x02001100) /* Setup */
     , (480571,   3, 0x20000014) /* SoundTable */
     , (480571,   8, 0x060033E3) /* Icon */
     , (480571,  22, 0x3400002B) /* PhysicsEffectTable */;


