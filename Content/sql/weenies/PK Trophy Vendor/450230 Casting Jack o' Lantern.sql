DELETE FROM `weenie` WHERE `class_Id` = 450230;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450230, 'ace450230-castingjackolanterntailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450230,   1,      32768) /* ItemType - Caster */
     , (450230,   3,         76) /* PaletteTemplate - Orange */
     , (450230,   5,         0) /* EncumbranceVal */
     , (450230,   9,   16777216) /* ValidLocations - Held */
     , (450230,  16,          1) /* ItemUseable - No */
     , (450230,  18,         32) /* UiEffects - Fire */
     , (450230,  19,        20) /* Value */
     , (450230,  46,        512) /* DefaultCombatStyle - Magic */
     , (450230,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450230,  94,         16) /* TargetType - Creature */
     , (450230, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450230,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450230,  12,   0.333) /* Shade */
     , (450230,  29,     1.1) /* WeaponDefense */
     , (450230,  39,    0.75) /* DefaultScale */
     , (450230, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450230,   1, 'Casting Jack o'' Lantern') /* Name */
     , (450230,  16, 'A small, heavy pumpkin, carved into a Jack o'' Lantern and swirling with magical energies.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450230,   1, 0x02001C0A) /* Setup */
     , (450230,   3, 0x20000014) /* SoundTable */
     , (450230,   6, 0x04001008) /* PaletteBase */
     , (450230,   7, 0x1000024C) /* ClothingBase */
     , (450230,   8, 0x06001E2C) /* Icon */
     , (450230,  22, 0x3400002B) /* PhysicsEffectTable */;
