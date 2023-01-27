DELETE FROM `weenie` WHERE `class_Id` = 450314;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450314, 'locrossbowquidditytailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450314,   1,        256) /* ItemType - MissileWeapon */
     , (450314,   5,        0) /* EncumbranceVal */
     , (450314,   8,        640) /* Mass */
     , (450314,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450314,  16,          1) /* ItemUseable - No */
     , (450314,  18,          1) /* UiEffects - Magical */
     , (450314,  19,       20) /* Value */
     , (450314,  44,          0) /* Damage */
     , (450314,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (450314,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450314,  49,        120) /* WeaponTime */
     , (450314,  50,          2) /* AmmoType - Bolt */
     , (450314,  51,          2) /* CombatUse - Missile */
     , (450314,  52,          2) /* ParentLocation - LeftHand */
     , (450314,  53,          3) /* PlacementPosition - LeftHand */
     , (450314,  60,        192) /* WeaponRange */
     , (450314,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450314, 150,        103) /* HookPlacement - Hook */
     , (450314, 151,          2) /* HookType - Wall */
     , (450314, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450314,  11, True ) /* IgnoreCollisions */
     , (450314,  13, True ) /* Ethereal */
     , (450314,  14, True ) /* GravityStatus */
     , (450314,  15, True ) /* LightsStatus */
     , (450314,  19, True ) /* Attackable */
     , (450314,  22, True ) /* Inscribable */
     , (450314,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450314,   5,  -0.025) /* ManaRate */
     , (450314,  21,       0) /* WeaponLength */
     , (450314,  22,       0) /* DamageVariance */
     , (450314,  26,    27.3) /* MaximumVelocity */
     , (450314,  29,       1) /* WeaponDefense */
     , (450314,  39,    1.25) /* DefaultScale */
     , (450314,  62,       1) /* WeaponOffense */
     , (450314,  63,     2.5) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450314,   1, 'Balister of the Quiddity') /* Name */
     , (450314,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (450314,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450314,   1, 0x02000A78) /* Setup */
     , (450314,   3, 0x20000014) /* SoundTable */
     , (450314,   8, 0x060020D5) /* Icon */
     , (450314,  22, 0x3400002B) /* PhysicsEffectTable */;

