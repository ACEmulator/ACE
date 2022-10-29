DELETE FROM `weenie` WHERE `class_Id` = 105280;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (105280, 'ace105280-holidaysweater', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (105280,   1,          2) /* ItemType - Armor */
     , (105280,   3,         39) /* PaletteTemplate - Black */
     , (105280,   4,      15360) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (105280,   5,          1) /* EncumbranceVal */
     , (105280,   9,       7680) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (105280,  16,          1) /* ItemUseable - No */
     , (105280,  18,          1) /* UiEffects - Magical */
     , (105280,  19,         20) /* Value */
     , (105280,  28,          1) /* ArmorLevel */
     , (105280,  53,        101) /* PlacementPosition - Resting */
     , (105280,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (105280, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (105280,  11, True ) /* IgnoreCollisions */
     , (105280,  13, True ) /* Ethereal */
     , (105280,  14, True ) /* GravityStatus */
     , (105280,  19, True ) /* Attackable */
     , (105280,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (105280,   5, -0.05000000074505806) /* ManaRate */
     , (105280,  13, 0.6000000238418579) /* ArmorModVsSlash */
     , (105280,  14, 0.6000000238418579) /* ArmorModVsPierce */
     , (105280,  15, 0.699999988079071) /* ArmorModVsBludgeon */
     , (105280,  16,    1.75) /* ArmorModVsCold */
     , (105280,  17,     0.5) /* ArmorModVsFire */
     , (105280,  18,     0.5) /* ArmorModVsAcid */
     , (105280,  19,       1) /* ArmorModVsElectric */
     , (105280, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (105280,   1, 'Holiday Sweater') /* Name */
     , (105280,  16, 'A sweater, knit with care by the Greatmother of Silyun to keep one warm during the festival season.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (105280,   1,   33559357) /* Setup */
     , (105280,   3,  536870932) /* SoundTable */
     , (105280,   7,  268437612) /* ClothingBase */
     , (105280,   8,  100693300) /* Icon */
     , (105280,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:55:22.2391822-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Corrected clothing base did\nAdded Palette template int to 39",
  "IsDone": false
}
*/
