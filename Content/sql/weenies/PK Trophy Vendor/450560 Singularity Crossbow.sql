DELETE FROM `weenie` WHERE `class_Id` = 450560;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450560, 'crossbowsingularitytailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450560,   1,        256) /* ItemType - MissileWeapon */
     , (450560,   3,          2) /* PaletteTemplate - Blue */
     , (450560,   5,       0) /* EncumbranceVal */
     , (450560,   8,        640) /* Mass */
     , (450560,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450560,  16,          1) /* ItemUseable - No */
     , (450560,  18,          1) /* UiEffects - Magical */
     , (450560,  19,          20) /* Value */
     , (450560,  44,          0) /* Damage */
     , (450560,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (450560,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450560,  49,        100) /* WeaponTime */
     , (450560,  50,          2) /* AmmoType - Bolt */
     , (450560,  51,          2) /* CombatUse - Missile */
     , (450560,  52,          2) /* ParentLocation - LeftHand */
     , (450560,  53,          3) /* PlacementPosition - LeftHand */
     , (450560,  60,        192) /* WeaponRange */
     , (450560,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450560, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450560,  22, True ) /* Inscribable */
     , (450560,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450560,   5,  -0.033) /* ManaRate */
     , (450560,  26,    27.3) /* MaximumVelocity */
     , (450560,  29,    1.07) /* WeaponDefense */
     , (450560,  39,    1.25) /* DefaultScale */
     , (450560,  62,       1) /* WeaponOffense */
     , (450560,  63,     2.2) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450560,   1, 'Singularity Crossbow') /* Name */
     , (450560,  15, 'A crossbow imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450560,   1, 0x02000B49) /* Setup */
     , (450560,   3, 0x20000014) /* SoundTable */
     , (450560,   6, 0x04000BEF) /* PaletteBase */
     , (450560,   7, 0x1000030F) /* ClothingBase */
     , (450560,   8, 0x0600245C) /* Icon */
     , (450560,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450560,  37,          3) /* ItemSkillLimit - Crossbow */;


