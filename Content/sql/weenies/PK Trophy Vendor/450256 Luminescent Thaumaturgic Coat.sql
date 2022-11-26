DELETE FROM `weenie` WHERE `class_Id` = 450256;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450256, 'coatlumingreentailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450256,   1,          2) /* ItemType - Armor */
     , (450256,   3,          8) /* PaletteTemplate - Green */
     , (450256,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450256,   5,        0) /* EncumbranceVal */
     , (450256,   8,        750) /* Mass */
     , (450256,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450256,  16,          1) /* ItemUseable - No */
     , (450256,  18,          1) /* UiEffects - Magical */
     , (450256,  19,       20) /* Value */
     , (450256,  27,         32) /* ArmorType - Metal */
     , (450256,  28,        0) /* ArmorLevel */
     , (450256,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450256,  11, True ) /* IgnoreCollisions */
     , (450256,  13, True ) /* Ethereal */
     , (450256,  14, True ) /* GravityStatus */
     , (450256,  19, True ) /* Attackable */
     , (450256,  22, True ) /* Inscribable */
     , (450256,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450256,   5,    -0.5) /* ManaRate */
     , (450256,  12,     0.5) /* Shade */
     , (450256,  13,    0.75) /* ArmorModVsSlash */
     , (450256,  14,    0.75) /* ArmorModVsPierce */
     , (450256,  15,    0.75) /* ArmorModVsBludgeon */
     , (450256,  16,    0.75) /* ArmorModVsCold */
     , (450256,  17,       1) /* ArmorModVsFire */
     , (450256,  18,       1) /* ArmorModVsAcid */
     , (450256,  19,    0.75) /* ArmorModVsElectric */
     , (450256, 110,       1) /* BulkMod */
     , (450256, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450256,   1, 'Luminescent Thaumaturgic Coat') /* Name */
     , (450256,  16, 'A richly enchanted and ornate coat once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. The seal of the Lightbringer adorns the chestplate.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450256,   1, 0x020000D4) /* Setup */
     , (450256,   3, 0x20000014) /* SoundTable */
     , (450256,   6, 0x0400007E) /* PaletteBase */
     , (450256,   7, 0x1000044B) /* ClothingBase */
     , (450256,   8, 0x06002A4A) /* Icon */
     , (450256,  22, 0x3400002B) /* PhysicsEffectTable */;

