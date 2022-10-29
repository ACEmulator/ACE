DELETE FROM `weenie` WHERE `class_Id` = 13241330;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (13241330, 'ace13241330-quarteredcloak', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (13241330,   1,          4) /* ItemType - Clothing */
     , (13241330,   3,          0) /* PaletteTemplate - Undef */
     , (13241330,   4,     131072) /* ClothingPriority - 131072 */
     , (13241330,   5,          1) /* EncumbranceVal */
     , (13241330,   9,  134217728) /* ValidLocations - Cloak */
     , (13241330,  16,          1) /* ItemUseable - No */
     , (13241330,  18,          1) /* UiEffects - Magical */
     , (13241330,  19,         20) /* Value */
     , (13241330,  28,          0) /* ArmorLevel */
     , (13241330,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (13241330,  11, True ) /* IgnoreCollisions */
     , (13241330,  13, True ) /* Ethereal */
     , (13241330,  14, True ) /* GravityStatus */
     , (13241330,  19, True ) /* Attackable */
     , (13241330,  22, True ) /* Inscribable */
     , (13241330, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (13241330,   1, 'Quartered Cloak') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (13241330,   1,   33561386) /* Setup */
     , (13241330,   3,  536870932) /* SoundTable */
     , (13241330,   7,  268437492) /* ClothingBase */
     , (13241330,   8,  100692132) /* Icon */
     , (13241330,  22,  872415275) /* PhysicsEffectTable */
     , (13241330,  50,  100690997) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-26T09:24:34.344344-04:00",
  "ModifiedBy": "Grims Bold",
  "Changelog": [],
  "UserChangeSummary": "Custom for tailoring",
  "IsDone": false
}
*/
