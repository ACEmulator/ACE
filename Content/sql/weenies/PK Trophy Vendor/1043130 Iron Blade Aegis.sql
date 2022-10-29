DELETE FROM `weenie` WHERE `class_Id` = 1043130;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1043130, 'ace1043130-ironbladeaegis', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1043130,   1,          2) /* ItemType - Armor */
     , (1043130,   5,        300) /* EncumbranceVal */
     , (1043130,   9,    2097152) /* ValidLocations - Shield */
     , (1043130,  16,          1) /* ItemUseable - No */
     , (1043130,  19,         20) /* Value */
     , (1043130,  51,          4) /* CombatUse - Shield */
     , (1043130,  52,          3) /* ParentLocation - Shield */
     , (1043130,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1043130, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1043130,  11, True ) /* IgnoreCollisions */
     , (1043130,  13, True ) /* Ethereal */
     , (1043130,  14, True ) /* GravityStatus */
     , (1043130,  19, True ) /* Attackable */
     , (1043130,  69, False) /* IsSellable */
     , (1043130,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1043130,  39, 1.2000000476837158) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1043130,   1, 'Iron Blade Aegis') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1043130,   1,   33561094) /* Setup */
     , (1043130,   3,  536870932) /* SoundTable */
     , (1043130,   8,  100691459) /* Icon */
     , (1043130,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:26:31.9933956-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
