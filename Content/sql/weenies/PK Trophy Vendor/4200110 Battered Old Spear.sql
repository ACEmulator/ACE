DELETE FROM `weenie` WHERE `class_Id` = 4200110;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200110, 'ace4200110-batteredoldspeartailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200110,   1,          1) /* ItemType - MeleeWeapon */
     , (4200110,   5,          0) /* EncumbranceVal */
     , (4200110,   8,        140) /* Mass */
     , (4200110,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200110,  16,          1) /* ItemUseable - No */
     , (4200110,  19,         20) /* Value */
     , (4200110,  44,          1) /* Damage */
     , (4200110,  45,          2) /* DamageType - Pierce */
     , (4200110,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200110,  47,          2) /* AttackType - Thrust */
     , (4200110,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200110,  49,         30) /* WeaponTime */
     , (4200110,  51,          1) /* CombatUse - Melee */
	 , (4200110,  52,          1) /* ParentLocation - RightHand */
     , (4200110,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200110, 150,        103) /* HookPlacement - Hook */
     , (4200110, 151,          2) /* HookType - Wall */
     , (4200110, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200110,  11, True ) /* IgnoreCollisions */
     , (4200110,  13, True ) /* Ethereal */
     , (4200110,  14, True ) /* GravityStatus */
     , (4200110,  19, True ) /* Attackable */
     , (4200110,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200110,   5,  -0.025) /* ManaRate */
     , (4200110,  21,     1.5) /* WeaponLength */
     , (4200110,  22,    0.45) /* DamageVariance */
     , (4200110,  26,       0) /* MaximumVelocity */
     , (4200110,  29,    1.05) /* WeaponDefense */
     , (4200110,  62,    1.05) /* WeaponOffense */
     , (4200110,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200110,   1, 'Battered Old Spear') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200110,   1, 0x02001423) /* Setup */
     , (4200110,   3, 0x20000014) /* SoundTable */
     , (4200110,   8, 0x06005FF9) /* Icon */
     , (4200110,  22, 0x3400002B) /* PhysicsEffectTable */;

