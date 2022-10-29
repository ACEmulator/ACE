DELETE FROM `weenie` WHERE `class_Id` = 1006805;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006805, 'ace1006805-nexuskoujiasleeves', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006805,   1,          2) /* ItemType - Armor */
     , (1006805,   3,          2) /* PaletteTemplate - Blue */
     , (1006805,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (1006805,   5,       1125) /* EncumbranceVal */
     , (1006805,   8,        550) /* Mass */
     , (1006805,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (1006805,  16,          1) /* ItemUseable - No */
     , (1006805,  19,         20) /* Value */
     , (1006805,  27,          0) /* ArmorType - None */
     , (1006805,  28,          1) /* ArmorLevel */
     , (1006805,  33,          1) /* Bonded - Bonded */
     , (1006805,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006805,  22, True ) /* Inscribable */
     , (1006805,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006805,  12, 0.10000000149011612) /* Shade */
     , (1006805,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006805,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006805,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006805,  16,       1) /* ArmorModVsCold */
     , (1006805,  17,       1) /* ArmorModVsFire */
     , (1006805,  18,       1) /* ArmorModVsAcid */
     , (1006805,  19,       1) /* ArmorModVsElectric */
     , (1006805, 110,       1) /* BulkMod */
     , (1006805, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006805,   1, 'Nexus Koujia Sleeves') /* Name */
     , (1006805,  15, 'A magnificent set of Koujia sleeves, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006805,  16, 'A magnificent set of Koujia sleeves, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006805,   1,   33554655) /* Setup */
     , (1006805,   3,  536870932) /* SoundTable */
     , (1006805,   6,   67108990) /* PaletteBase */
     , (1006805,   7,  268435851) /* ClothingBase */
     , (1006805,   8,  100670467) /* Icon */
     , (1006805,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:48:40.2795371-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
