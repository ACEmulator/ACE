DELETE FROM `weenie` WHERE `class_Id` = 1052514;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1052514, 'ace1052514-painterspalette', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1052514,   1,      32768) /* ItemType - Caster */
     , (1052514,   5,          1) /* EncumbranceVal */
     , (1052514,   9,   16777216) /* ValidLocations - Held */
     , (1052514,  16,          1) /* ItemUseable - No */
     , (1052514,  19,         20) /* Value */
     , (1052514,  46,        512) /* DefaultCombatStyle - Magic */
     , (1052514,  52,          1) /* ParentLocation - RightHand */
     , (1052514,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1052514,  94,         16) /* TargetType - Creature */
     , (1052514, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1052514,  11, True ) /* IgnoreCollisions */
     , (1052514,  13, True ) /* Ethereal */
     , (1052514,  14, True ) /* GravityStatus */
     , (1052514,  19, True ) /* Attackable */
     , (1052514,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1052514,   1, 'Painter''s Palette') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1052514,   1,   33561635) /* Setup */
     , (1052514,   3,  536870932) /* SoundTable */
     , (1052514,   8,  100693286) /* Icon */
     , (1052514,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T21:52:14.9801407-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Adding Default Stance\nRemoved Buffed Defense Mod.",
  "IsDone": false
}
*/
