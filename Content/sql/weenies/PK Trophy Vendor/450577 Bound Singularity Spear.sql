DELETE FROM `weenie` WHERE `class_Id` = 450577;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450577, 'spearsingularitynew2tailor2', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450577,   1,          1) /* ItemType - MeleeWeapon */
     , (450577,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450577,   5,        0) /* EncumbranceVal */
     , (450577,   8,        320) /* Mass */
     , (450577,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450577,  16,          1) /* ItemUseable - No */
     , (450577,  18,          1) /* UiEffects - Magical */
     , (450577,  19,          20) /* Value */
     , (450577,  33,          1) /* Bonded - Bonded */
     , (450577,  44,         0) /* Damage */
     , (450577,  45,          2) /* DamageType - Pierce */
     , (450577,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450577,  47,          2) /* AttackType - Thrust */
     , (450577,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450577,  49,         30) /* WeaponTime */
     , (450577,  51,          1) /* CombatUse - Melee */
     , (450577,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450577, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450577,  22, True ) /* Inscribable */
     , (450577,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450577,   5,  -0.033) /* ManaRate */
     , (450577,  21,    0.75) /* WeaponLength */
     , (450577,  22,    0.66) /* DamageVariance */
     , (450577,  29,    1.07) /* WeaponDefense */
     , (450577,  62,    1.07) /* WeaponOffense */
     , (450577,  77,       1) /* PhysicsScriptIntensity */
     , (450577, 136,     2.5) /* CriticalMultiplier */
     , (450577, 138,     1.8) /* SlayerDamageBonus */
     , (450577, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450577,   1, 'Bound Singularity Spear') /* Name */
     , (450577,  15, 'A spear imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450577,   1, 0x0200110D) /* Setup */
     , (450577,   3, 0x20000014) /* SoundTable */
     , (450577,   6, 0x04000BEF) /* PaletteBase */
     , (450577,   7, 0x1000029E) /* ClothingBase */
     , (450577,   8, 0x060033EA) /* Icon */
     , (450577,  19, 0x00000058) /* ActivationAnimation */
     , (450577,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450577,  30,         87) /* PhysicsScript - BreatheLightning */
     , (450577,  36, 0x0E000014) /* MutateFilter */;


