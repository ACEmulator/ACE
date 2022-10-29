DELETE FROM `weenie` WHERE `class_Id` = 1011986;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1011986, 'ace1011986-energycrown', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1011986,   1,          2) /* ItemType - Armor */
     , (1011986,   3,         82) /* PaletteTemplate - PinkPurple */
     , (1011986,   4,      16384) /* ClothingPriority - Head */
     , (1011986,   5,          1) /* EncumbranceVal */
     , (1011986,   8,        200) /* Mass */
     , (1011986,   9,          1) /* ValidLocations - HeadWear */
     , (1011986,  16,          1) /* ItemUseable - No */
     , (1011986,  18,          1) /* UiEffects - Magical */
     , (1011986,  19,         20) /* Value */
     , (1011986,  27,         32) /* ArmorType - Metal */
     , (1011986,  28,          1) /* ArmorLevel */
     , (1011986,  53,        101) /* PlacementPosition - Resting */
     , (1011986,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1011986, 150,        103) /* HookPlacement - Hook */
     , (1011986, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1011986,  11, True ) /* IgnoreCollisions */
     , (1011986,  13, True ) /* Ethereal */
     , (1011986,  14, True ) /* GravityStatus */
     , (1011986,  19, True ) /* Attackable */
     , (1011986,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1011986,   5, -0.05000000074505806) /* ManaRate */
     , (1011986,  12, 0.6600000262260437) /* Shade */
     , (1011986,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1011986,  14,       1) /* ArmorModVsPierce */
     , (1011986,  15,       1) /* ArmorModVsBludgeon */
     , (1011986,  16, 0.4000000059604645) /* ArmorModVsCold */
     , (1011986,  17, 0.4000000059604645) /* ArmorModVsFire */
     , (1011986,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1011986,  19, 0.4000000059604645) /* ArmorModVsElectric */
     , (1011986, 110,       1) /* BulkMod */
     , (1011986, 111,       1) /* SizeMod */
     , (1011986, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1011986,   1, 'Energy Crown') /* Name */
     , (1011986,  15, 'A crown made of some luminescent metal.') /* ShortDesc */
     , (1011986,  16, 'A crown made of some sort of solidified energy.  When you wear it, you feel revitalized.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1011986,   1,   33557336) /* Setup */
     , (1011986,   3,  536870932) /* SoundTable */
     , (1011986,   6,   67108990) /* PaletteBase */
     , (1011986,   7,  268436259) /* ClothingBase */
     , (1011986,   8,  100669185) /* Icon */
     , (1011986,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T23:45:26.203367-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
