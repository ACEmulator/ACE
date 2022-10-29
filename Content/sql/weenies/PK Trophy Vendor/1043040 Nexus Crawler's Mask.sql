DELETE FROM `weenie` WHERE `class_Id` = 1043040;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1043040, 'ace1043040-nexuscrawlersmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1043040,   1,          2) /* ItemType - Armor */
     , (1043040,   3,          3) /* PaletteTemplate - BluePurple */
     , (1043040,   4,      16384) /* ClothingPriority - Head */
     , (1043040,   5,          1) /* EncumbranceVal */
     , (1043040,   9,          1) /* ValidLocations - HeadWear */
     , (1043040,  16,          1) /* ItemUseable - No */
     , (1043040,  18,          1) /* UiEffects - Magical */
     , (1043040,  19,         20) /* Value */
     , (1043040,  28,          1) /* ArmorLevel */
     , (1043040,  53,        101) /* PlacementPosition - Resting */
     , (1043040,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1043040, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1043040,  11, True ) /* IgnoreCollisions */
     , (1043040,  13, True ) /* Ethereal */
     , (1043040,  14, True ) /* GravityStatus */
     , (1043040,  19, True ) /* Attackable */
     , (1043040,  22, True ) /* Inscribable */
     , (1043040,  85, True ) /* AppraisalHasAllowedWielder */
     , (1043040,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1043040,   5, -0.05000000074505806) /* ManaRate */
     , (1043040,  13,       1) /* ArmorModVsSlash */
     , (1043040,  14,       1) /* ArmorModVsPierce */
     , (1043040,  15,       1) /* ArmorModVsBludgeon */
     , (1043040,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1043040,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1043040,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1043040,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1043040, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1043040,   1, 'Nexus Crawler''s Mask') /* Name */
     , (1043040,  15, 'A mask crafted from the mask of the Apostate Nexus Master.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1043040,   1,   33561077) /* Setup */
     , (1043040,   3,  536870932) /* SoundTable */
     , (1043040,   6,   67108990) /* PaletteBase */
     , (1043040,   7,  268437424) /* ClothingBase */
     , (1043040,   8,  100691344) /* Icon */
     , (1043040,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:54:30.8776699-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
