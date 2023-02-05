DELETE FROM `weenie` WHERE `class_Id` = 480012;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480012, 'rationsfieldelaboratepk', 18, '2021-11-01 00:00:00') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480012,   1,         32) /* ItemType - Food */
     , (480012,   5,        125) /* EncumbranceVal */
     , (480012,   8,        230) /* Mass */
     , (480012,  11,        100) /* MaxStackSize */
     , (480012,  12,          1) /* StackSize */
     , (480012,  13,        125) /* StackUnitEncumbrance */
     , (480012,  14,        230) /* StackUnitMass */
     , (480012,  15,          10) /* StackUnitValue */
     , (480012,  16,          8) /* ItemUseable - Contained */
     , (480012,  19,          10) /* Value */
     , (480012,  89,          4) /* BoosterEnum - Stamina */
     , (480012,  90,        100) /* BoostValue */
     , (480012,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480012,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480012,   1, 'Elaborate Field Rations') /* Name */
     , (480012,  14, 'Use this item to eat it.') /* Use */
     , (480012,  15, 'An elaborate mix of reconstituted meat, nuts, and fruit. It''s very filling, and almost tasty.') /* ShortDesc */
     , (480012,  20, 'Elaborate Field Rations') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480012,   1, 0x02000181) /* Setup */
     , (480012,   3, 0x20000014) /* SoundTable */
     , (480012,   8, 0x060029D4) /* Icon */
     , (480012,  22, 0x3400002B) /* PhysicsEffectTable */;
