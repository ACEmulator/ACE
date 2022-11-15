DELETE FROM `weenie` WHERE `class_Id` = 450317;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450317, 'lospearquidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450317,   1,          1) /* ItemType - MeleeWeapon */
     , (450317,   5,        600) /* EncumbranceVal */
     , (450317,   8,        140) /* Mass */
     , (450317,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450317,  16,          1) /* ItemUseable - No */
     , (450317,  18,          1) /* UiEffects - Magical */
     , (450317,  19,       2000) /* Value */
     , (450317,  44,         18) /* Damage */
     , (450317,  45,          2) /* DamageType - Pierce */
     , (450317,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450317,  47,          2) /* AttackType - Thrust */
     , (450317,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450317,  49,         30) /* WeaponTime */
     , (450317,  51,          1) /* CombatUse - Melee */
     , (450317,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450317, 150,        103) /* HookPlacement - Hook */
     , (450317, 151,          2) /* HookType - Wall */
     , (450317, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450317,  11, True ) /* IgnoreCollisions */
     , (450317,  13, True ) /* Ethereal */
     , (450317,  14, True ) /* GravityStatus */
     , (450317,  15, True ) /* LightsStatus */
     , (450317,  19, True ) /* Attackable */
     , (450317,  22, True ) /* Inscribable */
     , (450317,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450317,   5,  -0.025) /* ManaRate */
     , (450317,  21,     1.5) /* WeaponLength */
     , (450317,  22,     0.5) /* DamageVariance */
     , (450317,  26,       0) /* MaximumVelocity */
     , (450317,  29,    1.06) /* WeaponDefense */
     , (450317,  62,    1.06) /* WeaponOffense */
     , (450317,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450317,   1, 'Lance of the Quiddity') /* Name */
     , (450317,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (450317,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450317,   1, 0x02000A74) /* Setup */
     , (450317,   3, 0x20000014) /* SoundTable */
     , (450317,   8, 0x060020D2) /* Icon */
     , (450317,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450317,  36, 0x0E000014) /* MutateFilter */;


