DELETE FROM `weenie` WHERE `class_Id` = 450709;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450709, 'breastplateplatemailhand', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450709,   1,          4) /* ItemType - Armor */
     , (450709,   3,         20) /* PaletteTemplate - Silver */
     , (450709,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450709,   5,       0) /* EncumbranceVal */
     , (450709,   8,       1100) /* Mass */
     , (450709,   9,        512) /* ValidLocations - ChestArmor */
     , (450709,  16,          1) /* ItemUseable - No */
     , (450709,  19,       20) /* Value */
     , (450709,  27,         32) /* ArmorType - Metal */
     , (450709,  28,        100) /* ArmorLevel */
     , (450709,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450709, 124,          3) /* Version */
     , (450709, 169,  118097668) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450709,  22, True ) /* Inscribable */
     , (450709, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450709,  12,    0.33) /* Shade */
     , (450709,  13,     1.3) /* ArmorModVsSlash */
     , (450709,  14,       1) /* ArmorModVsPierce */
     , (450709,  15,       1) /* ArmorModVsBludgeon */
     , (450709,  16,     0.4) /* ArmorModVsCold */
     , (450709,  17,     0.4) /* ArmorModVsFire */
     , (450709,  18,     0.6) /* ArmorModVsAcid */
     , (450709,  19,     0.4) /* ArmorModVsElectric */
     , (450709, 110,       1) /* BulkMod */
     , (450709, 111,     1.3) /* SizeMod */
     , (450709, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450709,   1, 'Celestial Hand Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450709,   1, 0x020001A6) /* Setup */
     , (450709,   3, 0x20000014) /* SoundTable */
     , (450709,   7, 0x100007D4) /* ClothingBase */
     , (450709,   8, 0x06007023) /* Icon */
     , (450709,  22, 0x3400002B) /* PhysicsEffectTable */;
