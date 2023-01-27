DELETE FROM `weenie` WHERE `class_Id` = 450309;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450309, 'ace450309-stormwoodswordtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450309,   1,          1) /* ItemType - MeleeWeapon */
     , (450309,   5,        0) /* EncumbranceVal */
     , (450309,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450309,  16,          1) /* ItemUseable - No */
     , (450309,  18,          1) /* UiEffects - Magical */
     , (450309,  19,        20) /* Value */
     , (450309,  44,         0) /* Damage */
     , (450309,  45,         64) /* DamageType - Electric */
     , (450309,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450309,  47,          6) /* AttackType - Thrust, Slash */
     , (450309,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450309,  49,          0) /* WeaponTime */
     , (450309,  51,          1) /* CombatUse - Melee */
     , (450309,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450309, 131,         75) /* MaterialType - Oak */
     , (450309, 151,          2) /* HookType - Wall */
     , (450309, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450309,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450309,   5,  -0.025) /* ManaRate */
     , (450309,  21,       0) /* WeaponLength */
     , (450309,  22,     0.5) /* DamageVariance */
     , (450309,  26,       0) /* MaximumVelocity */
     , (450309,  29,    1.18) /* WeaponDefense */
     , (450309,  39,     1.1) /* DefaultScale */
     , (450309,  62,    1.18) /* WeaponOffense */
     , (450309,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450309,   1, 'Stormwood Sword') /* Name */
     , (450309,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450309,  16, 'A sword imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450309,   1, 0x02001C47) /* Setup */
     , (450309,   3, 0x20000014) /* SoundTable */
     , (450309,   8, 0x0600755E) /* Icon */
     , (450309,  22, 0x3400002B) /* PhysicsEffectTable */;


