DELETE FROM `weenie` WHERE `class_Id` = 10049811;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10049811, 'ace10049811-iceheaumeoffrore', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10049811,   1,          2) /* ItemType - Armor */
     , (10049811,   3,          8) /* PaletteTemplate - Green */
     , (10049811,   4,      16384) /* ClothingPriority - Head */
     , (10049811,   5,       1100) /* EncumbranceVal */
     , (10049811,   8,        340) /* Mass */
     , (10049811,   9,          1) /* ValidLocations - HeadWear */
     , (10049811,  16,          1) /* ItemUseable - No */
     , (10049811,  18,        256) /* UiEffects - Acid */
     , (10049811,  19,         20) /* Value */
     , (10049811,  27,          0) /* ArmorType - None */
     , (10049811,  28,          1) /* ArmorLevel */
     , (10049811,  53,        101) /* PlacementPosition - Resting */
     , (10049811,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10049811, 150,        103) /* HookPlacement - Hook */
     , (10049811, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10049811,  11, True ) /* IgnoreCollisions */
     , (10049811,  13, True ) /* Ethereal */
     , (10049811,  14, True ) /* GravityStatus */
     , (10049811,  19, True ) /* Attackable */
     , (10049811,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10049811,  12, 0.800000011920929) /* Shade */
     , (10049811,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (10049811,  14,       1) /* ArmorModVsPierce */
     , (10049811,  15, 1.100000023841858) /* ArmorModVsBludgeon */
     , (10049811,  16,       2) /* ArmorModVsCold */
     , (10049811,  17,       2) /* ArmorModVsFire */
     , (10049811,  18, 0.699999988079071) /* ArmorModVsAcid */
     , (10049811,  19,       0) /* ArmorModVsElectric */
     , (10049811, 110,       1) /* BulkMod */
     , (10049811, 111,       1) /* SizeMod */
     , (10049811, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10049811,   1, 'Ice Heaume of Frore') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10049811,   1,   33555248) /* Setup */
     , (10049811,   3,  536870932) /* SoundTable */
     , (10049811,   6,   67108990) /* PaletteBase */
     , (10049811,   7,  268435629) /* ClothingBase */
     , (10049811,   8,  100667349) /* Icon */
     , (10049811,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:05:36.4603421-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": false
}
*/
