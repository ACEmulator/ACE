DELETE FROM `weenie` WHERE `class_Id` = 1049925;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049925, 'ace1049925-prismaticshadowpauldrons', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049925,   1,          2) /* ItemType - Armor */
     , (1049925,   3,          9) /* PaletteTemplate - Grey */
     , (1049925,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (1049925,   5,        720) /* EncumbranceVal */
     , (1049925,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (1049925,  16,          1) /* ItemUseable - No */
     , (1049925,  19,         20) /* Value */
     , (1049925,  28,          1) /* ArmorLevel */
     , (1049925,  33,          1) /* Bonded - Bonded */
     , (1049925,  36,       9999) /* ResistMagic */
     , (1049925,  53,        101) /* PlacementPosition - Resting */
     , (1049925,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049925, 158,          7) /* WieldRequirements - Level */
     , (1049925, 159,          1) /* WieldSkillType - Axe */
     , (1049925, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049925,  11, True ) /* IgnoreCollisions */
     , (1049925,  13, True ) /* Ethereal */
     , (1049925,  14, True ) /* GravityStatus */
     , (1049925,  19, True ) /* Attackable */
     , (1049925,  22, True ) /* Inscribable */
     , (1049925,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049925,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049925,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049925,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049925,  16,       2) /* ArmorModVsCold */
     , (1049925,  17,       2) /* ArmorModVsFire */
     , (1049925,  18,       2) /* ArmorModVsAcid */
     , (1049925,  19,       2) /* ArmorModVsElectric */
     , (1049925,  39, 1.100000023841858) /* DefaultScale */
     , (1049925, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049925,   1, 'Prismatic Shadow Pauldrons') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049925,   1,   33554641) /* Setup */
     , (1049925,   3,  536870932) /* SoundTable */
     , (1049925,   7,  268437586) /* ClothingBase */
     , (1049925,   8,  100693100) /* Icon */
     , (1049925,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:57:08.0156831-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette, unsellable\n",
  "IsDone": false
}
*/
