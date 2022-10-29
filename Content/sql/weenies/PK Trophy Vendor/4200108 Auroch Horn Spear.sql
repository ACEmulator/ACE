DELETE FROM `weenie` WHERE `class_Id` = 4200108;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200108, 'spearaurochhorntailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200108,   1,          1) /* ItemType - MeleeWeapon */
     , (4200108,   5,          0) /* EncumbranceVal */
     , (4200108,   8,        110) /* Mass */
     , (4200108,   9,    33554432) /* ValidLocations - MeleeWeapon */
     , (4200108,  16,          1) /* ItemUseable - No */
     , (4200108,  18,         64) /* UiEffects - Lightning */
     , (4200108,  19,         20) /* Value */
     , (4200108,  44,          1) /* Damage */
     , (4200108,  45,         64) /* DamageType - Electric */
     , (4200108,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200108,  47,          2) /* AttackType - Thrust */
     , (4200108,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200108,  49,         20) /* WeaponTime */
     , (4200108,  51,          1) /* CombatUse - Melee */
	 , (4200108,  52,          1) /* ParentLocation - RightHand */
     , (4200108,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200108, 150,        103) /* HookPlacement - Hook */
     , (4200108, 151,          2) /* HookType - Wall */
     , (4200108, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200108,  11, True ) /* IgnoreCollisions */
     , (4200108,  13, True ) /* Ethereal */
     , (4200108,  14, True ) /* GravityStatus */
     , (4200108,  19, True ) /* Attackable */
     , (4200108,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200108,  21,     1.5) /* WeaponLength */
     , (4200108,  22,    0.75) /* DamageVariance */
     , (4200108,  26,       0) /* MaximumVelocity */
     , (4200108,  29,    1.05) /* WeaponDefense */
     , (4200108,  62,    1.03) /* WeaponOffense */
     , (4200108,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200108,   1, 'Auroch Horn Spear') /* Name */
     , (4200108,  16, 'A spear made from the horn of an auroch. Mysterious electrical impulses flare along the shaft of the weapon.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200108,   1, 0x0200054D) /* Setup */
     , (4200108,   3, 0x20000014) /* SoundTable */
     , (4200108,   8, 0x060010D9) /* Icon */
     , (4200108,  22, 0x3400002B) /* PhysicsEffectTable */;
