DELETE FROM `weenie` WHERE `class_Id` = 450581;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450581, 'ace450581-ultimatesingularitybowtailor', 3, '2022-02-10 05:08:07') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450581,   1,        256) /* ItemType - MissileWeapon */
     , (450581,   3,          8) /* PaletteTemplate - Green */
     , (450581,   5,        0) /* EncumbranceVal */
     , (450581,   8,        140) /* Mass */
     , (450581,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450581,  16,          1) /* ItemUseable - No */
     , (450581,  18,          1) /* UiEffects - Magical */
     , (450581,  19,          20) /* Value */
     , (450581,  33,          1) /* Bonded - Bonded */
     , (450581,  44,         0) /* Damage */
     , (450581,  46,         16) /* DefaultCombatStyle - Bow */
     , (450581,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450581,  49,         50) /* WeaponTime */
     , (450581,  50,          1) /* AmmoType - Arrow */
     , (450581,  51,          2) /* CombatUse - Missile */
     , (450581,  52,          2) /* ParentLocation - LeftHand */
     , (450581,  53,          3) /* PlacementPosition - LeftHand */
     , (450581,  60,        192) /* WeaponRange */
     , (450581,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450581, 150,        103) /* HookPlacement - Hook */
     , (450581, 151,          2) /* HookType - Wall */
     , (450581, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450581,  22, True ) /* Inscribable */
     , (450581,  23, True ) /* DestroyOnSell */
     , (450581,  84, True ) /* IgnoreCloIcons */
     , (450581,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450581,   5,  -0.033) /* ManaRate */
     , (450581,  21,       0) /* WeaponLength */
     , (450581,  22,       0) /* DamageVariance */
     , (450581,  26,    27.3) /* MaximumVelocity */
     , (450581,  29,    1.15) /* WeaponDefense */
     , (450581,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450581,   1, 'Ultimate Singularity Bow') /* Name */
     , (450581,  15, 'A bow imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450581,   1, 0x02000B40) /* Setup */
     , (450581,   3, 0x20000014) /* SoundTable */
     , (450581,   6, 0x04000BEF) /* PaletteBase */
     , (450581,   7, 0x1000030E) /* ClothingBase */
     , (450581,   8, 0x0600222A) /* Icon */
     , (450581,  22, 0x3400002B) /* PhysicsEffectTable */;


