DELETE FROM `weenie` WHERE `class_Id` = 450319;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450319, 'loswordquidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450319,   1,          1) /* ItemType - MeleeWeapon */
     , (450319,   5,        0) /* EncumbranceVal */
     , (450319,   8,        220) /* Mass */
     , (450319,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450319,  16,          1) /* ItemUseable - No */
     , (450319,  18,          1) /* UiEffects - Magical */
     , (450319,  19,       20) /* Value */
     , (450319,  44,         0) /* Damage */
     , (450319,  45,          3) /* DamageType - Slash, Pierce */
     , (450319,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450319,  47,          6) /* AttackType - Thrust, Slash */
     , (450319,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450319,  49,         25) /* WeaponTime */
     , (450319,  51,          1) /* CombatUse - Melee */
     , (450319,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450319, 150,        103) /* HookPlacement - Hook */
     , (450319, 151,          2) /* HookType - Wall */
     , (450319, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450319,  11, True ) /* IgnoreCollisions */
     , (450319,  13, True ) /* Ethereal */
     , (450319,  14, True ) /* GravityStatus */
     , (450319,  15, True ) /* LightsStatus */
     , (450319,  19, True ) /* Attackable */
     , (450319,  22, True ) /* Inscribable */
     , (450319,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450319,   5,  -0.025) /* ManaRate */
     , (450319,  21,    0.95) /* WeaponLength */
     , (450319,  22,     0.5) /* DamageVariance */
     , (450319,  26,       0) /* MaximumVelocity */
     , (450319,  29,    1.06) /* WeaponDefense */
     , (450319,  39,     1.1) /* DefaultScale */
     , (450319,  62,    1.06) /* WeaponOffense */
     , (450319,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450319,   1, 'Blade of the Quiddity') /* Name */
     , (450319,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450319,   1, 0x02000A72) /* Setup */
     , (450319,   3, 0x20000014) /* SoundTable */
     , (450319,   8, 0x060020D4) /* Icon */
     , (450319,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450319,  36, 0x0E000014) /* MutateFilter */;

