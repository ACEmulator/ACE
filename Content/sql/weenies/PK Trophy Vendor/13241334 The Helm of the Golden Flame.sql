DELETE FROM `weenie` WHERE `class_Id` = 13241334;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (13241334, 'ace13241334-thehelmofthegoldenflame', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (13241334,   1,          2) /* ItemType - Armor */
     , (13241334,   3,         21) /* PaletteTemplate - Gold */
     , (13241334,   4,      16384) /* ClothingPriority - Head */
     , (13241334,   5,         10) /* EncumbranceVal */
     , (13241334,   9,          1) /* ValidLocations - HeadWear */
     , (13241334,  16,          1) /* ItemUseable - No */
     , (13241334,  18,          1) /* UiEffects - Magical */
     , (13241334,  19,         20) /* Value */
     , (13241334,  53,        101) /* PlacementPosition - Resting */
     , (13241334,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (13241334, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (13241334,  11, True ) /* IgnoreCollisions */
     , (13241334,  13, True ) /* Ethereal */
     , (13241334,  14, True ) /* GravityStatus */
     , (13241334,  19, True ) /* Attackable */
     , (13241334,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (13241334,   1, 'The Helm of the Golden Flame') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (13241334,   1,   33560127) /* Setup */
     , (13241334,   3,  536870932) /* SoundTable */
     , (13241334,   7,  268437174) /* ClothingBase */
     , (13241334,   8,  100689238) /* Icon */
     , (13241334,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-26T10:57:38.2522571-04:00",
  "ModifiedBy": "Grims Bold",
  "Changelog": [],
  "UserChangeSummary": "Custom Tailoring option",
  "IsDone": false
}
*/
