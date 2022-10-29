DELETE FROM `weenie` WHERE `class_Id` = 1008806;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008806, 'ace1008806-fenmalainhelmofthelightbringer', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008806,   1,          2) /* ItemType - Armor */
     , (1008806,   3,         13) /* PaletteTemplate - Purple */
     , (1008806,   4,      16384) /* ClothingPriority - Head */
     , (1008806,   5,        200) /* EncumbranceVal */
     , (1008806,   8,        200) /* Mass */
     , (1008806,   9,          1) /* ValidLocations - HeadWear */
     , (1008806,  16,          1) /* ItemUseable - No */
     , (1008806,  19,         20) /* Value */
     , (1008806,  27,         32) /* ArmorType - Metal */
     , (1008806,  28,          1) /* ArmorLevel */
     , (1008806,  33,          1) /* Bonded - Bonded */
     , (1008806,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008806, 114,          1) /* Attuned - Attuned */
     , (1008806, 150,        103) /* HookPlacement - Hook */
     , (1008806, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008806,  22, True ) /* Inscribable */
     , (1008806,  23, True ) /* DestroyOnSell */
     , (1008806,  69, False) /* IsSellable */
     , (1008806,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008806,  12, 0.8999999761581421) /* Shade */
     , (1008806,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1008806,  14,       1) /* ArmorModVsPierce */
     , (1008806,  15,       1) /* ArmorModVsBludgeon */
     , (1008806,  16,       0) /* ArmorModVsCold */
     , (1008806,  17,       0) /* ArmorModVsFire */
     , (1008806,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1008806,  19,       0) /* ArmorModVsElectric */
     , (1008806, 110,       1) /* BulkMod */
     , (1008806, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008806,   1, 'Fenmalain Helm of the Lightbringer') /* Name */
     , (1008806,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008806,   1,   33556941) /* Setup */
     , (1008806,   3,  536870932) /* SoundTable */
     , (1008806,   6,   67108990) /* PaletteBase */
     , (1008806,   7,  268436105) /* ClothingBase */
     , (1008806,   8,  100671289) /* Icon */
     , (1008806,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:11:31.8886133-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
