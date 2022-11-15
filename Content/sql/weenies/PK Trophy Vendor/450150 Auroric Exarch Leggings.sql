DELETE FROM `weenie` WHERE `class_Id` = 450150;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450150, 'leggingsaurorredtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450150,   1,          2) /* ItemType - Armor */
     , (450150,   3,         14) /* PaletteTemplate - Red */
     , (450150,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450150,   5,        0) /* EncumbranceVal */
     , (450150,   8,        400) /* Mass */
     , (450150,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450150,  16,          1) /* ItemUseable - No */
     , (450150,  18,          1) /* UiEffects - Magical */
     , (450150,  19,       20) /* Value */
     , (450150,  27,         32) /* ArmorType - Metal */
     , (450150,  28,        0) /* ArmorLevel */
     , (450150,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450150,  22, True ) /* Inscribable */
     , (450150,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450150,   5,    -0.5) /* ManaRate */
     , (450150,  12,     0.5) /* Shade */
     , (450150,  13,    0.75) /* ArmorModVsSlash */
     , (450150,  14,    0.75) /* ArmorModVsPierce */
     , (450150,  15,    0.75) /* ArmorModVsBludgeon */
     , (450150,  16,    0.75) /* ArmorModVsCold */
     , (450150,  17,       1) /* ArmorModVsFire */
     , (450150,  18,       1) /* ArmorModVsAcid */
     , (450150,  19,    0.75) /* ArmorModVsElectric */
     , (450150, 110,       1) /* BulkMod */
     , (450150, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450150,   1, 'Auroric Exarch Leggings') /* Name */
     , (450150,  16, 'A richly enchanted and ornate pair of leggings once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450150,   1, 0x020001A8) /* Setup */
     , (450150,   3, 0x20000014) /* SoundTable */
     , (450150,   6, 0x0400007E) /* PaletteBase */
     , (450150,   7, 0x1000044E) /* ClothingBase */
     , (450150,   8, 0x06002A48) /* Icon */
     , (450150,  22, 0x3400002B) /* PhysicsEffectTable */;

