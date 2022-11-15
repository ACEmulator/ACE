DELETE FROM `weenie` WHERE `class_Id` = 450268;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450268, 'leggingsluminredtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450268,   1,          2) /* ItemType - Armor */
     , (450268,   3,         14) /* PaletteTemplate - Red */
     , (450268,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450268,   5,        0) /* EncumbranceVal */
     , (450268,   8,        500) /* Mass */
     , (450268,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450268,  16,          1) /* ItemUseable - No */
     , (450268,  18,          1) /* UiEffects - Magical */
     , (450268,  19,       20) /* Value */
     , (450268,  27,         32) /* ArmorType - Metal */
     , (450268,  28,        0) /* ArmorLevel */
     , (450268,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450268,  22, True ) /* Inscribable */
     , (450268,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450268,   5,    -0.5) /* ManaRate */
     , (450268,  12,     0.5) /* Shade */
     , (450268,  13,    0.75) /* ArmorModVsSlash */
     , (450268,  14,    0.75) /* ArmorModVsPierce */
     , (450268,  15,    0.75) /* ArmorModVsBludgeon */
     , (450268,  16,    0.75) /* ArmorModVsCold */
     , (450268,  17,       1) /* ArmorModVsFire */
     , (450268,  18,       1) /* ArmorModVsAcid */
     , (450268,  19,    0.75) /* ArmorModVsElectric */
     , (450268, 110,       1) /* BulkMod */
     , (450268, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450268,   1, 'Luminescent Thaumaturgic Leggings') /* Name */
     , (450268,  16, 'A richly enchanted and ornate pair of leggings once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450268,   1, 0x020001A8) /* Setup */
     , (450268,   3, 0x20000014) /* SoundTable */
     , (450268,   6, 0x0400007E) /* PaletteBase */
     , (450268,   7, 0x1000044F) /* ClothingBase */
     , (450268,   8, 0x06002A47) /* Icon */
     , (450268,  22, 0x3400002B) /* PhysicsEffectTable */;

 