DELETE FROM `weenie` WHERE `class_Id` = 1040801;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1040801, 'ace1040801-nexuscrawlersmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1040801,   1,          2) /* ItemType - Armor */
     , (1040801,   3,         39) /* PaletteTemplate - Black */
     , (1040801,   4,      16384) /* ClothingPriority - Head */
     , (1040801,   5,          1) /* EncumbranceVal */
     , (1040801,   9,          1) /* ValidLocations - HeadWear */
     , (1040801,  16,          1) /* ItemUseable - No */
     , (1040801,  18,          1) /* UiEffects - Magical */
     , (1040801,  19,         20) /* Value */
     , (1040801,  28,          1) /* ArmorLevel */
     , (1040801,  33,          1) /* Bonded - Bonded */
     , (1040801,  53,        101) /* PlacementPosition - Resting */
     , (1040801,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1040801, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1040801,  11, True ) /* IgnoreCollisions */
     , (1040801,  13, True ) /* Ethereal */
     , (1040801,  14, True ) /* GravityStatus */
     , (1040801,  19, True ) /* Attackable */
     , (1040801,  22, True ) /* Inscribable */
     , (1040801,  85, True ) /* AppraisalHasAllowedWielder */
     , (1040801,  99, False) /* Ivoryable */
     , (1040801, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1040801,   5, -0.05000000074505806) /* ManaRate */
     , (1040801,  13,       1) /* ArmorModVsSlash */
     , (1040801,  14,       1) /* ArmorModVsPierce */
     , (1040801,  15,       1) /* ArmorModVsBludgeon */
     , (1040801,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1040801,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1040801,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1040801,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1040801, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1040801,   1, 'Nexus Crawler''s Mask') /* Name */
     , (1040801,  15, 'A mask crafted from the mask of the Apostate Nexus Master.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1040801,   1,   33556827) /* Setup */
     , (1040801,   3,  536870932) /* SoundTable */
     , (1040801,   6,   67108990) /* PaletteBase */
     , (1040801,   7,  268437424) /* ClothingBase */
     , (1040801,   8,  100671028) /* Icon */
     , (1040801,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:54:07.1380083-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
