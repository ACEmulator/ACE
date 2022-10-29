DELETE FROM `weenie` WHERE `class_Id` = 4200116;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200116, 'spearkreergtailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200116,   1,          1) /* ItemType - MeleeWeapon */
     , (4200116,   5,          0) /* EncumbranceVal */
     , (4200116,   8,        500) /* Mass */
     , (4200116,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200116,  16,          1) /* ItemUseable - No */
     , (4200116,  18,          1) /* UiEffects - Magical */
     , (4200116,  19,         20) /* Value */
     , (4200116,  44,          1) /* Damage */
     , (4200116,  45,         32) /* DamageType - Acid */
     , (4200116,  46,          8) /* DefaultCombatStyle - TwoHandedCombat */
     , (4200116,  47,          2) /* AttackType - Thrust */
     , (4200116,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200116,  49,         25) /* WeaponTime */
     , (4200116,  51,          1) /* CombatUse - Melee */
     , (4200116,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (4200116,  52,          1) /* ParentLocation - RightHand */
     , (4200116, 150,        103) /* HookPlacement - Hook */
     , (4200116, 151,          2) /* HookType - Wall */
     , (4200116, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200116,  11, True ) /* IgnoreCollisions */
     , (4200116,  13, True ) /* Ethereal */
     , (4200116,  14, True ) /* GravityStatus */
     , (4200116,  19, True ) /* Attackable */
     , (4200116,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200116,   5,  -0.033) /* ManaRate */
     , (4200116,  21,     1.3) /* WeaponLength */
     , (4200116,  22,    0.25) /* DamageVariance */
     , (4200116,  26,       0) /* MaximumVelocity */
     , (4200116,  29,    1.02) /* WeaponDefense */
     , (4200116,  62,    1.02) /* WeaponOffense */
     , (4200116,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200116,   1, 'Kreerg''s Spear') /* Name */
     , (4200116,  16, 'A spear modeled after the Mosswart hero Kreerg''s. A small stamp on the shaft reads: A Ketnan Product.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200116,   1, 0x02001110) /* Setup */
     , (4200116,   3, 0x20000014) /* SoundTable */
     , (4200116,   8, 0x0600340B) /* Icon */
     , (4200116,  22, 0x3400002B) /* PhysicsEffectTable */;

