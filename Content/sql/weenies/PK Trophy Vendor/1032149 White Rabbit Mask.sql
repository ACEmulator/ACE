DELETE FROM `weenie` WHERE `class_Id` = 1032149;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1032149, 'ace1032149-whiterabbitmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1032149,   1,          2) /* ItemType - Armor */
     , (1032149,   3,          4) /* PaletteTemplate - Brown */
     , (1032149,   4,      16384) /* ClothingPriority - Head */
     , (1032149,   5,        100) /* EncumbranceVal */
     , (1032149,   9,          1) /* ValidLocations - HeadWear */
     , (1032149,  16,          1) /* ItemUseable - No */
     , (1032149,  18,          1) /* UiEffects - Magical */
     , (1032149,  19,         20) /* Value */
     , (1032149,  28,          1) /* ArmorLevel */
     , (1032149,  53,        101) /* PlacementPosition - Resting */
     , (1032149,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1032149, 106,        335) /* ItemSpellcraft */
     , (1032149, 107,       1092) /* ItemCurMana */
     , (1032149, 108,       1200) /* ItemMaxMana */
     , (1032149, 109,        250) /* ItemDifficulty */
     , (1032149, 151,          2) /* HookType - Wall */
     , (1032149, 158,          7) /* WieldRequirements - Level */
     , (1032149, 159,          1) /* WieldSkillType - Axe */
     , (1032149, 160,         30) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1032149,   1, False) /* Stuck */
     , (1032149,  11, True ) /* IgnoreCollisions */
     , (1032149,  13, True ) /* Ethereal */
     , (1032149,  14, True ) /* GravityStatus */
     , (1032149,  19, True ) /* Attackable */
     , (1032149,  22, True ) /* Inscribable */
     , (1032149,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1032149,   5, -0.05000000074505806) /* ManaRate */
     , (1032149,  12,       0) /* Shade */
     , (1032149,  13,     1.5) /* ArmorModVsSlash */
     , (1032149,  14, 0.800000011920929) /* ArmorModVsPierce */
     , (1032149,  15, 1.2000000476837158) /* ArmorModVsBludgeon */
     , (1032149,  16,       2) /* ArmorModVsCold */
     , (1032149,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1032149,  18, 1.2000000476837158) /* ArmorModVsAcid */
     , (1032149,  19,       2) /* ArmorModVsElectric */
     , (1032149, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1032149,   1, 'White Rabbit Mask') /* Name */
     , (1032149,  15, 'A large mask depicting the head of the White Rabbit. It''s very odd.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1032149,   1,   33559763) /* Setup */
     , (1032149,   3,  536870932) /* SoundTable */
     , (1032149,   7,  268437068) /* ClothingBase */
     , (1032149,   8,  100688458) /* Icon */
     , (1032149,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T12:35:02.6784846-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "added palette and shade",
  "IsDone": false
}
*/
