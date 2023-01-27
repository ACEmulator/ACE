DELETE FROM `weenie` WHERE `class_Id` = 450166;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450166, 'coatrarepatriarchtwilighttailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450166,   1,          2) /* ItemType - Armor */
     , (450166,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450166,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450166,   5,        0) /* EncumbranceVal */
     , (450166,   8,        270) /* Mass */
     , (450166,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450166,  16,          1) /* ItemUseable - No */
     , (450166,  19,      20) /* Value */
     , (450166,  27,          2) /* ArmorType - Leather */
     , (450166,  28,        0) /* ArmorLevel */
     , (450166,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450166, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450166,  22, True ) /* Inscribable */
     , (450166,  69, False) /* IsSellable */
     , (450166, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450166,   5,  -0.033) /* ManaRate */
     , (450166,  13,     1.1) /* ArmorModVsSlash */
     , (450166,  14,     0.9) /* ArmorModVsPierce */
     , (450166,  15,     1.1) /* ArmorModVsBludgeon */
     , (450166,  16,     1.3) /* ArmorModVsCold */
     , (450166,  17,     0.9) /* ArmorModVsFire */
     , (450166,  18,     0.9) /* ArmorModVsAcid */
     , (450166,  19,     0.9) /* ArmorModVsElectric */
     , (450166, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450166,   1, 'Patriarch''s Twilight Coat') /* Name */
     , (450166,  16, 'Made of the finest silks embroidered with the most expensive gold thread and jewels, this coat is the pinnacle of excess. The coat is so dazzling it may befuddle those who look upon its wearer.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450166,   1, 0x02001396) /* Setup */
     , (450166,   3, 0x20000014) /* SoundTable */
     , (450166,   6, 0x0400007E) /* PaletteBase */
     , (450166,   7, 0x100005FD) /* ClothingBase */
     , (450166,   8, 0x06005C3C) /* Icon */
     , (450166,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450166,  52, 0x06005B0C) /* IconUnderlay */;

