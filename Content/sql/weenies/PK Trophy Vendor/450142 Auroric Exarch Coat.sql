DELETE FROM `weenie` WHERE `class_Id` = 450142;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450142, 'coataurorbluetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450142,   1,          2) /* ItemType - Armor */
     , (450142,   3,          2) /* PaletteTemplate - Blue */
     , (450142,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450142,   5,        0) /* EncumbranceVal */
     , (450142,   8,        700) /* Mass */
     , (450142,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450142,  16,          1) /* ItemUseable - No */
     , (450142,  18,          1) /* UiEffects - Magical */
     , (450142,  19,       20) /* Value */
     , (450142,  27,         32) /* ArmorType - Metal */
     , (450142,  28,        0) /* ArmorLevel */
     , (450142,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450142,  11, True ) /* IgnoreCollisions */
     , (450142,  13, True ) /* Ethereal */
     , (450142,  14, True ) /* GravityStatus */
     , (450142,  19, True ) /* Attackable */
     , (450142,  22, True ) /* Inscribable */
     , (450142,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450142,   5,    -0.5) /* ManaRate */
     , (450142,  12,     0.5) /* Shade */
     , (450142,  13,    0.75) /* ArmorModVsSlash */
     , (450142,  14,    0.75) /* ArmorModVsPierce */
     , (450142,  15,    0.75) /* ArmorModVsBludgeon */
     , (450142,  16,    0.75) /* ArmorModVsCold */
     , (450142,  17,       1) /* ArmorModVsFire */
     , (450142,  18,       1) /* ArmorModVsAcid */
     , (450142,  19,    0.75) /* ArmorModVsElectric */
     , (450142, 110,       1) /* BulkMod */
     , (450142, 111,       1) /* SizeMod */
     , (450142, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450142,   1, 'Auroric Exarch Coat') /* Name */
     , (450142,  16, 'A richly enchanted and ornate coat once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. The seal of the Lightbringer adorns the chestplate.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450142,   1, 0x020000D4) /* Setup */
     , (450142,   3, 0x20000014) /* SoundTable */
     , (450142,   6, 0x0400007E) /* PaletteBase */
     , (450142,   7, 0x1000044A) /* ClothingBase */
     , (450142,   8, 0x06002A4B) /* Icon */
     , (450142,  22, 0x3400002B) /* PhysicsEffectTable */;


