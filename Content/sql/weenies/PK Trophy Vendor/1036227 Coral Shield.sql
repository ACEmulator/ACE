DELETE FROM `weenie` WHERE `class_Id` = 1036227;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1036227, 'ace1036227-coralshield', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1036227,   1,          2) /* ItemType - Armor */
     , (1036227,   5,          0) /* EncumbranceVal */
     , (1036227,   9,    2097152) /* ValidLocations - Shield */
     , (1036227,  16,          1) /* ItemUseable - No */
     , (1036227,  19,         20) /* Value */
     , (1036227,  51,          4) /* CombatUse - Shield */
     , (1036227,  53,        101) /* PlacementPosition - Resting */
     , (1036227,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1036227, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1036227,  11, True ) /* IgnoreCollisions */
     , (1036227,  13, True ) /* Ethereal */
     , (1036227,  14, True ) /* GravityStatus */
     , (1036227,  19, True ) /* Attackable */
     , (1036227,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1036227,   1, 'Coral Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1036227,   1,   33560375) /* Setup */
     , (1036227,   3,  536870932) /* SoundTable */
     , (1036227,   8,  100689610) /* Icon */
     , (1036227,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:17:18.4737548-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
