DELETE FROM `weenie` WHERE `class_Id` = 450266;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450266, 'leggingsluminbluetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450266,   1,          2) /* ItemType - Armor */
     , (450266,   3,          2) /* PaletteTemplate - Blue */
     , (450266,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450266,   5,        0) /* EncumbranceVal */
     , (450266,   8,        500) /* Mass */
     , (450266,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450266,  16,          1) /* ItemUseable - No */
     , (450266,  18,          1) /* UiEffects - Magical */
     , (450266,  19,       20) /* Value */
     , (450266,  27,         32) /* ArmorType - Metal */
     , (450266,  28,        0) /* ArmorLevel */
     , (450266,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450266,  22, True ) /* Inscribable */
     , (450266,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450266,   5,    -0.5) /* ManaRate */
     , (450266,  12,     0.5) /* Shade */
     , (450266,  13,    0.75) /* ArmorModVsSlash */
     , (450266,  14,    0.75) /* ArmorModVsPierce */
     , (450266,  15,    0.75) /* ArmorModVsBludgeon */
     , (450266,  16,    0.75) /* ArmorModVsCold */
     , (450266,  17,       1) /* ArmorModVsFire */
     , (450266,  18,       1) /* ArmorModVsAcid */
     , (450266,  19,    0.75) /* ArmorModVsElectric */
     , (450266, 110,       1) /* BulkMod */
     , (450266, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450266,   1, 'Luminescent Thaumaturgic Leggings') /* Name */
     , (450266,  16, 'A richly enchanted and ornate pair of leggings once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450266,   1, 0x020001A8) /* Setup */
     , (450266,   3, 0x20000014) /* SoundTable */
     , (450266,   6, 0x0400007E) /* PaletteBase */
     , (450266,   7, 0x1000044F) /* ClothingBase */
     , (450266,   8, 0x06002A5F) /* Icon */
     , (450266,  22, 0x3400002B) /* PhysicsEffectTable */;
