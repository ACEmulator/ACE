DELETE FROM `weenie` WHERE `class_Id` = 1026498;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1026498, 'ace1026498-crownofanointedblood', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1026498,   1,          2) /* ItemType - Armor */
     , (1026498,   3,          8) /* PaletteTemplate - Green */
     , (1026498,   4,      16384) /* ClothingPriority - Head */
     , (1026498,   5,          1) /* EncumbranceVal */
     , (1026498,   8,        200) /* Mass */
     , (1026498,   9,          1) /* ValidLocations - HeadWear */
     , (1026498,  19,         20) /* Value */
     , (1026498,  27,         32) /* ArmorType - Metal */
     , (1026498,  28,          1) /* ArmorLevel */
     , (1026498,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1026498, 150,        103) /* HookPlacement - Hook */
     , (1026498, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1026498,  22, True ) /* Inscribable */
     , (1026498,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1026498,   5, -0.032999999821186066) /* ManaRate */
     , (1026498,  12, 0.6600000262260437) /* Shade */
     , (1026498,  13,       1) /* ArmorModVsSlash */
     , (1026498,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1026498,  15, 1.7999999523162842) /* ArmorModVsBludgeon */
     , (1026498,  16, 0.6499999761581421) /* ArmorModVsCold */
     , (1026498,  17, 0.6499999761581421) /* ArmorModVsFire */
     , (1026498,  18, 1.399999976158142) /* ArmorModVsAcid */
     , (1026498,  19, 0.6499999761581421) /* ArmorModVsElectric */
     , (1026498, 110,       1) /* BulkMod */
     , (1026498, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1026498,   1, 'Crown of Anointed Blood') /* Name */
     , (1026498,  15, 'This veiled crown was likely used during the rites of the Falatacot.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1026498,   1,   33558591) /* Setup */
     , (1026498,   3,  536870932) /* SoundTable */
     , (1026498,   6,   67108990) /* PaletteBase */
     , (1026498,   7,  268436791) /* ClothingBase */
     , (1026498,   8,  100675772) /* Icon */
     , (1026498,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T06:14:02.7018395-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
