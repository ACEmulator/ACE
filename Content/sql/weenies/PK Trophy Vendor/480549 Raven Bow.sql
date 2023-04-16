DELETE FROM `weenie` WHERE `class_Id` = 480549;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480549, 'ace480549-ravenbowpk', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480549,   1,        256) /* ItemType - MissileWeapon */
     , (480549,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (480549,   5,        0) /* EncumbranceVal */
     , (480549,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480549,  16,          1) /* ItemUseable - No */
     , (480549,  19,        20) /* Value */
     , (480549,  46,         16) /* DefaultCombatStyle - Bow */
     , (480549,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480549,  50,          1) /* AmmoType - Arrow */
     , (480549,  51,          2) /* CombatUse - Missile */
     , (480549,  52,          2) /* ParentLocation - LeftHand */
     , (480549,  53,          3) /* PlacementPosition - LeftHand */
     , (480549,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480549, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480549,  11, True ) /* IgnoreCollisions */
     , (480549,  13, True ) /* Ethereal */
     , (480549,  14, True ) /* GravityStatus */
     , (480549,  19, True ) /* Attackable */
     , (480549,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480549,  12,       0) /* Shade */
     , (480549,  21,       0) /* WeaponLength */
     , (480549,  26,    27.5) /* MaximumVelocity */
     , (480549,  39,     1.1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480549,   1, 'Raven Bow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480549,   1, 0x02001306) /* Setup */
     , (480549,   3, 0x20000014) /* SoundTable */
     , (480549,   6, 0x04001A23) /* PaletteBase */
     , (480549,   7, 0x1000060C) /* ClothingBase */
     , (480549,   8, 0x06005CCE) /* Icon */
     , (480549,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480549,  50, 0x060030AD) /* IconOverlay */;
