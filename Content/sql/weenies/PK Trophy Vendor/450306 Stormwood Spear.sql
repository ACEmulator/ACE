DELETE FROM `weenie` WHERE `class_Id` = 450306;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450306, 'ace450306-stormwoodspeartailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450306,   1,          1) /* ItemType - MeleeWeapon */
     , (450306,   5,        0) /* EncumbranceVal */
     , (450306,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450306,  16,          1) /* ItemUseable - No */
     , (450306,  18,          1) /* UiEffects - Magical */
     , (450306,  19,        20) /* Value */
     , (450306,  44,         0) /* Damage */
     , (450306,  45,         64) /* DamageType - Electric */
     , (450306,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450306,  47,          6) /* AttackType - Thrust, Slash */
     , (450306,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450306,  49,         35) /* WeaponTime */
     , (450306,  51,          1) /* CombatUse - Melee */
     , (450306, 131,         75) /* MaterialType - Oak */
     , (450306, 151,          2) /* HookType - Wall */
     , (450306, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450306,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450306,   5,  -0.025) /* ManaRate */
     , (450306,  22,    0.63) /* DamageVariance */
     , (450306,  26,       0) /* MaximumVelocity */
     , (450306,  29,    1.13) /* WeaponDefense */
     , (450306,  39,       0) /* DefaultScale */
     , (450306,  62,    1.23) /* WeaponOffense */
     , (450306,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450306,   1, 'Stormwood Spear') /* Name */
     , (450306,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450306,  16, 'A spear imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450306,   1, 0x02001C44) /* Setup */
     , (450306,   3, 0x20000014) /* SoundTable */
     , (450306,   8, 0x0600755B) /* Icon */
     , (450306,  22, 0x3400002B) /* PhysicsEffectTable */;

