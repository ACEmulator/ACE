DELETE FROM `weenie` WHERE `class_Id` = 1038922;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1038922, 'ace1038922-tthuunshield', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1038922,   1,          2) /* ItemType - Armor */
     , (1038922,   5,          0) /* EncumbranceVal */
     , (1038922,   9,    2097152) /* ValidLocations - Shield */
     , (1038922,  16,          1) /* ItemUseable - No */
     , (1038922,  19,         20) /* Value */
     , (1038922,  51,          4) /* CombatUse - Shield */
     , (1038922,  52,          3) /* ParentLocation - Shield */
     , (1038922,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1038922, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1038922,  11, True ) /* IgnoreCollisions */
     , (1038922,  13, True ) /* Ethereal */
     , (1038922,  14, True ) /* GravityStatus */
     , (1038922,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1038922,  39, 1.600000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1038922,   1, 'T''thuun Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1038922,   1,   33560679) /* Setup */
     , (1038922,   3,  536870932) /* SoundTable */
     , (1038922,   8,  100690288) /* Icon */
     , (1038922,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:27:01.6492163-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
