DELETE FROM `weenie` WHERE `class_Id` = 1006801;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006801, 'ace1006801-nexusamulileggings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006801,   1,          2) /* ItemType - Armor */
     , (1006801,   3,          2) /* PaletteTemplate - Blue */
     , (1006801,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (1006801,   5,       2688) /* EncumbranceVal */
     , (1006801,   8,       1275) /* Mass */
     , (1006801,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (1006801,  16,          1) /* ItemUseable - No */
     , (1006801,  19,         20) /* Value */
     , (1006801,  27,          2) /* ArmorType - Leather */
     , (1006801,  28,          1) /* ArmorLevel */
     , (1006801,  33,          1) /* Bonded - Bonded */
     , (1006801,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006801,  22, True ) /* Inscribable */
     , (1006801,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006801,  12, 0.10000000149011612) /* Shade */
     , (1006801,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006801,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006801,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006801,  16,       1) /* ArmorModVsCold */
     , (1006801,  17,       1) /* ArmorModVsFire */
     , (1006801,  18,       1) /* ArmorModVsAcid */
     , (1006801,  19,       1) /* ArmorModVsElectric */
     , (1006801, 110,       1) /* BulkMod */
     , (1006801, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006801,   1, 'Nexus Amuli Leggings') /* Name */
     , (1006801,  15, 'A magnificent set of Amuli leggings, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006801,  16, 'A magnificent set of Amuli leggings, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006801,   1,   33554856) /* Setup */
     , (1006801,   3,  536870932) /* SoundTable */
     , (1006801,   6,   67108990) /* PaletteBase */
     , (1006801,   7,  268435872) /* ClothingBase */
     , (1006801,   8,  100670443) /* Icon */
     , (1006801,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:08:48.1318316-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
