DELETE FROM `weenie` WHERE `class_Id` = 450075;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450075, 'ace450075-maskofthehopeslayertailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450075,   1,          2) /* ItemType - Armor */
     , (450075,   3,          9) /* PaletteTemplate - Grey */
     , (450075,   4,      16384) /* ClothingPriority - Head */
     , (450075,   5,        200) /* EncumbranceVal */
     , (450075,   9,          1) /* ValidLocations - HeadWear */
     , (450075,  16,          1) /* ItemUseable - No */
     , (450075,  19,         20) /* Value */
     , (450075,  27,          2) /* ArmorType - Leather */
     , (450075,  28,          1) /* ArmorLevel */
     , (450075,  53,        101) /* PlacementPosition - Resting */
     , (450075,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450075, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450075,  11, True ) /* IgnoreCollisions */
     , (450075,  13, True ) /* Ethereal */
     , (450075,  14, True ) /* GravityStatus */
     , (450075,  19, True ) /* Attackable */
     , (450075,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450075,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (450075,  14,       1) /* ArmorModVsPierce */
     , (450075,  15,       1) /* ArmorModVsBludgeon */
     , (450075,  16, 0.800000011920929) /* ArmorModVsCold */
     , (450075,  17, 0.800000011920929) /* ArmorModVsFire */
     , (450075,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (450075,  19, 0.800000011920929) /* ArmorModVsElectric */
     , (450075, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450075,   1, 'Mask of the Hopeslayer') /* Name */
     , (450075,  15, 'A helm, crated in the manner of the Shadow Armors, but drawing the appearance of the Hopeslayer himself.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450075,   1,   33560103) /* Setup */
     , (450075,   3,  536870932) /* SoundTable */
     , (450075,   7,  268437152) /* ClothingBase */
     , (450075,   8,  100689128) /* Icon */
     , (450075,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-21T23:32:34.4725792-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Fixing palette base",
  "IsDone": false
}
*/
