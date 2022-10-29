DELETE FROM `weenie` WHERE `class_Id` = 1006804;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006804, 'ace1006804-nexusceldonsleeves', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006804,   1,          2) /* ItemType - Armor */
     , (1006804,   3,          2) /* PaletteTemplate - Blue */
     , (1006804,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (1006804,   5,       1800) /* EncumbranceVal */
     , (1006804,   8,        700) /* Mass */
     , (1006804,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (1006804,  16,          1) /* ItemUseable - No */
     , (1006804,  19,         20) /* Value */
     , (1006804,  27,          0) /* ArmorType - None */
     , (1006804,  28,          1) /* ArmorLevel */
     , (1006804,  33,          1) /* Bonded - Bonded */
     , (1006804,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006804,  22, True ) /* Inscribable */
     , (1006804,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006804,  12, 0.10000000149011612) /* Shade */
     , (1006804,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006804,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006804,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006804,  16,       1) /* ArmorModVsCold */
     , (1006804,  17,       1) /* ArmorModVsFire */
     , (1006804,  18,       1) /* ArmorModVsAcid */
     , (1006804,  19,       1) /* ArmorModVsElectric */
     , (1006804, 110,       1) /* BulkMod */
     , (1006804, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006804,   1, 'Nexus Celdon Sleeves') /* Name */
     , (1006804,  15, 'A magnificent set of Celdon sleeves, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006804,  16, 'A magnificent set of Celdon sleeves, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006804,   1,   33554655) /* Setup */
     , (1006804,   3,  536870932) /* SoundTable */
     , (1006804,   6,   67108990) /* PaletteBase */
     , (1006804,   7,  268435847) /* ClothingBase */
     , (1006804,   8,  100670427) /* Icon */
     , (1006804,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:48:52.752057-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
