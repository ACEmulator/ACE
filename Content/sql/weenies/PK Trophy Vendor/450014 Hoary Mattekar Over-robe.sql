DELETE FROM `weenie` WHERE `class_Id` = 450014;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450014, 'ace450014-hoarymattekaroverrobetailortailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450014,   1,          4) /* ItemType - Armor */
     , (450014,   3,         61) /* PaletteTemplate - White */
     , (450014,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450014,   5,        0) /* EncumbranceVal */
     , (450014,   9,        512) /* ValidLocations - ChestArmor */
     , (450014,  16,          1) /* ItemUseable - No */
     , (450014,  19,      20) /* Value */
     , (450014,  27,          1) /* ArmorType - Cloth */
     , (450014,  28,        0) /* ArmorLevel */
     , (450014,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450014, 150,        103) /* HookPlacement - Hook */
     , (450014, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450014,  22, True ) /* Inscribable */
     , (450014, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450014,  12,       1) /* Shade */
     , (450014,  13,     1.2) /* ArmorModVsSlash */
     , (450014,  14,     0.9) /* ArmorModVsPierce */
     , (450014,  15,     0.9) /* ArmorModVsBludgeon */
     , (450014,  16,       2) /* ArmorModVsCold */
     , (450014,  17,     0.7) /* ArmorModVsFire */
     , (450014,  18,       1) /* ArmorModVsAcid */
     , (450014,  19,       2) /* ArmorModVsElectric */
     , (450014, 110,       1) /* BulkMod */
     , (450014, 111,       1) /* SizeMod */
     , (450014, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450014,   1, 'Hoary Mattekar Over-robe') /* Name */
     , (450014,  16, 'Rare, lightweight, but warm over-robe crafted from the hide of the elusive Hoary Mattekar, rumored to appear only under certain conditions.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450014,   1, 0x020001A6) /* Setup */
     , (450014,   3, 0x20000014) /* SoundTable */
     , (450014,   6, 0x0400007E) /* PaletteBase */
     , (450014,   7, 0x100007E2) /* ClothingBase */
     , (450014,   8, 0x06002239) /* Icon */
     , (450014,  22, 0x3400002B) /* PhysicsEffectTable */;
