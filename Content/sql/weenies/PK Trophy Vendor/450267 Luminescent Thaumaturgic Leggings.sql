DELETE FROM `weenie` WHERE `class_Id` = 450267;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450267, 'leggingslumingreentailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450267,   1,          2) /* ItemType - Armor */
     , (450267,   3,          8) /* PaletteTemplate - Green */
     , (450267,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450267,   5,        0) /* EncumbranceVal */
     , (450267,   8,        500) /* Mass */
     , (450267,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450267,  16,          1) /* ItemUseable - No */
     , (450267,  18,          1) /* UiEffects - Magical */
     , (450267,  19,       20) /* Value */
     , (450267,  27,         32) /* ArmorType - Metal */
     , (450267,  28,        0) /* ArmorLevel */
     , (450267,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450267,  22, True ) /* Inscribable */
     , (450267,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450267,   5,    -0.5) /* ManaRate */
     , (450267,  12,     0.5) /* Shade */
     , (450267,  13,    0.75) /* ArmorModVsSlash */
     , (450267,  14,    0.75) /* ArmorModVsPierce */
     , (450267,  15,    0.75) /* ArmorModVsBludgeon */
     , (450267,  16,    0.75) /* ArmorModVsCold */
     , (450267,  17,       1) /* ArmorModVsFire */
     , (450267,  18,       1) /* ArmorModVsAcid */
     , (450267,  19,    0.75) /* ArmorModVsElectric */
     , (450267, 110,       1) /* BulkMod */
     , (450267, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450267,   1, 'Luminescent Thaumaturgic Leggings') /* Name */
     , (450267,  16, 'A richly enchanted and ornate pair of leggings once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450267,   1, 0x020001A8) /* Setup */
     , (450267,   3, 0x20000014) /* SoundTable */
     , (450267,   6, 0x0400007E) /* PaletteBase */
     , (450267,   7, 0x1000044F) /* ClothingBase */
     , (450267,   8, 0x06002A5E) /* Icon */
     , (450267,  22, 0x3400002B) /* PhysicsEffectTable */;

