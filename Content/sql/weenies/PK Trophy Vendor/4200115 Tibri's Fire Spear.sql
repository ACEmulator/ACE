DELETE FROM `weenie` WHERE `class_Id` = 4200115;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200115, 'tibrisfirespeartailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200115,   1,          1) /* ItemType - MeleeWeapon */
     , (4200115,   3,         20) /* PaletteTemplate - Silver */
     , (4200115,   5,          0) /* EncumbranceVal */
     , (4200115,   8,        140) /* Mass */
     , (4200115,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200115,  16,          1) /* ItemUseable - No */
     , (4200115,  18,         32) /* UiEffects - Fire */
     , (4200115,  19,         20) /* Value */
     , (4200115,  44,          1) /* Damage */
     , (4200115,  45,         16) /* DamageType - Fire */
     , (4200115,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200115,  47,          2) /* AttackType - Thrust */
     , (4200115,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200115,  49,         30) /* WeaponTime */
     , (4200115,  51,          1) /* CombatUse - Melee */
     , (4200115,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (4200115,  52,          1) /* ParentLocation - RightHand */
     , (4200115, 150,        103) /* HookPlacement - Hook */
     , (4200115, 151,          2) /* HookType - Wall */
     , (4200115, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200115,  11, True ) /* IgnoreCollisions */
     , (4200115,  13, True ) /* Ethereal */
     , (4200115,  14, True ) /* GravityStatus */
     , (4200115,  19, True ) /* Attackable */
     , (4200115,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200115,   5,  -0.033) /* ManaRate */
     , (4200115,  21,     1.5) /* WeaponLength */
     , (4200115,  22,    0.75) /* DamageVariance */
     , (4200115,  26,       0) /* MaximumVelocity */
     , (4200115,  29,    1.04) /* WeaponDefense */
     , (4200115,  62,    1.04) /* WeaponOffense */
	 , (4200115,  76,     0.7) /* Translucency */
     , (4200115,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200115,   1, 'Tibri''s Fire Spear') /* Name */
     , (4200115,  16, 'Tibri''s flaming spear does fire damage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200115,   1, 0x020003D4) /* Setup */
     , (4200115,   6, 0x04000BEF) /* PaletteBase */
     , (4200115,   8, 0x060010D9) /* Icon */
     , (4200115,  22, 0x3400002B) /* PhysicsEffectTable */;
