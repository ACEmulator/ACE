DELETE FROM `weenie` WHERE `class_Id` = 1025335;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1025335, 'ace1025335-virindiconsulmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1025335,   1,          2) /* ItemType - Armor */
     , (1025335,   3,          4) /* PaletteTemplate - Brown */
     , (1025335,   4,      16384) /* ClothingPriority - Head */
     , (1025335,   5,          0) /* EncumbranceVal */
     , (1025335,   8,         75) /* Mass */
     , (1025335,   9,          1) /* ValidLocations - HeadWear */
     , (1025335,  16,          1) /* ItemUseable - No */
     , (1025335,  18,          1) /* UiEffects - Magical */
     , (1025335,  19,         20) /* Value */
     , (1025335,  27,          2) /* ArmorType - Leather */
     , (1025335,  28,          1) /* ArmorLevel */
     , (1025335,  53,        101) /* PlacementPosition - Resting */
     , (1025335,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1025335, 150,        103) /* HookPlacement - Hook */
     , (1025335, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1025335,  11, True ) /* IgnoreCollisions */
     , (1025335,  13, True ) /* Ethereal */
     , (1025335,  14, True ) /* GravityStatus */
     , (1025335,  19, True ) /* Attackable */
     , (1025335,  22, True ) /* Inscribable */
     , (1025335,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1025335,   5, -0.0333000011742115) /* ManaRate */
     , (1025335,  12, 0.6600000262260437) /* Shade */
     , (1025335,  13,       1) /* ArmorModVsSlash */
     , (1025335,  14,       1) /* ArmorModVsPierce */
     , (1025335,  15,       1) /* ArmorModVsBludgeon */
     , (1025335,  16,       2) /* ArmorModVsCold */
     , (1025335,  17,       1) /* ArmorModVsFire */
     , (1025335,  18,       1) /* ArmorModVsAcid */
     , (1025335,  19,       2) /* ArmorModVsElectric */
     , (1025335, 110,       1) /* BulkMod */
     , (1025335, 111,       1) /* SizeMod */
     , (1025335, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1025335,   1, 'Virindi Consul Mask') /* Name */
     , (1025335,  15, 'A red Virindi mask reconstructed from the remains of a defeated Virindi Consul.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1025335,   1,   33558445) /* Setup */
     , (1025335,   3,  536870932) /* SoundTable */
     , (1025335,   6,   67108990) /* PaletteBase */
     , (1025335,   7,  268436675) /* ClothingBase */
     , (1025335,   8,  100674854) /* Icon */
     , (1025335,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:13:42.5605358-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
