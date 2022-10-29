DELETE FROM `weenie` WHERE `class_Id` = 1030872;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030872, 'ace1030872-eyeofthefallen', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030872,   1,      32768) /* ItemType - Caster */
     , (1030872,   5,          0) /* EncumbranceVal */
     , (1030872,   8,         50) /* Mass */
     , (1030872,   9,   16777216) /* ValidLocations - Held */
     , (1030872,  16,          1) /* ItemUseable - No */
     , (1030872,  19,         20) /* Value */
     , (1030872,  46,        512) /* DefaultCombatStyle - Magic */
     , (1030872,  52,          1) /* ParentLocation - RightHand */
     , (1030872,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030872,  94,         16) /* TargetType - Creature */
     , (1030872, 150,        103) /* HookPlacement - Hook */
     , (1030872, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030872,  11, True ) /* IgnoreCollisions */
     , (1030872,  13, True ) /* Ethereal */
     , (1030872,  14, True ) /* GravityStatus */
     , (1030872,  19, True ) /* Attackable */
     , (1030872,  22, True ) /* Inscribable */
     , (1030872,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030872,  39, 0.800000011920929) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030872,   1, 'Eye of the Fallen') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030872,   1,   33559279) /* Setup */
     , (1030872,   3,  536870932) /* SoundTable */
     , (1030872,   8,  100677502) /* Icon */
     , (1030872,  22,  872415275) /* PhysicsEffectTable */
     , (1030872,  27, 1073741873) /* UseUserAnimation - MagicHeal */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T14:52:21.5780091-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Updated Crit Multi to 2 from 1 - Done",
  "IsDone": true
}
*/
