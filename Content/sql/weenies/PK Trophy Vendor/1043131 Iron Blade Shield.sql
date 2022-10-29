DELETE FROM `weenie` WHERE `class_Id` = 1043131;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1043131, 'ace1043131-ironbladeshield', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1043131,   1,          2) /* ItemType - Armor */
     , (1043131,   5,          0) /* EncumbranceVal */
     , (1043131,   9,    2097152) /* ValidLocations - Shield */
     , (1043131,  16,          1) /* ItemUseable - No */
     , (1043131,  19,         20) /* Value */
     , (1043131,  51,          4) /* CombatUse - Shield */
     , (1043131,  52,          3) /* ParentLocation - Shield */
     , (1043131,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1043131, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1043131,  11, True ) /* IgnoreCollisions */
     , (1043131,  13, True ) /* Ethereal */
     , (1043131,  14, True ) /* GravityStatus */
     , (1043131,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1043131,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1043131,   1, 'Iron Blade Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1043131,   1,   33561095) /* Setup */
     , (1043131,   3,  536870932) /* SoundTable */
     , (1043131,   8,  100691460) /* Icon */
     , (1043131,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:25:56.3805309-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
