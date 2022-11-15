DELETE FROM `weenie` WHERE `class_Id` = 450580;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450580, 'ace450580-boundsingularitygreatswordtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450580,   1,          1) /* ItemType - MeleeWeapon */
     , (450580,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450580,   5,        0) /* EncumbranceVal */
     , (450580,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450580,  16,          1) /* ItemUseable - No */
     , (450580,  18,          1) /* UiEffects - Magical */
     , (450580,  19,          20) /* Value */
     , (450580,  33,          1) /* Bonded - Bonded */
     , (450580,  44,         0) /* Damage */
     , (450580,  45,          1) /* DamageType - Slash */
     , (450580,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450580,  47,          4) /* AttackType - Slash */
     , (450580,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450580,  49,         40) /* WeaponTime */
     , (450580,  51,          5) /* CombatUse - TwoHanded */
     , (450580,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450580, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450580,  22, True ) /* Inscribable */
     , (450580,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450580,   5,  -0.033) /* ManaRate */
     , (450580,  21,       0) /* WeaponLength */
     , (450580,  22,    0.55) /* DamageVariance */
     , (450580,  26,       0) /* MaximumVelocity */
     , (450580,  29,    1.07) /* WeaponDefense */
     , (450580,  39,     1.3) /* DefaultScale */
     , (450580,  62,    1.07) /* WeaponOffense */
     , (450580,  63,       1) /* DamageMod */
     , (450580, 136,     2.5) /* CriticalMultiplier */
     , (450580, 138,     1.8) /* SlayerDamageBonus */
     , (450580, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450580,   1, 'Bound Singularity Greatsword') /* Name */
     , (450580,  15, 'A two handed sword imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450580,   1, 0x0200110F) /* Setup */
     , (450580,   3, 0x20000014) /* SoundTable */
     , (450580,   6, 0x04000BEF) /* PaletteBase */
     , (450580,   7, 0x1000029F) /* ClothingBase */
     , (450580,   8, 0x06006B98) /* Icon */
     , (450580,  22, 0x3400002B) /* PhysicsEffectTable */;

