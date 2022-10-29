DELETE FROM `weenie` WHERE `class_Id` = 1032163;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1032163, 'ace1032163-twoheadedsnowmanmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1032163,   1,          2) /* ItemType - Armor */
     , (1032163,   3,          4) /* PaletteTemplate - Brown */
     , (1032163,   4,      16384) /* ClothingPriority - Head */
     , (1032163,   5,          0) /* EncumbranceVal */
     , (1032163,   9,          1) /* ValidLocations - HeadWear */
     , (1032163,  16,          1) /* ItemUseable - No */
     , (1032163,  19,         20) /* Value */
     , (1032163,  28,          1) /* ArmorLevel */
     , (1032163,  53,        101) /* PlacementPosition - Resting */
     , (1032163,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1032163, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1032163,   1, False) /* Stuck */
     , (1032163,  11, True ) /* IgnoreCollisions */
     , (1032163,  13, True ) /* Ethereal */
     , (1032163,  14, True ) /* GravityStatus */
     , (1032163,  19, True ) /* Attackable */
     , (1032163,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1032163,  12,       0) /* Shade */
     , (1032163,  13,     0.5) /* ArmorModVsSlash */
     , (1032163,  14, 0.4000000059604645) /* ArmorModVsPierce */
     , (1032163,  15, 0.4000000059604645) /* ArmorModVsBludgeon */
     , (1032163,  16, 0.6000000238418579) /* ArmorModVsCold */
     , (1032163,  17, 0.20000000298023224) /* ArmorModVsFire */
     , (1032163,  18,    0.75) /* ArmorModVsAcid */
     , (1032163,  19, 0.3499999940395355) /* ArmorModVsElectric */
     , (1032163, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1032163,   1, 'Two Headed Snowman Mask') /* Name */
     , (1032163,  16, 'A mask crafted from the hollowed-out heads of a Two Headed Snowman.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1032163,   1,   33559771) /* Setup */
     , (1032163,   3,  536870932) /* SoundTable */
     , (1032163,   7,  268437076) /* ClothingBase */
     , (1032163,   8,  100688431) /* Icon */
     , (1032163,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:22:27.2900684-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "added palette and shade\n\ncustom",
  "IsDone": false
}
*/
