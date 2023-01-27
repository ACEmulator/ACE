DELETE FROM `weenie` WHERE `class_Id` = 450303;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450303, 'ace450303-stormwoodbowtailor', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450303,   1,        256) /* ItemType - MissileWeapon */
     , (450303,   5,        0) /* EncumbranceVal */
     , (450303,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450303,  16,          1) /* ItemUseable - No */
     , (450303,  18,          1) /* UiEffects - Magical */
     , (450303,  19,        20) /* Value */
     , (450303,  44,          0) /* Damage */
     , (450303,  46,         16) /* DefaultCombatStyle - Bow */
     , (450303,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450303,  49,         45) /* WeaponTime */
     , (450303,  50,          1) /* AmmoType - Arrow */
     , (450303,  51,          2) /* CombatUse - Missile */
     , (450303,  52,          2) /* ParentLocation - LeftHand */
     , (450303,  53,          3) /* PlacementPosition - LeftHand */
     , (450303,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450303, 131,         75) /* MaterialType - Oak */
     , (450303, 151,          2) /* HookType - Wall */
     , (450303, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450303,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450303,   5,  -0.025) /* ManaRate */
     , (450303,  21,       0) /* WeaponLength */
     , (450303,  22,       0) /* DamageVariance */
     , (450303,  26,    27.3) /* MaximumVelocity */
     , (450303,  29,    1.18) /* WeaponDefense */
     , (450303,  39,     1.1) /* DefaultScale */
     , (450303,  62,       1) /* WeaponOffense */
     , (450303,  63,    2.37) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450303,   1, 'Stormwood Bow') /* Name */
     , (450303,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450303,  16, 'A bow imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450303,   1, 0x02001C40) /* Setup */
     , (450303,   3, 0x20000014) /* SoundTable */
     , (450303,   8, 0x06007557) /* Icon */
     , (450303,  22, 0x3400002B) /* PhysicsEffectTable */;

