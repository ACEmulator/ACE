DELETE FROM `weenie` WHERE `class_Id` = 450229;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450229, 'ace450229-castingjackolanterntailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450229,   1,      32768) /* ItemType - Caster */
     , (450229,   3,         76) /* PaletteTemplate - Orange */
     , (450229,   5,         0) /* EncumbranceVal */
     , (450229,   9,   16777216) /* ValidLocations - Held */
     , (450229,  16,          1) /* ItemUseable - No */
     , (450229,  19,        20) /* Value */
     , (450229,  46,        512) /* DefaultCombatStyle - Magic */
     , (450229,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450229,  94,         16) /* TargetType - Creature */
     , (450229, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450229,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450229,  12,   0.333) /* Shade */
     , (450229,  29,     1.1) /* WeaponDefense */
     , (450229,  39,    0.75) /* DefaultScale */
     , (450229, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450229,   1, 'Casting Jack o'' Lantern') /* Name */
     , (450229,  16, 'A small, heavy pumpkin, carved into a Jack o'' Lantern and swirling with magical energies.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450229,   1, 0x02001C09) /* Setup */
     , (450229,   3, 0x20000014) /* SoundTable */
     , (450229,   6, 0x04001008) /* PaletteBase */
     , (450229,   7, 0x1000024C) /* ClothingBase */
     , (450229,   8, 0x06001E2C) /* Icon */
     , (450229,  22, 0x3400002B) /* PhysicsEffectTable */;
