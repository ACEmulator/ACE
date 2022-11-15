DELETE FROM `weenie` WHERE `class_Id` = 450072;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450072, 'ace450072-thestagofbellenessetailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450072,   1,          2) /* ItemType - Armor */
     , (450072,   3,          9) /* PaletteTemplate - Grey */
     , (450072,   4,      16384) /* ClothingPriority - Head */
     , (450072,   5,          1) /* EncumbranceVal */
     , (450072,   9,          1) /* ValidLocations - HeadWear */
     , (450072,  19,         20) /* Value */
     , (450072,  53,        101) /* PlacementPosition - Resting */
     , (450072,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450072, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450072,  11, True ) /* IgnoreCollisions */
     , (450072,  13, True ) /* Ethereal */
     , (450072,  14, True ) /* GravityStatus */
     , (450072,  19, True ) /* Attackable */
     , (450072,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450072,   1, 'The Stag of Bellenesse') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450072,   1,   33560109) /* Setup */
     , (450072,   3,  536870932) /* SoundTable */
     , (450072,   7,  268437157) /* ClothingBase */
     , (450072,   8,  100689165) /* Icon */
     , (450072,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:35:14.3076564-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette base int",
  "IsDone": false
}
*/
