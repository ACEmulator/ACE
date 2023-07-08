DELETE FROM `weenie` WHERE `class_Id` = 480538;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480538, 'lugianmorningstarhighpk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480538,   1,          1) /* ItemType - MeleeWeapon */
     , (480538,   5,      0) /* EncumbranceVal */
     , (480538,   8,       3680) /* Mass */
     , (480538,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480538,  16,          1) /* ItemUseable - No */
     , (480538,  19,        20) /* Value */
     , (480538,  44,         0) /* Damage */
     , (480538,  45,          2) /* DamageType - Pierce */
     , (480538,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480538,  47,          4) /* AttackType - Slash */
     , (480538,  48,          5) /* WeaponSkill - Mace */
     , (480538,  49,        140) /* WeaponTime */
     , (480538,  51,          1) /* CombatUse - Melee */
     , (480538,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480538, 150,        103) /* HookPlacement - Hook */
     , (480538, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480538,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480538,  21,     1.8) /* WeaponLength */
     , (480538,  22,     0.5) /* DamageVariance */
     , (480538,  39,       2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480538,   1, 'Lugian Morning Star') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480538,   1, 0x0200013C) /* Setup */
     , (480538,   3, 0x20000014) /* SoundTable */
     , (480538,   8, 0x060010D0) /* Icon */
     , (480538,  22, 0x3400002B) /* PhysicsEffectTable */;
