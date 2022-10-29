DELETE FROM `weenie` WHERE `class_Id` = 10049813;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10049813, 'ace10049813-iceheaumeoffrore', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10049813,   1,          2) /* ItemType - Armor */
     , (10049813,   3,         39) /* PaletteTemplate - Black */
     , (10049813,   4,      16384) /* ClothingPriority - Head */
     , (10049813,   5,       1100) /* EncumbranceVal */
     , (10049813,   8,        340) /* Mass */
     , (10049813,   9,          1) /* ValidLocations - HeadWear */
     , (10049813,  16,          1) /* ItemUseable - No */
     , (10049813,  18,        512) /* UiEffects - Bludgeoning */
     , (10049813,  19,         20) /* Value */
     , (10049813,  27,          0) /* ArmorType - None */
     , (10049813,  28,          1) /* ArmorLevel */
     , (10049813,  53,        101) /* PlacementPosition - Resting */
     , (10049813,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10049813, 150,        103) /* HookPlacement - Hook */
     , (10049813, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10049813,  11, True ) /* IgnoreCollisions */
     , (10049813,  13, True ) /* Ethereal */
     , (10049813,  14, True ) /* GravityStatus */
     , (10049813,  19, True ) /* Attackable */
     , (10049813,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10049813,  12, 0.800000011920929) /* Shade */
     , (10049813,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (10049813,  14,       1) /* ArmorModVsPierce */
     , (10049813,  15, 1.100000023841858) /* ArmorModVsBludgeon */
     , (10049813,  16,       2) /* ArmorModVsCold */
     , (10049813,  17,       2) /* ArmorModVsFire */
     , (10049813,  18, 0.699999988079071) /* ArmorModVsAcid */
     , (10049813,  19,       0) /* ArmorModVsElectric */
     , (10049813, 110,       1) /* BulkMod */
     , (10049813, 111,       1) /* SizeMod */
     , (10049813, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10049813,   1, 'Ice Heaume of Frore') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10049813,   1,   33555248) /* Setup */
     , (10049813,   3,  536870932) /* SoundTable */
     , (10049813,   6,   67108990) /* PaletteBase */
     , (10049813,   7,  268435629) /* ClothingBase */
     , (10049813,   8,  100667349) /* Icon */
     , (10049813,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:06:51.3541389-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": false
}
*/
