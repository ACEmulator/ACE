DELETE FROM `weenie` WHERE `class_Id` = 4200119;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200119, 'ace4200119-spectralspeartailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200119,   1,          1) /* ItemType - MeleeWeapon */
     , (4200119,   3,          2) /* PaletteTemplate - Blue */
     , (4200119,   5,          0) /* EncumbranceVal */
     , (4200119,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200119,  16,          1) /* ItemUseable - No */
     , (4200119,  18,          1) /* UiEffects - Magical */
     , (4200119,  19,         20) /* Value */
     , (4200119,  44,          1) /* Damage */
     , (4200119,  45,          8) /* DamageType - Cold */
     , (4200119,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200119,  47,          2) /* AttackType - Thrust */
     , (4200119,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200119,  49,         35) /* WeaponTime */
     , (4200119,  51,          1) /* CombatUse - Melee */
     , (4200119,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
	 , (4200119,  52,          1) /* ParentLocation - RightHand */
     , (4200119, 151,          2) /* HookType - Wall */
     , (4200119, 353,          11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200119,  22, True ) /* Inscribable */
     , (4200119,  69, False) /* IsSellable */
     , (4200119,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200119,   5,   -0.05) /* ManaRate */
     , (4200119,  21,       0) /* WeaponLength */
     , (4200119,  22,     0.2) /* DamageVariance */
     , (4200119,  26,       0) /* MaximumVelocity */
     , (4200119,  29,    1.15) /* WeaponDefense */
     , (4200119,  62,     1.2) /* WeaponOffense */
     , (4200119,  63,       1) /* DamageMod */
     , (4200119,  76,     0.7) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200119,   1, 'Spectral  Spear') /* Name */
     , (4200119,  16, 'A ghostly blue spear. Tendrils of ethereal light spill from it. This weapon won''t last long.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200119,   1, 0x020017FE) /* Setup */
     , (4200119,   3, 0x20000014) /* SoundTable */
     , (4200119,   7, 0x100003C8) /* ClothingBase */
     , (4200119,   8, 0x060026B8) /* Icon */
     , (4200119,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200119,  52, 0x060065FC) /* IconUnderlay */;

