DELETE FROM `weenie` WHERE `class_Id` = 1006600;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006600, 'ace1006600-greateramulishadowcoat', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006600,   1,          2) /* ItemType - Armor */
     , (1006600,   3,         21) /* PaletteTemplate - Gold */
     , (1006600,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (1006600,   5,          0) /* EncumbranceVal */
     , (1006600,   8,       1000) /* Mass */
     , (1006600,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (1006600,  16,          1) /* ItemUseable - No */
     , (1006600,  19,         20) /* Value */
     , (1006600,  27,          8) /* ArmorType - Scalemail */
     , (1006600,  28,          1) /* ArmorLevel */
     , (1006600,  53,        101) /* PlacementPosition - Resting */
     , (1006600,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006600,  11, True ) /* IgnoreCollisions */
     , (1006600,  13, True ) /* Ethereal */
     , (1006600,  14, True ) /* GravityStatus */
     , (1006600,  19, True ) /* Attackable */
     , (1006600,  22, True ) /* Inscribable */
     , (1006600,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006600,  12, 0.30000001192092896) /* Shade */
     , (1006600,  13,       1) /* ArmorModVsSlash */
     , (1006600,  14, 1.100000023841858) /* ArmorModVsPierce */
     , (1006600,  15,       1) /* ArmorModVsBludgeon */
     , (1006600,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1006600,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1006600,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1006600,  19,     0.5) /* ArmorModVsElectric */
     , (1006600, 110,       1) /* BulkMod */
     , (1006600, 111,       1) /* SizeMod */
     , (1006600, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006600,   1, 'Greater Amuli Shadow Coat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006600,   1,   33554854) /* Setup */
     , (1006600,   3,  536870932) /* SoundTable */
     , (1006600,   6,   67108990) /* PaletteBase */
     , (1006600,   7,  268435873) /* ClothingBase */
     , (1006600,   8,  100670435) /* Icon */
     , (1006600,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T17:23:03.5049487-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
