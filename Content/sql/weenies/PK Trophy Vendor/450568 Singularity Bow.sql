DELETE FROM `weenie` WHERE `class_Id` = 450568;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450568, 'bowsingularitynewtailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450568,   1,        256) /* ItemType - MissileWeapon */
     , (450568,   3,          2) /* PaletteTemplate - Blue */
     , (450568,   5,        0) /* EncumbranceVal */
     , (450568,   8,        140) /* Mass */
     , (450568,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450568,  16,          1) /* ItemUseable - No */
     , (450568,  18,          1) /* UiEffects - Magical */
     , (450568,  19,          0) /* Value */
     , (450568,  44,          0) /* Damage */
     , (450568,  46,         16) /* DefaultCombatStyle - Bow */
     , (450568,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450568,  49,         50) /* WeaponTime */
     , (450568,  50,          1) /* AmmoType - Arrow */
     , (450568,  51,          2) /* CombatUse - Missile */
     , (450568,  52,          2) /* ParentLocation - LeftHand */
     , (450568,  53,          3) /* PlacementPosition - LeftHand */
     , (450568,  60,        192) /* WeaponRange */
     , (450568,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450568, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450568,  22, True ) /* Inscribable */
     , (450568,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450568,   5,  -0.033) /* ManaRate */
     , (450568,  21,       0) /* WeaponLength */
     , (450568,  22,       0) /* DamageVariance */
     , (450568,  26,    27.3) /* MaximumVelocity */
     , (450568,  29,    1.07) /* WeaponDefense */
     , (450568,  39,     1.1) /* DefaultScale */
     , (450568,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450568,   1, 'Singularity Bow') /* Name */
     , (450568,  15, 'A bow imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450568,   1, 0x02000B4C) /* Setup */
     , (450568,   3, 0x20000014) /* SoundTable */
     , (450568,   6, 0x04000BEF) /* PaletteBase */
     , (450568,   7, 0x1000030E) /* ClothingBase */
     , (450568,   8, 0x06002458) /* Icon */
     , (450568,  22, 0x3400002B) /* PhysicsEffectTable */;

