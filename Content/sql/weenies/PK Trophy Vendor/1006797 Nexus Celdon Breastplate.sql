DELETE FROM `weenie` WHERE `class_Id` = 1006797;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006797, 'ace1006797-nexusceldonbreastplate', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006797,   1,          2) /* ItemType - Armor */
     , (1006797,   3,          2) /* PaletteTemplate - Blue */
     , (1006797,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1006797,   5,       3300) /* EncumbranceVal */
     , (1006797,   8,       1200) /* Mass */
     , (1006797,   9,        512) /* ValidLocations - ChestArmor */
     , (1006797,  16,          1) /* ItemUseable - No */
     , (1006797,  19,         20) /* Value */
     , (1006797,  27,          0) /* ArmorType - None */
     , (1006797,  28,          1) /* ArmorLevel */
     , (1006797,  33,          1) /* Bonded - Bonded */
     , (1006797,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1006797, 106,        270) /* ItemSpellcraft */
     , (1006797, 107,        900) /* ItemCurMana */
     , (1006797, 108,        900) /* ItemMaxMana */
     , (1006797, 109,        150) /* ItemDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006797,  22, True ) /* Inscribable */
     , (1006797,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006797,   5, -0.10000000149011612) /* ManaRate */
     , (1006797,  12, 0.10000000149011612) /* Shade */
     , (1006797,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006797,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006797,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006797,  16,       1) /* ArmorModVsCold */
     , (1006797,  17,       1) /* ArmorModVsFire */
     , (1006797,  18,       1) /* ArmorModVsAcid */
     , (1006797,  19,       1) /* ArmorModVsElectric */
     , (1006797, 110,       1) /* BulkMod */
     , (1006797, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006797,   1, 'Nexus Celdon Breastplate') /* Name */
     , (1006797,  15, 'A magnificent Celdon breastplate, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006797,  16, 'A magnificent Celdon breastplate, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006797,   1,   33554642) /* Setup */
     , (1006797,   3,  536870932) /* SoundTable */
     , (1006797,   6,   67108990) /* PaletteBase */
     , (1006797,   7,  268435848) /* ClothingBase */
     , (1006797,   8,  100670403) /* Icon */
     , (1006797,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:50:06.8754167-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
