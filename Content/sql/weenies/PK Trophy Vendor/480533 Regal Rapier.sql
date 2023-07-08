DELETE FROM `weenie` WHERE `class_Id` = 480533;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480533, 'swordregalpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480533,   1,          1) /* ItemType - MeleeWeapon */
     , (480533,   5,        0) /* EncumbranceVal */
     , (480533,   8,        180) /* Mass */
     , (480533,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480533,  16,          1) /* ItemUseable - No */
     , (480533,  18,          1) /* UiEffects - Magical */
     , (480533,  19,       20) /* Value */
     , (480533,  44,         0) /* Damage */
     , (480533,  45,          3) /* DamageType - Slash, Pierce */
     , (480533,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480533,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (480533,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480533,  49,         45) /* WeaponTime */
     , (480533,  51,          1) /* CombatUse - Melee */
     , (480533,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480533, 150,        103) /* HookPlacement - Hook */
     , (480533, 151,          2) /* HookType - Wall */
     , (480533, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480533,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480533,   5,  -0.033) /* ManaRate */
     , (480533,  21,    0.95) /* WeaponLength */
     , (480533,  39,     1.1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480533,   1, 'Regal Rapier') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480533,   1, 0x0200120E) /* Setup */
     , (480533,   3, 0x20000014) /* SoundTable */
     , (480533,   8, 0x0600356D) /* Icon */
     , (480533,  22, 0x3400002B) /* PhysicsEffectTable */;


