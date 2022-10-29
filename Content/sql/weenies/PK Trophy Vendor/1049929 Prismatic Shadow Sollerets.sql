DELETE FROM `weenie` WHERE `class_Id` = 1049929;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049929, 'ace1049929-prismaticshadowsollerets', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049929,   1,          2) /* ItemType - Armor */
     , (1049929,   3,          9) /* PaletteTemplate - Grey */
     , (1049929,   4,      65536) /* ClothingPriority - Feet */
     , (1049929,   5,        540) /* EncumbranceVal */
     , (1049929,   9,        256) /* ValidLocations - FootWear */
     , (1049929,  16,          1) /* ItemUseable - No */
     , (1049929,  19,         20) /* Value */
     , (1049929,  28,          1) /* ArmorLevel */
     , (1049929,  33,          1) /* Bonded - Bonded */
     , (1049929,  36,       9999) /* ResistMagic */
     , (1049929,  53,        101) /* PlacementPosition - Resting */
     , (1049929,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049929, 158,          7) /* WieldRequirements - Level */
     , (1049929, 159,          1) /* WieldSkillType - Axe */
     , (1049929, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049929,  11, True ) /* IgnoreCollisions */
     , (1049929,  13, True ) /* Ethereal */
     , (1049929,  14, True ) /* GravityStatus */
     , (1049929,  19, True ) /* Attackable */
     , (1049929,  22, True ) /* Inscribable */
     , (1049929,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049929,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049929,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049929,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049929,  16,       2) /* ArmorModVsCold */
     , (1049929,  17,       2) /* ArmorModVsFire */
     , (1049929,  18,       2) /* ArmorModVsAcid */
     , (1049929,  19,       2) /* ArmorModVsElectric */
     , (1049929, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049929,   1, 'Prismatic Shadow Sollerets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049929,   1,   33554654) /* Setup */
     , (1049929,   3,  536870932) /* SoundTable */
     , (1049929,   7,  268437587) /* ClothingBase */
     , (1049929,   8,  100693101) /* Icon */
     , (1049929,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:56:49.4715293-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette, unsellable\n",
  "IsDone": false
}
*/
