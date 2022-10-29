DELETE FROM `weenie` WHERE `class_Id` = 1049921;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049921, 'ace1049921-prismaticshadowhelm', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049921,   1,          2) /* ItemType - Armor */
     , (1049921,   3,          9) /* PaletteTemplate - Grey */
     , (1049921,   4,      16384) /* ClothingPriority - Head */
     , (1049921,   5,        666) /* EncumbranceVal */
     , (1049921,   9,          1) /* ValidLocations - HeadWear */
     , (1049921,  16,          1) /* ItemUseable - No */
     , (1049921,  19,         20) /* Value */
     , (1049921,  28,          1) /* ArmorLevel */
     , (1049921,  33,          1) /* Bonded - Bonded */
     , (1049921,  36,       9999) /* ResistMagic */
     , (1049921,  53,        101) /* PlacementPosition - Resting */
     , (1049921,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049921, 158,          7) /* WieldRequirements - Level */
     , (1049921, 159,          1) /* WieldSkillType - Axe */
     , (1049921, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049921,  11, True ) /* IgnoreCollisions */
     , (1049921,  13, True ) /* Ethereal */
     , (1049921,  14, True ) /* GravityStatus */
     , (1049921,  19, True ) /* Attackable */
     , (1049921,  22, True ) /* Inscribable */
     , (1049921,  69, False) /* IsSellable */
     , (1049921, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049921,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049921,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049921,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049921,  16,       2) /* ArmorModVsCold */
     , (1049921,  17,       2) /* ArmorModVsFire */
     , (1049921,  18,       2) /* ArmorModVsAcid */
     , (1049921,  19,       2) /* ArmorModVsElectric */
     , (1049921, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049921,   1, 'Prismatic Shadow Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049921,   1,   33555048) /* Setup */
     , (1049921,   3,  536870932) /* SoundTable */
     , (1049921,   7,  268437585) /* ClothingBase */
     , (1049921,   8,  100693099) /* Icon */
     , (1049921,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-18T14:28:47.5878564-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette, unsellable\n",
  "IsDone": false
}
*/
