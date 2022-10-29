DELETE FROM `weenie` WHERE `class_Id` = 1051858;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1051858, 'ace1051858-rynthidsorcereroftormentsmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1051858,   1,          2) /* ItemType - Armor */
     , (1051858,   3,         14) /* PaletteTemplate - Red */
     , (1051858,   4,      16384) /* ClothingPriority - Head */
     , (1051858,   5,          1) /* EncumbranceVal */
     , (1051858,   9,          1) /* ValidLocations - HeadWear */
     , (1051858,  16,          1) /* ItemUseable - No */
     , (1051858,  18,          1) /* UiEffects - Magical */
     , (1051858,  19,         20) /* Value */
     , (1051858,  28,          1) /* ArmorLevel */
     , (1051858,  33,          1) /* Bonded - Bonded */
     , (1051858,  53,        101) /* PlacementPosition - Resting */
     , (1051858,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1051858, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1051858,  11, True ) /* IgnoreCollisions */
     , (1051858,  13, True ) /* Ethereal */
     , (1051858,  14, True ) /* GravityStatus */
     , (1051858,  19, True ) /* Attackable */
     , (1051858,  22, True ) /* Inscribable */
     , (1051858,  85, True ) /* AppraisalHasAllowedWielder */
     , (1051858,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1051858,   5, -0.05000000074505806) /* ManaRate */
     , (1051858,  13,       1) /* ArmorModVsSlash */
     , (1051858,  14,       1) /* ArmorModVsPierce */
     , (1051858,  15,       1) /* ArmorModVsBludgeon */
     , (1051858,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1051858,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1051858,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1051858,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1051858, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1051858,   1, 'Rynthid Sorcerer of Torment''s Mask') /* Name */
     , (1051858,  15, 'A mask crafted from the damaged mask of a Rynthid Sorcerer of Torment.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1051858,   1,   33561594) /* Setup */
     , (1051858,   3,  536870932) /* SoundTable */
     , (1051858,   7,  268437593) /* ClothingBase */
     , (1051858,   8,  100693222) /* Icon */
     , (1051858,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:52:57.7762176-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette template int",
  "IsDone": false
}
*/
