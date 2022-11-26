DELETE FROM `weenie` WHERE `class_Id` = 450708;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450708, 'breastplateplatemailweb', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450708,   1,          4) /* ItemType - Armor */
     , (450708,   3,         20) /* PaletteTemplate - Silver */
     , (450708,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450708,   5,       0) /* EncumbranceVal */
     , (450708,   8,       1100) /* Mass */
     , (450708,   9,        512) /* ValidLocations - ChestArmor */
     , (450708,  16,          1) /* ItemUseable - No */
     , (450708,  19,       20) /* Value */
     , (450708,  27,         32) /* ArmorType - Metal */
     , (450708,  28,        100) /* ArmorLevel */
     , (450708,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450708, 124,          3) /* Version */
     , (450708, 169,  118097668) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450708,  22, True ) /* Inscribable */
     , (450708, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450708,  12,    0.33) /* Shade */
     , (450708,  13,     1.3) /* ArmorModVsSlash */
     , (450708,  14,       1) /* ArmorModVsPierce */
     , (450708,  15,       1) /* ArmorModVsBludgeon */
     , (450708,  16,     0.4) /* ArmorModVsCold */
     , (450708,  17,     0.4) /* ArmorModVsFire */
     , (450708,  18,     0.6) /* ArmorModVsAcid */
     , (450708,  19,     0.4) /* ArmorModVsElectric */
     , (450708, 110,       1) /* BulkMod */
     , (450708, 111,     1.3) /* SizeMod */
     , (450708, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450708,   1, 'Eldrytch Web Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450708,   1, 0x020001A6) /* Setup */
     , (450708,   3, 0x20000014) /* SoundTable */
     , (450708,   7, 0x100007D5) /* ClothingBase */
     , (450708,   8, 0x06007024) /* Icon */
     , (450708,  22, 0x3400002B) /* PhysicsEffectTable */;