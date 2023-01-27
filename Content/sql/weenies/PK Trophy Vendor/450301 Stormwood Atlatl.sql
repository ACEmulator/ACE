DELETE FROM `weenie` WHERE `class_Id` = 450301;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450301, 'ace450301-stormwoodatlatltailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450301,   1,        256) /* ItemType - MissileWeapon */
     , (450301,   5,        0) /* EncumbranceVal */
     , (450301,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450301,  16,          1) /* ItemUseable - No */
     , (450301,  18,          1) /* UiEffects - Magical */
     , (450301,  19,        20) /* Value */
     , (450301,  44,          0) /* Damage */
     , (450301,  46,       1024) /* DefaultCombatStyle - Atlatl */
     , (450301,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450301,  49,         25) /* WeaponTime */
     , (450301,  50,          4) /* AmmoType - Atlatl */
     , (450301,  51,          2) /* CombatUse - Missile */
     , (450301,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450301, 131,         75) /* MaterialType - Oak */
     , (450301, 151,          2) /* HookType - Wall */
     , (450301, 353,         10) /* WeaponType - Thrown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450301,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450301,   5,  -0.025) /* ManaRate */
     , (450301,  21,       0) /* WeaponLength */
     , (450301,  22,       0) /* DamageVariance */
     , (450301,  26,    24.9) /* MaximumVelocity */
     , (450301,  29,    1.0) /* WeaponDefense */
     , (450301,  39,     1.1) /* DefaultScale */
     , (450301,  62,       1) /* WeaponOffense */
     , (450301,  63,    0) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450301,   1, 'Stormwood Atlatl') /* Name */
     , (450301,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450301,  16, 'An atlatl imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450301,   1, 0x02001C3E) /* Setup */
     , (450301,   3, 0x20000014) /* SoundTable */
     , (450301,   8, 0x06007555) /* Icon */
     , (450301,  22, 0x3400002B) /* PhysicsEffectTable */;

