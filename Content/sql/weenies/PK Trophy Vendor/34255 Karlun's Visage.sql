DELETE FROM `weenie` WHERE `class_Id` = 34255;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (34255, 'ace34255-karlunsvisage', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (34255,   1,          2) /* ItemType - Armor */
     , (34255,   3,         14) /* PaletteTemplate - Red */
     , (34255,   4,      16384) /* ClothingPriority - Head */
     , (34255,   5,          1) /* EncumbranceVal */
     , (34255,   9,          1) /* ValidLocations - HeadWear */
     , (34255,  19,         20) /* Value */
     , (34255,  53,        101) /* PlacementPosition - Resting */
     , (34255,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (34255, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (34255,  11, True ) /* IgnoreCollisions */
     , (34255,  13, True ) /* Ethereal */
     , (34255,  14, True ) /* GravityStatus */
     , (34255,  19, True ) /* Attackable */
     , (34255,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (34255,   1, 'Karlun''s Visage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (34255,   1,   33560128) /* Setup */
     , (34255,   3,  536870932) /* SoundTable */
     , (34255,   7,  268437175) /* ClothingBase */
     , (34255,   8,  100689241) /* Icon */
     , (34255,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:41:20.463311-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
