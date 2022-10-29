DELETE FROM `weenie` WHERE `class_Id` = 34144;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (34144, 'ace34144-thestagofbellenesse', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (34144,   1,          2) /* ItemType - Armor */
     , (34144,   3,          9) /* PaletteTemplate - Grey */
     , (34144,   4,      16384) /* ClothingPriority - Head */
     , (34144,   5,          1) /* EncumbranceVal */
     , (34144,   9,          1) /* ValidLocations - HeadWear */
     , (34144,  19,         20) /* Value */
     , (34144,  53,        101) /* PlacementPosition - Resting */
     , (34144,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (34144, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (34144,  11, True ) /* IgnoreCollisions */
     , (34144,  13, True ) /* Ethereal */
     , (34144,  14, True ) /* GravityStatus */
     , (34144,  19, True ) /* Attackable */
     , (34144,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (34144,   1, 'The Stag of Bellenesse') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (34144,   1,   33560109) /* Setup */
     , (34144,   3,  536870932) /* SoundTable */
     , (34144,   7,  268437157) /* ClothingBase */
     , (34144,   8,  100689165) /* Icon */
     , (34144,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:35:14.3076564-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette base int",
  "IsDone": false
}
*/
