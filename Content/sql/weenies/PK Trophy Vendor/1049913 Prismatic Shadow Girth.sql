DELETE FROM `weenie` WHERE `class_Id` = 1049913;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049913, 'ace1049913-prismaticshadowgirth', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049913,   1,          2) /* ItemType - Armor */
     , (1049913,   3,          9) /* PaletteTemplate - Grey */
     , (1049913,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (1049913,   5,       1099) /* EncumbranceVal */
     , (1049913,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (1049913,  16,          1) /* ItemUseable - No */
     , (1049913,  19,         20) /* Value */
     , (1049913,  28,          1) /* ArmorLevel */
     , (1049913,  33,          1) /* Bonded - Bonded */
     , (1049913,  36,       9999) /* ResistMagic */
     , (1049913,  53,        101) /* PlacementPosition - Resting */
     , (1049913,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049913, 158,          7) /* WieldRequirements - Level */
     , (1049913, 159,          1) /* WieldSkillType - Axe */
     , (1049913, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049913,  11, True ) /* IgnoreCollisions */
     , (1049913,  13, True ) /* Ethereal */
     , (1049913,  14, True ) /* GravityStatus */
     , (1049913,  19, True ) /* Attackable */
     , (1049913,  22, True ) /* Inscribable */
     , (1049913,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049913,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049913,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049913,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049913,  16,       2) /* ArmorModVsCold */
     , (1049913,  17,       2) /* ArmorModVsFire */
     , (1049913,  18,       2) /* ArmorModVsAcid */
     , (1049913,  19,       2) /* ArmorModVsElectric */
     , (1049913, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049913,   1, 'Prismatic Shadow Girth') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049913,   1,   33554647) /* Setup */
     , (1049913,   3,  536870932) /* SoundTable */
     , (1049913,   7,  268437583) /* ClothingBase */
     , (1049913,   8,  100693097) /* Icon */
     , (1049913,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:58:04.9901352-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette, unsellable\n",
  "IsDone": false
}
*/
