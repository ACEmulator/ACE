DELETE FROM `weenie` WHERE `class_Id` = 32155;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (32155, 'ace32155-ursuinguise', 2, '2021-11-20 00:19:17') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (32155,   1,          2) /* ItemType - Armor */
     , (32155,   3,          4) /* PaletteTemplate - Brown */
     , (32155,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (32155,   5,          0) /* EncumbranceVal */
     , (32155,   9,        512) /* ValidLocations - ChestArmor */
     , (32155,  16,          1) /* ItemUseable - No */
     , (32155,  19,         20) /* Value */
     , (32155,  28,          1) /* ArmorLevel */
     , (32155,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (32155, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (32155,   1, False) /* Stuck */
     , (32155,  11, True ) /* IgnoreCollisions */
     , (32155,  13, True ) /* Ethereal */
     , (32155,  14, True ) /* GravityStatus */
     , (32155,  19, True ) /* Attackable */
     , (32155,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (32155,  12,       0) /* Shade */
     , (32155,  13,     0.5) /* ArmorModVsSlash */
     , (32155,  14,     0.5) /* ArmorModVsPierce */
     , (32155,  15,    0.75) /* ArmorModVsBludgeon */
     , (32155,  16, 0.6499999761581421) /* ArmorModVsCold */
     , (32155,  17, 0.550000011920929) /* ArmorModVsFire */
     , (32155,  18, 0.550000011920929) /* ArmorModVsAcid */
     , (32155,  19, 0.6499999761581421) /* ArmorModVsElectric */
     , (32155, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (32155,   1, 'Ursuin Guise') /* Name */
     , (32155,  16, 'An awkward Ursuin Guise. All you need is an Ursuin Mask to complete the look.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (32155,   1,   33559782) /* Setup */
     , (32155,   3,  536870932) /* SoundTable */
     , (32155,   7,  268437085) /* ClothingBase */
     , (32155,   8,  100688468) /* Icon */
     , (32155,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T19:23:48.2529277-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "-Added clothingbase DID\r\n-Added palette int for brown\r\n-Added shade float for 0\r\n-Marked as done",
  "IsDone": true
}
*/
