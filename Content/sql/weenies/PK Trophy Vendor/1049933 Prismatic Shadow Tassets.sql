DELETE FROM `weenie` WHERE `class_Id` = 1049933;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049933, 'ace1049933-prismaticshadowtassets', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049933,   1,          2) /* ItemType - Armor */
     , (1049933,   3,          9) /* PaletteTemplate - Grey */
     , (1049933,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (1049933,   5,        919) /* EncumbranceVal */
     , (1049933,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (1049933,  16,          1) /* ItemUseable - No */
     , (1049933,  19,         20) /* Value */
     , (1049933,  28,          1) /* ArmorLevel */
     , (1049933,  33,          1) /* Bonded - Bonded */
     , (1049933,  36,       9999) /* ResistMagic */
     , (1049933,  53,        101) /* PlacementPosition - Resting */
     , (1049933,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049933, 158,          7) /* WieldRequirements - Level */
     , (1049933, 159,          1) /* WieldSkillType - Axe */
     , (1049933, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049933,  11, True ) /* IgnoreCollisions */
     , (1049933,  13, True ) /* Ethereal */
     , (1049933,  14, True ) /* GravityStatus */
     , (1049933,  19, True ) /* Attackable */
     , (1049933,  22, True ) /* Inscribable */
     , (1049933,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049933,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049933,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049933,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049933,  16,       2) /* ArmorModVsCold */
     , (1049933,  17,       2) /* ArmorModVsFire */
     , (1049933,  18,       2) /* ArmorModVsAcid */
     , (1049933,  19,       2) /* ArmorModVsElectric */
     , (1049933,  39, 1.3300000429153442) /* DefaultScale */
     , (1049933, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049933,   1, 'Prismatic Shadow Tassets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049933,   1,   33554656) /* Setup */
     , (1049933,   3,  536870932) /* SoundTable */
     , (1049933,   7,  268437579) /* ClothingBase */
     , (1049933,   8,  100693093) /* Icon */
     , (1049933,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:56:31.9865088-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette, unsellable\n",
  "IsDone": false
}
*/
