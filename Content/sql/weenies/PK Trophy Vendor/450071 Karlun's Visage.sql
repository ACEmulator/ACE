DELETE FROM `weenie` WHERE `class_Id` = 450071;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450071, 'ace450071-karlunsvisagetailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450071,   1,          2) /* ItemType - Armor */
     , (450071,   3,         14) /* PaletteTemplate - Red */
     , (450071,   4,      16384) /* ClothingPriority - Head */
     , (450071,   5,          1) /* EncumbranceVal */
     , (450071,   9,          1) /* ValidLocations - HeadWear */
     , (450071,  19,         20) /* Value */
     , (450071,  53,        101) /* PlacementPosition - Resting */
     , (450071,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450071, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450071,  11, True ) /* IgnoreCollisions */
     , (450071,  13, True ) /* Ethereal */
     , (450071,  14, True ) /* GravityStatus */
     , (450071,  19, True ) /* Attackable */
     , (450071,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450071,   1, 'Karlun''s Visage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450071,   1,   33560128) /* Setup */
     , (450071,   3,  536870932) /* SoundTable */
     , (450071,   7,  268437175) /* ClothingBase */
     , (450071,   8,  100689241) /* Icon */
     , (450071,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:41:20.463311-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
