DELETE FROM `weenie` WHERE `class_Id` = 450733;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450733, 'slippersjalehtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450733,   1,          4) /* ItemType - Clothing */
     , (450733,   3,          9) /* PaletteTemplate - Grey */
     , (450733,   4,      65536) /* ClothingPriority - Feet */
     , (450733,   5,        0) /* EncumbranceVal */
     , (450733,   8,         45) /* Mass */
     , (450733,   9,        256) /* ValidLocations - FootWear */
     , (450733,  16,          1) /* ItemUseable - No */
     , (450733,  19,       20) /* Value */
     , (450733,  27,          1) /* ArmorType - Cloth */
     , (450733,  28,          0) /* ArmorLevel */
     , (450733,  33,          1) /* Bonded - Bonded */
     , (450733,  44,          1) /* Damage */
     , (450733,  45,          4) /* DamageType - Bludgeon */
     , (450733,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450733, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450733,  22, True ) /* Inscribable */
     , (450733,  69, False) /* IsSellable */
     , (450733,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450733,  12,     0.8) /* Shade */
     , (450733,  13,     0.8) /* ArmorModVsSlash */
     , (450733,  14,     0.8) /* ArmorModVsPierce */
     , (450733,  15,       1) /* ArmorModVsBludgeon */
     , (450733,  16,     0.2) /* ArmorModVsCold */
     , (450733,  17,     0.2) /* ArmorModVsFire */
     , (450733,  18,     0.1) /* ArmorModVsAcid */
     , (450733,  19,     0.2) /* ArmorModVsElectric */
     , (450733,  22,    0.75) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450733,   1, 'Jaleh''s Slippers') /* Name */
     , (450733,  15, 'These slippers were once worn by Jaleh al-Thani. They are soft and crafted from fine silk.') /* ShortDesc */
     , (450733,  33, 'SlippersJalehTaken') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450733,   1, 0x020000DE) /* Setup */
     , (450733,   3, 0x20000014) /* SoundTable */
     , (450733,   6, 0x0400007E) /* PaletteBase */
     , (450733,   7, 0x10000105) /* ClothingBase */
     , (450733,   8, 0x06002AE6) /* Icon */
     , (450733,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450733,  36, 0x0E000016) /* MutateFilter */;
