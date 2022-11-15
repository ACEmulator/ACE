DELETE FROM `weenie` WHERE `class_Id` = 1005851;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1005851, 'ace1005851-faranrobewithhood', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1005851,   1,          4) /* ItemType - Armor */
     , (1005851,   3,          4) /* PaletteTemplate - Brown */
     , (1005851,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1005851,   5,        0) /* EncumbranceVal */
     , (1005851,   8,        150) /* Mass */
     , (1005851,   9,        512) /* ValidLocations - ChestArmor */
     , (1005851,  16,          1) /* ItemUseable - No */
     , (1005851,  19,         20) /* Value */
     , (1005851,  27,         32) /* ArmorType - Metal */
     , (1005851,  28,          0) /* ArmorLevel */
     , (1005851,  53,        101) /* PlacementPosition - Resting */
     , (1005851,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1005851,  11, True ) /* IgnoreCollisions */
     , (1005851,  13, True ) /* Ethereal */
     , (1005851,  14, True ) /* GravityStatus */
     , (1005851,  19, True ) /* Attackable */
     , (1005851,  22, True ) /* Inscribable */
     , (1005851, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1005851,  12,     0.5) /* Shade */
     , (1005851,  13, 0.800000011920929) /* ArmorModVsSlash */
     , (1005851,  14, 0.800000011920929) /* ArmorModVsPierce */
     , (1005851,  15,       1) /* ArmorModVsBludgeon */
     , (1005851,  16, 0.20000000298023224) /* ArmorModVsCold */
     , (1005851,  17, 0.20000000298023224) /* ArmorModVsFire */
     , (1005851,  18, 0.10000000149011612) /* ArmorModVsAcid */
     , (1005851,  19, 0.20000000298023224) /* ArmorModVsElectric */
     , (1005851, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1005851,   1, 'Faran Robe with Hood') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1005851,   1,   33554854) /* Setup */
     , (1005851,   3,  536870932) /* SoundTable */
     , (1005851,   6,   67108990) /* PaletteBase */
     , (1005851,   7,  268435854) /* ClothingBase */
     , (1005851,   8,  100670354) /* Icon */
     , (1005851,  22,  872415275) /* PhysicsEffectTable */
     , (1005851,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T18:36:48.6525162-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": false
}
*/
