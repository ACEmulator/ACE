DELETE FROM `weenie` WHERE `class_Id` = 450573;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450573, 'crossbowsingularitynew2tailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450573,   1,        256) /* ItemType - MissileWeapon */
     , (450573,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450573,   5,       0) /* EncumbranceVal */
     , (450573,   8,        640) /* Mass */
     , (450573,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450573,  16,          1) /* ItemUseable - No */
     , (450573,  18,          1) /* UiEffects - Magical */
     , (450573,  19,         20) /* Value */
     , (450573,  33,          1) /* Bonded - Bonded */
     , (450573,  44,          0) /* Damage */
     , (450573,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (450573,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450573,  49,        100) /* WeaponTime */
     , (450573,  50,          2) /* AmmoType - Bolt */
     , (450573,  51,          2) /* CombatUse - Missile */
     , (450573,  52,          2) /* ParentLocation - LeftHand */
     , (450573,  53,          3) /* PlacementPosition - LeftHand */
     , (450573,  60,        192) /* WeaponRange */
     , (450573,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450573, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450573,  11, True ) /* IgnoreCollisions */
     , (450573,  13, True ) /* Ethereal */
     , (450573,  14, True ) /* GravityStatus */
     , (450573,  19, True ) /* Attackable */
     , (450573,  22, True ) /* Inscribable */
     , (450573,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450573,   5,  -0.033) /* ManaRate */
     , (450573,  26,    27.3) /* MaximumVelocity */
     , (450573,  29,    1.07) /* WeaponDefense */
     , (450573,  39,    1.25) /* DefaultScale */
     , (450573,  62,       1) /* WeaponOffense */
     , (450573,  63,     2.1) /* DamageMod */
     , (450573, 136,     2.5) /* CriticalMultiplier */
     , (450573, 138,     1.8) /* SlayerDamageBonus */
     , (450573, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450573,   1, 'Bound Singularity Crossbow') /* Name */
     , (450573,  15, 'A crossbow imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450573,   1, 0x02001106) /* Setup */
     , (450573,   3, 0x20000014) /* SoundTable */
     , (450573,   6, 0x04000BEF) /* PaletteBase */
     , (450573,   7, 0x1000030F) /* ClothingBase */
     , (450573,   8, 0x060033E6) /* Icon */
     , (450573,  22, 0x3400002B) /* PhysicsEffectTable */;


