DELETE FROM `weenie` WHERE `class_Id` = 480528;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480528, 'dirkregalpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480528,   1,          1) /* ItemType - MeleeWeapon */
     , (480528,   5,         0) /* EncumbranceVal */
     , (480528,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480528,  16,          1) /* ItemUseable - No */
     , (480528,  18,          1) /* UiEffects - Magical */
     , (480528,  19,       20) /* Value */
     , (480528,  44,         0) /* Damage */
     , (480528,  45,          3) /* DamageType - Slash, Pierce */
     , (480528,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480528,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (480528,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480528,  49,         10) /* WeaponTime */
     , (480528,  51,          1) /* CombatUse - Melee */
     , (480528,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480528, 150,        103) /* HookPlacement - Hook */
     , (480528, 151,          2) /* HookType - Wall */
     , (480528, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480528,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480528,   5,  -0.033) /* ManaRate */
     , (480528,  21,     0.4) /* WeaponLength */
     , (480528,  22,     0.4) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480528,   1, 'Regal Stilleto') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480528,   1, 0x02001209) /* Setup */
     , (480528,   3, 0x20000014) /* SoundTable */
     , (480528,   8, 0x06003572) /* Icon */
     , (480528,  22, 0x3400002B) /* PhysicsEffectTable */;
