DELETE FROM `weenie` WHERE `class_Id` = 32927;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (32927, 'ace32927-whiterabbitgirth', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (32927,   1,          2) /* ItemType - Armor */
     , (32927,   3,         90) /* PaletteTemplate - DyeWinterSilver */
     , (32927,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (32927,   5,        100) /* EncumbranceVal */
     , (32927,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (32927,  18,          1) /* UiEffects - Magical */
     , (32927,  19,         20) /* Value */
     , (32927,  28,          1) /* ArmorLevel */
     , (32927, 106,        335) /* ItemSpellcraft */
     , (32927, 107,       1200) /* ItemCurMana */
     , (32927, 108,       1200) /* ItemMaxMana */
     , (32927, 109,        250) /* ItemDifficulty */
     , (32927, 151,          2) /* HookType - Wall */
     , (32927, 158,          7) /* WieldRequirements - Level */
     , (32927, 159,          1) /* WieldSkillType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (32927,  22, True ) /* Inscribable */
     , (32927, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (32927,   5, -0.05000000074505806) /* ManaRate */
     , (32927,  13,     1.5) /* ArmorModVsSlash */
     , (32927,  14, 0.800000011920929) /* ArmorModVsPierce */
     , (32927,  15, 1.2000000476837158) /* ArmorModVsBludgeon */
     , (32927,  16,       2) /* ArmorModVsCold */
     , (32927,  17, 0.800000011920929) /* ArmorModVsFire */
     , (32927,  18, 1.2000000476837158) /* ArmorModVsAcid */
     , (32927,  19,       2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (32927,   1, 'White Rabbit Girth') /* Name */
     , (32927,  16, 'A rabbit hide girth with a fluffy bunny tail.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (32927,   1,   33554647) /* Setup */
     , (32927,   3,  536870932) /* SoundTable */
     , (32927,   6,   67108990) /* PaletteBase */
     , (32927,   7,  268437117) /* ClothingBase */
     , (32927,   8,  100688875) /* Icon */
     , (32927,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (32927,   2, 2154729006) /* Container */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T12:31:15.8911566-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
