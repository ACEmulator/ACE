DELETE FROM `weenie` WHERE `class_Id` = 480530;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480530, 'macenregalpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480530,   1,          1) /* ItemType - MeleeWeapon */
     , (480530,   5,        0) /* EncumbranceVal */
     , (480530,   8,        750) /* Mass */
     , (480530,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480530,  16,          1) /* ItemUseable - No */
     , (480530,  18,          1) /* UiEffects - Magical */
     , (480530,  19,       20) /* Value */
     , (480530,  44,         0) /* Damage */
     , (480530,  45,          4) /* DamageType - Bludgeon */
     , (480530,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480530,  47,          4) /* AttackType - Slash */
     , (480530,  48,         45) /* WeaponSkill - LightWeapons */
     , (480530,  49,         50) /* WeaponTime */
     , (480530,  51,          1) /* CombatUse - Melee */
     , (480530,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480530, 150,        103) /* HookPlacement - Hook */
     , (480530, 151,          2) /* HookType - Wall */
     , (480530, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480530,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480530,   5,  -0.033) /* ManaRate */
     , (480530,  21,     0.9) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480530,   1, 'Regal Morning Star') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480530,   1, 0x0200120B) /* Setup */
     , (480530,   3, 0x20000014) /* SoundTable */
     , (480530,   8, 0x06003574) /* Icon */
     , (480530,  22, 0x3400002B) /* PhysicsEffectTable */;

