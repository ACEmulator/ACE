DELETE FROM `weenie` WHERE `class_Id` = 480545;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480545, 'ace480545-ravencrossbowpk', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480545,   1,        256) /* ItemType - MissileWeapon */
     , (480545,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (480545,   5,       0) /* EncumbranceVal */
     , (480545,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480545,  16,          1) /* ItemUseable - No */
     , (480545,  19,        20) /* Value */
     , (480545,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (480545,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480545,  49,         20) /* WeaponTime */
     , (480545,  50,          2) /* AmmoType - Bolt */
     , (480545,  51,          2) /* CombatUse - Missile */
     , (480545,  52,          2) /* ParentLocation - LeftHand */
     , (480545,  53,          3) /* PlacementPosition - LeftHand */
     , (480545,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480545, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480545,  11, True ) /* IgnoreCollisions */
     , (480545,  13, True ) /* Ethereal */
     , (480545,  14, True ) /* GravityStatus */
     , (480545,  19, True ) /* Attackable */
     , (480545,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480545,  12,       0) /* Shade */
     , (480545,  21,       0) /* WeaponLength */
     , (480545,  26,    27.5) /* MaximumVelocity */
     , (480545,  39,    1.25) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480545,   1, 'Raven Crossbow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480545,   1, 0x02001307) /* Setup */
     , (480545,   3, 0x20000014) /* SoundTable */
     , (480545,   6, 0x04001A24) /* PaletteBase */
     , (480545,   7, 0x1000060B) /* ClothingBase */
     , (480545,   8, 0x06005CC4) /* Icon */
     , (480545,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480545,  50, 0x060030AD) /* IconOverlay */;
