DELETE FROM `weenie` WHERE `class_Id` = 1049909;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049909, 'ace1049909-prismaticshadowgauntlets', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049909,   1,          2) /* ItemType - Armor */
     , (1049909,   3,          9) /* PaletteTemplate - Grey */
     , (1049909,   4,      32768) /* ClothingPriority - Hands */
     , (1049909,   5,        919) /* EncumbranceVal */
     , (1049909,   9,         32) /* ValidLocations - HandWear */
     , (1049909,  16,          1) /* ItemUseable - No */
     , (1049909,  19,         20) /* Value */
     , (1049909,  28,          1) /* ArmorLevel */
     , (1049909,  33,          1) /* Bonded - Bonded */
     , (1049909,  36,       9999) /* ResistMagic */
     , (1049909,  53,        101) /* PlacementPosition - Resting */
     , (1049909,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049909, 158,          7) /* WieldRequirements - Level */
     , (1049909, 159,          1) /* WieldSkillType - Axe */
     , (1049909, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049909,  11, True ) /* IgnoreCollisions */
     , (1049909,  13, True ) /* Ethereal */
     , (1049909,  14, True ) /* GravityStatus */
     , (1049909,  19, True ) /* Attackable */
     , (1049909,  22, True ) /* Inscribable */
     , (1049909,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049909,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049909,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049909,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049909,  16,       2) /* ArmorModVsCold */
     , (1049909,  17,       2) /* ArmorModVsFire */
     , (1049909,  18,       2) /* ArmorModVsAcid */
     , (1049909,  19,       2) /* ArmorModVsElectric */
     , (1049909, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049909,   1, 'Prismatic Shadow Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049909,   1,   33554648) /* Setup */
     , (1049909,   3,  536870932) /* SoundTable */
     , (1049909,   7,  268437582) /* ClothingBase */
     , (1049909,   8,  100693096) /* Icon */
     , (1049909,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:58:20.6244117-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette, unsellable\n",
  "IsDone": false
}
*/
