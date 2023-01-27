DELETE FROM `weenie` WHERE `class_Id` = 450148;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450148, 'leggingsaurorbluetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450148,   1,          2) /* ItemType - Armor */
     , (450148,   3,          2) /* PaletteTemplate - Blue */
     , (450148,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450148,   5,        0) /* EncumbranceVal */
     , (450148,   8,        400) /* Mass */
     , (450148,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450148,  16,          1) /* ItemUseable - No */
     , (450148,  18,          1) /* UiEffects - Magical */
     , (450148,  19,       20) /* Value */
     , (450148,  27,         32) /* ArmorType - Metal */
     , (450148,  28,        0) /* ArmorLevel */
     , (450148,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450148,  22, True ) /* Inscribable */
     , (450148,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450148,   5,    -0.5) /* ManaRate */
     , (450148,  12,     0.5) /* Shade */
     , (450148,  13,    0.75) /* ArmorModVsSlash */
     , (450148,  14,    0.75) /* ArmorModVsPierce */
     , (450148,  15,    0.75) /* ArmorModVsBludgeon */
     , (450148,  16,    0.75) /* ArmorModVsCold */
     , (450148,  17,       1) /* ArmorModVsFire */
     , (450148,  18,       1) /* ArmorModVsAcid */
     , (450148,  19,    0.75) /* ArmorModVsElectric */
     , (450148, 110,       1) /* BulkMod */
     , (450148, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450148,   1, 'Auroric Exarch Leggings') /* Name */
     , (450148,  16, 'A richly enchanted and ornate pair of leggings once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450148,   1, 0x020001A8) /* Setup */
     , (450148,   3, 0x20000014) /* SoundTable */
     , (450148,   6, 0x0400007E) /* PaletteBase */
     , (450148,   7, 0x1000044E) /* ClothingBase */
     , (450148,   8, 0x06002A60) /* Icon */
     , (450148,  22, 0x3400002B) /* PhysicsEffectTable */;


