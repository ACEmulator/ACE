DELETE FROM `weenie` WHERE `class_Id` = 480504;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480504, 'swordsterilepk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480504,   1,          1) /* ItemType - MeleeWeapon */
     , (480504,   5,        0) /* EncumbranceVal */
     , (480504,   8,        220) /* Mass */
     , (480504,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480504,  16,          1) /* ItemUseable - No */
     , (480504,  18,         32) /* UiEffects - Fire */
     , (480504,  19,       20) /* Value */
     , (480504,  44,         0) /* Damage */
     , (480504,  45,         16) /* DamageType - Fire */
     , (480504,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480504,  47,          6) /* AttackType - Thrust, Slash */
     , (480504,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480504,  49,         35) /* WeaponTime */
     , (480504,  51,          1) /* CombatUse - Melee */
     , (480504,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480504, 150,        103) /* HookPlacement - Hook */
     , (480504, 151,          2) /* HookType - Wall */
     , (480504, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480504,  11, True ) /* IgnoreCollisions */
     , (480504,  13, True ) /* Ethereal */
     , (480504,  14, True ) /* GravityStatus */
     , (480504,  19, True ) /* Attackable */
     , (480504,  22, True ) /* Inscribable */
     , (480504,  23, True ) /* DestroyOnSell */
     , (480504,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480504,   5,   -0.05) /* ManaRate */
     , (480504,  21,    0.95) /* WeaponLength */
     , (480504,  22,     0.5) /* DamageVariance */
     , (480504,  26,       0) /* MaximumVelocity */
     , (480504,  39,     1.1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480504,   1, 'Sterile Sword') /* Name */
     , (480504,  16, 'This sword appears to be made from the withered flesh of some sort of creature.') /* LongDesc */
     , (480504,  33, 'WitheredAtollSword0105') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480504,   1, 0x020012AD) /* Setup */
     , (480504,   3, 0x20000014) /* SoundTable */
     , (480504,   8, 0x06003717) /* Icon */
     , (480504,  22, 0x3400002B) /* PhysicsEffectTable */;

