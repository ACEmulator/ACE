DELETE FROM `weenie` WHERE `class_Id` = 1006609;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006609, 'ace1006609-greaterceldonshadowleggings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006609,   1,          2) /* ItemType - Armor */
     , (1006609,   3,         21) /* PaletteTemplate - Gold */
     , (1006609,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (1006609,   5,          0) /* EncumbranceVal */
     , (1006609,   8,       1200) /* Mass */
     , (1006609,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (1006609,  16,          1) /* ItemUseable - No */
     , (1006609,  19,         20) /* Value */
     , (1006609,  27,         32) /* ArmorType - Metal */
     , (1006609,  28,          1) /* ArmorLevel */
     , (1006609,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006609,  22, True ) /* Inscribable */
     , (1006609,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006609,  12, 0.6000000238418579) /* Shade */
     , (1006609,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006609,  14,       1) /* ArmorModVsPierce */
     , (1006609,  15,       1) /* ArmorModVsBludgeon */
     , (1006609,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1006609,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1006609,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1006609,  19,     0.5) /* ArmorModVsElectric */
     , (1006609, 110,       1) /* BulkMod */
     , (1006609, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006609,   1, 'Greater Celdon Shadow Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006609,   1,   33554856) /* Setup */
     , (1006609,   3,  536870932) /* SoundTable */
     , (1006609,   6,   67108990) /* PaletteBase */
     , (1006609,   7,  268435844) /* ClothingBase */
     , (1006609,   8,  100670419) /* Icon */
     , (1006609,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T18:10:31.4895428-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
