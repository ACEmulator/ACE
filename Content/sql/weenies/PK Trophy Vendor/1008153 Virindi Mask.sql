DELETE FROM `weenie` WHERE `class_Id` = 1008153;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008153, 'ace1008153-virindimask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008153,   1,          2) /* ItemType - Armor */
     , (1008153,   3,          3) /* PaletteTemplate - BluePurple */
     , (1008153,   4,      16384) /* ClothingPriority - Head */
     , (1008153,   5,          0) /* EncumbranceVal */
     , (1008153,   8,         75) /* Mass */
     , (1008153,   9,          1) /* ValidLocations - HeadWear */
     , (1008153,  16,          1) /* ItemUseable - No */
     , (1008153,  18,          1) /* UiEffects - Magical */
     , (1008153,  19,         20) /* Value */
     , (1008153,  27,          2) /* ArmorType - Leather */
     , (1008153,  28,          1) /* ArmorLevel */
     , (1008153,  53,        101) /* PlacementPosition - Resting */
     , (1008153,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008153, 150,        103) /* HookPlacement - Hook */
     , (1008153, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008153,  11, True ) /* IgnoreCollisions */
     , (1008153,  13, True ) /* Ethereal */
     , (1008153,  14, True ) /* GravityStatus */
     , (1008153,  19, True ) /* Attackable */
     , (1008153,  22, True ) /* Inscribable */
     , (1008153,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008153,   5, -0.0333000011742115) /* ManaRate */
     , (1008153,  12, 0.6600000262260437) /* Shade */
     , (1008153,  13,       1) /* ArmorModVsSlash */
     , (1008153,  14,       1) /* ArmorModVsPierce */
     , (1008153,  15,       1) /* ArmorModVsBludgeon */
     , (1008153,  16,       2) /* ArmorModVsCold */
     , (1008153,  17,       1) /* ArmorModVsFire */
     , (1008153,  18,       1) /* ArmorModVsAcid */
     , (1008153,  19,       2) /* ArmorModVsElectric */
     , (1008153, 110,       1) /* BulkMod */
     , (1008153, 111,       1) /* SizeMod */
     , (1008153, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008153,   1, 'Virindi Mask') /* Name */
     , (1008153,  16, 'A mask made out of some indeterminable metal. It seems to reflect light in a strange manner.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008153,   1,   33556827) /* Setup */
     , (1008153,   3,  536870932) /* SoundTable */
     , (1008153,   6,   67108990) /* PaletteBase */
     , (1008153,   7,  268436258) /* ClothingBase */
     , (1008153,   8,  100671028) /* Icon */
     , (1008153,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:12:11.9314968-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
