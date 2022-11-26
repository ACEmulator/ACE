DELETE FROM `weenie` WHERE `class_Id` = 450706;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450706, 'breastplateplatemailblood', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450706,   1,          4) /* ItemType - Armor */
     , (450706,   3,         20) /* PaletteTemplate - Silver */
     , (450706,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450706,   5,       0) /* EncumbranceVal */
     , (450706,   8,       1100) /* Mass */
     , (450706,   9,        512) /* ValidLocations - ChestArmor */
     , (450706,  16,          1) /* ItemUseable - No */
     , (450706,  19,       20) /* Value */
     , (450706,  27,         32) /* ArmorType - Metal */
     , (450706,  28,        100) /* ArmorLevel */
     , (450706,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450706, 124,          3) /* Version */
     , (450706, 169,  118097668) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450706,  22, True ) /* Inscribable */
     , (450706, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450706,  12,    0.33) /* Shade */
     , (450706,  13,     1.3) /* ArmorModVsSlash */
     , (450706,  14,       1) /* ArmorModVsPierce */
     , (450706,  15,       1) /* ArmorModVsBludgeon */
     , (450706,  16,     0.4) /* ArmorModVsCold */
     , (450706,  17,     0.4) /* ArmorModVsFire */
     , (450706,  18,     0.6) /* ArmorModVsAcid */
     , (450706,  19,     0.4) /* ArmorModVsElectric */
     , (450706, 110,       1) /* BulkMod */
     , (450706, 111,     1.3) /* SizeMod */
     , (450706, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450706,   1, 'Radiant Blood Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450706,   1, 0x020001A6) /* Setup */
     , (450706,   3, 0x20000014) /* SoundTable */
     , (450706,   7, 0x100007D6) /* ClothingBase */
     , (450706,   8, 0x06007025) /* Icon */
     , (450706,  22, 0x3400002B) /* PhysicsEffectTable */;