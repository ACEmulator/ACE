DELETE FROM `weenie` WHERE `class_Id` = 1046345;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1046345, 'ace1046345-oyoroileggings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1046345,   1,          2) /* ItemType - Armor */
     , (1046345,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (1046345,   5,       2247) /* EncumbranceVal */
     , (1046345,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (1046345,  16,          1) /* ItemUseable - No */
     , (1046345,  19,         20) /* Value */
     , (1046345,  28,          0) /* ArmorLevel */
     , (1046345,  33,          1) /* Bonded - Bonded */
     , (1046345,  53,        101) /* PlacementPosition - Resting */
     , (1046345,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1046345,  11, True ) /* IgnoreCollisions */
     , (1046345,  13, True ) /* Ethereal */
     , (1046345,  14, True ) /* GravityStatus */
     , (1046345,  19, True ) /* Attackable */
     , (1046345,  22, True ) /* Inscribable */
     , (1046345, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1046345,   5, -0.03333330154418945) /* ManaRate */
     , (1046345,  13, 2.9000000953674316) /* ArmorModVsSlash */
     , (1046345,  14, 3.200000047683716) /* ArmorModVsPierce */
     , (1046345,  15, 2.9000000953674316) /* ArmorModVsBludgeon */
     , (1046345,  16, 2.299999952316284) /* ArmorModVsCold */
     , (1046345,  17, 2.299999952316284) /* ArmorModVsFire */
     , (1046345,  18,     2.5) /* ArmorModVsAcid */
     , (1046345,  19, 2.299999952316284) /* ArmorModVsElectric */
     , (1046345, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1046345,   1, 'O-Yoroi Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1046345,   1,   33554856) /* Setup */
     , (1046345,   3,  536870932) /* SoundTable */
     , (1046345,   6,   67108990) /* PaletteBase */
     , (1046345,   7,  268437547) /* ClothingBase */
     , (1046345,   8,  100692824) /* Icon */
     , (1046345,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:01:13.0173977-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": false
}
*/
