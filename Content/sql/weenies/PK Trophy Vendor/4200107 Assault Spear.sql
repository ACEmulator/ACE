DELETE FROM `weenie` WHERE `class_Id` = 4200107;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200107, 'speartumerokwartailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200107,   1,          1) /* ItemType - MeleeWeapon */
     , (4200107,   3,         14) /* PaletteTemplate - Red */
     , (4200107,   5,          0) /* EncumbranceVal */
     , (4200107,   8,        140) /* Mass */
     , (4200107,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200107,  16,          1) /* ItemUseable - No */
     , (4200107,  19,         20) /* Value */
     , (4200107,  44,          1) /* Damage */
     , (4200107,  45,          2) /* DamageType - Pierce */
     , (4200107,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200107,  47,          2) /* AttackType - Thrust */
     , (4200107,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200107,  49,         25) /* WeaponTime */
     , (4200107,  51,          1) /* CombatUse - Melee */
	 , (4200107,  52,          1) /* ParentLocation - RightHand */
     , (4200107, 150,        103) /* HookPlacement - Hook */
     , (4200107, 151,          2) /* HookType - Wall */
	 , (4200107, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200107,  15, True ) /* LightsStatus */
     , (4200107,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200107,   5,  -0.025) /* ManaRate */
     , (4200107,  21,     1.5) /* WeaponLength */
     , (4200107,  22,    0.95) /* DamageVariance */
     , (4200107,  29,    1.06) /* WeaponDefense */
     , (4200107,  39,     1.2) /* DefaultScale */
     , (4200107,  62,    1.06) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200107,   1, 'Assault Spear') /* Name */
     , (4200107,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200107,   1, 0x02000AD5) /* Setup */
     , (4200107,   3, 0x20000014) /* SoundTable */
     , (4200107,   6, 0x04000BEF) /* PaletteBase */
     , (4200107,   7, 0x100002E7) /* ClothingBase */
     , (4200107,   8, 0x06002103) /* Icon */
     , (4200107,  22, 0x3400002B) /* PhysicsEffectTable */

