DELETE FROM `weenie` WHERE `class_Id` = 1006798;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1006798, 'ace1006798-nexuskoujiabreastplate', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1006798,   1,          2) /* ItemType - Armor */
     , (1006798,   3,          2) /* PaletteTemplate - Blue */
     , (1006798,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1006798,   5,       1875) /* EncumbranceVal */
     , (1006798,   8,        850) /* Mass */
     , (1006798,   9,        512) /* ValidLocations - ChestArmor */
     , (1006798,  16,          1) /* ItemUseable - No */
     , (1006798,  19,         20) /* Value */
     , (1006798,  27,          0) /* ArmorType - None */
     , (1006798,  28,          1) /* ArmorLevel */
     , (1006798,  33,          1) /* Bonded - Bonded */
     , (1006798,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1006798, 106,        270) /* ItemSpellcraft */
     , (1006798, 107,        900) /* ItemCurMana */
     , (1006798, 108,        900) /* ItemMaxMana */
     , (1006798, 109,        150) /* ItemDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1006798,  22, True ) /* Inscribable */
     , (1006798,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1006798,   5, -0.10000000149011612) /* ManaRate */
     , (1006798,  12, 0.10000000149011612) /* Shade */
     , (1006798,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1006798,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1006798,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1006798,  16,       1) /* ArmorModVsCold */
     , (1006798,  17,       1) /* ArmorModVsFire */
     , (1006798,  18,       1) /* ArmorModVsAcid */
     , (1006798,  19,       1) /* ArmorModVsElectric */
     , (1006798, 110,       1) /* BulkMod */
     , (1006798, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1006798,   1, 'Nexus Koujia Breastplate') /* Name */
     , (1006798,  15, 'A magnificent Koujia breastplate, infused with the essence of the Nexus Crystal.') /* ShortDesc */
     , (1006798,  16, 'A magnificent Koujia breastplate, infused with the essence of the Nexus Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1006798,   1,   33554642) /* Setup */
     , (1006798,   3,  536870932) /* SoundTable */
     , (1006798,   6,   67108990) /* PaletteBase */
     , (1006798,   7,  268435852) /* ClothingBase */
     , (1006798,   8,  100670451) /* Icon */
     , (1006798,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (1006798,   209,      2)  /* Mana Renewal Other IV */
     , (1006798,   272,      2)  /* Magic Resistance Other V */
     , (1006798,   909,      2)  /* Leadership Mastery Other VI */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:48:17.111859-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
