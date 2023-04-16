DELETE FROM `weenie` WHERE `class_Id` = 480561;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480561, 'ace480561-thefistofbellenessepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480561,   1,          1) /* ItemType - MeleeWeapon */
     , (480561,   5,       0) /* EncumbranceVal */
     , (480561,   8,       2080) /* Mass */
     , (480561,   9,   33554432) /* ValidLocations - TwoHanded */
     , (480561,  16,          1) /* ItemUseable - No */
     , (480561,  19,        20) /* Value */
     , (480561,  44,         0) /* Damage */
     , (480561,  45,          4) /* DamageType - Bludgeon */
     , (480561,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (480561,  47,          4) /* AttackType - Slash */
     , (480561,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (480561,  49,          1) /* WeaponTime */
     , (480561,  51,          5) /* CombatUse - TwoHanded */
     , (480561,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480561, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480561,  11, True ) /* IgnoreCollisions */
     , (480561,  13, True ) /* Ethereal */
     , (480561,  14, True ) /* GravityStatus */
     , (480561,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480561,  21,    1.24) /* WeaponLength */
     , (480561,  22,     0.5) /* DamageVariance */
     , (480561,  39,     1.6) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480561,   1, 'The Fist of Bellenesse') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480561,   1, 0x0200133F) /* Setup */
     , (480561,   3, 0x20000014) /* SoundTable */
     , (480561,   6, 0x04001F21) /* PaletteBase */
     , (480561,   8, 0x06006B5E) /* Icon */
     , (480561,  22, 0x3400002B) /* PhysicsEffectTable */;
