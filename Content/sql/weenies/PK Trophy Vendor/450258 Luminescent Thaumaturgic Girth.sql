DELETE FROM `weenie` WHERE `class_Id` = 450258;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450258, 'girthlumingreentailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450258,   1,          2) /* ItemType - Armor */
     , (450258,   3,          8) /* PaletteTemplate - Green */
     , (450258,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450258,   5,        0) /* EncumbranceVal */
     , (450258,   8,        325) /* Mass */
     , (450258,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450258,  16,          1) /* ItemUseable - No */
     , (450258,  18,          1) /* UiEffects - Magical */
     , (450258,  19,       20) /* Value */
     , (450258,  27,         32) /* ArmorType - Metal */
     , (450258,  28,        0) /* ArmorLevel */
     , (450258,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450258,  22, True ) /* Inscribable */
     , (450258,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450258,   5,    -0.5) /* ManaRate */
     , (450258,  12,     0.5) /* Shade */
     , (450258,  13,    0.75) /* ArmorModVsSlash */
     , (450258,  14,    0.75) /* ArmorModVsPierce */
     , (450258,  15,    0.75) /* ArmorModVsBludgeon */
     , (450258,  16,    0.75) /* ArmorModVsCold */
     , (450258,  17,       1) /* ArmorModVsFire */
     , (450258,  18,       1) /* ArmorModVsAcid */
     , (450258,  19,    0.75) /* ArmorModVsElectric */
     , (450258, 110,       1) /* BulkMod */
     , (450258, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450258,   1, 'Luminescent Thaumaturgic Girth') /* Name */
     , (450258,  16, 'A richly enchanted and ornate girth once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450258,   1, 0x020000D7) /* Setup */
     , (450258,   3, 0x20000014) /* SoundTable */
     , (450258,   6, 0x0400007E) /* PaletteBase */
     , (450258,   7, 0x1000044D) /* ClothingBase */
     , (450258,   8, 0x06002A56) /* Icon */
     , (450258,  22, 0x3400002B) /* PhysicsEffectTable */;

