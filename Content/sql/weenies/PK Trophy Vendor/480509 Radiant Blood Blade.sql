DELETE FROM `weenie` WHERE `class_Id` = 480509;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480509, 'ace480509-radiantbloodbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480509,   1,          1) /* ItemType - MeleeWeapon */
     , (480509,   5,        0) /* EncumbranceVal */
     , (480509,   8,        220) /* Mass */
     , (480509,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480509,  16,          1) /* ItemUseable - No */
     , (480509,  18,          1) /* UiEffects - Magical */
	 , (480509,  19,       20) /* Value */
     , (480509,  44,        0) /* Damage */
     , (480509,  45,          3) /* DamageType - Slash, Pierce */
     , (480509,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480509,  47,          6) /* AttackType - Thrust, Slash */
     , (480509,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480509,  49,         50) /* WeaponTime */
     , (480509,  51,          1) /* CombatUse - Melee */
     , (480509,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480509, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480509,  23, True ) /* DestroyOnSell */
     , (480509,  69, False) /* IsSellable */
     , (480509,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480509,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480509,   1, 'Radiant Blood Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480509,   1, 0x02000FAC) /* Setup */
     , (480509,   3, 0x20000014) /* SoundTable */
     , (480509,   8, 0x06002D21) /* Icon */
     , (480509,  22, 0x3400002B) /* PhysicsEffectTable */;
