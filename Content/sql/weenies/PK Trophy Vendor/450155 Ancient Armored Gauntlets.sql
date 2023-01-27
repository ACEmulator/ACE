DELETE FROM `weenie` WHERE `class_Id` = 450155;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450155, 'gauntletshizkri3tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450155,   1,          2) /* ItemType - Armor */
     , (450155,   3,         14) /* PaletteTemplate - Red */
     , (450155,   4,      32768) /* ClothingPriority - Hands */
     , (450155,   5,        0) /* EncumbranceVal */
     , (450155,   8,        460) /* Mass */
     , (450155,   9,         32) /* ValidLocations - HandWear */
     , (450155,  16,          1) /* ItemUseable - No */
     , (450155,  18,          1) /* UiEffects - Magical */
     , (450155,  19,      20) /* Value */
     , (450155,  27,         32) /* ArmorType - Metal */
     , (450155,  28,        0) /* ArmorLevel */
     , (450155,  44,         12) /* Damage */
     , (450155,  45,          4) /* DamageType - Bludgeon */
     , (450155,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450155,  22, True ) /* Inscribable */
     , (450155,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450155,   5,  -0.033) /* ManaRate */
     , (450155,  12,    0.66) /* Shade */
     , (450155,  13,     1.3) /* ArmorModVsSlash */
     , (450155,  14,     0.8) /* ArmorModVsPierce */
     , (450155,  15,     1.3) /* ArmorModVsBludgeon */
     , (450155,  16,       1) /* ArmorModVsCold */
     , (450155,  17,       1) /* ArmorModVsFire */
     , (450155,  18,     1.1) /* ArmorModVsAcid */
     , (450155,  19,     0.5) /* ArmorModVsElectric */
     , (450155,  22,    0.75) /* DamageVariance */
     , (450155, 110,       1) /* BulkMod */
     , (450155, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450155,   1, 'Ancient Armored Gauntlets') /* Name */
     , (450155,  16, 'These armored gauntlets appear to have been an ornamental piece. Obviously this is only one part of a complete suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450155,   1, 0x020000D8) /* Setup */
     , (450155,   3, 0x20000014) /* SoundTable */
     , (450155,   6, 0x0400007E) /* PaletteBase */
     , (450155,   7, 0x1000055C) /* ClothingBase */
     , (450155,   8, 0x060033C6) /* Icon */
     , (450155,  22, 0x3400002B) /* PhysicsEffectTable */;

