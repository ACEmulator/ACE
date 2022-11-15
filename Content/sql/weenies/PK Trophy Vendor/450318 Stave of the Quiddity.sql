DELETE FROM `weenie` WHERE `class_Id` = 450318;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450318, 'lostaffquidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450318,   1,          1) /* ItemType - MeleeWeapon */
     , (450318,   5,        0) /* EncumbranceVal */
     , (450318,   8,         90) /* Mass */
     , (450318,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450318,  16,          1) /* ItemUseable - No */
     , (450318,  18,          1) /* UiEffects - Magical */
     , (450318,  19,       20) /* Value */
     , (450318,  44,         0) /* Damage */
     , (450318,  45,          4) /* DamageType - Bludgeon */
     , (450318,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450318,  47,          6) /* AttackType - Thrust, Slash */
     , (450318,  48,         45) /* WeaponSkill - LightWeapons */
     , (450318,  49,         30) /* WeaponTime */
     , (450318,  51,          1) /* CombatUse - Melee */
     , (450318,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450318, 150,        103) /* HookPlacement - Hook */
     , (450318, 151,          2) /* HookType - Wall */
     , (450318, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450318,  11, True ) /* IgnoreCollisions */
     , (450318,  13, True ) /* Ethereal */
     , (450318,  14, True ) /* GravityStatus */
     , (450318,  15, True ) /* LightsStatus */
     , (450318,  19, True ) /* Attackable */
     , (450318,  22, True ) /* Inscribable */
     , (450318,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450318,   5,  -0.025) /* ManaRate */
     , (450318,  21,    1.33) /* WeaponLength */
     , (450318,  22,    0.33) /* DamageVariance */
     , (450318,  29,    1.05) /* WeaponDefense */
     , (450318,  39,    0.67) /* DefaultScale */
     , (450318,  62,    1.05) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450318,   1, 'Stave of the Quiddity') /* Name */
     , (450318,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (450318,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450318,   1, 0x02000A73) /* Setup */
     , (450318,   3, 0x20000014) /* SoundTable */
     , (450318,   8, 0x060020D3) /* Icon */
     , (450318,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450318,  36, 0x0E000014) /* MutateFilter */;

