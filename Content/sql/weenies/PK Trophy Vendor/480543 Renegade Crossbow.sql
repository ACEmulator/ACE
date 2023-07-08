DELETE FROM `weenie` WHERE `class_Id` = 480543;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480543, 'crossbowrenegaderaidspk', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480543,   1,        256) /* ItemType - MissileWeapon */
     , (480543,   5,       0) /* EncumbranceVal */
     , (480543,   8,        220) /* Mass */
     , (480543,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480543,  16,          1) /* ItemUseable - No */
     , (480543,  18,          1) /* UiEffects - Magical */
     , (480543,  19,       20) /* Value */
     , (480543,  44,          0) /* Damage */
     , (480543,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (480543,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480543,  49,         60) /* WeaponTime */
     , (480543,  50,          2) /* AmmoType - Bolt */
     , (480543,  51,          2) /* CombatUse - Missile */
     , (480543,  52,          2) /* ParentLocation - LeftHand */
     , (480543,  53,          3) /* PlacementPosition - LeftHand */
     , (480543,  60,        192) /* WeaponRange */
     , (480543,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (480543, 150,        103) /* HookPlacement - Hook */
     , (480543, 151,          2) /* HookType - Wall */
     , (480543, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480543,  11, True ) /* IgnoreCollisions */
     , (480543,  13, True ) /* Ethereal */
     , (480543,  14, True ) /* GravityStatus */
     , (480543,  15, True ) /* LightsStatus */
     , (480543,  19, True ) /* Attackable */
     , (480543,  22, True ) /* Inscribable */
     , (480543,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480543,   5,   -0.05) /* ManaRate */
     , (480543,  21,    0.75) /* WeaponLength */
     , (480543,  39,     1.2) /* DefaultScale */
     , (480543,  77,       1) /* PhysicsScriptIntensity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480543,   1, 'Renegade Crossbow') /* Name */
     , (480543,  16, 'Picked up from a defeated Renegade Tumerok Commander') /* LongDesc */
     , (480543,  33, 'GotCrossBowRenegadeRaids') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480543,   1, 0x02000F69) /* Setup */
     , (480543,   3, 0x20000014) /* SoundTable */
     , (480543,   6, 0x04000BEF) /* PaletteBase */
     , (480543,   8, 0x06002B5A) /* Icon */
     , (480543,  19, 0x00000058) /* ActivationAnimation */
     , (480543,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480543,  30,         87) /* PhysicsScript - BreatheLightning */;
