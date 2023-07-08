DELETE FROM `weenie` WHERE `class_Id` = 480569;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480569, 'ace480569-thunderturkeylegpk', 6, '2022-03-31 06:02:40') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480569,   1,          1) /* ItemType - MeleeWeapon */
     , (480569,   5,        0) /* EncumbranceVal */
     , (480569,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480569,  16,          1) /* ItemUseable - No */
     , (480569,  19,        20) /* Value */
     , (480569,  44,          0) /* Damage */
     , (480569,  45,          4) /* DamageType - Bludgeon */
     , (480569,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480569,  47,          4) /* AttackType - Slash */
     , (480569,  48,         45) /* WeaponSkill - LightWeapons */
     , (480569,  49,         50) /* WeaponTime */
     , (480569,  51,          1) /* CombatUse - Melee */
     , (480569,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480569, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480569,  22, True ) /* Inscribable */
     , (480569,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480569,  21,       0) /* WeaponLength */
     , (480569,  22,     0.5) /* DamageVariance */
     , (480569,  26,       0) /* MaximumVelocity */
     , (480569,  39,     0.9) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480569,   1, 'Thunder Turkey Leg') /* Name */
     , (480569,  16, 'A golden brown turkey leg with a crispy skin. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480569,   1, 0x02001C0E) /* Setup */
     , (480569,   3, 0x20000014) /* SoundTable */
     , (480569,   8, 0x060019FE) /* Icon */
     , (480569,  22, 0x3400002B) /* PhysicsEffectTable */;

