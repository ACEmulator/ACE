DELETE FROM `weenie` WHERE `class_Id` = 1051857;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1051857, 'ace1051857-rynthidravagersmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1051857,   1,          2) /* ItemType - Armor */
     , (1051857,   3,         14) /* PaletteTemplate - Red */
     , (1051857,   4,      16384) /* ClothingPriority - Head */
     , (1051857,   5,          1) /* EncumbranceVal */
     , (1051857,   9,          1) /* ValidLocations - HeadWear */
     , (1051857,  16,          1) /* ItemUseable - No */
     , (1051857,  18,          1) /* UiEffects - Magical */
     , (1051857,  19,         20) /* Value */
     , (1051857,  53,        101) /* PlacementPosition - Resting */
     , (1051857,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1051857, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1051857,  11, True ) /* IgnoreCollisions */
     , (1051857,  13, True ) /* Ethereal */
     , (1051857,  14, True ) /* GravityStatus */
     , (1051857,  19, True ) /* Attackable */
     , (1051857,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1051857,   1, 'Rynthid Ravager''s Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1051857,   1,   33561593) /* Setup */
     , (1051857,   3,  536870932) /* SoundTable */
     , (1051857,   7,  268437596) /* ClothingBase */
     , (1051857,   8,  100693221) /* Icon */
     , (1051857,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:51:24.3263611-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette template int",
  "IsDone": false
}
*/
