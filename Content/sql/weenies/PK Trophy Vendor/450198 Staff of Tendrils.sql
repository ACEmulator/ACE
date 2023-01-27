DELETE FROM `weenie` WHERE `class_Id` = 450198;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450198, 'staffraretendrilstailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450198,   1,          1) /* ItemType - MeleeWeapon */
     , (450198,   5,        0) /* EncumbranceVal */
     , (450198,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450198,  16,          1) /* ItemUseable - No */
     , (450198,  19,      20) /* Value */
     , (450198,  44,         0) /* Damage */
     , (450198,  45,          4) /* DamageType - Bludgeon */
     , (450198,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450198,  47,          6) /* AttackType - Thrust, Slash */
     , (450198,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450198,  49,         30) /* WeaponTime */
     , (450198,  51,          1) /* CombatUse - Melee */
     , (450198,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450198, 151,          2) /* HookType - Wall */
     , (450198, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450198,  11, True ) /* IgnoreCollisions */
     , (450198,  13, True ) /* Ethereal */
     , (450198,  14, True ) /* GravityStatus */
     , (450198,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450198,   5,  -0.033) /* ManaRate */
     , (450198,  21,       0) /* WeaponLength */
     , (450198,  22,     0.2) /* DamageVariance */
     , (450198,  26,       0) /* MaximumVelocity */
     , (450198,  29,    1.18) /* WeaponDefense */
     , (450198,  39,    0.67) /* DefaultScale */
     , (450198,  62,    1.18) /* WeaponOffense */
     , (450198,  63,       1) /* DamageMod */
     , (450198, 147,    0.33) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450198,   1, 'Staff of Tendrils') /* Name */
     , (450198,  16, 'This stout wooden staff is shod with iron on both ends. Intricate designs of vines adorn the staff, making it both functional as well as attractive. This staff seems to feed off of the energy of its user, which is not uncommon among the weapons of the Milantans.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450198,   1, 0x02001360) /* Setup */
     , (450198,   3, 0x20000014) /* SoundTable */
     , (450198,   6, 0x04000BEF) /* PaletteBase */
     , (450198,   8, 0x06005BAF) /* Icon */
     , (450198,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450198,  52, 0x06005B0C) /* IconUnderlay */;
