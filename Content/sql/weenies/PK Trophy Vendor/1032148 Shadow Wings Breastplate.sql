DELETE FROM `weenie` WHERE `class_Id` = 1032148;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1032148, 'ace1032148-shadowwingsbreastplate', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1032148,   1,          2) /* ItemType - Armor */
     , (1032148,   3,         39) /* PaletteTemplate - Black */
     , (1032148,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1032148,   5,          1) /* EncumbranceVal */
     , (1032148,   8,        140) /* Mass */
     , (1032148,   9,        512) /* ValidLocations - ChestArmor */
     , (1032148,  16,          1) /* ItemUseable - No */
     , (1032148,  19,         20) /* Value */
     , (1032148,  27,          2) /* ArmorType - Leather */
     , (1032148,  28,          1) /* ArmorLevel */
     , (1032148,  53,        101) /* PlacementPosition - Resting */
     , (1032148,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1032148, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1032148,  11, True ) /* IgnoreCollisions */
     , (1032148,  13, True ) /* Ethereal */
     , (1032148,  14, True ) /* GravityStatus */
     , (1032148,  19, True ) /* Attackable */
     , (1032148,  22, True ) /* Inscribable */
     , (1032148, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1032148,  12, 0.6600000262260437) /* Shade */
     , (1032148,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1032148,  14,       1) /* ArmorModVsPierce */
     , (1032148,  15,       1) /* ArmorModVsBludgeon */
     , (1032148,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1032148,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1032148,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1032148,  19,       1) /* ArmorModVsElectric */
     , (1032148, 110, 1.6699999570846558) /* BulkMod */
     , (1032148, 111,     2.5) /* SizeMod */
     , (1032148, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1032148,   1, 'Shadow Wings Breastplate') /* Name */
     , (1032148,  16, 'A modified Shadow Breastplate. Shadowy wings protrude from the shoulders.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1032148,   1,   33559762) /* Setup */
     , (1032148,   3,  536870932) /* SoundTable */
     , (1032148,   6,   67108990) /* PaletteBase */
     , (1032148,   7,  268437067) /* ClothingBase */
     , (1032148,   8,  100688450) /* Icon */
     , (1032148,  22,  872415275) /* PhysicsEffectTable */
     , (1032148,  36,  234881042) /* MutateFilter */
     , (1032148,  46,  939524146) /* TsysMutationFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T13:27:41.2480445-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "-Marked as done",
  "IsDone": true
}
*/
