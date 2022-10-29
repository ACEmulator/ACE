DELETE FROM `weenie` WHERE `class_Id` = 4200113;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200113, 'spearrarechampionsdemisetailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200113,   1,          1) /* ItemType - MeleeWeapon */
     , (4200113,   5,          0) /* EncumbranceVal */
     , (4200113,   8,         90) /* Mass */
     , (4200113,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200113,  16,          1) /* ItemUseable - No */
     , (4200113,  17,        202) /* RareId */
     , (4200113,  18,        256) /* UiEffects - Acid */
     , (4200113,  19,         20) /* Value */
     , (4200113,  44,          1) /* Damage */
     , (4200113,  45,         32) /* DamageType - Acid */
     , (4200113,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200113,  47,          2) /* AttackType - Thrust */
     , (4200113,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200113,  49,         40) /* WeaponTime */
     , (4200113,  51,          1) /* CombatUse - Melee */
     , (4200113,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (4200113,  52,          1) /* ParentLocation - RightHand */
     , (4200113, 151,          2) /* HookType - Wall */
     , (4200113, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200113,  11, True ) /* IgnoreCollisions */
     , (4200113,  13, True ) /* Ethereal */
     , (4200113,  14, True ) /* GravityStatus */
     , (4200113,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200113,   5,  -0.033) /* ManaRate */
     , (4200113,  21,       0) /* WeaponLength */
     , (4200113,  22,   0.184) /* DamageVariance */
     , (4200113,  26,       0) /* MaximumVelocity */
     , (4200113,  29,    1.18) /* WeaponDefense */
     , (4200113,  39,     1.1) /* DefaultScale */
     , (4200113,  62,    1.18) /* WeaponOffense */
     , (4200113,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200113,   1, 'Champion''s Demise') /* Name */
     , (4200113,  16, 'You''re a piece of shit.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200113,   1, 0x02001357) /* Setup */
     , (4200113,   3, 0x20000014) /* SoundTable */
     , (4200113,   6, 0x04000BEF) /* PaletteBase */
     , (4200113,   7, 0x10000860) /* ClothingBase */
     , (4200113,   8, 0x06005B9D) /* Icon */
     , (4200113,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200113,  36, 0x0E000012) /* MutateFilter */
     , (4200113,  46, 0x38000032) /* TsysMutationFilter */
     , (4200113,  52, 0x06005B0C) /* IconUnderlay */;

