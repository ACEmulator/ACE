DELETE FROM `weenie` WHERE `class_Id` = 1007705;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1007705, 'ace1007705-greaterceldonshadowleggings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1007705,   1,          2) /* ItemType - Armor */
     , (1007705,   3,         21) /* PaletteTemplate - Gold */
     , (1007705,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (1007705,   5,       2100) /* EncumbranceVal */
     , (1007705,   8,       1200) /* Mass */
     , (1007705,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (1007705,  16,          1) /* ItemUseable - No */
     , (1007705,  19,         20) /* Value */
     , (1007705,  27,         32) /* ArmorType - Metal */
     , (1007705,  28,        170) /* ArmorLevel */
     , (1007705,  33,          1) /* Bonded - Bonded */
     , (1007705,  53,        101) /* PlacementPosition - Resting */
     , (1007705,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1007705,  11, True ) /* IgnoreCollisions */
     , (1007705,  13, True ) /* Ethereal */
     , (1007705,  14, True ) /* GravityStatus */
     , (1007705,  19, True ) /* Attackable */
     , (1007705,  22, True ) /* Inscribable */
     , (1007705,  23, True ) /* DestroyOnSell */
     , (1007705,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1007705,  12,     0.5) /* Shade */
     , (1007705,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1007705,  14,       1) /* ArmorModVsPierce */
     , (1007705,  15,       1) /* ArmorModVsBludgeon */
     , (1007705,  16, 0.10000000149011612) /* ArmorModVsCold */
     , (1007705,  17, 0.10000000149011612) /* ArmorModVsFire */
     , (1007705,  18, 0.10000000149011612) /* ArmorModVsAcid */
     , (1007705,  19, 0.10000000149011612) /* ArmorModVsElectric */
     , (1007705, 110,       1) /* BulkMod */
     , (1007705, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1007705,   1, 'Greater Celdon Shadow Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1007705,   1,   33554856) /* Setup */
     , (1007705,   3,  536870932) /* SoundTable */
     , (1007705,   6,   67108990) /* PaletteBase */
     , (1007705,   7,  268435844) /* ClothingBase */
     , (1007705,   8,  100670419) /* Icon */
     , (1007705,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T08:36:42.6216359-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dmitri",
  "IsDone": false
}
*/
