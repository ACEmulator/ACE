DELETE FROM `weenie` WHERE `class_Id` = 480507;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480507, 'ace480507-celestialhandbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480507,   1,          1) /* ItemType - MeleeWeapon */
     , (480507,   5,        0) /* EncumbranceVal */
     , (480507,   8,        220) /* Mass */
     , (480507,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480507,  16,          1) /* ItemUseable - No */
	 , (480507,  19,       20) /* Value */
     , (480507,  18,          1) /* UiEffects - Magical */
     , (480507,  44,        0) /* Damage */
     , (480507,  45,          3) /* DamageType - Slash, Pierce */
     , (480507,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480507,  47,          6) /* AttackType - Thrust, Slash */
     , (480507,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480507,  51,          1) /* CombatUse - Melee */
     , (480507,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480507, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480507,  23, True ) /* DestroyOnSell */
     , (480507,  69, False) /* IsSellable */
     , (480507,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480507,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480507,   1, 'Celestial Hand Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480507,   1, 0x02001656) /* Setup */
     , (480507,   3, 0x20000014) /* SoundTable */
     , (480507,   8, 0x0600658A) /* Icon */
     , (480507,  22, 0x3400002B) /* PhysicsEffectTable */;
