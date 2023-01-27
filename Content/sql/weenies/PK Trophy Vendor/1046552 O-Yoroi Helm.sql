DELETE FROM `weenie` WHERE `class_Id` = 1046552;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1046552, 'ace1046552-oyoroihelm', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1046552,   1,          2) /* ItemType - Armor */
     , (1046552,   4,      16384) /* ClothingPriority - Head */
     , (1046552,   5,        533) /* EncumbranceVal */
     , (1046552,   9,          1) /* ValidLocations - HeadWear */
     , (1046552,  16,          1) /* ItemUseable - No */
     , (1046552,  19,         20) /* Value */
     , (1046552,  28,          1) /* ArmorLevel */
     , (1046552,  33,          1) /* Bonded - Bonded */
     , (1046552,  53,        101) /* PlacementPosition - Resting */
     , (1046552,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1046552,  11, True ) /* IgnoreCollisions */
     , (1046552,  13, True ) /* Ethereal */
     , (1046552,  14, True ) /* GravityStatus */
     , (1046552,  19, True ) /* Attackable */
     , (1046552,  22, True ) /* Inscribable */
     , (1046552, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1046552,   5, -0.03333330154418945) /* ManaRate */
     , (1046552,  13, 2.9000000953674316) /* ArmorModVsSlash */
     , (1046552,  14, 3.200000047683716) /* ArmorModVsPierce */
     , (1046552,  15, 2.9000000953674316) /* ArmorModVsBludgeon */
     , (1046552,  16, 2.299999952316284) /* ArmorModVsCold */
     , (1046552,  17, 2.299999952316284) /* ArmorModVsFire */
     , (1046552,  18,     2.5) /* ArmorModVsAcid */
     , (1046552,  19, 2.299999952316284) /* ArmorModVsElectric */
     , (1046552, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1046552,   1, 'O-Yoroi Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1046552,   1,   33555248) /* Setup */
     , (1046552,   3,  536870932) /* SoundTable */
     , (1046552,   6,   67108990) /* PaletteBase */
     , (1046552,   7,  268437551) /* ClothingBase */
     , (1046552,   8,  100692814) /* Icon */
     , (1046552,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:00:32.300157-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": false
}
*/
