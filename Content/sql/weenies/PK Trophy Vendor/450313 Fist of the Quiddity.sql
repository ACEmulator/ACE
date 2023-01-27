DELETE FROM `weenie` WHERE `class_Id` = 450313;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450313, 'locestusquidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450313,   1,          1) /* ItemType - MeleeWeapon */
     , (450313,   5,        0) /* EncumbranceVal */
     , (450313,   8,         90) /* Mass */
     , (450313,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450313,  16,          1) /* ItemUseable - No */
     , (450313,  18,          1) /* UiEffects - Magical */
     , (450313,  19,       20) /* Value */
     , (450313,  44,         0) /* Damage */
     , (450313,  45,          4) /* DamageType - Bludgeon */
     , (450313,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450313,  47,          1) /* AttackType - Punch */
     , (450313,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450313,  49,         20) /* WeaponTime */
     , (450313,  51,          1) /* CombatUse - Melee */
     , (450313,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450313, 150,        103) /* HookPlacement - Hook */
     , (450313, 151,          2) /* HookType - Wall */
     , (450313, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450313,  11, True ) /* IgnoreCollisions */
     , (450313,  13, True ) /* Ethereal */
     , (450313,  14, True ) /* GravityStatus */
     , (450313,  15, True ) /* LightsStatus */
     , (450313,  19, True ) /* Attackable */
     , (450313,  22, True ) /* Inscribable */
     , (450313,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450313,   5,  -0.025) /* ManaRate */
     , (450313,  21,    0.52) /* WeaponLength */
     , (450313,  22,    0.75) /* DamageVariance */
     , (450313,  26,       0) /* MaximumVelocity */
     , (450313,  29,    1.08) /* WeaponDefense */
     , (450313,  39,     0.8) /* DefaultScale */
     , (450313,  62,    1.07) /* WeaponOffense */
     , (450313,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450313,   1, 'Fist of the Quiddity') /* Name */
     , (450313,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (450313,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450313,   1, 0x02000A75) /* Setup */
     , (450313,   3, 0x20000014) /* SoundTable */
     , (450313,   8, 0x060020CF) /* Icon */
     , (450313,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450313,  36, 0x0E000014) /* MutateFilter */;


