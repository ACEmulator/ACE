DELETE FROM `weenie` WHERE `class_Id` = 13241326;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (13241326, 'ace13241326-legendaryrobeofutterdarkness', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (13241326,   1,          2) /* ItemType - Armor */
     , (13241326,   3,         90) /* PaletteTemplate - DyeWinterSilver */
     , (13241326,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (13241326,   5,         10) /* EncumbranceVal */
     , (13241326,   8,        150) /* Mass */
     , (13241326,   9,        512) /* ValidLocations - ChestArmor */
     , (13241326,  16,          1) /* ItemUseable - No */
     , (13241326,  18,          1) /* UiEffects - Magical */
     , (13241326,  19,         20) /* Value */
     , (13241326,  27,          2) /* ArmorType - Leather */
     , (13241326,  53,        101) /* PlacementPosition - Resting */
     , (13241326,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (13241326,  11, True ) /* IgnoreCollisions */
     , (13241326,  13, True ) /* Ethereal */
     , (13241326,  14, True ) /* GravityStatus */
     , (13241326,  19, True ) /* Attackable */
     , (13241326,  22, True ) /* Inscribable */
     , (13241326,  23, True ) /* DestroyOnSell */
     , (13241326,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (13241326,   1, 'Legendary Robe of Utter Darkness') /* Name */
     , (13241326,  16, 'Hoshino Kei''s corrupted Robe of Perfect Light, which became infused with Dark Falatacot Magics during the ritual which raised her as one of the undead.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (13241326,   1,   33554854) /* Setup */
     , (13241326,   3,  536870932) /* SoundTable */
     , (13241326,   7,  268437540) /* ClothingBase */
     , (13241326,   8,  100692654) /* Icon */
     , (13241326,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-25T14:37:44.6795643-04:00",
  "ModifiedBy": "Grims Bold",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
