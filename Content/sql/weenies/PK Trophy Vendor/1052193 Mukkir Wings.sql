DELETE FROM `weenie` WHERE `class_Id` = 1052193;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1052193, 'ace1052193-mukkirwings', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1052193,   1,          4) /* ItemType - Clothing */
     , (1052193,   3,          1) /* PaletteTemplate - AquaBlue */
     , (1052193,   4,     131072) /* ClothingPriority - 131072 */
     , (1052193,   5,          1) /* EncumbranceVal */
     , (1052193,   9,  134217728) /* ValidLocations - Cloak */
     , (1052193,  16,          1) /* ItemUseable - No */
     , (1052193,  18,          1) /* UiEffects - Magical */
     , (1052193,  19,         20) /* Value */
     , (1052193,  53,        101) /* PlacementPosition - Resting */
     , (1052193,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1052193,  11, True ) /* IgnoreCollisions */
     , (1052193,  13, True ) /* Ethereal */
     , (1052193,  14, True ) /* GravityStatus */
     , (1052193,  19, True ) /* Attackable */
     , (1052193,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1052193,   1, 'Mukkir Wings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1052193,   1,   33561386) /* Setup */
     , (1052193,   3,  536870932) /* SoundTable */
     , (1052193,   7,  268437607) /* ClothingBase */
     , (1052193,   8,  100693238) /* Icon */
     , (1052193,  22,  872415275) /* PhysicsEffectTable */
     , (1052193,  50,  100690999) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:55:47.8906941-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added Palette template",
  "IsDone": false
}
*/
