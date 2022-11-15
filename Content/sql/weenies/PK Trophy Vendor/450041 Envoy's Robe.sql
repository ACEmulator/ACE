DELETE FROM `weenie` WHERE `class_Id` = 450041;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450041, 'robeenvoytailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450041,   1,          4) /* ItemType - Armor */
     , (450041,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450041,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450041,   5,         0) /* EncumbranceVal */
     , (450041,   8,         15) /* Mass */
     , (450041,   9,        512) /* ValidLocations - ChestArmor */
     , (450041,  16,          1) /* ItemUseable - No */
     , (450041,  19,          20) /* Value */
     , (450041,  27,         32) /* ArmorType - Metal */
     , (450041,  28,        0) /* ArmorLevel */
     , (450041,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450041, 114,          2) /* Attuned - Sticky */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450041,  22, True ) /* Inscribable */
     , (450041, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450041,  12,       1) /* Shade */
     , (450041,  13,     1.3) /* ArmorModVsSlash */
     , (450041,  14,       1) /* ArmorModVsPierce */
     , (450041,  15,       1) /* ArmorModVsBludgeon */
     , (450041,  16,       0) /* ArmorModVsCold */
     , (450041,  17,       0) /* ArmorModVsFire */
     , (450041,  18,     0.6) /* ArmorModVsAcid */
     , (450041,  19,       0) /* ArmorModVsElectric */
     , (450041, 110,       1) /* BulkMod */
     , (450041, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450041,   1, 'Envoy''s Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450041,   1, 0x020001A6) /* Setup */
     , (450041,   3, 0x20000014) /* SoundTable */
     , (450041,   6, 0x0400007E) /* PaletteBase */
     , (450041,   7, 0x10000532) /* ClothingBase */
     , (450041,   8, 0x06000FDA) /* Icon */
     , (450041,  22, 0x3400002B) /* PhysicsEffectTable */;
