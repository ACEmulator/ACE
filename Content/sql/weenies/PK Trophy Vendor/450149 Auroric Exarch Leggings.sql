DELETE FROM `weenie` WHERE `class_Id` = 450149;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450149, 'leggingsaurorgreentailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450149,   1,          2) /* ItemType - Armor */
     , (450149,   3,          8) /* PaletteTemplate - Green */
     , (450149,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450149,   5,        0) /* EncumbranceVal */
     , (450149,   8,        400) /* Mass */
     , (450149,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450149,  16,          1) /* ItemUseable - No */
     , (450149,  18,          1) /* UiEffects - Magical */
     , (450149,  19,       20) /* Value */
     , (450149,  27,         32) /* ArmorType - Metal */
     , (450149,  28,        0) /* ArmorLevel */
     , (450149,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450149,  22, True ) /* Inscribable */
     , (450149,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450149,   5,    -0.5) /* ManaRate */
     , (450149,  12,     0.5) /* Shade */
     , (450149,  13,    0.75) /* ArmorModVsSlash */
     , (450149,  14,    0.75) /* ArmorModVsPierce */
     , (450149,  15,    0.75) /* ArmorModVsBludgeon */
     , (450149,  16,    0.75) /* ArmorModVsCold */
     , (450149,  17,       1) /* ArmorModVsFire */
     , (450149,  18,       1) /* ArmorModVsAcid */
     , (450149,  19,    0.75) /* ArmorModVsElectric */
     , (450149, 110,       1) /* BulkMod */
     , (450149, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450149,   1, 'Auroric Exarch Leggings') /* Name */
     , (450149,  16, 'A richly enchanted and ornate pair of leggings once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450149,   1, 0x020001A8) /* Setup */
     , (450149,   3, 0x20000014) /* SoundTable */
     , (450149,   6, 0x0400007E) /* PaletteBase */
     , (450149,   7, 0x1000044E) /* ClothingBase */
     , (450149,   8, 0x06002A46) /* Icon */
     , (450149,  22, 0x3400002B) /* PhysicsEffectTable */;

