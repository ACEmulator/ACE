DELETE FROM `weenie` WHERE `class_Id` = 1031304;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1031304, 'ace1031304-luminousrobe', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1031304,   1,          2) /* ItemType - Armor */
     , (1031304,   3,         15) /* PaletteTemplate - RedPurple */
     , (1031304,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (1031304,   5,        150) /* EncumbranceVal */
     , (1031304,   9,        512) /* ValidLocations - ChestArmor */
     , (1031304,  16,          1) /* ItemUseable - No */
     , (1031304,  19,         20) /* Value */
     , (1031304,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (1031304,  27,         32) /* ArmorType - Metal */
     , (1031304,  28,          0) /* ArmorLevel */
     , (1031304,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1031304, 106,        350) /* ItemSpellcraft */
     , (1031304, 107,       3000) /* ItemCurMana */
     , (1031304, 108,       3000) /* ItemMaxMana */
     , (1031304, 151,          2) /* HookType - Wall */
     , (1031304, 257,          6) /* ItemAttributeLimit */
     , (1031304, 258,        295) /* ItemAttributeLevelLimit */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1031304,  22, True ) /* Inscribable */
     , (1031304, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1031304,   5, -0.033330000936985016) /* ManaRate */
     , (1031304,  13,       1) /* ArmorModVsSlash */
     , (1031304,  14,       1) /* ArmorModVsPierce */
     , (1031304,  15,       1) /* ArmorModVsBludgeon */
     , (1031304,  16,     1.5) /* ArmorModVsCold */
     , (1031304,  17, 0.8999999761581421) /* ArmorModVsFire */
     , (1031304,  18, 0.8999999761581421) /* ArmorModVsAcid */
     , (1031304,  19, 0.8999999761581421) /* ArmorModVsElectric */
     , (1031304, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1031304,   1, 'Luminous Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1031304,   1,   33554854) /* Setup */
     , (1031304,   3,  536870932) /* SoundTable */
     , (1031304,   6,   67108990) /* PaletteBase */
     , (1031304,   7,  268437011) /* ClothingBase */
     , (1031304,   8,  100687721) /* Icon */
     , (1031304,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:20:37.614951-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "-Marked as done",
  "IsDone": true
}
*/
