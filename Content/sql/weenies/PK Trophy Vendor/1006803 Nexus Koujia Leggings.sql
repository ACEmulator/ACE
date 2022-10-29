DELETE FROM `weenie` WHERE `class_Id` = 1006803;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006803, 'ace1006803-nexuskoujialeggings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006803,   1,          2) /* ItemType - Armor */
     , (1006803,   3,          2) /* PaletteTemplate - Blue */
     , (1006803,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (1006803,   5,       3100) /* EncumbranceVal */
     , (1006803,   8,       1350) /* Mass */
     , (1006803,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (1006803,  16,          1) /* ItemUseable - No */
     , (1006803,  19,         20) /* Value */
     , (1006803,  27,          0) /* ArmorType - None */
     , (1006803,  28,          1) /* ArmorLevel */
     , (1006803,  33,          1) /* Bonded - Bonded */
     , (1006803,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006803,  22, True ) /* Inscribable */
     , (1006803,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006803,  12, 0.10000000149011612) /* Shade */
     , (1006803,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006803,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006803,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006803,  16,       1) /* ArmorModVsCold */
     , (1006803,  17,       1) /* ArmorModVsFire */
     , (1006803,  18,       1) /* ArmorModVsAcid */
     , (1006803,  19,       1) /* ArmorModVsElectric */
     , (1006803, 110,       1) /* BulkMod */
     , (1006803, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006803,   1, 'Nexus Koujia Leggings') /* Name */
     , (1006803,  15, 'A magnificent set of Koujia leggings, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006803,  16, 'A magnificent set of Koujia leggings, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006803,   1,   33554856) /* Setup */
     , (1006803,   3,  536870932) /* SoundTable */
     , (1006803,   6,   67108990) /* PaletteBase */
     , (1006803,   7,  268435849) /* ClothingBase */
     , (1006803,   8,  100670459) /* Icon */
     , (1006803,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:48:27.7435304-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
