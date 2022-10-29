DELETE FROM `weenie` WHERE `class_Id` = 1024879;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1024879, 'ace1024879-virindiprofatrixmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1024879,   1,          2) /* ItemType - Armor */
     , (1024879,   3,          3) /* PaletteTemplate - BluePurple */
     , (1024879,   4,      16384) /* ClothingPriority - Head */
     , (1024879,   5,          0) /* EncumbranceVal */
     , (1024879,   8,         75) /* Mass */
     , (1024879,   9,          1) /* ValidLocations - HeadWear */
     , (1024879,  16,          1) /* ItemUseable - No */
     , (1024879,  18,          1) /* UiEffects - Magical */
     , (1024879,  19,         20) /* Value */
     , (1024879,  27,          2) /* ArmorType - Leather */
     , (1024879,  28,          1) /* ArmorLevel */
     , (1024879,  53,        101) /* PlacementPosition - Resting */
     , (1024879,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1024879, 150,        103) /* HookPlacement - Hook */
     , (1024879, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1024879,  11, True ) /* IgnoreCollisions */
     , (1024879,  13, True ) /* Ethereal */
     , (1024879,  14, True ) /* GravityStatus */
     , (1024879,  19, True ) /* Attackable */
     , (1024879,  22, True ) /* Inscribable */
     , (1024879,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1024879,   5, -0.0333000011742115) /* ManaRate */
     , (1024879,  12, 0.6600000262260437) /* Shade */
     , (1024879,  13,       1) /* ArmorModVsSlash */
     , (1024879,  14,       1) /* ArmorModVsPierce */
     , (1024879,  15,       1) /* ArmorModVsBludgeon */
     , (1024879,  16,       2) /* ArmorModVsCold */
     , (1024879,  17,       1) /* ArmorModVsFire */
     , (1024879,  18,       1) /* ArmorModVsAcid */
     , (1024879,  19,       2) /* ArmorModVsElectric */
     , (1024879, 110,       1) /* BulkMod */
     , (1024879, 111,       1) /* SizeMod */
     , (1024879, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1024879,   1, 'Virindi Profatrix Mask') /* Name */
     , (1024879,  15, 'A black virindi mask taken from the fallen form of a Virindi Profatrix.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1024879,   1,   33558415) /* Setup */
     , (1024879,   3,  536870932) /* SoundTable */
     , (1024879,   6,   67108990) /* PaletteBase */
     , (1024879,   7,  268436648) /* ClothingBase */
     , (1024879,   8,  100674853) /* Icon */
     , (1024879,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:13:11.0507586-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
