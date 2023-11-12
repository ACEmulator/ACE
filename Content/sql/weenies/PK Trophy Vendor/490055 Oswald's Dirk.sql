DELETE FROM `weenie` WHERE `class_Id` = 490055;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490055, 'dirkoswaldpk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490055,   1,          1) /* ItemType - MeleeWeapon */
     , (490055,   5,        150) /* EncumbranceVal */
     , (490055,   8,         90) /* Mass */
     , (490055,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (490055,  16,          1) /* ItemUseable - No */
     , (490055,  19,       20) /* Value */
     , (490055,  44,         0) /* Damage */
     , (490055,  45,          3) /* DamageType - Slash, Pierce */
     , (490055,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (490055,  47,        66) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (490055,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (490055,  49,         40) /* WeaponTime */
     , (490055,  51,          1) /* CombatUse - Melee */
     , (490055,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490055, 150,        103) /* HookPlacement - Hook */
     , (490055, 151,          2) /* HookType - Wall */
     , (490055, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490055,  11, True ) /* IgnoreCollisions */
     , (490055,  13, True ) /* Ethereal */
     , (490055,  14, True ) /* GravityStatus */
     , (490055,  19, True ) /* Attackable */
     , (490055,  22, True ) /* Inscribable */
     , (490055,  23, True ) /* DestroyOnSell */
     , (490055,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490055,   5,  -0.025) /* ManaRate */
     , (490055,  21,     0.4) /* WeaponLength */
     , (490055,  22,     0.6) /* DamageVariance */
     , (490055,  26,       0) /* MaximumVelocity */
     , (490055,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490055,   1, 'Oswald''s Dirk') /* Name */
     , (490055,  15, 'A very sharp and light dirk capable of multiple blows at once.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490055,   1, 0x02000F35) /* Setup */
     , (490055,   3, 0x20000014) /* SoundTable */
     , (490055,   8, 0x06002AEF) /* Icon */
     , (490055,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490055,  36, 0x0E000014) /* MutateFilter */;

