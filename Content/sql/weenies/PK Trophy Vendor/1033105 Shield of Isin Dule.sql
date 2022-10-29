DELETE FROM `weenie` WHERE `class_Id` = 1033105;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1033105, 'ace1033105-shieldofisindule', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1033105,   1,          2) /* ItemType - Armor */
     , (1033105,   5,          0) /* EncumbranceVal */
     , (1033105,   9,    2097152) /* ValidLocations - Shield */
     , (1033105,  16,          1) /* ItemUseable - No */
     , (1033105,  19,         20) /* Value */
     , (1033105,  51,          4) /* CombatUse - Shield */
     , (1033105,  52,          3) /* ParentLocation - Shield */
     , (1033105,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1033105,  11, True ) /* IgnoreCollisions */
     , (1033105,  13, True ) /* Ethereal */
     , (1033105,  14, True ) /* GravityStatus */
     , (1033105,  19, True ) /* Attackable */
     , (1033105,  22, True ) /* Inscribable */
     , (1033105,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1033105,   1, 'Shield of Isin Dule') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1033105,   1,   33559923) /* Setup */
     , (1033105,   3,  536870932) /* SoundTable */
     , (1033105,   8,  100688919) /* Icon */
     , (1033105,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:23:33.4806741-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
