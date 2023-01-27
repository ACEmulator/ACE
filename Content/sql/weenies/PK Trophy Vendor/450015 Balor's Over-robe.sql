DELETE FROM `weenie` WHERE `class_Id` = 450015;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450015, 'ace450015-balorsoverrobetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450015,   1,          4) /* ItemType - Armor */
     , (450015,   3,         90) /* PaletteTemplate - DyeWinterSilver */
     , (450015,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450015,   5,        0) /* EncumbranceVal */
     , (450015,   9,        512) /* ValidLocations - ChestArmor */
     , (450015,  16,          1) /* ItemUseable - No */
     , (450015,  19,       20) /* Value */
     , (450015,  28,        0) /* ArmorLevel */
     , (450015, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450015,  22, True ) /* Inscribable */
     , (450015, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450015,   5,  -0.025) /* ManaRate */
     , (450015,  13,       1) /* ArmorModVsSlash */
     , (450015,  14,       1) /* ArmorModVsPierce */
     , (450015,  15,       1) /* ArmorModVsBludgeon */
     , (450015,  16,       2) /* ArmorModVsCold */
     , (450015,  17,     0.6) /* ArmorModVsFire */
     , (450015,  18,     0.6) /* ArmorModVsAcid */
     , (450015,  19,     0.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450015,   1, 'Balor''s Over-robe') /* Name */
     , (450015,  16, 'A lovingly crafted over-robe. It''s white fur glistens with protective magic.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450015,   1, 0x020001A6) /* Setup */
     , (450015,   3, 0x20000014) /* SoundTable */
     , (450015,   6, 0x0400007E) /* PaletteBase */
     , (450015,   7, 0x1000018D) /* ClothingBase */
     , (450015,   8, 0x060023CE) /* Icon */
     , (450015,  22, 0x3400002B) /* PhysicsEffectTable */;


