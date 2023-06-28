DELETE FROM `weenie` WHERE `class_Id` = 480506;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480506, 'ace480506-harvestersbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480506,   1,          1) /* ItemType - MeleeWeapon */
     , (480506,   5,        0) /* EncumbranceVal */
     , (480506,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480506,  16,          1) /* ItemUseable - No */
     , (480506,  19,       20) /* Value */
     , (480506,  44,        0) /* Damage */
     , (480506,  45,          3) /* DamageType - Fire */
     , (480506,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480506,  47,          6) /* AttackType - Thrust, Slash */
     , (480506,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480506,  51,          1) /* CombatUse - Melee */
     , (480506,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480506, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480506,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480506,  21,       1) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480506,   1, 'Harvester''s Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480506,   1, 0x0200130B) /* Setup */
     , (480506,   3, 0x20000014) /* SoundTable */
     , (480506,   8, 0x06005AED) /* Icon */
     , (480506,  22, 0x3400002B) /* PhysicsEffectTable */;
