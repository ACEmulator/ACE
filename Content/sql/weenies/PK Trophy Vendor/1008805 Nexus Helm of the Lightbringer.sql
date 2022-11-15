DELETE FROM `weenie` WHERE `class_Id` = 1008805;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008805, 'ace1008805-nexushelmofthelightbringer', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008805,   1,          2) /* ItemType - Armor */
     , (1008805,   3,          2) /* PaletteTemplate - Blue */
     , (1008805,   4,      16384) /* ClothingPriority - Head */
     , (1008805,   5,        0) /* EncumbranceVal */
     , (1008805,   8,        200) /* Mass */
     , (1008805,   9,          1) /* ValidLocations - HeadWear */
     , (1008805,  16,          1) /* ItemUseable - No */
     , (1008805,  19,         20) /* Value */
     , (1008805,  27,         32) /* ArmorType - Metal */
     , (1008805,  28,          1) /* ArmorLevel */
     , (1008805,  33,          1) /* Bonded - Bonded */
     , (1008805,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008805, 114,          1) /* Attuned - Attuned */
     , (1008805, 150,        103) /* HookPlacement - Hook */
     , (1008805, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008805,  22, True ) /* Inscribable */
     , (1008805,  23, True ) /* DestroyOnSell */
     , (1008805,  69, False) /* IsSellable */
     , (1008805,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008805,  12, 0.6600000262260437) /* Shade */
     , (1008805,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1008805,  14,       1) /* ArmorModVsPierce */
     , (1008805,  15,       1) /* ArmorModVsBludgeon */
     , (1008805,  16,       0) /* ArmorModVsCold */
     , (1008805,  17,       0) /* ArmorModVsFire */
     , (1008805,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1008805,  19,       0) /* ArmorModVsElectric */
     , (1008805, 110,       1) /* BulkMod */
     , (1008805, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008805,   1, 'Nexus Helm of the Lightbringer') /* Name */
     , (1008805,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008805,   1,   33556941) /* Setup */
     , (1008805,   3,  536870932) /* SoundTable */
     , (1008805,   6,   67108990) /* PaletteBase */
     , (1008805,   7,  268436105) /* ClothingBase */
     , (1008805,   8,  100671292) /* Icon */
     , (1008805,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:12:42.3685908-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
