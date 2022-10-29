DELETE FROM `weenie` WHERE `class_Id` = 1008808;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008808, 'ace1008808-shendolainhelmofthelightbringer', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008808,   1,          2) /* ItemType - Armor */
     , (1008808,   3,         13) /* PaletteTemplate - Purple */
     , (1008808,   4,      16384) /* ClothingPriority - Head */
     , (1008808,   5,        200) /* EncumbranceVal */
     , (1008808,   8,        200) /* Mass */
     , (1008808,   9,          1) /* ValidLocations - HeadWear */
     , (1008808,  16,          1) /* ItemUseable - No */
     , (1008808,  19,         20) /* Value */
     , (1008808,  27,         32) /* ArmorType - Metal */
     , (1008808,  28,          1) /* ArmorLevel */
     , (1008808,  33,          1) /* Bonded - Bonded */
     , (1008808,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008808, 114,          1) /* Attuned - Attuned */
     , (1008808, 150,        103) /* HookPlacement - Hook */
     , (1008808, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008808,  22, True ) /* Inscribable */
     , (1008808,  23, True ) /* DestroyOnSell */
     , (1008808,  69, False) /* IsSellable */
     , (1008808,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008808,  12, 0.20000000298023224) /* Shade */
     , (1008808,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1008808,  14,       1) /* ArmorModVsPierce */
     , (1008808,  15,       1) /* ArmorModVsBludgeon */
     , (1008808,  16,       0) /* ArmorModVsCold */
     , (1008808,  17,       0) /* ArmorModVsFire */
     , (1008808,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1008808,  19,       0) /* ArmorModVsElectric */
     , (1008808, 110,       1) /* BulkMod */
     , (1008808, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008808,   1, 'Shendolain Helm of the Lightbringer') /* Name */
     , (1008808,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008808,   1,   33556941) /* Setup */
     , (1008808,   3,  536870932) /* SoundTable */
     , (1008808,   6,   67108990) /* PaletteBase */
     , (1008808,   7,  268436105) /* ClothingBase */
     , (1008808,   8,  100671293) /* Icon */
     , (1008808,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:13:48.2673208-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
