DELETE FROM `weenie` WHERE `class_Id` = 1006615;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006615, 'ace1006615-greaterceldonshadowsleeves', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006615,   1,          2) /* ItemType - Armor */
     , (1006615,   3,         21) /* PaletteTemplate - Gold */
     , (1006615,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (1006615,   5,          0) /* EncumbranceVal */
     , (1006615,   8,        700) /* Mass */
     , (1006615,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (1006615,  16,          1) /* ItemUseable - No */
     , (1006615,  19,         20) /* Value */
     , (1006615,  27,         32) /* ArmorType - Metal */
     , (1006615,  28,          1) /* ArmorLevel */
     , (1006615,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006615,  22, True ) /* Inscribable */
     , (1006615,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006615,  12, 0.6000000238418579) /* Shade */
     , (1006615,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006615,  14,       1) /* ArmorModVsPierce */
     , (1006615,  15,       1) /* ArmorModVsBludgeon */
     , (1006615,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1006615,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1006615,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1006615,  19,     0.5) /* ArmorModVsElectric */
     , (1006615, 110,       1) /* BulkMod */
     , (1006615, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006615,   1, 'Greater Celdon Shadow Sleeves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006615,   1,   33554655) /* Setup */
     , (1006615,   3,  536870932) /* SoundTable */
     , (1006615,   6,   67108990) /* PaletteBase */
     , (1006615,   7,  268435847) /* ClothingBase */
     , (1006615,   8,  100670427) /* Icon */
     , (1006615,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T18:10:16.1959633-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
