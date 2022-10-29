DELETE FROM `weenie` WHERE `class_Id` = 1011985;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1011985, 'ace1011985-heavyursuincoat', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1011985,   1,          2) /* ItemType - Armor */
     , (1011985,   3,         18) /* PaletteTemplate - YellowBrown */
     , (1011985,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1011985,   5,          1) /* EncumbranceVal */
     , (1011985,   8,        260) /* Mass */
     , (1011985,   9,        512) /* ValidLocations - ChestArmor */
     , (1011985,  16,          1) /* ItemUseable - No */
     , (1011985,  19,         20) /* Value */
     , (1011985,  27,         32) /* ArmorType - Metal */
     , (1011985,  28,          1) /* ArmorLevel */
     , (1011985,  53,        101) /* PlacementPosition - Resting */
     , (1011985,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1011985, 150,        103) /* HookPlacement - Hook */
     , (1011985, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1011985,  11, True ) /* IgnoreCollisions */
     , (1011985,  13, True ) /* Ethereal */
     , (1011985,  14, True ) /* GravityStatus */
     , (1011985,  19, True ) /* Attackable */
     , (1011985,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1011985,  12, 0.8999999761581421) /* Shade */
     , (1011985,  13,       1) /* ArmorModVsSlash */
     , (1011985,  14,       1) /* ArmorModVsPierce */
     , (1011985,  15,       1) /* ArmorModVsBludgeon */
     , (1011985,  16,       2) /* ArmorModVsCold */
     , (1011985,  17, 0.699999988079071) /* ArmorModVsFire */
     , (1011985,  18,       1) /* ArmorModVsAcid */
     , (1011985,  19, 2.4000000953674316) /* ArmorModVsElectric */
     , (1011985, 110,       1) /* BulkMod */
     , (1011985, 111,       1) /* SizeMod */
     , (1011985, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1011985,   1, 'Heavy Ursuin Coat') /* Name */
     , (1011985,  15, 'Some tattered shreds of clothing that you have managed to assemble into a coat.') /* ShortDesc */
     , (1011985,  16, 'Some tattered shreds of the Dread Ursuin''s pelt that you have managed to assemble into a coat.  The creature''s healing ability seems to have not gone away with its death, allowing for the coat to seal itself as you watch.  It''s actually quite morbid.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1011985,   1,   33554644) /* Setup */
     , (1011985,   3,  536870932) /* SoundTable */
     , (1011985,   6,   67108990) /* PaletteBase */
     , (1011985,   7,  268436102) /* ClothingBase */
     , (1011985,   8,  100667377) /* Icon */
     , (1011985,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T06:14:35.7310398-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
