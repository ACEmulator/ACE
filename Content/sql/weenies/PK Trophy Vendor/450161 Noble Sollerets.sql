DELETE FROM `weenie` WHERE `class_Id` = 450161;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450161, 'solleretsnobletailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450161,   1,          2) /* ItemType - Armor */
     , (450161,   3,         21) /* PaletteTemplate - Gold */
     , (450161,   4,      65536) /* ClothingPriority - Feet */
     , (450161,   5,        0) /* EncumbranceVal */
     , (450161,   8,        450) /* Mass */
     , (450161,   9,        256) /* ValidLocations - FootWear */
     , (450161,  16,          1) /* ItemUseable - No */
     , (450161,  19,       20) /* Value */
     , (450161,  27,          2) /* ArmorType - Leather */
     , (450161,  28,        0) /* ArmorLevel */
     , (450161,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450161,  22, True ) /* Inscribable */
     , (450161, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450161,   5,  -0.017) /* ManaRate */
     , (450161,  12,    0.66) /* Shade */
     , (450161,  13,     1.2) /* ArmorModVsSlash */
     , (450161,  14,     1.2) /* ArmorModVsPierce */
     , (450161,  15,     1.4) /* ArmorModVsBludgeon */
     , (450161,  16,     1.4) /* ArmorModVsCold */
     , (450161,  17,       1) /* ArmorModVsFire */
     , (450161,  18,     0.8) /* ArmorModVsAcid */
     , (450161,  19,     0.8) /* ArmorModVsElectric */
     , (450161, 110,       1) /* BulkMod */
     , (450161, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450161,   1, 'Noble Sollerets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450161,   1, 0x020000DE) /* Setup */
     , (450161,   3, 0x20000014) /* SoundTable */
     , (450161,   6, 0x0400007E) /* PaletteBase */
     , (450161,   7, 0x1000058C) /* ClothingBase */
     , (450161,   8, 0x06000FAD) /* Icon */
     , (450161,  22, 0x3400002B) /* PhysicsEffectTable */;

