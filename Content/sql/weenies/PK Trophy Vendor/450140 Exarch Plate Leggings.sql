DELETE FROM `weenie` WHERE `class_Id` = 450140;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450140, 'leggingsexarchseagreytailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450140,   1,          2) /* ItemType - Armor */
     , (450140,   3,          9) /* PaletteTemplate - Grey */
     , (450140,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450140,   5,         0) /* EncumbranceVal */
     , (450140,   8,        400) /* Mass */
     , (450140,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450140,  16,          1) /* ItemUseable - No */
     , (450140,  18,          1) /* UiEffects - Magical */
     , (450140,  19,       20) /* Value */
     , (450140,  27,         32) /* ArmorType - Metal */
     , (450140,  28,          0) /* ArmorLevel */
     , (450140,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450140,  22, True ) /* Inscribable */
     , (450140,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450140,   5,  -0.125) /* ManaRate */
     , (450140,  12,     0.5) /* Shade */
     , (450140,  13,       0) /* ArmorModVsSlash */
     , (450140,  14,       0) /* ArmorModVsPierce */
     , (450140,  15,       0) /* ArmorModVsBludgeon */
     , (450140,  16,       0) /* ArmorModVsCold */
     , (450140,  17,       0) /* ArmorModVsFire */
     , (450140,  18,       0) /* ArmorModVsAcid */
     , (450140,  19,       0) /* ArmorModVsElectric */
     , (450140, 110,       1) /* BulkMod */
     , (450140, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450140,   1, 'Exarch Plate Leggings') /* Name */
     , (450140,  16, 'A heavily enchanted set of crystalline leggings, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450140,   1, 0x020001A8) /* Setup */
     , (450140,   3, 0x20000014) /* SoundTable */
     , (450140,   6, 0x0400007E) /* PaletteBase */
     , (450140,   7, 0x10000296) /* ClothingBase */
     , (450140,   8, 0x06001BD3) /* Icon */
     , (450140,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450140,  41,         34) /* ItemSpecializedOnly - WarMagic */;

