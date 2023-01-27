DELETE FROM `weenie` WHERE `class_Id` = 450311;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450311, 'loaxequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450311,   1,          1) /* ItemType - MeleeWeapon */
     , (450311,   5,        0) /* EncumbranceVal */
     , (450311,   8,        320) /* Mass */
     , (450311,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450311,  16,          1) /* ItemUseable - No */
     , (450311,  18,          1) /* UiEffects - Magical */
     , (450311,  19,       20) /* Value */
     , (450311,  44,         0) /* Damage */
     , (450311,  45,          1) /* DamageType - Slash */
     , (450311,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450311,  47,          4) /* AttackType - Slash */
     , (450311,  48,         45) /* WeaponSkill - LightWeapons */
     , (450311,  49,         60) /* WeaponTime */
     , (450311,  51,          1) /* CombatUse - Melee */
     , (450311,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450311, 150,        103) /* HookPlacement - Hook */
     , (450311, 151,          2) /* HookType - Wall */
     , (450311, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450311,  11, True ) /* IgnoreCollisions */
     , (450311,  13, True ) /* Ethereal */
     , (450311,  14, True ) /* GravityStatus */
     , (450311,  15, True ) /* LightsStatus */
     , (450311,  19, True ) /* Attackable */
     , (450311,  22, True ) /* Inscribable */
     , (450311,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450311,   5,  -0.025) /* ManaRate */
     , (450311,  21,    0.75) /* WeaponLength */
     , (450311,  22,     0.5) /* DamageVariance */
     , (450311,  29,    1.05) /* WeaponDefense */
     , (450311,  62,    1.07) /* WeaponOffense */
     , (450311,  77,       1) /* PhysicsScriptIntensity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450311,   1, 'Axe of the Quiddity') /* Name */
     , (450311,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (450311,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450311,   1, 0x02000A70) /* Setup */
     , (450311,   3, 0x20000014) /* SoundTable */
     , (450311,   8, 0x060020CD) /* Icon */
     , (450311,  19, 0x00000058) /* ActivationAnimation */
     , (450311,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450311,  30,         87) /* PhysicsScript - BreatheLightning */
     , (450311,  36, 0x0E000014) /* MutateFilter */;

