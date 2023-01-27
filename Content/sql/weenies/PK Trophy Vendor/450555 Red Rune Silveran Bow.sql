DELETE FROM `weenie` WHERE `class_Id` = 450555;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450555, 'ace450555-redrunesilveranbowtailor', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450555,   1,        256) /* ItemType - MissileWeapon */
     , (450555,   5,        0) /* EncumbranceVal */
     , (450555,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450555,  16,          1) /* ItemUseable - No */
     , (450555,  19,      20) /* Value */
     , (450555,  44,         0) /* Damage */
     , (450555,  45,          0) /* DamageType - Undef */
     , (450555,  46,         16) /* DefaultCombatStyle - Bow */
     , (450555,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450555,  49,         50) /* WeaponTime */
     , (450555,  50,          1) /* AmmoType - Arrow */
     , (450555,  51,          2) /* CombatUse - Missile */
     , (450555,  52,          2) /* ParentLocation - LeftHand */
     , (450555,  53,          3) /* PlacementPosition - LeftHand */
     , (450555,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450555, 151,          2) /* HookType - Wall */
     , (450555, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450555,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450555,   5,   -0.05) /* ManaRate */
     , (450555,  21,       0) /* WeaponLength */
     , (450555,  22,       0) /* DamageVariance */
     , (450555,  26,    27.3) /* MaximumVelocity */
     , (450555,  29,    1.15) /* WeaponDefense */
     , (450555,  62,       1) /* WeaponOffense */
     , (450555,  63,     2.5) /* DamageMod */
     , (450555, 136,       2) /* CriticalMultiplier */
     , (450555, 147,    0.33) /* CriticalFrequency */
     , (450555, 157,     1.5) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450555,   1, 'Red Rune Silveran Bow') /* Name */
     , (450555,  15, 'A bow crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450555,   1, 0x0200158D) /* Setup */
     , (450555,   3, 0x20000014) /* SoundTable */
     , (450555,   8, 0x06006420) /* Icon */
     , (450555,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450555,  50, 0x06006413) /* IconOverlay */;

