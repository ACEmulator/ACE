DELETE FROM `weenie` WHERE `class_Id` = 1036228;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1036228, 'ace1036228-coralshield', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1036228,   1,          2) /* ItemType - Armor */
     , (1036228,   5,          0) /* EncumbranceVal */
     , (1036228,   9,    2097152) /* ValidLocations - Shield */
     , (1036228,  16,          1) /* ItemUseable - No */
     , (1036228,  19,         20) /* Value */
     , (1036228,  28,          1) /* ArmorLevel */
     , (1036228,  51,          4) /* CombatUse - Shield */
     , (1036228,  53,        101) /* PlacementPosition - Resting */
     , (1036228,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1036228, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1036228,  11, True ) /* IgnoreCollisions */
     , (1036228,  13, True ) /* Ethereal */
     , (1036228,  14, True ) /* GravityStatus */
     , (1036228,  19, True ) /* Attackable */
     , (1036228,  22, True ) /* Inscribable */
     , (1036228,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1036228,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (1036228,  14, 0.4000000059604645) /* ArmorModVsPierce */
     , (1036228,  15, 1.7999999523162842) /* ArmorModVsBludgeon */
     , (1036228,  16,       2) /* ArmorModVsCold */
     , (1036228,  17,       2) /* ArmorModVsFire */
     , (1036228,  18,       2) /* ArmorModVsAcid */
     , (1036228,  19, 0.4000000059604645) /* ArmorModVsElectric */
     , (1036228, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1036228,   1, 'Coral Shield') /* Name */
     , (1036228,  16, 'A shield made of the remnants of many Coral Golems. It appears to be held together by some sort of viscous water that offers protection against most types of damage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1036228,   1,   33560374) /* Setup */
     , (1036228,   3,  536870932) /* SoundTable */
     , (1036228,   8,  100689609) /* Icon */
     , (1036228,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:17:43.6968514-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
