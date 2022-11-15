DELETE FROM `weenie` WHERE `class_Id` = 450574;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450574, 'bowsingularitynew2tailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450574,   1,        256) /* ItemType - MissileWeapon */
     , (450574,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450574,   5,        0) /* EncumbranceVal */
     , (450574,   8,        140) /* Mass */
     , (450574,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450574,  16,          1) /* ItemUseable - No */
     , (450574,  18,          1) /* UiEffects - Magical */
     , (450574,  19,         20) /* Value */
     , (450574,  33,          1) /* Bonded - Bonded */
     , (450574,  44,          0) /* Damage */
     , (450574,  46,         16) /* DefaultCombatStyle - Bow */
     , (450574,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450574,  49,         50) /* WeaponTime */
     , (450574,  50,          1) /* AmmoType - Arrow */
     , (450574,  51,          2) /* CombatUse - Missile */
     , (450574,  52,          2) /* ParentLocation - LeftHand */
     , (450574,  53,          3) /* PlacementPosition - LeftHand */
     , (450574,  60,        192) /* WeaponRange */
     , (450574,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450574, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450574,  11, True ) /* IgnoreCollisions */
     , (450574,  13, True ) /* Ethereal */
     , (450574,  14, True ) /* GravityStatus */
     , (450574,  19, True ) /* Attackable */
     , (450574,  22, True ) /* Inscribable */
     , (450574,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450574,   5,  -0.033) /* ManaRate */
     , (450574,  21,       0) /* WeaponLength */
     , (450574,  22,       0) /* DamageVariance */
     , (450574,  26,    27.3) /* MaximumVelocity */
     , (450574,  29,    1.07) /* WeaponDefense */
     , (450574,  39,     1.1) /* DefaultScale */
     , (450574,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450574,   1, 'Bound Singularity Bow') /* Name */
     , (450574,  15, 'A bow imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450574,   1, 0x02001105) /* Setup */
     , (450574,   3, 0x20000014) /* SoundTable */
     , (450574,   6, 0x04000BEF) /* PaletteBase */
     , (450574,   7, 0x1000030E) /* ClothingBase */
     , (450574,   8, 0x060033E5) /* Icon */
     , (450574,  22, 0x3400002B) /* PhysicsEffectTable */;


