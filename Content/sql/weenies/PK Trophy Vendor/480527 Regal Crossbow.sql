DELETE FROM `weenie` WHERE `class_Id` = 480527;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480527, 'crossbowregalpk', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480527,   1,        256) /* ItemType - MissileWeapon */
     , (480527,   5,        0) /* EncumbranceVal */
     , (480527,   8,        640) /* Mass */
     , (480527,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480527,  16,          1) /* ItemUseable - No */
     , (480527,  18,          1) /* UiEffects - Magical */
     , (480527,  19,       20) /* Value */
     , (480527,  44,          0) /* Damage */
     , (480527,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (480527,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480527,  49,        120) /* WeaponTime */
     , (480527,  50,          2) /* AmmoType - Bolt */
     , (480527,  51,          2) /* CombatUse - Missile */
     , (480527,  52,          2) /* ParentLocation - LeftHand */
     , (480527,  53,          3) /* PlacementPosition - LeftHand */
     , (480527,  60,        192) /* WeaponRange */
     , (480527,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480527, 150,        103) /* HookPlacement - Hook */
     , (480527, 151,          2) /* HookType - Wall */
     , (480527, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480527,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480527,   5,  -0.033) /* ManaRate */
     , (480527,  26,    27.3) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480527,   1, 'Regal Crossbow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480527,   1, 0x02001213) /* Setup */
     , (480527,   3, 0x20000014) /* SoundTable */
     , (480527,   8, 0x06003571) /* Icon */
     , (480527,  22, 0x3400002B) /* PhysicsEffectTable */;

