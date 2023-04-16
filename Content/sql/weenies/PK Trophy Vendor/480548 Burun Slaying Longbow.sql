DELETE FROM `weenie` WHERE `class_Id` = 480548;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480548, 'bownobleburunpk', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480548,   1,        256) /* ItemType - MissileWeapon */
     , (480548,   5,        0) /* EncumbranceVal */
     , (480548,   8,        140) /* Mass */
     , (480548,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480548,  16,          1) /* ItemUseable - No */
     , (480548,  18,         16) /* UiEffects - BoostStamina */
     , (480548,  19,       20) /* Value */
     , (480548,  44,          0) /* Damage */
     , (480548,  46,         16) /* DefaultCombatStyle - Bow */
     , (480548,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480548,  49,         45) /* WeaponTime */
     , (480548,  50,          1) /* AmmoType - Arrow */
     , (480548,  51,          2) /* CombatUse - Missile */
     , (480548,  52,          2) /* ParentLocation - LeftHand */
     , (480548,  53,          3) /* PlacementPosition - LeftHand */
     , (480548,  60,        175) /* WeaponRange */
     , (480548,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480548, 150,        103) /* HookPlacement - Hook */
     , (480548, 151,          2) /* HookType - Wall */
     , (480548, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480548,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480548,   5,  -0.033) /* ManaRate */
     , (480548,  26,    27.3) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480548,   1, 'Burun Slaying Longbow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480548,   1, 0x02001194) /* Setup */
     , (480548,   3, 0x20000014) /* SoundTable */
     , (480548,   8, 0x06003570) /* Icon */
     , (480548,  22, 0x3400002B) /* PhysicsEffectTable */;


