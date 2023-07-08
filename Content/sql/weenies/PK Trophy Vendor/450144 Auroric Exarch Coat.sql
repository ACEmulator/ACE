DELETE FROM `weenie` WHERE `class_Id` = 450144;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450144, 'coataurorredtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450144,   1,          2) /* ItemType - Armor */
     , (450144,   3,         14) /* PaletteTemplate - Red */
     , (450144,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450144,   5,        0) /* EncumbranceVal */
     , (450144,   8,        700) /* Mass */
     , (450144,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450144,  16,          1) /* ItemUseable - No */
     , (450144,  18,          1) /* UiEffects - Magical */
     , (450144,  19,       20) /* Value */
     , (450144,  27,         32) /* ArmorType - Metal */
     , (450144,  28,        0) /* ArmorLevel */
     , (450144,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450144,  11, True ) /* IgnoreCollisions */
     , (450144,  13, True ) /* Ethereal */
     , (450144,  14, True ) /* GravityStatus */
     , (450144,  19, True ) /* Attackable */
     , (450144,  22, True ) /* Inscribable */
     , (450144,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450144,   5,    -0.5) /* ManaRate */
     , (450144,  12,     0.5) /* Shade */
     , (450144,  13,    0.75) /* ArmorModVsSlash */
     , (450144,  14,    0.75) /* ArmorModVsPierce */
     , (450144,  15,    0.75) /* ArmorModVsBludgeon */
     , (450144,  16,    0.75) /* ArmorModVsCold */
     , (450144,  17,       1) /* ArmorModVsFire */
     , (450144,  18,       1) /* ArmorModVsAcid */
     , (450144,  19,    0.75) /* ArmorModVsElectric */
     , (450144, 110,       1) /* BulkMod */
     , (450144, 111,       1) /* SizeMod */
     , (450144, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450144,   1, 'Auroric Exarch Coat') /* Name */
     , (450144,  16, 'A richly enchanted and ornate coat once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. The seal of the Lightbringer adorns the chestplate.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450144,   1, 0x020000D4) /* Setup */
     , (450144,   3, 0x20000014) /* SoundTable */
     , (450144,   6, 0x0400007E) /* PaletteBase */
     , (450144,   7, 0x1000044A) /* ClothingBase */
     , (450144,   8, 0x06002A4D) /* Icon */
     , (450144,  22, 0x3400002B) /* PhysicsEffectTable */;


