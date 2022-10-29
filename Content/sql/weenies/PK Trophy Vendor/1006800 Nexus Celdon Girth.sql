DELETE FROM `weenie` WHERE `class_Id` = 1006800;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006800, 'ace1006800-nexusceldongirth', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006800,   1,          2) /* ItemType - Armor */
     , (1006800,   3,          2) /* PaletteTemplate - Blue */
     , (1006800,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (1006800,   5,       1575) /* EncumbranceVal */
     , (1006800,   8,        625) /* Mass */
     , (1006800,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (1006800,  16,          1) /* ItemUseable - No */
     , (1006800,  19,         20) /* Value */
     , (1006800,  27,          0) /* ArmorType - None */
     , (1006800,  28,          1) /* ArmorLevel */
     , (1006800,  33,          1) /* Bonded - Bonded */
     , (1006800,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006800,  22, True ) /* Inscribable */
     , (1006800,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006800,  12, 0.10000000149011612) /* Shade */
     , (1006800,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006800,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006800,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006800,  16,       1) /* ArmorModVsCold */
     , (1006800,  17,       1) /* ArmorModVsFire */
     , (1006800,  18,       1) /* ArmorModVsAcid */
     , (1006800,  19,       1) /* ArmorModVsElectric */
     , (1006800, 110,       1) /* BulkMod */
     , (1006800, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006800,   1, 'Nexus Celdon Girth') /* Name */
     , (1006800,  15, 'A magnificent Celdon girth, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006800,  16, 'A magnificent Celdon girth, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006800,   1,   33554647) /* Setup */
     , (1006800,   3,  536870932) /* SoundTable */
     , (1006800,   6,   67108990) /* PaletteBase */
     , (1006800,   7,  268435843) /* ClothingBase */
     , (1006800,   8,  100670411) /* Icon */
     , (1006800,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:49:16.0516259-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
