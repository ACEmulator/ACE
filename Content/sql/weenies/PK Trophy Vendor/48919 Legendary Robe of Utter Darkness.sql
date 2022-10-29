DELETE FROM `weenie` WHERE `class_Id` = 48919;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (48919, 'ace48919-legendaryrobeofutterdarkness', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (48919,   1,          2) /* ItemType - Armor */
     , (48919,   3,         90) /* PaletteTemplate - DyeWinterSilver */
     , (48919,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (48919,   5,        450) /* EncumbranceVal */
     , (48919,   8,        150) /* Mass */
     , (48919,   9,        512) /* ValidLocations - ChestArmor */
     , (48919,  16,          1) /* ItemUseable - No */
     , (48919,  18,          1) /* UiEffects - Magical */
     , (48919,  19,         20) /* Value */
     , (48919,  27,          2) /* ArmorType - Leather */
     , (48919,  28,        175) /* ArmorLevel */
     , (48919,  53,        101) /* PlacementPosition - Resting */
     , (48919,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (48919, 158,          7) /* WieldRequirements - Level */
     , (48919, 160,        200) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (48919,  11, True ) /* IgnoreCollisions */
     , (48919,  13, True ) /* Ethereal */
     , (48919,  14, True ) /* GravityStatus */
     , (48919,  19, True ) /* Attackable */
     , (48919,  22, True ) /* Inscribable */
     , (48919,  23, True ) /* DestroyOnSell */
     , (48919,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (48919,  13, 0.6000000238418579) /* ArmorModVsSlash */
     , (48919,  14, 0.6000000238418579) /* ArmorModVsPierce */
     , (48919,  15, 0.6000000238418579) /* ArmorModVsBludgeon */
     , (48919,  16, 0.6000000238418579) /* ArmorModVsCold */
     , (48919,  17, 0.6000000238418579) /* ArmorModVsFire */
     , (48919,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (48919,  19, 0.6000000238418579) /* ArmorModVsElectric */
     , (48919, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (48919,   1, 'Legendary Robe of Utter Darkness') /* Name */
     , (48919,  16, 'Hoshino Kei''s corrupted Robe of Perfect Light, which became infused with Dark Falatacot Magics during the ritual which raised her as one of the undead.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (48919,   1,   33554854) /* Setup */
     , (48919,   3,  536870932) /* SoundTable */
     , (48919,   7,  268437540) /* ClothingBase */
     , (48919,   8,  100692654) /* Icon */
     , (48919,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-21T23:30:53.9877345-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
