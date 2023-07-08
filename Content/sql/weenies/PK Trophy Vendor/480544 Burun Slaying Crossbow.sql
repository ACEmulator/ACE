DELETE FROM `weenie` WHERE `class_Id` = 480544;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480544, 'crossbownobleburunpk', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480544,   1,        256) /* ItemType - MissileWeapon */
     , (480544,   5,        0) /* EncumbranceVal */
     , (480544,   8,        640) /* Mass */
     , (480544,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480544,  16,          1) /* ItemUseable - No */
     , (480544,  18,         16) /* UiEffects - BoostStamina */
     , (480544,  19,       20) /* Value */
     , (480544,  44,          0) /* Damage */
     , (480544,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (480544,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480544,  49,        120) /* WeaponTime */
     , (480544,  50,          2) /* AmmoType - Bolt */
     , (480544,  51,          2) /* CombatUse - Missile */
     , (480544,  52,          2) /* ParentLocation - LeftHand */
     , (480544,  53,          3) /* PlacementPosition - LeftHand */
     , (480544,  60,        192) /* WeaponRange */
     , (480544,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480544, 150,        103) /* HookPlacement - Hook */
     , (480544, 151,          2) /* HookType - Wall */
     , (480544, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480544,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480544,   5,  -0.033) /* ManaRate */
     , (480544,  26,    27.3) /* MaximumVelocity */
     , (480544,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480544,   1, 'Burun Slaying Crossbow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480544,   1, 0x0200118A) /* Setup */
     , (480544,   3, 0x20000014) /* SoundTable */
     , (480544,   8, 0x06003571) /* Icon */
     , (480544,  22, 0x3400002B) /* PhysicsEffectTable */;


