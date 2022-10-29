DELETE FROM `weenie` WHERE `class_Id` = 4200167;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200167, 'tailor-ace4200167-redrunesilverancrossbow', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200167,   1,        256) /* ItemType - MissileWeapon */
     , (4200167,   5,          1) /* EncumbranceVal */
     , (4200167,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200167,  16,          1) /* ItemUseable - No */
     , (4200167,  19,         20) /* Value */
     , (4200167,  44,          0) /* Damage */
     , (4200167,  45,          0) /* DamageType - Undef */
     , (4200167,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200167,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200167,  49,         60) /* WeaponTime */
     , (4200167,  50,          2) /* AmmoType - Bolt */
     , (4200167,  51,          2) /* CombatUse - Missile */
     , (4200167,  52,          2) /* ParentLocation - LeftHand */
     , (4200167,  53,          3) /* PlacementPosition - LeftHand */
     , (4200167,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200167, 151,          2) /* HookType - Wall */
     , (4200167, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200167,  19, True ) /* Attackable */
     , (4200167,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200167,   5,   -0.05) /* ManaRate */
     , (4200167,  21,       0) /* WeaponLength */
     , (4200167,  22,       0) /* DamageVariance */
     , (4200167,  26,    27.3) /* MaximumVelocity */
     , (4200167,  39,    1.25) /* DefaultScale */
     , (4200167,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200167,   1, 'Red Rune Silveran Crossbow') /* Name */
     , (4200167,  15, 'A crossbow crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200167,   1, 0x02001596) /* Setup */
     , (4200167,   3, 0x20000014) /* SoundTable */
     , (4200167,   8, 0x06006425) /* Icon */
     , (4200167,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200167,  50, 0x06006413) /* IconOverlay */;
