DELETE FROM `weenie` WHERE `class_Id` = 480574;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480574, 'axeinsensatepk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480574,   1,          1) /* ItemType - MeleeWeapon */
     , (480574,   5,        0) /* EncumbranceVal */
     , (480574,   8,        320) /* Mass */
     , (480574,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480574,  16,          1) /* ItemUseable - No */
     , (480574,  18,         32) /* UiEffects - Fire */
     , (480574,  19,       20) /* Value */
     , (480574,  44,         0) /* Damage */
     , (480574,  45,         16) /* DamageType - Fire */
     , (480574,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480574,  47,          4) /* AttackType - Slash */
     , (480574,  48,         45) /* WeaponSkill - LightWeapons */
     , (480574,  49,         60) /* WeaponTime */
     , (480574,  51,          1) /* CombatUse - Melee */
     , (480574,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480574, 114,          1) /* Attuned - Attuned */
     , (480574, 150,        103) /* HookPlacement - Hook */
     , (480574, 151,          2) /* HookType - Wall */
     , (480574, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480574,  11, True ) /* IgnoreCollisions */
     , (480574,  13, True ) /* Ethereal */
     , (480574,  14, True ) /* GravityStatus */
     , (480574,  19, True ) /* Attackable */
     , (480574,  22, True ) /* Inscribable */
     , (480574,  23, True ) /* DestroyOnSell */
     , (480574,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480574,   5,   -0.05) /* ManaRate */
     , (480574,  21,    0.75) /* WeaponLength */
     , (480574,  22,     0.5) /* DamageVariance */
     , (480574,  26,       0) /* MaximumVelocity */
     , (480574,  77,       1) /* PhysicsScriptIntensity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480574,   1, 'Insensate Axe') /* Name */
     , (480574,  16, 'This axe appears to be made from the withered flesh of some sort of creature.') /* LongDesc */
     , (480574,  33, 'WitheredAtollAxe0105') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480574,   1, 0x020012AF) /* Setup */
     , (480574,   3, 0x20000014) /* SoundTable */
     , (480574,   8, 0x06003718) /* Icon */
     , (480574,  19, 0x00000058) /* ActivationAnimation */
     , (480574,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480574,  30,         87) /* PhysicsScript - BreatheLightning */;
