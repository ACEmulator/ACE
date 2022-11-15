DELETE FROM `weenie` WHERE `class_Id` = 450557;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450557, 'ace450557-redrunesilverangreatswordtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450557,   1,          1) /* ItemType - MeleeWeapon */
     , (450557,   5,        0) /* EncumbranceVal */
     , (450557,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450557,  16,          1) /* ItemUseable - No */
     , (450557,  19,      20) /* Value */
     , (450557,  44,         0) /* Damage */
     , (450557,  45,          1) /* DamageType - Slash */
     , (450557,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450557,  47,          4) /* AttackType - Slash */
     , (450557,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450557,  49,         35) /* WeaponTime */
     , (450557,  51,          5) /* CombatUse - TwoHanded */
     , (450557,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450557, 151,          2) /* HookType - Wall */
     , (450557, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450557,  19, True ) /* Attackable */
     , (450557,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450557,   5,   -0.05) /* ManaRate */
     , (450557,  21,       0) /* WeaponLength */
     , (450557,  22,    0.55) /* DamageVariance */
     , (450557,  26,       0) /* MaximumVelocity */
     , (450557,  29,     1.1) /* WeaponDefense */
     , (450557,  62,    1.15) /* WeaponOffense */
     , (450557,  63,       1) /* DamageMod */
     , (450557, 136,       2) /* CriticalMultiplier */
     , (450557, 147,   0.299) /* CriticalFrequency */
     , (450557, 157,     1.6) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450557,   1, 'Red Rune Silveran Greatsword') /* Name */
     , (450557,  15, 'A fine two handed sword crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450557,   1, 0x020018EE) /* Setup */
     , (450557,   3, 0x20000014) /* SoundTable */
     , (450557,   8, 0x06006B82) /* Icon */
     , (450557,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450557,  50, 0x06006413) /* IconOverlay */;

