DELETE FROM `weenie` WHERE `class_Id` = 450310;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450310, 'ace450310-stormwoodclawtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450310,   1,          1) /* ItemType - MeleeWeapon */
     , (450310,   5,        0) /* EncumbranceVal */
     , (450310,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450310,  16,          1) /* ItemUseable - No */
     , (450310,  18,          1) /* UiEffects - Magical */
     , (450310,  19,         20) /* Value */
     , (450310,  44,         0) /* Damage */
     , (450310,  45,         64) /* DamageType - Electric */
     , (450310,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450310,  47,          1) /* AttackType - Punch */
     , (450310,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450310,  49,         20) /* WeaponTime */
     , (450310,  51,          1) /* CombatUse - Melee */
     , (450310,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450310, 131,         75) /* MaterialType - Oak */
     , (450310, 151,          2) /* HookType - Wall */
     , (450310, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450310,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450310,   5,  -0.025) /* ManaRate */
     , (450310,  21,       0) /* WeaponLength */
     , (450310,  22,    0.48) /* DamageVariance */
     , (450310,  26,       0) /* MaximumVelocity */
     , (450310,  29,    1.18) /* WeaponDefense */
     , (450310,  39,     0.8) /* DefaultScale */
     , (450310,  62,    1.18) /* WeaponOffense */
     , (450310,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450310,   1, 'Stormwood Claw') /* Name */
     , (450310,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450310,  16, 'A claw imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450310,   1, 0x02001C48) /* Setup */
     , (450310,   3, 0x20000014) /* SoundTable */
     , (450310,   8, 0x0600755F) /* Icon */
     , (450310,  22, 0x3400002B) /* PhysicsEffectTable */;

