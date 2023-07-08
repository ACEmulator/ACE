DELETE FROM `weenie` WHERE `class_Id` = 480531;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480531, 'spearregalpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480531,   1,          1) /* ItemType - MeleeWeapon */
     , (480531,   5,        0) /* EncumbranceVal */
     , (480531,   8,        150) /* Mass */
     , (480531,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480531,  16,          1) /* ItemUseable - No */
     , (480531,  18,          1) /* UiEffects - Magical */
     , (480531,  19,       20) /* Value */
     , (480531,  44,         0) /* Damage */
     , (480531,  45,          3) /* DamageType - Slash, Pierce */
     , (480531,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480531,  47,          6) /* AttackType - Thrust, Slash */
     , (480531,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480531,  49,         25) /* WeaponTime */
     , (480531,  51,          1) /* CombatUse - Melee */
     , (480531,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480531, 150,        103) /* HookPlacement - Hook */
     , (480531, 151,          2) /* HookType - Wall */
     , (480531, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480531,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480531,   5,  -0.033) /* ManaRate */
     , (480531,  21,     1.3) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480531,   1, 'Regal Swordstaff') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480531,   1, 0x0200120C) /* Setup */
     , (480531,   3, 0x20000014) /* SoundTable */
     , (480531,   8, 0x06003576) /* Icon */
     , (480531,  22, 0x3400002B) /* PhysicsEffectTable */;

