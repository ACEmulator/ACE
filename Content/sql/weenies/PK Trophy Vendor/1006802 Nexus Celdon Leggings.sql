DELETE FROM `weenie` WHERE `class_Id` = 1006802;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006802, 'ace1006802-nexusceldonleggings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006802,   1,          2) /* ItemType - Armor */
     , (1006802,   3,          2) /* PaletteTemplate - Blue */
     , (1006802,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (1006802,   5,       3300) /* EncumbranceVal */
     , (1006802,   8,       1200) /* Mass */
     , (1006802,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (1006802,  16,          1) /* ItemUseable - No */
     , (1006802,  19,         20) /* Value */
     , (1006802,  27,          0) /* ArmorType - None */
     , (1006802,  28,          1) /* ArmorLevel */
     , (1006802,  33,          1) /* Bonded - Bonded */
     , (1006802,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006802,  22, True ) /* Inscribable */
     , (1006802,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006802,  12, 0.10000000149011612) /* Shade */
     , (1006802,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006802,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006802,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006802,  16,       1) /* ArmorModVsCold */
     , (1006802,  17,       1) /* ArmorModVsFire */
     , (1006802,  18,       1) /* ArmorModVsAcid */
     , (1006802,  19,       1) /* ArmorModVsElectric */
     , (1006802, 110,       1) /* BulkMod */
     , (1006802, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006802,   1, 'Nexus Celdon Leggings') /* Name */
     , (1006802,  15, 'A magnificent set of Celdon leggings, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006802,  16, 'A magnificent set of Celdon leggings, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006802,   1,   33554856) /* Setup */
     , (1006802,   3,  536870932) /* SoundTable */
     , (1006802,   6,   67108990) /* PaletteBase */
     , (1006802,   7,  268435844) /* ClothingBase */
     , (1006802,   8,  100670419) /* Icon */
     , (1006802,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:49:04.86848-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
