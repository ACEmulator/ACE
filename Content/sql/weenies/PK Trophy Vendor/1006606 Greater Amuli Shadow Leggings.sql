DELETE FROM `weenie` WHERE `class_Id` = 1006606;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006606, 'ace1006606-greateramulishadowleggings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006606,   1,          2) /* ItemType - Armor */
     , (1006606,   3,         18) /* PaletteTemplate - YellowBrown */
     , (1006606,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (1006606,   5,          0) /* EncumbranceVal */
     , (1006606,   8,       1275) /* Mass */
     , (1006606,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (1006606,  16,          1) /* ItemUseable - No */
     , (1006606,  19,         20) /* Value */
     , (1006606,  27,          2) /* ArmorType - Leather */
     , (1006606,  28,          1) /* ArmorLevel */
     , (1006606,  53,        101) /* PlacementPosition - Resting */
     , (1006606,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006606,  11, True ) /* IgnoreCollisions */
     , (1006606,  13, True ) /* Ethereal */
     , (1006606,  14, True ) /* GravityStatus */
     , (1006606,  19, True ) /* Attackable */
     , (1006606,  22, True ) /* Inscribable */
     , (1006606,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006606,  12, 0.30000001192092896) /* Shade */
     , (1006606,  13,       1) /* ArmorModVsSlash */
     , (1006606,  14, 0.800000011920929) /* ArmorModVsPierce */
     , (1006606,  15,       1) /* ArmorModVsBludgeon */
     , (1006606,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1006606,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1006606,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1006606,  19, 0.6000000238418579) /* ArmorModVsElectric */
     , (1006606, 110,       1) /* BulkMod */
     , (1006606, 111,       1) /* SizeMod */
     , (1006606, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006606,   1, 'Greater Amuli Shadow Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006606,   1,   33554856) /* Setup */
     , (1006606,   3,  536870932) /* SoundTable */
     , (1006606,   6,   67108990) /* PaletteBase */
     , (1006606,   7,  268435872) /* ClothingBase */
     , (1006606,   8,  100670443) /* Icon */
     , (1006606,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T18:11:14.5618301-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
