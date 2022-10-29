DELETE FROM `weenie` WHERE `class_Id` = 1011998;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1011998, 'ace1011998-virindiinquisitorsmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1011998,   1,          2) /* ItemType - Armor */
     , (1011998,   3,         14) /* PaletteTemplate - Red */
     , (1011998,   4,      16384) /* ClothingPriority - Head */
     , (1011998,   5,          0) /* EncumbranceVal */
     , (1011998,   8,         75) /* Mass */
     , (1011998,   9,          1) /* ValidLocations - HeadWear */
     , (1011998,  16,          1) /* ItemUseable - No */
     , (1011998,  18,          1) /* UiEffects - Magical */
     , (1011998,  19,         20) /* Value */
     , (1011998,  27,          2) /* ArmorType - Leather */
     , (1011998,  28,          1) /* ArmorLevel */
     , (1011998,  53,        101) /* PlacementPosition - Resting */
     , (1011998,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1011998, 150,        103) /* HookPlacement - Hook */
     , (1011998, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1011998,  11, True ) /* IgnoreCollisions */
     , (1011998,  13, True ) /* Ethereal */
     , (1011998,  14, True ) /* GravityStatus */
     , (1011998,  19, True ) /* Attackable */
     , (1011998,  22, True ) /* Inscribable */
     , (1011998,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1011998,   5, -0.0333000011742115) /* ManaRate */
     , (1011998,  12, 0.6600000262260437) /* Shade */
     , (1011998,  13,       1) /* ArmorModVsSlash */
     , (1011998,  14,       1) /* ArmorModVsPierce */
     , (1011998,  15,       1) /* ArmorModVsBludgeon */
     , (1011998,  16,       2) /* ArmorModVsCold */
     , (1011998,  17,       1) /* ArmorModVsFire */
     , (1011998,  18,       1) /* ArmorModVsAcid */
     , (1011998,  19,       2) /* ArmorModVsElectric */
     , (1011998, 110,       1) /* BulkMod */
     , (1011998, 111,       1) /* SizeMod */
     , (1011998, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1011998,   1, 'Virindi Inquisitor''s Mask') /* Name */
     , (1011998,  16, 'A mask made out of some indeterminable metal. It seems to reflect light in a strange manner. Occasionally the eyes glow with a violet radiance.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1011998,   1,   33556827) /* Setup */
     , (1011998,   3,  536870932) /* SoundTable */
     , (1011998,   6,   67108990) /* PaletteBase */
     , (1011998,   7,  268436258) /* ClothingBase */
     , (1011998,   8,  100672106) /* Icon */
     , (1011998,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:12:42.8627723-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
