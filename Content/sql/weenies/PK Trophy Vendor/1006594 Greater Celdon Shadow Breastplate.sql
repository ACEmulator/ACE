DELETE FROM `weenie` WHERE `class_Id` = 1006594;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006594, 'ace1006594-greaterceldonshadowbreastplate', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006594,   1,          2) /* ItemType - Armor */
     , (1006594,   3,         21) /* PaletteTemplate - Gold */
     , (1006594,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1006594,   5,          0) /* EncumbranceVal */
     , (1006594,   8,       1200) /* Mass */
     , (1006594,   9,        512) /* ValidLocations - ChestArmor */
     , (1006594,  16,          1) /* ItemUseable - No */
     , (1006594,  19,         20) /* Value */
     , (1006594,  27,         32) /* ArmorType - Metal */
     , (1006594,  28,          1) /* ArmorLevel */
     , (1006594,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006594,  22, True ) /* Inscribable */
     , (1006594,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006594,  12, 0.6000000238418579) /* Shade */
     , (1006594,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006594,  14,       1) /* ArmorModVsPierce */
     , (1006594,  15,       1) /* ArmorModVsBludgeon */
     , (1006594,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1006594,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1006594,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1006594,  19,     0.5) /* ArmorModVsElectric */
     , (1006594, 110,       1) /* BulkMod */
     , (1006594, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006594,   1, 'Greater Celdon Shadow Breastplate') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006594,   1,   33554642) /* Setup */
     , (1006594,   3,  536870932) /* SoundTable */
     , (1006594,   6,   67108990) /* PaletteBase */
     , (1006594,   7,  268435848) /* ClothingBase */
     , (1006594,   8,  100670403) /* Icon */
     , (1006594,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T18:11:01.3528377-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
