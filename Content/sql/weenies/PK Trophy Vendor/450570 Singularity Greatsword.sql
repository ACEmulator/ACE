DELETE FROM `weenie` WHERE `class_Id` = 450570;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450570, 'ace450570-singularitygreatswordtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450570,   1,          1) /* ItemType - MeleeWeapon */
     , (450570,   3,          2) /* PaletteTemplate - Blue */
     , (450570,   5,        0) /* EncumbranceVal */
     , (450570,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450570,  16,          1) /* ItemUseable - No */
     , (450570,  18,          1) /* UiEffects - Magical */
     , (450570,  19,          20) /* Value */
     , (450570,  44,         0) /* Damage */
     , (450570,  45,          1) /* DamageType - Slash */
     , (450570,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450570,  47,          4) /* AttackType - Slash */
     , (450570,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450570,  49,         40) /* WeaponTime */
     , (450570,  51,          5) /* CombatUse - TwoHanded */
     , (450570,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450570, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450570,  22, True ) /* Inscribable */
     , (450570,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450570,   5,  -0.033) /* ManaRate */
     , (450570,  21,       0) /* WeaponLength */
     , (450570,  22,    0.55) /* DamageVariance */
     , (450570,  26,       0) /* MaximumVelocity */
     , (450570,  29,    1.07) /* WeaponDefense */
     , (450570,  39,     1.3) /* DefaultScale */
     , (450570,  62,    1.07) /* WeaponOffense */
     , (450570,  63,       1) /* DamageMod */
     , (450570, 136,     2.5) /* CriticalMultiplier */
     , (450570, 138,     1.8) /* SlayerDamageBonus */
     , (450570, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450570,   1, 'Singularity Greatsword') /* Name */
     , (450570,  15, 'A two handed sword imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450570,   1, 0x020009E9) /* Setup */
     , (450570,   3, 0x20000014) /* SoundTable */
     , (450570,   6, 0x04000BEF) /* PaletteBase */
     , (450570,   7, 0x1000029F) /* ClothingBase */
     , (450570,   8, 0x06006B97) /* Icon */
     , (450570,  22, 0x3400002B) /* PhysicsEffectTable */;

