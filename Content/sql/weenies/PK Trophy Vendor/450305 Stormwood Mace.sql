DELETE FROM `weenie` WHERE `class_Id` = 450305;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450305, 'ace450305-stormwoodmacetailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450305,   1,          1) /* ItemType - MeleeWeapon */
     , (450305,   5,        0) /* EncumbranceVal */
     , (450305,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450305,  16,          1) /* ItemUseable - No */
     , (450305,  18,          1) /* UiEffects - Magical */
     , (450305,  19,        20) /* Value */
     , (450305,  44,         0) /* Damage */
     , (450305,  45,         64) /* DamageType - Electric */
     , (450305,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450305,  47,          4) /* AttackType - Slash */
     , (450305,  48,         45) /* WeaponSkill - LightWeapons */
     , (450305,  49,         35) /* WeaponTime */
     , (450305,  51,          1) /* CombatUse - Melee */
     , (450305,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450305, 131,         75) /* MaterialType - Oak */
     , (450305, 151,          2) /* HookType - Wall */
     , (450305, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450305,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450305,   5,  -0.025) /* ManaRate */
     , (450305,  21,       0) /* WeaponLength */
     , (450305,  22,    0.28) /* DamageVariance */
     , (450305,  26,       0) /* MaximumVelocity */
     , (450305,  29,     1.2) /* WeaponDefense */
     , (450305,  62,    1.16) /* WeaponOffense */
     , (450305,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450305,   1, 'Stormwood Mace') /* Name */
     , (450305,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450305,  16, 'A mace imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450305,   1, 0x02001C43) /* Setup */
     , (450305,   3, 0x20000014) /* SoundTable */
     , (450305,   8, 0x0600755A) /* Icon */
     , (450305,  22, 0x3400002B) /* PhysicsEffectTable */;
