DELETE FROM `weenie` WHERE `class_Id` = 1025842;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1025842, 'ace1025842-plaguefangsrobe', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1025842,   1,          2) /* ItemType - Armor */
     , (1025842,   3,         14) /* PaletteTemplate - Red */
     , (1025842,   4,      81664) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (1025842,   5,       1195) /* EncumbranceVal */
     , (1025842,   8,        340) /* Mass */
     , (1025842,   9,      32512) /* ValidLocations - Armor */
     , (1025842,  16,          1) /* ItemUseable - No */
     , (1025842,  19,         20) /* Value */
     , (1025842,  27,          1) /* ArmorType - Cloth */
     , (1025842,  28,          0) /* ArmorLevel */
     , (1025842,  53,        101) /* PlacementPosition - Resting */
     , (1025842,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1025842, 106,        400) /* ItemSpellcraft */
     , (1025842, 107,       1000) /* ItemCurMana */
     , (1025842, 108,       1000) /* ItemMaxMana */
     , (1025842, 109,        125) /* ItemDifficulty */
     , (1025842, 158,          7) /* WieldRequirements - Level */
     , (1025842, 159,          1) /* WieldSkillType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1025842,  11, True ) /* IgnoreCollisions */
     , (1025842,  13, True ) /* Ethereal */
     , (1025842,  14, True ) /* GravityStatus */
     , (1025842,  19, True ) /* Attackable */
     , (1025842,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1025842,   5, -0.032999999821186066) /* ManaRate */
     , (1025842,  12,       1) /* Shade */
     , (1025842,  13,    0.25) /* ArmorModVsSlash */
     , (1025842,  14,    0.75) /* ArmorModVsPierce */
     , (1025842,  15, 0.6000000238418579) /* ArmorModVsBludgeon */
     , (1025842,  16,    0.25) /* ArmorModVsCold */
     , (1025842,  17, 0.6499999761581421) /* ArmorModVsFire */
     , (1025842,  18,    0.75) /* ArmorModVsAcid */
     , (1025842,  19,    0.75) /* ArmorModVsElectric */
     , (1025842, 110,       1) /* BulkMod */
     , (1025842, 111,       1) /* SizeMod */
     , (1025842, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1025842,   1, 'Plaguefang''s Robe') /* Name */
     , (1025842,  15, 'A robe crafted from the hide of the vile doomshark, Plaguefang.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1025842,   1,   33554854) /* Setup */
     , (1025842,   3,  536870932) /* SoundTable */
     , (1025842,   6,   67108990) /* PaletteBase */
     , (1025842,   7,  268436755) /* ClothingBase */
     , (1025842,   8,  100675613) /* Icon */
     , (1025842,  22,  872415275) /* PhysicsEffectTable */
     , (1025842,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:19:08.9059394-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
