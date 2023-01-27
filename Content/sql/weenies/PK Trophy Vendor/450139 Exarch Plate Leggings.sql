DELETE FROM `weenie` WHERE `class_Id` = 450139;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450139, 'leggingsexarchseabluetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450139,   1,          2) /* ItemType - Armor */
     , (450139,   3,          2) /* PaletteTemplate - Blue */
     , (450139,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450139,   5,         0) /* EncumbranceVal */
     , (450139,   8,        400) /* Mass */
     , (450139,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450139,  16,          1) /* ItemUseable - No */
     , (450139,  18,          1) /* UiEffects - Magical */
     , (450139,  19,       20) /* Value */
     , (450139,  27,         32) /* ArmorType - Metal */
     , (450139,  28,          0) /* ArmorLevel */
     , (450139,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450139,  22, True ) /* Inscribable */
     , (450139,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450139,   5,  -0.125) /* ManaRate */
     , (450139,  12,     0.5) /* Shade */
     , (450139,  13,       0) /* ArmorModVsSlash */
     , (450139,  14,       0) /* ArmorModVsPierce */
     , (450139,  15,       0) /* ArmorModVsBludgeon */
     , (450139,  16,       0) /* ArmorModVsCold */
     , (450139,  17,       0) /* ArmorModVsFire */
     , (450139,  18,       0) /* ArmorModVsAcid */
     , (450139,  19,       0) /* ArmorModVsElectric */
     , (450139, 110,       1) /* BulkMod */
     , (450139, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450139,   1, 'Exarch Plate Leggings') /* Name */
     , (450139,  16, 'A heavily enchanted set of crystalline leggings, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450139,   1, 0x020001A8) /* Setup */
     , (450139,   3, 0x20000014) /* SoundTable */
     , (450139,   6, 0x0400007E) /* PaletteBase */
     , (450139,   7, 0x10000296) /* ClothingBase */
     , (450139,   8, 0x06001BD3) /* Icon */
     , (450139,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450139,  41,         34) /* ItemSpecializedOnly - WarMagic */;

