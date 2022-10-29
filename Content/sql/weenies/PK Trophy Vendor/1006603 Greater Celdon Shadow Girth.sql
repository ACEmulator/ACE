DELETE FROM `weenie` WHERE `class_Id` = 1006603;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006603, 'ace1006603-greaterceldonshadowgirth', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006603,   1,          2) /* ItemType - Armor */
     , (1006603,   3,         21) /* PaletteTemplate - Gold */
     , (1006603,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (1006603,   5,          0) /* EncumbranceVal */
     , (1006603,   8,        625) /* Mass */
     , (1006603,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (1006603,  16,          1) /* ItemUseable - No */
     , (1006603,  19,         20) /* Value */
     , (1006603,  27,         32) /* ArmorType - Metal */
     , (1006603,  28,          1) /* ArmorLevel */
     , (1006603,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006603,  22, True ) /* Inscribable */
     , (1006603,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006603,  12, 0.6000000238418579) /* Shade */
     , (1006603,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006603,  14,       1) /* ArmorModVsPierce */
     , (1006603,  15,       1) /* ArmorModVsBludgeon */
     , (1006603,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1006603,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1006603,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1006603,  19,     0.5) /* ArmorModVsElectric */
     , (1006603, 110,       1) /* BulkMod */
     , (1006603, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006603,   1, 'Greater Celdon Shadow Girth') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006603,   1,   33554647) /* Setup */
     , (1006603,   3,  536870932) /* SoundTable */
     , (1006603,   6,   67108990) /* PaletteBase */
     , (1006603,   7,  268435843) /* ClothingBase */
     , (1006603,   8,  100670411) /* Icon */
     , (1006603,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T18:10:46.4725807-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
