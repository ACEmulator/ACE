DELETE FROM `weenie` WHERE `class_Id` = 450589;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450589, 'ace450589-ultimatesingularitygreatswordtailor', 6, '2022-02-10 05:08:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450589,   1,          1) /* ItemType - MeleeWeapon */
     , (450589,   3,          8) /* PaletteTemplate - Green */
     , (450589,   5,        0) /* EncumbranceVal */
     , (450589,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450589,  16,          1) /* ItemUseable - No */
     , (450589,  18,          1) /* UiEffects - Magical */
     , (450589,  19,          20) /* Value */
     , (450589,  33,          1) /* Bonded - Bonded */
     , (450589,  44,         0) /* Damage */
     , (450589,  45,          1) /* DamageType - Slash */
     , (450589,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450589,  47,          4) /* AttackType - Slash */
     , (450589,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450589,  49,         40) /* WeaponTime */
     , (450589,  51,          5) /* CombatUse - TwoHanded */
     , (450589,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450589, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450589,  22, True ) /* Inscribable */
     , (450589,  23, True ) /* DestroyOnSell */
     , (450589,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450589,   5,  -0.033) /* ManaRate */
     , (450589,  21,       0) /* WeaponLength */
     , (450589,  22,    0.55) /* DamageVariance */
     , (450589,  26,       0) /* MaximumVelocity */
     , (450589,  29,    1.15) /* WeaponDefense */
     , (450589,  39,     1.3) /* DefaultScale */
     , (450589,  62,    1.15) /* WeaponOffense */
     , (450589,  63,       1) /* DamageMod */
     , (450589, 136,     2.5) /* CriticalMultiplier */
     , (450589, 138,     1.8) /* SlayerDamageBonus */
     , (450589, 147,    0.25) /* CriticalFrequency */
     , (450589, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450589,   1, 'Ultimate Singularity Greatsword') /* Name */
     , (450589,  15, 'A two handed sword imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450589,   1, 0x02000B47) /* Setup */
     , (450589,   3, 0x20000014) /* SoundTable */
     , (450589,   6, 0x04000BEF) /* PaletteBase */
     , (450589,   7, 0x1000029F) /* ClothingBase */
     , (450589,   8, 0x06006B96) /* Icon */
     , (450589,  22, 0x3400002B) /* PhysicsEffectTable */;

