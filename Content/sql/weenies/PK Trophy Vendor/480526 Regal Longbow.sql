DELETE FROM `weenie` WHERE `class_Id` = 480526;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480526, 'bowregalpk', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480526,   1,        256) /* ItemType - MissileWeapon */
     , (480526,   5,        0) /* EncumbranceVal */
     , (480526,   8,        140) /* Mass */
     , (480526,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480526,  16,          1) /* ItemUseable - No */
     , (480526,  18,          1) /* UiEffects - Magical */
     , (480526,  19,       20) /* Value */
     , (480526,  44,          0) /* Damage */
     , (480526,  46,         16) /* DefaultCombatStyle - Bow */
     , (480526,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480526,  49,         45) /* WeaponTime */
     , (480526,  50,          1) /* AmmoType - Arrow */
     , (480526,  51,          2) /* CombatUse - Missile */
     , (480526,  52,          2) /* ParentLocation - LeftHand */
     , (480526,  53,          3) /* PlacementPosition - LeftHand */
     , (480526,  60,        175) /* WeaponRange */
     , (480526,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480526, 150,        103) /* HookPlacement - Hook */
     , (480526, 151,          2) /* HookType - Wall */
     , (480526, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480526,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480526,   5,  -0.033) /* ManaRate */
     , (480526,  26,    27.3) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480526,   1, 'Regal Longbow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480526,   1, 0x02001212) /* Setup */
     , (480526,   3, 0x20000014) /* SoundTable */
     , (480526,   8, 0x06003570) /* Icon */
     , (480526,  22, 0x3400002B) /* PhysicsEffectTable */;


