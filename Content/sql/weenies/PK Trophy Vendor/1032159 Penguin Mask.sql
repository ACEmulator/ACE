DELETE FROM `weenie` WHERE `class_Id` = 1032159;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1032159, 'ace1032159-penguinmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1032159,   1,          2) /* ItemType - Armor */
     , (1032159,   3,          4) /* PaletteTemplate - Brown */
     , (1032159,   4,      16384) /* ClothingPriority - Head */
     , (1032159,   5,          1) /* EncumbranceVal */
     , (1032159,   9,          1) /* ValidLocations - HeadWear */
     , (1032159,  16,          1) /* ItemUseable - No */
     , (1032159,  19,         20) /* Value */
     , (1032159,  27,         32) /* ArmorType - Metal */
     , (1032159,  28,         10) /* ArmorLevel */
     , (1032159,  53,        101) /* PlacementPosition - Resting */
     , (1032159,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1032159, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1032159,   1, False) /* Stuck */
     , (1032159,  11, True ) /* IgnoreCollisions */
     , (1032159,  13, True ) /* Ethereal */
     , (1032159,  14, True ) /* GravityStatus */
     , (1032159,  19, True ) /* Attackable */
     , (1032159,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1032159,  12,       0) /* Shade */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1032159,   1, 'Penguin Mask') /* Name */
     , (1032159,  16, 'A mask crafted to resemble the head of the noble Penguin.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1032159,   1,   33559767) /* Setup */
     , (1032159,   3,  536870932) /* SoundTable */
     , (1032159,   7,  268437072) /* ClothingBase */
     , (1032159,   8,  100688479) /* Icon */
     , (1032159,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T23:44:56.6904545-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "add palette and shade",
  "IsDone": false
}
*/
