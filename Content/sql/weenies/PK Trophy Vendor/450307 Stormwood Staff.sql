DELETE FROM `weenie` WHERE `class_Id` = 450307;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450307, 'ace450307-stormwoodstafftailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450307,   1,          1) /* ItemType - MeleeWeapon */
     , (450307,   5,        0) /* EncumbranceVal */
     , (450307,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450307,  16,          1) /* ItemUseable - No */
     , (450307,  18,          1) /* UiEffects - Magical */
     , (450307,  19,        20) /* Value */
     , (450307,  44,         0) /* Damage */
     , (450307,  45,         64) /* DamageType - Electric */
     , (450307,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450307,  47,          6) /* AttackType - Thrust, Slash */
     , (450307,  48,         45) /* WeaponSkill - LightWeapons */
     , (450307,  49,         30) /* WeaponTime */
     , (450307,  51,          1) /* CombatUse - Melee */
     , (450307,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450307, 131,         75) /* MaterialType - Oak */
     , (450307, 151,          2) /* HookType - Wall */
     , (450307, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450307,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450307,   5,  -0.025) /* ManaRate */
     , (450307,  21,       0) /* WeaponLength */
     , (450307,  22,    0.35) /* DamageVariance */
     , (450307,  26,       0) /* MaximumVelocity */
     , (450307,  29,    1.23) /* WeaponDefense */
     , (450307,  39,     0.9) /* DefaultScale */
     , (450307,  62,    1.13) /* WeaponOffense */
     , (450307,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450307,   1, 'Stormwood Staff') /* Name */
     , (450307,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450307,  16, 'A staff imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450307,   1, 0x02001C45) /* Setup */
     , (450307,   3, 0x20000014) /* SoundTable */
     , (450307,   8, 0x0600755C) /* Icon */
     , (450307,  22, 0x3400002B) /* PhysicsEffectTable */;

