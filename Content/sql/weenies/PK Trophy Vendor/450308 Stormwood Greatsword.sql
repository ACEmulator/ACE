DELETE FROM `weenie` WHERE `class_Id` = 450308;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450308, 'ace450308-stormwoodgreatswordtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450308,   1,          1) /* ItemType - MeleeWeapon */
     , (450308,   5,        0) /* EncumbranceVal */
     , (450308,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450308,  16,          1) /* ItemUseable - No */
     , (450308,  18,          1) /* UiEffects - Magical */
     , (450308,  19,        20) /* Value */
     , (450308,  44,         0) /* Damage */
     , (450308,  45,         64) /* DamageType - Electric */
     , (450308,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450308,  47,          4) /* AttackType - Slash */
     , (450308,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450308,  49,         50) /* WeaponTime */
     , (450308,  51,          5) /* CombatUse - TwoHanded */
     , (450308,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450308, 131,         75) /* MaterialType - Oak */
     , (450308, 151,          2) /* HookType - Wall */
     , (450308, 292,          2) /* Cleaving */
     , (450308, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450308,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450308,   5,  -0.025) /* ManaRate */
     , (450308,  22,    0.35) /* DamageVariance */
     , (450308,  26,       0) /* MaximumVelocity */
     , (450308,  29,    1.18) /* WeaponDefense */
     , (450308,  39,       0) /* DefaultScale */
     , (450308,  62,    1.18) /* WeaponOffense */
     , (450308,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450308,   1, 'Stormwood Greatsword') /* Name */
     , (450308,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450308,  16, 'A two handed sword imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450308,   1, 0x02001C46) /* Setup */
     , (450308,   3, 0x20000014) /* SoundTable */
     , (450308,   8, 0x0600755D) /* Icon */
     , (450308,  22, 0x3400002B) /* PhysicsEffectTable */;

