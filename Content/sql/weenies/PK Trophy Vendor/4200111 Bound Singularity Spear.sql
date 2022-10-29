DELETE FROM `weenie` WHERE `class_Id` = 4200111;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200111, 'spearsingularitynew2tailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200111,   1,          1) /* ItemType - MeleeWeapon */
     , (4200111,   3,         82) /* PaletteTemplate - PinkPurple */
     , (4200111,   5,          0) /* EncumbranceVal */
     , (4200111,   8,        320) /* Mass */
     , (4200111,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200111,  16,          1) /* ItemUseable - No */
     , (4200111,  18,          1) /* UiEffects - Magical */
     , (4200111,  19,         20) /* Value */
     , (4200111,  44,          1) /* Damage */
     , (4200111,  45,         64) /* DamageType - Pierce */
     , (4200111,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200111,  47,          2) /* AttackType - Thrust */
     , (4200111,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200111,  49,         30) /* WeaponTime */
     , (4200111,  51,          1) /* CombatUse - Melee */
     , (4200111,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200111,  52,          1) /* ParentLocation - RightHand */
     , (4200111, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200111,  22, True ) /* Inscribable */
     , (4200111,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200111,   5,  -0.033) /* ManaRate */
     , (4200111,  21,    0.75) /* WeaponLength */
     , (4200111,  22,    0.66) /* DamageVariance */
     , (4200111,  29,    1.07) /* WeaponDefense */
     , (4200111,  62,    1.07) /* WeaponOffense */
     , (4200111,  77,       1) /* PhysicsScriptIntensity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200111,   1, 'Bound Singularity Spear') /* Name */
     , (4200111,  15, 'A spear imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200111,   1, 0x0200110D) /* Setup */
     , (4200111,   3, 0x20000014) /* SoundTable */
     , (4200111,   6, 0x04000BEF) /* PaletteBase */
     , (4200111,   7, 0x1000029E) /* ClothingBase */
     , (4200111,   8, 0x060033EA) /* Icon */
     , (4200111,  19, 0x00000058) /* ActivationAnimation */
     , (4200111,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200111,  30,         87) /* PhysicsScript - BreatheLightning */
     , (4200111,  36, 0x0E000014) /* MutateFilter */;
