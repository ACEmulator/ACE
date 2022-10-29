DELETE FROM `weenie` WHERE `class_Id` = 4200112;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200112, 'spearnobleburuntailorr', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200112,   1,          1) /* ItemType - MeleeWeapon */
     , (4200112,   5,          0) /* EncumbranceVal */
     , (4200112,   8,        150) /* Mass */
     , (4200112,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200112,  16,          1) /* ItemUseable - No */
     , (4200112,  18,         16) /* UiEffects - BoostStamina */
     , (4200112,  19,         20) /* Value */
     , (4200112,  44,          1) /* Damage */
     , (4200112,  45,          1) /* DamageType - Slash */
     , (4200112,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200112,  47,          4) /* AttackType - Slash */
     , (4200112,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200112,  49,         25) /* WeaponTime */
     , (4200112,  51,          1) /* CombatUse - Melee */
     , (4200112,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (4200112,  52,          1) /* ParentLocation - RightHand */
     , (4200112, 150,        103) /* HookPlacement - Hook */
     , (4200112, 151,          2) /* HookType - Wall */
     , (4200112, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200112,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200112,   5,  -0.033) /* ManaRate */
     , (4200112,  21,     1.3) /* WeaponLength */
     , (4200112,  22,     0.6) /* DamageVariance */
     , (4200112,  29,    1.09) /* WeaponDefense */
     , (4200112,  62,    1.09) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200112,   1, 'Burun Slaying Swordstaff') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200112,   1, 0x0200118E) /* Setup */
     , (4200112,   3, 0x20000014) /* SoundTable */
     , (4200112,   8, 0x06003576) /* Icon */
     , (4200112,  22, 0x3400002B) /* PhysicsEffectTable */;
