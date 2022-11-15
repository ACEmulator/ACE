DELETE FROM `weenie` WHERE `class_Id` = 450013;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450013, 'ace450013-hoorymattekaroverrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450013,   1,          4) /* ItemType - Armor */
     , (450013,   3,         67) /* PaletteTemplate - GreenSlime */
     , (450013,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450013,   5,        0) /* EncumbranceVal */
     , (450013,   9,        512) /* ValidLocations - ChestArmor */
     , (450013,  16,          1) /* ItemUseable - No */
     , (450013,  19,          20) /* Value */
     , (450013,  27,          32) /* ArmorType - Cloth */
     , (450013,  28,         0) /* ArmorLevel */
     , (450013,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450013, 150,        103) /* HookPlacement - Hook */
     , (450013, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450013,  22, True ) /* Inscribable */
     , (450013, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450013,  12,       1) /* Shade */
     , (450013,  13,     0.3) /* ArmorModVsSlash */
     , (450013,  14,     0.3) /* ArmorModVsPierce */
     , (450013,  15,     0.3) /* ArmorModVsBludgeon */
     , (450013,  16,       0) /* ArmorModVsCold */
     , (450013,  17,       0) /* ArmorModVsFire */
     , (450013,  18,       0) /* ArmorModVsAcid */
     , (450013,  19,       0) /* ArmorModVsElectric */
     , (450013, 165,       0) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450013,   1, 'Hoory Mattekar Over-robe') /* Name */
     , (450013,   7, 'A new-and-improved genuine artificial quality knockoff of the rare Hoary Mattekar Robe, masterfully tailored of high-quality materials to even fit over other armor pieces!  Brought to you by the impressive generosity of Ketnan Enterprises.') /* Inscription */
     , (450013,   8, '-') /* ScribeName */
     , (450013,  14, 'This over-robe looks scrathy and uncomfortable to wear.') /* Use */
     , (450013,  16, 'An over-robe purchased on Tusker Island.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450013,   1, 0x020001A6) /* Setup */
     , (450013,   3, 0x20000014) /* SoundTable */
     , (450013,   6, 0x0400007E) /* PaletteBase */
     , (450013,   7, 0x100007E2) /* ClothingBase */
     , (450013,   8, 0x06002235) /* Icon */
     , (450013,  22, 0x3400002B) /* PhysicsEffectTable */;
