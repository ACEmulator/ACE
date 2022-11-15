DELETE FROM `weenie` WHERE `class_Id` = 450312;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450312, 'lobowquidditytailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450312,   1,        256) /* ItemType - MissileWeapon */
     , (450312,   5,        0) /* EncumbranceVal */
     , (450312,   8,        140) /* Mass */
     , (450312,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450312,  16,          1) /* ItemUseable - No */
     , (450312,  18,          1) /* UiEffects - Magical */
     , (450312,  19,       20) /* Value */
     , (450312,  44,          0) /* Damage */
     , (450312,  46,         16) /* DefaultCombatStyle - Bow */
     , (450312,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450312,  49,         50) /* WeaponTime */
     , (450312,  50,          1) /* AmmoType - Arrow */
     , (450312,  51,          2) /* CombatUse - Missile */
     , (450312,  52,          2) /* ParentLocation - LeftHand */
     , (450312,  53,          3) /* PlacementPosition - LeftHand */
     , (450312,  60,        200) /* WeaponRange */
     , (450312,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450312, 150,        103) /* HookPlacement - Hook */
     , (450312, 151,          2) /* HookType - Wall */
     , (450312, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450312,  11, True ) /* IgnoreCollisions */
     , (450312,  13, True ) /* Ethereal */
     , (450312,  14, True ) /* GravityStatus */
     , (450312,  15, True ) /* LightsStatus */
     , (450312,  19, True ) /* Attackable */
     , (450312,  22, True ) /* Inscribable */
     , (450312,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450312,   5,  -0.025) /* ManaRate */
     , (450312,  21,       0) /* WeaponLength */
     , (450312,  22,       0) /* DamageVariance */
     , (450312,  26,    26.3) /* MaximumVelocity */
     , (450312,  29,       1) /* WeaponDefense */
     , (450312,  62,       1) /* WeaponOffense */
     , (450312,  63,    2.13) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450312,   1, 'Bow of the Quiddity') /* Name */
     , (450312,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (450312,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450312,   1, 0x02000A77) /* Setup */
     , (450312,   3, 0x20000014) /* SoundTable */
     , (450312,   8, 0x060020CE) /* Icon */
     , (450312,  22, 0x3400002B) /* PhysicsEffectTable */;

