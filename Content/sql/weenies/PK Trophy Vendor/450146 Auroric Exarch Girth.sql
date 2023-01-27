DELETE FROM `weenie` WHERE `class_Id` = 450146;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450146, 'girthaurorgreentailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450146,   1,          2) /* ItemType - Armor */
     , (450146,   3,          8) /* PaletteTemplate - Green */
     , (450146,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450146,   5,        0) /* EncumbranceVal */
     , (450146,   8,        325) /* Mass */
     , (450146,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450146,  16,          1) /* ItemUseable - No */
     , (450146,  18,          1) /* UiEffects - Magical */
     , (450146,  19,       20) /* Value */
     , (450146,  27,         32) /* ArmorType - Metal */
     , (450146,  28,        0) /* ArmorLevel */
     , (450146,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450146,  22, True ) /* Inscribable */
     , (450146,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450146,   5,    -0.5) /* ManaRate */
     , (450146,  12,     0.5) /* Shade */
     , (450146,  13,    0.75) /* ArmorModVsSlash */
     , (450146,  14,    0.75) /* ArmorModVsPierce */
     , (450146,  15,    0.75) /* ArmorModVsBludgeon */
     , (450146,  16,    0.75) /* ArmorModVsCold */
     , (450146,  17,       1) /* ArmorModVsFire */
     , (450146,  18,       1) /* ArmorModVsAcid */
     , (450146,  19,    0.75) /* ArmorModVsElectric */
     , (450146, 110,       1) /* BulkMod */
     , (450146, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450146,   1, 'Auroric Exarch Girth') /* Name */
     , (450146,  16, 'A richly enchanted and ornate girth once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450146,   1, 0x020000D7) /* Setup */
     , (450146,   3, 0x20000014) /* SoundTable */
     , (450146,   6, 0x0400007E) /* PaletteBase */
     , (450146,   7, 0x1000044C) /* ClothingBase */
     , (450146,   8, 0x06002A55) /* Icon */
     , (450146,  22, 0x3400002B) /* PhysicsEffectTable */;


