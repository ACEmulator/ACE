DELETE FROM `weenie` WHERE `class_Id` = 480532;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480532, 'staffregalpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480532,   1,          1) /* ItemType - MeleeWeapon */
     , (480532,   5,        0) /* EncumbranceVal */
     , (480532,   8,        150) /* Mass */
     , (480532,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480532,  16,          1) /* ItemUseable - No */
     , (480532,  18,          1) /* UiEffects - Magical */
     , (480532,  19,       20) /* Value */
     , (480532,  44,         0) /* Damage */
     , (480532,  45,          4) /* DamageType - Bludgeon */
     , (480532,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480532,  47,          6) /* AttackType - Thrust, Slash */
     , (480532,  48,         45) /* WeaponSkill - LightWeapons */
     , (480532,  49,         25) /* WeaponTime */
     , (480532,  51,          1) /* CombatUse - Melee */
     , (480532,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480532, 150,        103) /* HookPlacement - Hook */
     , (480532, 151,          2) /* HookType - Wall */
     , (480532, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480532,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480532,   5,  -0.033) /* ManaRate */
     , (480532,  21,     1.3) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480532,   1, 'Regal Quarterstaff') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480532,   1, 0x0200120D) /* Setup */
     , (480532,   3, 0x20000014) /* SoundTable */
     , (480532,   8, 0x06003577) /* Icon */
     , (480532,  22, 0x3400002B) /* PhysicsEffectTable */;

