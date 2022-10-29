DELETE FROM `weenie` WHERE `class_Id` = 1012207;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1012207, 'ace1012207-invisomask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1012207,   1,          2) /* ItemType - Armor */
     , (1012207,   3,         61) /* PaletteTemplate - White */
     , (1012207,   4,      16384) /* ClothingPriority - Head */
     , (1012207,   5,          0) /* EncumbranceVal */
     , (1012207,   8,         75) /* Mass */
     , (1012207,   9,          1) /* ValidLocations - HeadWear */
     , (1012207,  16,          1) /* ItemUseable - No */
     , (1012207,  19,         20) /* Value */
     , (1012207,  27,          2) /* ArmorType - Leather */
     , (1012207,  28,          1) /* ArmorLevel */
     , (1012207,  53,        101) /* PlacementPosition - Resting */
     , (1012207,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1012207,  11, True ) /* IgnoreCollisions */
     , (1012207,  13, True ) /* Ethereal */
     , (1012207,  14, True ) /* GravityStatus */
     , (1012207,  19, True ) /* Attackable */
     , (1012207,  22, True ) /* Inscribable */
     , (1012207,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1012207,  12, 0.6600000262260437) /* Shade */
     , (1012207,  13,     0.5) /* ArmorModVsSlash */
     , (1012207,  14,   0.375) /* ArmorModVsPierce */
     , (1012207,  15,    0.25) /* ArmorModVsBludgeon */
     , (1012207,  16,     0.5) /* ArmorModVsCold */
     , (1012207,  17,   0.375) /* ArmorModVsFire */
     , (1012207,  18,   0.125) /* ArmorModVsAcid */
     , (1012207,  19,   0.125) /* ArmorModVsElectric */
     , (1012207,  39,     0.5) /* DefaultScale */
     , (1012207, 110,       1) /* BulkMod */
     , (1012207, 111,       1) /* SizeMod */
     , (1012207, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1012207,   1, 'Inviso Mask') /* Name */
     , (1012207,  16, 'It seems to be an inside out Doll mask!  It''s amazing what information you can glean if you are observant enough.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1012207,   1,   33557364) /* Setup */
     , (1012207,   3,  536870932) /* SoundTable */
     , (1012207,   6,   67108990) /* PaletteBase */
     , (1012207,   7,  268436265) /* ClothingBase */
     , (1012207,   8,  100672219) /* Icon */
     , (1012207,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:45:02.9357149-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
