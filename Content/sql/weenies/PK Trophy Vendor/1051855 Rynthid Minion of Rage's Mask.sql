DELETE FROM `weenie` WHERE `class_Id` = 1051855;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1051855, 'ace1051855-rynthidminionofragesmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1051855,   1,          2) /* ItemType - Armor */
     , (1051855,   3,         14) /* PaletteTemplate - Red */
     , (1051855,   4,      16384) /* ClothingPriority - Head */
     , (1051855,   5,          1) /* EncumbranceVal */
     , (1051855,   9,          1) /* ValidLocations - HeadWear */
     , (1051855,  16,          1) /* ItemUseable - No */
     , (1051855,  18,          1) /* UiEffects - Magical */
     , (1051855,  19,         20) /* Value */
     , (1051855,  53,        101) /* PlacementPosition - Resting */
     , (1051855,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1051855, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1051855,  11, True ) /* IgnoreCollisions */
     , (1051855,  13, True ) /* Ethereal */
     , (1051855,  14, True ) /* GravityStatus */
     , (1051855,  19, True ) /* Attackable */
     , (1051855,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1051855,   1, 'Rynthid Minion of Rage''s Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1051855,   1,   33561594) /* Setup */
     , (1051855,   3,  536870932) /* SoundTable */
     , (1051855,   7,  268437595) /* ClothingBase */
     , (1051855,   8,  100693220) /* Icon */
     , (1051855,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:51:51.4366121-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette template int",
  "IsDone": false
}
*/
