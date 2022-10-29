DELETE FROM `weenie` WHERE `class_Id` = 1008807;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008807, 'ace1008807-caulnalainhelmofthelightbringer', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008807,   1,          2) /* ItemType - Armor */
     , (1008807,   3,         13) /* PaletteTemplate - Purple */
     , (1008807,   4,      16384) /* ClothingPriority - Head */
     , (1008807,   5,        200) /* EncumbranceVal */
     , (1008807,   8,        200) /* Mass */
     , (1008807,   9,          1) /* ValidLocations - HeadWear */
     , (1008807,  16,          1) /* ItemUseable - No */
     , (1008807,  19,         20) /* Value */
     , (1008807,  27,         32) /* ArmorType - Metal */
     , (1008807,  28,          1) /* ArmorLevel */
     , (1008807,  33,          1) /* Bonded - Bonded */
     , (1008807,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008807, 114,          1) /* Attuned - Attuned */
     , (1008807, 150,        103) /* HookPlacement - Hook */
     , (1008807, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008807,  22, True ) /* Inscribable */
     , (1008807,  23, True ) /* DestroyOnSell */
     , (1008807,  69, False) /* IsSellable */
     , (1008807,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008807,  12,     0.5) /* Shade */
     , (1008807,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1008807,  14,       1) /* ArmorModVsPierce */
     , (1008807,  15,       1) /* ArmorModVsBludgeon */
     , (1008807,  16,       0) /* ArmorModVsCold */
     , (1008807,  17,       0) /* ArmorModVsFire */
     , (1008807,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1008807,  19,       0) /* ArmorModVsElectric */
     , (1008807, 110,       1) /* BulkMod */
     , (1008807, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008807,   1, 'Caulnalain Helm of the Lightbringer') /* Name */
     , (1008807,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008807,   1,   33556941) /* Setup */
     , (1008807,   3,  536870932) /* SoundTable */
     , (1008807,   6,   67108990) /* PaletteBase */
     , (1008807,   7,  268436105) /* ClothingBase */
     , (1008807,   8,  100671288) /* Icon */
     , (1008807,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:09:59.6842372-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
