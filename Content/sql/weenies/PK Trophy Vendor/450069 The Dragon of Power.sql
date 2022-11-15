DELETE FROM `weenie` WHERE `class_Id` = 450069;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450069, 'ace450069-thedragonofpowertailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450069,   1,          2) /* ItemType - Armor */
     , (450069,   3,          8) /* PaletteTemplate - Green */
     , (450069,   4,      16384) /* ClothingPriority - Head */
     , (450069,   5,          1) /* EncumbranceVal */
     , (450069,   9,          1) /* ValidLocations - HeadWear */
     , (450069,  19,         20) /* Value */
     , (450069,  53,        101) /* PlacementPosition - Resting */
     , (450069,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450069, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450069,  11, True ) /* IgnoreCollisions */
     , (450069,  13, True ) /* Ethereal */
     , (450069,  14, True ) /* GravityStatus */
     , (450069,  19, True ) /* Attackable */
     , (450069,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450069,   1, 'The Dragon of Power') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450069,   1,   33560112) /* Setup */
     , (450069,   3,  536870932) /* SoundTable */
     , (450069,   7,  268437165) /* ClothingBase */
     , (450069,   8,  100689200) /* Icon */
     , (450069,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:36:59.3225458-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
