DELETE FROM `weenie` WHERE `class_Id` = 1046551;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1046551, 'ace1046551-oyoroigauntlets', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1046551,   1,          2) /* ItemType - Armor */
     , (1046551,   4,      32768) /* ClothingPriority - Hands */
     , (1046551,   5,        0) /* EncumbranceVal */
     , (1046551,   9,         32) /* ValidLocations - HandWear */
     , (1046551,  16,          1) /* ItemUseable - No */
     , (1046551,  19,         20) /* Value */
     , (1046551,  28,          0) /* ArmorLevel */
     , (1046551,  33,          1) /* Bonded - Bonded */
     , (1046551,  53,        101) /* PlacementPosition - Resting */
     , (1046551,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1046551,  11, True ) /* IgnoreCollisions */
     , (1046551,  13, True ) /* Ethereal */
     , (1046551,  14, True ) /* GravityStatus */
     , (1046551,  19, True ) /* Attackable */
     , (1046551,  22, True ) /* Inscribable */
     , (1046551, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1046551,   5, -0.03333330154418945) /* ManaRate */
     , (1046551,  13, 2.9000000953674316) /* ArmorModVsSlash */
     , (1046551,  14, 3.200000047683716) /* ArmorModVsPierce */
     , (1046551,  15, 2.9000000953674316) /* ArmorModVsBludgeon */
     , (1046551,  16, 2.299999952316284) /* ArmorModVsCold */
     , (1046551,  17, 2.299999952316284) /* ArmorModVsFire */
     , (1046551,  18,     2.5) /* ArmorModVsAcid */
     , (1046551,  19, 2.299999952316284) /* ArmorModVsElectric */
     , (1046551, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1046551,   1, 'O-Yoroi Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1046551,   1,   33554648) /* Setup */
     , (1046551,   3,  536870932) /* SoundTable */
     , (1046551,   6,   67108990) /* PaletteBase */
     , (1046551,   7,  268437550) /* ClothingBase */
     , (1046551,   8,  100675987) /* Icon */
     , (1046551,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:00:54.3946259-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
