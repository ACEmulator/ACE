DELETE FROM `weenie` WHERE `class_Id` = 450077;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450077, 'ace450077-ursuinguisetailor', 2, '2021-11-20 00:19:17') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450077,   1,          4) /* ItemType - Armor */
     , (450077,   3,          4) /* PaletteTemplate - Brown */
     , (450077,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450077,   5,          0) /* EncumbranceVal */
     , (450077,   9,        512) /* ValidLocations - ChestArmor */
     , (450077,  16,          1) /* ItemUseable - No */
     , (450077,  19,         20) /* Value */
     , (450077,  28,          1) /* ArmorLevel */
     , (450077,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450077, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450077,   1, False) /* Stuck */
     , (450077,  11, True ) /* IgnoreCollisions */
     , (450077,  13, True ) /* Ethereal */
     , (450077,  14, True ) /* GravityStatus */
     , (450077,  19, True ) /* Attackable */
     , (450077,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450077,  12,       0) /* Shade */
     , (450077,  13,     0.5) /* ArmorModVsSlash */
     , (450077,  14,     0.5) /* ArmorModVsPierce */
     , (450077,  15,    0.75) /* ArmorModVsBludgeon */
     , (450077,  16, 0.6499999761581421) /* ArmorModVsCold */
     , (450077,  17, 0.550000011920929) /* ArmorModVsFire */
     , (450077,  18, 0.550000011920929) /* ArmorModVsAcid */
     , (450077,  19, 0.6499999761581421) /* ArmorModVsElectric */
     , (450077, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450077,   1, 'Ursuin Guise') /* Name */
     , (450077,  16, 'An awkward Ursuin Guise. All you need is an Ursuin Mask to complete the look.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450077,   1,   33559782) /* Setup */
     , (450077,   3,  536870932) /* SoundTable */
     , (450077,   7,  268437085) /* ClothingBase */
     , (450077,   8,  100688468) /* Icon */
     , (450077,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T19:23:48.2529277-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "-Added clothingbase DID\r\n-Added palette int for brown\r\n-Added shade float for 0\r\n-Marked as done",
  "IsDone": true
}
*/
