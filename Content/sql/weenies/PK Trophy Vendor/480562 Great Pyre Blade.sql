DELETE FROM `weenie` WHERE `class_Id` = 480562;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480562, 'ace480562-greatpyrebladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480562,   1,          1) /* ItemType - MeleeWeapon */
     , (480562,   3,          2) /* PaletteTemplate - Blue */
     , (480562,   5,        0) /* EncumbranceVal */
     , (480562,   9,   33554432) /* ValidLocations - TwoHanded */
     , (480562,  16,          1) /* ItemUseable - No */
     , (480562,  18,         32) /* UiEffects - Fire */
     , (480562,  19,        20) /* Value */
     , (480562,  44,         0) /* Damage */
     , (480562,  45,         16) /* DamageType - Fire */
     , (480562,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (480562,  47,          4) /* AttackType - Slash */
     , (480562,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (480562,  49,         40) /* WeaponTime */
     , (480562,  51,          5) /* CombatUse - TwoHanded */
     , (480562,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480562, 292,          2) /* Cleaving */
     , (480562, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480562,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480562,  21,    1.25) /* WeaponLength */
     , (480562,  22,    0.15) /* DamageVariance */
     , (480562,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480562,   1, 'Great Pyre Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480562,   1, 0x020018AA) /* Setup */
     , (480562,   3, 0x20000014) /* SoundTable */
     , (480562,   6, 0x04001A25) /* PaletteBase */
     , (480562,   7, 0x10000764) /* ClothingBase */
     , (480562,   8, 0x06006B48) /* Icon */
     , (480562,  22, 0x3400002B) /* PhysicsEffectTable */;
