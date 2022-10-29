DELETE FROM `weenie` WHERE `class_Id` = 13241329;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (13241329, 'ace13241329-hulkingbunnyslippers', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (13241329,   1,          2) /* ItemType - Armor */
     , (13241329,   3,          4) /* PaletteTemplate - Brown */
     , (13241329,   4,      65536) /* ClothingPriority - Feet */
     , (13241329,   5,        500) /* EncumbranceVal */
     , (13241329,   9,        256) /* ValidLocations - FootWear */
     , (13241329,  16,          1) /* ItemUseable - No */
     , (13241329,  19,         20) /* Value */
     , (13241329,  53,        101) /* PlacementPosition - Resting */
     , (13241329,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (13241329, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (13241329,  11, True ) /* IgnoreCollisions */
     , (13241329,  13, True ) /* Ethereal */
     , (13241329,  14, True ) /* GravityStatus */
     , (13241329,  22, True ) /* Inscribable */
     , (13241329,  69, False) /* IsSellable */
     , (13241329, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (13241329,  39,       2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (13241329,   1, 'Hulking Bunny Slippers') /* Name */
     , (13241329,  16, 'A pair of hulking bunny slippers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (13241329,   1,   33557435) /* Setup */
     , (13241329,   6,   67108990) /* PaletteBase */
     , (13241329,   7,  268437202) /* ClothingBase */
     , (13241329,   8,  100672378) /* Icon */
     , (13241329,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-26T09:08:47.5530548-04:00",
  "ModifiedBy": "Grims Bold",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": true
}
*/
