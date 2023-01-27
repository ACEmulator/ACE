DELETE FROM `weenie` WHERE `class_Id` = 450016;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450016, 'ace450016-empyreanoverrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450016,   1,          4) /* ItemType - Armor */
     , (450016,   3,          4) /* PaletteTemplate - Brown */
     , (450016,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450016,   5,        0) /* EncumbranceVal */
     , (450016,   9,        512) /* ValidLocations - ChestArmor */
     , (450016,  16,          1) /* ItemUseable - No */
     , (450016,  19,        20) /* Value */
     , (450016,  27,          2) /* ArmorType - Leather */
     , (450016,  28,         0) /* ArmorLevel */
     , (450016,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450016, 124,          3) /* Version */
     , (450016, 169,  118161678) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450016,  22, True ) /* Inscribable */
     , (450016, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450016,  13,     1.2) /* ArmorModVsSlash */
     , (450016,  14,     0.8) /* ArmorModVsPierce */
     , (450016,  15,       1) /* ArmorModVsBludgeon */
     , (450016,  16,     0.5) /* ArmorModVsCold */
     , (450016,  17,     0.5) /* ArmorModVsFire */
     , (450016,  18,     0.7) /* ArmorModVsAcid */
     , (450016,  19,     0.8) /* ArmorModVsElectric */
     , (450016, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450016,   1, 'Empyrean Over-robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450016,   1, 0x020001A6) /* Setup */
     , (450016,   3, 0x20000014) /* SoundTable */
     , (450016,   6, 0x0400007E) /* PaletteBase */
     , (450016,   7, 0x100007E3) /* ClothingBase */
     , (450016,   8, 0x06001B8D) /* Icon */
     , (450016,  22, 0x3400002B) /* PhysicsEffectTable */;
