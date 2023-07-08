DELETE FROM `weenie` WHERE `class_Id` = 480508;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480508, 'ace480508-eldrytchwebbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480508,   1,          1) /* ItemType - MeleeWeapon */
     , (480508,   5,        0) /* EncumbranceVal */
     , (480508,   8,        220) /* Mass */
     , (480508,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480508,  16,          1) /* ItemUseable - No */
	 , (480508,  19,       20) /* Value */
     , (480508,  18,          1) /* UiEffects - Magical */
     , (480508,  44,        0) /* Damage */
     , (480508,  45,          3) /* DamageType - Slash, Pierce */
     , (480508,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480508,  47,          6) /* AttackType - Thrust, Slash */
     , (480508,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480508,  49,         50) /* WeaponTime */
     , (480508,  51,          1) /* CombatUse - Melee */
     , (480508,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480508, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480508,  23, True ) /* DestroyOnSell */
     , (480508,  69, False) /* IsSellable */
     , (480508,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480508,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480508,   1, 'Eldrytch Web Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480508,   1, 0x0200143A) /* Setup */
     , (480508,   3, 0x20000014) /* SoundTable */
     , (480508,   8, 0x0600603E) /* Icon */
     , (480508,  22, 0x3400002B) /* PhysicsEffectTable */;
