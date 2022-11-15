DELETE FROM `weenie` WHERE `class_Id` = 450157;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450157, 'helmnobletailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450157,   1,          2) /* ItemType - Armor */
     , (450157,   3,         21) /* PaletteTemplate - Gold */
     , (450157,   4,      16384) /* ClothingPriority - Head */
     , (450157,   5,        0) /* EncumbranceVal */
     , (450157,   8,        350) /* Mass */
     , (450157,   9,          1) /* ValidLocations - HeadWear */
     , (450157,  16,          1) /* ItemUseable - No */
     , (450157,  19,       20) /* Value */
     , (450157,  27,          2) /* ArmorType - Leather */
     , (450157,  28,        0) /* ArmorLevel */
     , (450157,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450157,  11, True ) /* IgnoreCollisions */
     , (450157,  13, True ) /* Ethereal */
     , (450157,  14, True ) /* GravityStatus */
     , (450157,  19, True ) /* Attackable */
     , (450157,  22, True ) /* Inscribable */
     , (450157, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450157,   5,  -0.017) /* ManaRate */
     , (450157,  12,    0.66) /* Shade */
     , (450157,  13,     1.2) /* ArmorModVsSlash */
     , (450157,  14,     1.2) /* ArmorModVsPierce */
     , (450157,  15,     1.4) /* ArmorModVsBludgeon */
     , (450157,  16,     1.4) /* ArmorModVsCold */
     , (450157,  17,       1) /* ArmorModVsFire */
     , (450157,  18,     0.8) /* ArmorModVsAcid */
     , (450157,  19,     0.8) /* ArmorModVsElectric */
     , (450157, 110,       1) /* BulkMod */
     , (450157, 111,       1) /* SizeMod */
     , (450157, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450157,   1, 'Noble Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450157,   1, 0x02001228) /* Setup */
     , (450157,   3, 0x20000014) /* SoundTable */
     , (450157,   6, 0x0400007E) /* PaletteBase */
     , (450157,   7, 0x1000058F) /* ClothingBase */
     , (450157,   8, 0x06002D88) /* Icon */
     , (450157,  22, 0x3400002B) /* PhysicsEffectTable */;


