DELETE FROM `weenie` WHERE `class_Id` = 1049917;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049917, 'ace1049917-prismaticshadowgreaves', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049917,   1,          2) /* ItemType - Armor */
     , (1049917,   3,          9) /* PaletteTemplate - Grey */
     , (1049917,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (1049917,   5,        919) /* EncumbranceVal */
     , (1049917,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (1049917,  16,          1) /* ItemUseable - No */
     , (1049917,  19,         20) /* Value */
     , (1049917,  28,          1) /* ArmorLevel */
     , (1049917,  33,          1) /* Bonded - Bonded */
     , (1049917,  36,       9999) /* ResistMagic */
     , (1049917,  53,        101) /* PlacementPosition - Resting */
     , (1049917,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049917, 158,          7) /* WieldRequirements - Level */
     , (1049917, 159,          1) /* WieldSkillType - Axe */
     , (1049917, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049917,  11, True ) /* IgnoreCollisions */
     , (1049917,  13, True ) /* Ethereal */
     , (1049917,  14, True ) /* GravityStatus */
     , (1049917,  19, True ) /* Attackable */
     , (1049917,  22, True ) /* Inscribable */
     , (1049917,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049917,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049917,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049917,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049917,  16,       2) /* ArmorModVsCold */
     , (1049917,  17,       2) /* ArmorModVsFire */
     , (1049917,  18,       2) /* ArmorModVsAcid */
     , (1049917,  19,       2) /* ArmorModVsElectric */
     , (1049917,  39, 1.3300000429153442) /* DefaultScale */
     , (1049917, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049917,   1, 'Prismatic Shadow Greaves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049917,   1,   33554641) /* Setup */
     , (1049917,   3,  536870932) /* SoundTable */
     , (1049917,   7,  268437584) /* ClothingBase */
     , (1049917,   8,  100693098) /* Icon */
     , (1049917,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:57:46.8551838-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette, unsellable\n",
  "IsDone": false
}
*/
