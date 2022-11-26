DELETE FROM `weenie` WHERE `class_Id` = 1006799;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006799, 'ace1006799-nexusamulicoat', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006799,   1,          2) /* ItemType - Armor */
     , (1006799,   3,          2) /* PaletteTemplate - Blue */
     , (1006799,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (1006799,   5,       2000) /* EncumbranceVal */
     , (1006799,   8,       1000) /* Mass */
     , (1006799,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (1006799,  16,          1) /* ItemUseable - No */
     , (1006799,  19,         20) /* Value */
     , (1006799,  27,          8) /* ArmorType - Scalemail */
     , (1006799,  28,          1) /* ArmorLevel */
     , (1006799,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1006799, 106,        270) /* ItemSpellcraft */
     , (1006799, 107,        900) /* ItemCurMana */
     , (1006799, 108,        900) /* ItemMaxMana */
     , (1006799, 109,        150) /* ItemDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006799,  22, True ) /* Inscribable */
     , (1006799,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006799,   5, -0.10000000149011612) /* ManaRate */
     , (1006799,  12, 0.10000000149011612) /* Shade */
     , (1006799,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006799,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006799,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006799,  16,       1) /* ArmorModVsCold */
     , (1006799,  17,       1) /* ArmorModVsFire */
     , (1006799,  18,       1) /* ArmorModVsAcid */
     , (1006799,  19,       1) /* ArmorModVsElectric */
     , (1006799, 110,       1) /* BulkMod */
     , (1006799, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006799,   1, 'Nexus Amuli Coat') /* Name */
     , (1006799,  15, 'A magnificent Amuli coat, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006799,  16, 'A magnificent Amuli coat, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006799,   1,   33554854) /* Setup */
     , (1006799,   3,  536870932) /* SoundTable */
     , (1006799,   6,   67108990) /* PaletteBase */
     , (1006799,   7,  268435873) /* ClothingBase */
     , (1006799,   8,  100670435) /* Icon */
     , (1006799,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:07:35.5819191-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
