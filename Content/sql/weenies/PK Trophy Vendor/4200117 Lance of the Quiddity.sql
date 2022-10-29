DELETE FROM `weenie` WHERE `class_Id` = 4200117;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200117, 'tumerokspearfalcon-creatureonlytailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200117,   1,          1) /* ItemType - MeleeWeapon */
     , (4200117,   5,          0) /* EncumbranceVal */
     , (4200117,   8,        140) /* Mass */
     , (4200117,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200117,  16,          1) /* ItemUseable - No */
     , (4200117,  19,         20) /* Value */
     , (4200117,  44,          1) /* Damage */
     , (4200117,  45,          8) /* DamageType - Cold */
     , (4200117,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200117,  47,          2) /* AttackType - Thrust */
     , (4200117,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200117,  49,         30) /* WeaponTime */
     , (4200117,  51,          1) /* CombatUse - Melee */
     , (4200117,  52,          1) /* ParentLocation - RightHand */
	 , (4200117, 353,         11) /* WeaponType - TwoHanded */
     , (4200117,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200117,  15, True ) /* LightsStatus */
     , (4200117,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200117,   5,  -0.025) /* ManaRate */
     , (4200117,  21,     1.5) /* WeaponLength */
     , (4200117,  22,     0.5) /* DamageVariance */
     , (4200117,  29,    1.06) /* WeaponDefense */
     , (4200117,  62,    1.06) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200117,   1, 'Lance of the Quiddity') /* Name */
     , (4200117,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (4200117,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200117,   1, 0x02000A74) /* Setup */
     , (4200117,   3, 0x20000014) /* SoundTable */
     , (4200117,   8, 0x060020D2) /* Icon */
     , (4200117,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200117,  36, 0x0E000014) /* MutateFilter */;

