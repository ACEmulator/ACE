DELETE FROM `weenie` WHERE `class_Id` = 1014932;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1014932, 'ace1014932-crimpedhat', 4, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1014932,   1,          4) /* ItemType - Armor */
     , (1014932,   3,          2) /* PaletteTemplate - Blue */
     , (1014932,   4,      16384) /* ClothingPriority - Head */
     , (1014932,   5,         50) /* EncumbranceVal */
     , (1014932,   8,         15) /* Mass */
     , (1014932,   9,          1) /* ValidLocations - HeadWear */
     , (1014932,  16,          1) /* ItemUseable - No */
     , (1014932,  19,         20) /* Value */
     , (1014932,  27,         32) /* ArmorType - Metal */
     , (1014932,  28,          1) /* ArmorLevel */
     , (1014932,  53,        101) /* PlacementPosition - Resting */
     , (1014932,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1014932,  11, True ) /* IgnoreCollisions */
     , (1014932,  13, True ) /* Ethereal */
     , (1014932,  14, True ) /* GravityStatus */
     , (1014932,  19, True ) /* Attackable */
     , (1014932,  22, True ) /* Inscribable */
     , (1014932,  69, False) /* IsSellable */
     , (1014932, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1014932,  12, 0.6600000262260437) /* Shade */
     , (1014932,  13, 0.800000011920929) /* ArmorModVsSlash */
     , (1014932,  14, 0.800000011920929) /* ArmorModVsPierce */
     , (1014932,  15,       1) /* ArmorModVsBludgeon */
     , (1014932,  16, 0.20000000298023224) /* ArmorModVsCold */
     , (1014932,  17, 0.20000000298023224) /* ArmorModVsFire */
     , (1014932,  18, 0.10000000149011612) /* ArmorModVsAcid */
     , (1014932,  19, 0.20000000298023224) /* ArmorModVsElectric */
     , (1014932, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1014932,   1, 'Crimped Hat') /* Name */
     , (1014932,  15, 'A hat, given as a reward for helping out the Royal Guard''s investigation into the attempt on High Queen Elysa''s life.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1014932,   1,   33554643) /* Setup */
     , (1014932,   3,  536870932) /* SoundTable */
     , (1014932,   6,   67108990) /* PaletteBase */
     , (1014932,   7,  268436720) /* ClothingBase */
     , (1014932,   8,  100675479) /* Icon */
     , (1014932,  22,  872415275) /* PhysicsEffectTable */
     , (1014932,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:47:23.2589172-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
