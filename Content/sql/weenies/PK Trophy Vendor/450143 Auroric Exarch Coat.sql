DELETE FROM `weenie` WHERE `class_Id` = 450143;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450143, 'coataurorgreentailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450143,   1,          2) /* ItemType - Armor */
     , (450143,   3,          8) /* PaletteTemplate - Green */
     , (450143,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450143,   5,        0) /* EncumbranceVal */
     , (450143,   8,        700) /* Mass */
     , (450143,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450143,  16,          1) /* ItemUseable - No */
     , (450143,  18,          1) /* UiEffects - Magical */
     , (450143,  19,       20) /* Value */
     , (450143,  27,         32) /* ArmorType - Metal */
     , (450143,  28,        0) /* ArmorLevel */
     , (450143,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450143,  11, True ) /* IgnoreCollisions */
     , (450143,  13, True ) /* Ethereal */
     , (450143,  14, True ) /* GravityStatus */
     , (450143,  19, True ) /* Attackable */
     , (450143,  22, True ) /* Inscribable */
     , (450143,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450143,   5,    -0.5) /* ManaRate */
     , (450143,  12,     0.5) /* Shade */
     , (450143,  13,    0.75) /* ArmorModVsSlash */
     , (450143,  14,    0.75) /* ArmorModVsPierce */
     , (450143,  15,    0.75) /* ArmorModVsBludgeon */
     , (450143,  16,    0.75) /* ArmorModVsCold */
     , (450143,  17,       1) /* ArmorModVsFire */
     , (450143,  18,       1) /* ArmorModVsAcid */
     , (450143,  19,    0.75) /* ArmorModVsElectric */
     , (450143, 110,       1) /* BulkMod */
     , (450143, 111,       1) /* SizeMod */
     , (450143, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450143,   1, 'Auroric Exarch Coat') /* Name */
     , (450143,  16, 'A richly enchanted and ornate coat once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. The seal of the Lightbringer adorns the chestplate.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450143,   1, 0x020000D4) /* Setup */
     , (450143,   3, 0x20000014) /* SoundTable */
     , (450143,   6, 0x0400007E) /* PaletteBase */
     , (450143,   7, 0x1000044A) /* ClothingBase */
     , (450143,   8, 0x06002A49) /* Icon */
     , (450143,  22, 0x3400002B) /* PhysicsEffectTable */;

