DELETE FROM `weenie` WHERE `class_Id` = 450147;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450147, 'girthaurorredtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450147,   1,          2) /* ItemType - Armor */
     , (450147,   3,         14) /* PaletteTemplate - Red */
     , (450147,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450147,   5,        0) /* EncumbranceVal */
     , (450147,   8,        325) /* Mass */
     , (450147,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450147,  16,          1) /* ItemUseable - No */
     , (450147,  18,          1) /* UiEffects - Magical */
     , (450147,  19,       20) /* Value */
     , (450147,  27,         32) /* ArmorType - Metal */
     , (450147,  28,        0) /* ArmorLevel */
     , (450147,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450147,  22, True ) /* Inscribable */
     , (450147,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450147,   5,    -0.5) /* ManaRate */
     , (450147,  12,     0.5) /* Shade */
     , (450147,  13,    0.75) /* ArmorModVsSlash */
     , (450147,  14,    0.75) /* ArmorModVsPierce */
     , (450147,  15,    0.75) /* ArmorModVsBludgeon */
     , (450147,  16,    0.75) /* ArmorModVsCold */
     , (450147,  17,       1) /* ArmorModVsFire */
     , (450147,  18,       1) /* ArmorModVsAcid */
     , (450147,  19,    0.75) /* ArmorModVsElectric */
     , (450147, 110,       1) /* BulkMod */
     , (450147, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450147,   1, 'Auroric Exarch Girth') /* Name */
     , (450147,  16, 'A richly enchanted and ornate girth once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450147,   1, 0x020000D7) /* Setup */
     , (450147,   3, 0x20000014) /* SoundTable */
     , (450147,   6, 0x0400007E) /* PaletteBase */
     , (450147,   7, 0x1000044C) /* ClothingBase */
     , (450147,   8, 0x06002A51) /* Icon */
     , (450147,  22, 0x3400002B) /* PhysicsEffectTable */;

