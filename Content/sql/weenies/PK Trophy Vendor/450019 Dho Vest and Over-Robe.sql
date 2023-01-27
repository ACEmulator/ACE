DELETE FROM `weenie` WHERE `class_Id` = 450019;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450019, 'ace450019-dhovestandoverrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450019,   1,          4) /* ItemType - Armor */
     , (450019,   3,          9) /* PaletteTemplate - Grey */
     , (450019,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450019,   5,        0) /* EncumbranceVal */
     , (450019,   9,        512) /* ValidLocations - ChestArmor */
     , (450019,  16,          1) /* ItemUseable - No */
     , (450019,  19,        20) /* Value */
     , (450019,  27,          2) /* ArmorType - Leather */
     , (450019,  28,         0) /* ArmorLevel */
     , (450019,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450019, 124,          3) /* Version */
     , (450019, 169,  118161678) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450019,  22, True ) /* Inscribable */
     , (450019, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450019,  13,     1.2) /* ArmorModVsSlash */
     , (450019,  14,     0.8) /* ArmorModVsPierce */
     , (450019,  15,       1) /* ArmorModVsBludgeon */
     , (450019,  16,     0.5) /* ArmorModVsCold */
     , (450019,  17,     0.5) /* ArmorModVsFire */
     , (450019,  18,     0.7) /* ArmorModVsAcid */
     , (450019,  19,     0.8) /* ArmorModVsElectric */
     , (450019, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450019,   1, 'Dho Vest and Over-Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450019,   1, 0x020001A6) /* Setup */
     , (450019,   3, 0x20000014) /* SoundTable */
     , (450019,   6, 0x0400007E) /* PaletteBase */
     , (450019,   7, 0x100007E4) /* ClothingBase */
     , (450019,   8, 0x06001BA0) /* Icon */
     , (450019,  22, 0x3400002B) /* PhysicsEffectTable */;
