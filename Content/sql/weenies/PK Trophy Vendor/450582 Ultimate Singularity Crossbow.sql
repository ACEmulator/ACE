DELETE FROM `weenie` WHERE `class_Id` = 450582;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450582, 'ace450582-ultimatesingularitycrossbowtailor', 3, '2022-02-10 05:08:07') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450582,   1,        256) /* ItemType - MissileWeapon */
     , (450582,   3,          8) /* PaletteTemplate - Green */
     , (450582,   5,       0) /* EncumbranceVal */
     , (450582,   8,        640) /* Mass */
     , (450582,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450582,  16,          1) /* ItemUseable - No */
     , (450582,  18,          1) /* UiEffects - Magical */
     , (450582,  19,          20) /* Value */
     , (450582,  33,          1) /* Bonded - Bonded */
     , (450582,  44,         0) /* Damage */
     , (450582,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (450582,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450582,  49,        100) /* WeaponTime */
     , (450582,  50,          2) /* AmmoType - Bolt */
     , (450582,  51,          2) /* CombatUse - Missile */
     , (450582,  52,          2) /* ParentLocation - LeftHand */
     , (450582,  53,          3) /* PlacementPosition - LeftHand */
     , (450582,  60,        192) /* WeaponRange */
     , (450582,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450582, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450582,  22, True ) /* Inscribable */
     , (450582,  23, True ) /* DestroyOnSell */
     , (450582,  84, True ) /* IgnoreCloIcons */
     , (450582,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450582,   5,  -0.033) /* ManaRate */
     , (450582,  26,    27.3) /* MaximumVelocity */
     , (450582,  29,    1.15) /* WeaponDefense */
     , (450582,  39,    1.25) /* DefaultScale */
     , (450582,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450582,   1, 'Ultimate Singularity Crossbow') /* Name */
     , (450582,  15, 'A crossbow imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450582,   1, 0x02000B41) /* Setup */
     , (450582,   3, 0x20000014) /* SoundTable */
     , (450582,   6, 0x04000BEF) /* PaletteBase */
     , (450582,   7, 0x1000030F) /* ClothingBase */
     , (450582,   8, 0x06002231) /* Icon */
     , (450582,  22, 0x3400002B) /* PhysicsEffectTable */;


