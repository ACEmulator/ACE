DELETE FROM `weenie` WHERE `class_Id` = 1049901;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1049901, 'ace1049901-prismaticshadowbracers', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1049901,   1,          2) /* ItemType - Armor */
     , (1049901,   3,          9) /* PaletteTemplate - Grey */
     , (1049901,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (1049901,   5,        540) /* EncumbranceVal */
     , (1049901,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (1049901,  16,          1) /* ItemUseable - No */
     , (1049901,  19,         20) /* Value */
     , (1049901,  28,          1) /* ArmorLevel */
     , (1049901,  33,          1) /* Bonded - Bonded */
     , (1049901,  36,       9999) /* ResistMagic */
     , (1049901,  53,        101) /* PlacementPosition - Resting */
     , (1049901,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1049901, 158,          7) /* WieldRequirements - Level */
     , (1049901, 159,          1) /* WieldSkillType - Axe */
     , (1049901, 160,        115) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1049901,  11, True ) /* IgnoreCollisions */
     , (1049901,  13, True ) /* Ethereal */
     , (1049901,  14, True ) /* GravityStatus */
     , (1049901,  19, True ) /* Attackable */
     , (1049901,  22, True ) /* Inscribable */
     , (1049901,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1049901,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1049901,  14, 1.399999976158142) /* ArmorModVsPierce */
     , (1049901,  15, 1.399999976158142) /* ArmorModVsBludgeon */
     , (1049901,  16,       2) /* ArmorModVsCold */
     , (1049901,  17,       2) /* ArmorModVsFire */
     , (1049901,  18,       2) /* ArmorModVsAcid */
     , (1049901,  19,       2) /* ArmorModVsElectric */
     , (1049901, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1049901,   1, 'Prismatic Shadow Bracers') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1049901,   1,   33554641) /* Setup */
     , (1049901,   3,  536870932) /* SoundTable */
     , (1049901,   7,  268437580) /* ClothingBase */
     , (1049901,   8,  100693094) /* Icon */
     , (1049901,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:58:56.239365-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette and unsellable",
  "IsDone": false
}
*/
