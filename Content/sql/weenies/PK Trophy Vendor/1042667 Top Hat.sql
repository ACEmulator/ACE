DELETE FROM `weenie` WHERE `class_Id` = 1042667;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1042667, 'ace1042667-tophat', 4, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1042667,   1,          4) /* ItemType - Armor */
     , (1042667,   3,         39) /* PaletteTemplate - Black */
     , (1042667,   4,      16384) /* ClothingPriority - Head */
     , (1042667,   5,          1) /* EncumbranceVal */
     , (1042667,   9,          1) /* ValidLocations - HeadWear */
     , (1042667,  16,          1) /* ItemUseable - No */
     , (1042667,  19,         20) /* Value */
     , (1042667,  28,          1) /* ArmorLevel */
     , (1042667,  53,        101) /* PlacementPosition - Resting */
     , (1042667,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1042667, 105,         10) /* ItemWorkmanship */
     , (1042667, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1042667,  11, True ) /* IgnoreCollisions */
     , (1042667,  13, True ) /* Ethereal */
     , (1042667,  14, True ) /* GravityStatus */
     , (1042667,  19, True ) /* Attackable */
     , (1042667,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1042667,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1042667,  14,       1) /* ArmorModVsPierce */
     , (1042667,  15,       1) /* ArmorModVsBludgeon */
     , (1042667,  16, 0.4000000059604645) /* ArmorModVsCold */
     , (1042667,  17, 0.4000000059604645) /* ArmorModVsFire */
     , (1042667,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1042667,  19, 0.4000000059604645) /* ArmorModVsElectric */
     , (1042667, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1042667,   1, 'Top Hat') /* Name */
     , (1042667,  15, 'A finely crafted Top Hat that can be tailored with other items.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1042667,   1,   33560948) /* Setup */
     , (1042667,   3,  536870932) /* SoundTable */
     , (1042667,   6,   67108990) /* PaletteBase */
     , (1042667,   7,  268437408) /* ClothingBase */
     , (1042667,   8,  100688217) /* Icon */
     , (1042667,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-21T06:12:16.1118939-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
