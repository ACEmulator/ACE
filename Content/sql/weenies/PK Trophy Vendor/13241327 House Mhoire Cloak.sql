DELETE FROM `weenie` WHERE `class_Id` = 13241327;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (13241327, 'ace13241327-housemhoirecloak', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (13241327,   1,          4) /* ItemType - Clothing */
     , (13241327,   3,         20) /* PaletteTemplate - Silver */
     , (13241327,   4,     131072) /* ClothingPriority - 131072 */
     , (13241327,   5,          1) /* EncumbranceVal */
     , (13241327,   9,  134217728) /* ValidLocations - Cloak */
     , (13241327,  19,         20) /* Value */
     , (13241327,  28,          0) /* ArmorLevel */
     , (13241327,  53,        101) /* PlacementPosition - Resting */
     , (13241327,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (13241327,  11, True ) /* IgnoreCollisions */
     , (13241327,  13, True ) /* Ethereal */
     , (13241327,  14, True ) /* GravityStatus */
     , (13241327,  19, True ) /* Attackable */
     , (13241327,  22, True ) /* Inscribable */
     , (13241327,  91, True ) /* Retained */
     , (13241327, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (13241327,   1, 'House Mhoire Cloak') /* Name */
     , (13241327,  16, 'Cloak') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (13241327,   1,   33561386) /* Setup */
     , (13241327,   3,  536870932) /* SoundTable */
     , (13241327,   7,  268437480) /* ClothingBase */
     , (13241327,   8,  100692121) /* Icon */
     , (13241327,  22,  872415275) /* PhysicsEffectTable */
     , (13241327,  50,  100691000) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-26T10:16:08.7719598-04:00",
  "ModifiedBy": "Grims Bold",
  "Changelog": [],
  "UserChangeSummary": "custom for tailoring",
  "IsDone": false
}
*/
