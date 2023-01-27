DELETE FROM `weenie` WHERE `class_Id` = 1025703;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1025703, 'ace1025703-dappersuit', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1025703,   1,          4) /* ItemType - Armor */
     , (1025703,   3,          2) /* PaletteTemplate - Blue */
     , (1025703,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1025703,   5,        500) /* EncumbranceVal */
     , (1025703,   8,        150) /* Mass */
     , (1025703,   9,        512) /* ValidLocations - ChestArmor */
     , (1025703,  16,          1) /* ItemUseable - No */
     , (1025703,  19,         20) /* Value */
     , (1025703,  27,         32) /* ArmorType - Metal */
     , (1025703,  28,          1) /* ArmorLevel */
     , (1025703,  53,        101) /* PlacementPosition - Resting */
     , (1025703,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1025703,  11, True ) /* IgnoreCollisions */
     , (1025703,  13, True ) /* Ethereal */
     , (1025703,  14, True ) /* GravityStatus */
     , (1025703,  19, True ) /* Attackable */
     , (1025703,  22, True ) /* Inscribable */
     , (1025703,  69, False) /* IsSellable */
     , (1025703, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1025703,  12,       0) /* Shade */
     , (1025703,  13, 0.10000000149011612) /* ArmorModVsSlash */
     , (1025703,  14, 0.10000000149011612) /* ArmorModVsPierce */
     , (1025703,  15, 0.10000000149011612) /* ArmorModVsBludgeon */
     , (1025703,  16, 0.10000000149011612) /* ArmorModVsCold */
     , (1025703,  17, 0.10000000149011612) /* ArmorModVsFire */
     , (1025703,  18, 0.10000000149011612) /* ArmorModVsAcid */
     , (1025703,  19, 0.10000000149011612) /* ArmorModVsElectric */
     , (1025703, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1025703,   1, 'Dapper Suit') /* Name */
     , (1025703,  15, 'A suit designed by the Gharu''ndim tailor, Xuut. The fibers of the suit look as though they could withstand the dyeing process.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1025703,   1,   33554854) /* Setup */
     , (1025703,   3,  536870932) /* SoundTable */
     , (1025703,   6,   67108990) /* PaletteBase */
     , (1025703,   7,  268436721) /* ClothingBase */
     , (1025703,   8,  100675511) /* Icon */
     , (1025703,  22,  872415275) /* PhysicsEffectTable */
     , (1025703,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T21:37:43.154083-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
