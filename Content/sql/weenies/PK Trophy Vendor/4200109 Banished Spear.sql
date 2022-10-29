DELETE FROM `weenie` WHERE `class_Id` = 4200109;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200109, 'spearbanishedtailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200109,   1,          1) /* ItemType - MeleeWeapon */
     , (4200109,   5,          0) /* EncumbranceVal */
     , (4200109,   8,        340) /* Mass */
     , (4200109,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200109,  16,          1) /* ItemUseable - No */
     , (4200109,  19,         20) /* Value */
     , (4200109,  44,          1) /* Damage */
     , (4200109,  45,         64) /* DamageType - Electric */
     , (4200109,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200109,  47,          2) /* AttackType - Thrust */
     , (4200109,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200109,  49,         30) /* WeaponTime */
     , (4200109,  51,          1) /* CombatUse - Melee */
     , (4200109,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200109, 150,        103) /* HookPlacement - Hook */
     , (4200109, 151,          2) /* HookType - Wall */
	 , (4200109,  52,          1) /* ParentLocation - RightHand */
     , (4200109, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200109,  11, True ) /* IgnoreCollisions */
     , (4200109,  13, True ) /* Ethereal */
     , (4200109,  14, True ) /* GravityStatus */
     , (4200109,  19, True ) /* Attackable */
     , (4200109,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200109,   5,  -0.033) /* ManaRate */
     , (4200109,  21,    0.95) /* WeaponLength */
     , (4200109,  22,     0.5) /* DamageVariance */
     , (4200109,  26,       0) /* MaximumVelocity */
     , (4200109,  29,    1.06) /* WeaponDefense */
     , (4200109,  39,       1) /* DefaultScale */
     , (4200109,  62,    1.06) /* WeaponOffense */
     , (4200109,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200109,   1, 'Banished Spear') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200109,   1, 0x020012DB) /* Setup */
     , (4200109,   3, 0x20000014) /* SoundTable */
     , (4200109,   8, 0x0600376F) /* Icon */
     , (4200109,  22, 0x3400002B) /* PhysicsEffectTable */;
