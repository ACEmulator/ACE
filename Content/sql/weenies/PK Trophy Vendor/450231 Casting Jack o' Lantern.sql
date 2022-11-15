DELETE FROM `weenie` WHERE `class_Id` = 450231;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450231, 'ace450231-castingjackolanterntailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450231,   1,      32768) /* ItemType - Caster */
     , (450231,   3,         76) /* PaletteTemplate - Orange */
     , (450231,   5,         0) /* EncumbranceVal */
     , (450231,   9,   16777216) /* ValidLocations - Held */
     , (450231,  16,          1) /* ItemUseable - No */
     , (450231,  18,       1024) /* UiEffects - Slashing */
     , (450231,  19,        20) /* Value */
     , (450231,  46,        512) /* DefaultCombatStyle - Magic */
     , (450231,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450231,  94,         16) /* TargetType - Creature */
     , (450231, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450231,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450231,  12,   0.333) /* Shade */
     , (450231,  29,     1.1) /* WeaponDefense */
     , (450231,  39,    0.75) /* DefaultScale */
     , (450231, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450231,   1, 'Casting Jack o'' Lantern') /* Name */
     , (450231,  16, 'A small, heavy pumpkin, carved into a Jack o'' Lantern and swirling with magical energies.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450231,   1, 0x02001C0B) /* Setup */
     , (450231,   3, 0x20000014) /* SoundTable */
     , (450231,   6, 0x04001008) /* PaletteBase */
     , (450231,   7, 0x1000024C) /* ClothingBase */
     , (450231,   8, 0x06001E2C) /* Icon */
     , (450231,  22, 0x3400002B) /* PhysicsEffectTable */;
