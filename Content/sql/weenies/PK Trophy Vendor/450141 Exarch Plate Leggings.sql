DELETE FROM `weenie` WHERE `class_Id` = 450141;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450141, 'leggingsexarchsilvertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450141,   1,          2) /* ItemType - Armor */
     , (450141,   3,         20) /* PaletteTemplate - Silver */
     , (450141,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450141,   5,         0) /* EncumbranceVal */
     , (450141,   8,        400) /* Mass */
     , (450141,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450141,  16,          1) /* ItemUseable - No */
     , (450141,  18,          1) /* UiEffects - Magical */
     , (450141,  19,       20) /* Value */
     , (450141,  27,         32) /* ArmorType - Metal */
     , (450141,  28,          0) /* ArmorLevel */
     , (450141,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450141,  22, True ) /* Inscribable */
     , (450141,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450141,   5,  -0.125) /* ManaRate */
     , (450141,  12,     0.5) /* Shade */
     , (450141,  13,       0) /* ArmorModVsSlash */
     , (450141,  14,       0) /* ArmorModVsPierce */
     , (450141,  15,       0) /* ArmorModVsBludgeon */
     , (450141,  16,       0) /* ArmorModVsCold */
     , (450141,  17,       0) /* ArmorModVsFire */
     , (450141,  18,       0) /* ArmorModVsAcid */
     , (450141,  19,       0) /* ArmorModVsElectric */
     , (450141, 110,       1) /* BulkMod */
     , (450141, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450141,   1, 'Exarch Plate Leggings') /* Name */
     , (450141,  16, 'A heavily enchanted set of crystalline leggings, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450141,   1, 0x020001A8) /* Setup */
     , (450141,   3, 0x20000014) /* SoundTable */
     , (450141,   6, 0x0400007E) /* PaletteBase */
     , (450141,   7, 0x10000296) /* ClothingBase */
     , (450141,   8, 0x06001BD3) /* Icon */
     , (450141,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450141,  41,         34) /* ItemSpecializedOnly - WarMagic */;


