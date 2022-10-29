DELETE FROM `weenie` WHERE `class_Id` = 1008809;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008809, 'ace1008809-heraldshelmofthelightbringer', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008809,   1,          2) /* ItemType - Armor */
     , (1008809,   3,         39) /* PaletteTemplate - Black */
     , (1008809,   4,      16384) /* ClothingPriority - Head */
     , (1008809,   5,        200) /* EncumbranceVal */
     , (1008809,   8,        200) /* Mass */
     , (1008809,   9,          1) /* ValidLocations - HeadWear */
     , (1008809,  16,          1) /* ItemUseable - No */
     , (1008809,  19,         20) /* Value */
     , (1008809,  27,         32) /* ArmorType - Metal */
     , (1008809,  28,          1) /* ArmorLevel */
     , (1008809,  33,          1) /* Bonded - Bonded */
     , (1008809,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008809, 114,          1) /* Attuned - Attuned */
     , (1008809, 150,        103) /* HookPlacement - Hook */
     , (1008809, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008809,  22, True ) /* Inscribable */
     , (1008809,  23, True ) /* DestroyOnSell */
     , (1008809,  69, False) /* IsSellable */
     , (1008809,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008809,  12, 0.6600000262260437) /* Shade */
     , (1008809,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1008809,  14,       1) /* ArmorModVsPierce */
     , (1008809,  15,       1) /* ArmorModVsBludgeon */
     , (1008809,  16,       0) /* ArmorModVsCold */
     , (1008809,  17,       0) /* ArmorModVsFire */
     , (1008809,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1008809,  19,       0) /* ArmorModVsElectric */
     , (1008809, 110,       1) /* BulkMod */
     , (1008809, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008809,   1, 'Herald''s Helm of the Lightbringer') /* Name */
     , (1008809,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008809,   1,   33556941) /* Setup */
     , (1008809,   3,  536870932) /* SoundTable */
     , (1008809,   6,   67108990) /* PaletteBase */
     , (1008809,   7,  268436105) /* ClothingBase */
     , (1008809,   8,  100671291) /* Icon */
     , (1008809,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:14:56.309001-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
