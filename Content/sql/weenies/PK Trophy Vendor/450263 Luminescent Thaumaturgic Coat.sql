DELETE FROM `weenie` WHERE `class_Id` = 450263;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450263, 'coatluminbluetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450263,   1,          2) /* ItemType - Armor */
     , (450263,   3,          2) /* PaletteTemplate - Blue */
     , (450263,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450263,   5,        0) /* EncumbranceVal */
     , (450263,   8,        750) /* Mass */
     , (450263,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450263,  16,          1) /* ItemUseable - No */
     , (450263,  18,          1) /* UiEffects - Magical */
     , (450263,  19,       20) /* Value */
     , (450263,  27,         32) /* ArmorType - Metal */
     , (450263,  28,        0) /* ArmorLevel */
     , (450263,  36,       9999) /* ResistMagic */
     , (450263,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450263,  22, True ) /* Inscribable */
     , (450263,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450263,   5,    -0.5) /* ManaRate */
     , (450263,  12,     0.5) /* Shade */
     , (450263,  13,    0.75) /* ArmorModVsSlash */
     , (450263,  14,    0.75) /* ArmorModVsPierce */
     , (450263,  15,    0.75) /* ArmorModVsBludgeon */
     , (450263,  16,    0.75) /* ArmorModVsCold */
     , (450263,  17,       1) /* ArmorModVsFire */
     , (450263,  18,       1) /* ArmorModVsAcid */
     , (450263,  19,    0.75) /* ArmorModVsElectric */
     , (450263, 110,       1) /* BulkMod */
     , (450263, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450263,   1, 'Luminescent Thaumaturgic Coat') /* Name */
     , (450263,  16, 'A richly enchanted and ornate coat once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. The seal of the Lightbringer adorns the chestplate.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450263,   1, 0x020000D4) /* Setup */
     , (450263,   3, 0x20000014) /* SoundTable */
     , (450263,   6, 0x0400007E) /* PaletteBase */
     , (450263,   7, 0x1000044B) /* ClothingBase */
     , (450263,   8, 0x06002A4C) /* Icon */
     , (450263,  22, 0x3400002B) /* PhysicsEffectTable */;


