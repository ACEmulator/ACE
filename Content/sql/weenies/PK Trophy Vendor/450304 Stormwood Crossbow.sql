DELETE FROM `weenie` WHERE `class_Id` = 450304;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450304, 'ace450304-stormwoodcrossbowtailor', 3, '2022-08-22 03:09:27') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450304,   1,        256) /* ItemType - MissileWeapon */
     , (450304,   5,       1920) /* EncumbranceVal */
     , (450304,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450304,  16,          1) /* ItemUseable - No */
     , (450304,  18,          1) /* UiEffects - Magical */
     , (450304,  19,        20) /* Value */
     , (450304,  44,          0) /* Damage */
     , (450304,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (450304,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450304,  49,        120) /* WeaponTime */
     , (450304,  50,          2) /* AmmoType - Bolt */
     , (450304,  51,          2) /* CombatUse - Missile */
     , (450304,  52,          2) /* ParentLocation - LeftHand */
     , (450304,  53,          3) /* PlacementPosition - LeftHand */
     , (450304,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450304, 131,         75) /* MaterialType - Oak */
     , (450304, 151,          2) /* HookType - Wall */
     , (450304, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450304,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450304,   5,  -0.025) /* ManaRate */
     , (450304,  21,       0) /* WeaponLength */
     , (450304,  22,       0) /* DamageVariance */
     , (450304,  26,    27.3) /* MaximumVelocity */
     , (450304,  29,    1.18) /* WeaponDefense */
     , (450304,  39,    1.25) /* DefaultScale */
     , (450304,  62,       1) /* WeaponOffense */
     , (450304,  63,    2.63) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450304,   1, 'Stormwood Crossbow') /* Name */
     , (450304,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450304,  16, 'A crossbow imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450304,   1, 0x02001C41) /* Setup */
     , (450304,   3, 0x20000014) /* SoundTable */
     , (450304,   8, 0x06007558) /* Icon */
     , (450304,  22, 0x3400002B) /* PhysicsEffectTable */;


