DELETE FROM `weenie` WHERE `class_Id` = 1051854;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1051854, 'ace1051854-rynthidminionoftormentsmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1051854,   1,          2) /* ItemType - Armor */
     , (1051854,   3,         14) /* PaletteTemplate - Red */
     , (1051854,   4,      16384) /* ClothingPriority - Head */
     , (1051854,   5,          1) /* EncumbranceVal */
     , (1051854,   9,          1) /* ValidLocations - HeadWear */
     , (1051854,  16,          1) /* ItemUseable - No */
     , (1051854,  18,          1) /* UiEffects - Magical */
     , (1051854,  19,         20) /* Value */
     , (1051854,  28,          1) /* ArmorLevel */
     , (1051854,  53,        101) /* PlacementPosition - Resting */
     , (1051854,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1051854, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1051854,  11, True ) /* IgnoreCollisions */
     , (1051854,  13, True ) /* Ethereal */
     , (1051854,  14, True ) /* GravityStatus */
     , (1051854,  19, True ) /* Attackable */
     , (1051854,  22, True ) /* Inscribable */
     , (1051854,  85, True ) /* AppraisalHasAllowedWielder */
     , (1051854,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1051854,   5, -0.05000000074505806) /* ManaRate */
     , (1051854,  13,       1) /* ArmorModVsSlash */
     , (1051854,  14,       1) /* ArmorModVsPierce */
     , (1051854,  15,       1) /* ArmorModVsBludgeon */
     , (1051854,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1051854,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1051854,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1051854,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1051854, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1051854,   1, 'Rynthid Minion of Torment''s Mask') /* Name */
     , (1051854,  15, 'A mask crafted from the damaged mask of a Rynthid Minion of Torment.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1051854,   1,   33561594) /* Setup */
     , (1051854,   3,  536870932) /* SoundTable */
     , (1051854,   7,  268437592) /* ClothingBase */
     , (1051854,   8,  100693219) /* Icon */
     , (1051854,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:52:25.47298-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette template int",
  "IsDone": false
}
*/
