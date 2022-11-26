DELETE FROM `weenie` WHERE `class_Id` = 1009622;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1009622, 'ace1009622-chefshat', 4, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1009622,   1,          4) /* ItemType - Armor */
     , (1009622,   3,         20) /* PaletteTemplate - Silver */
     , (1009622,   4,      16384) /* ClothingPriority - Head */
     , (1009622,   5,          1) /* EncumbranceVal */
     , (1009622,   8,         15) /* Mass */
     , (1009622,   9,          1) /* ValidLocations - HeadWear */
     , (1009622,  16,          1) /* ItemUseable - No */
     , (1009622,  19,         20) /* Value */
     , (1009622,  27,         32) /* ArmorType - Metal */
     , (1009622,  28,          0) /* ArmorLevel */
     , (1009622,  53,        101) /* PlacementPosition - Resting */
     , (1009622,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1009622, 150,        103) /* HookPlacement - Hook */
     , (1009622, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1009622,  11, True ) /* IgnoreCollisions */
     , (1009622,  13, True ) /* Ethereal */
     , (1009622,  14, True ) /* GravityStatus */
     , (1009622,  19, True ) /* Attackable */
     , (1009622,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1009622,  12, 0.6600000262260437) /* Shade */
     , (1009622,  13, 0.800000011920929) /* ArmorModVsSlash */
     , (1009622,  14, 0.800000011920929) /* ArmorModVsPierce */
     , (1009622,  15,       1) /* ArmorModVsBludgeon */
     , (1009622,  16, 0.20000000298023224) /* ArmorModVsCold */
     , (1009622,  17, 0.20000000298023224) /* ArmorModVsFire */
     , (1009622,  18, 0.10000000149011612) /* ArmorModVsAcid */
     , (1009622,  19, 0.20000000298023224) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1009622,   1, 'Chef''s Hat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1009622,   1,   33557034) /* Setup */
     , (1009622,   3,  536870932) /* SoundTable */
     , (1009622,   6,   67108990) /* PaletteBase */
     , (1009622,   7,  268436180) /* ClothingBase */
     , (1009622,   8,  100668247) /* Icon */
     , (1009622,  22,  872415275) /* PhysicsEffectTable */
     , (1009622,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-25T19:13:49.6290259-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
