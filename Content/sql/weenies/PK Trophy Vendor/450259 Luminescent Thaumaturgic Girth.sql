DELETE FROM `weenie` WHERE `class_Id` = 450259;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450259, 'girthluminredtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450259,   1,          2) /* ItemType - Armor */
     , (450259,   3,         14) /* PaletteTemplate - Red */
     , (450259,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450259,   5,        0) /* EncumbranceVal */
     , (450259,   8,        325) /* Mass */
     , (450259,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450259,  16,          1) /* ItemUseable - No */
     , (450259,  18,          1) /* UiEffects - Magical */
     , (450259,  19,       20) /* Value */
     , (450259,  27,         32) /* ArmorType - Metal */
     , (450259,  28,        0) /* ArmorLevel */
     , (450259,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450259,  22, True ) /* Inscribable */
     , (450259,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450259,   5,    -0.5) /* ManaRate */
     , (450259,  12,     0.5) /* Shade */
     , (450259,  13,    0.75) /* ArmorModVsSlash */
     , (450259,  14,    0.75) /* ArmorModVsPierce */
     , (450259,  15,    0.75) /* ArmorModVsBludgeon */
     , (450259,  16,    0.75) /* ArmorModVsCold */
     , (450259,  17,       1) /* ArmorModVsFire */
     , (450259,  18,       1) /* ArmorModVsAcid */
     , (450259,  19,    0.75) /* ArmorModVsElectric */
     , (450259, 110,       1) /* BulkMod */
     , (450259, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450259,   1, 'Luminescent Thaumaturgic Girth') /* Name */
     , (450259,  16, 'A richly enchanted and ornate girth once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450259,   1, 0x020000D7) /* Setup */
     , (450259,   3, 0x20000014) /* SoundTable */
     , (450259,   6, 0x0400007E) /* PaletteBase */
     , (450259,   7, 0x1000044D) /* ClothingBase */
     , (450259,   8, 0x06002A52) /* Icon */
     , (450259,  22, 0x3400002B) /* PhysicsEffectTable */;


