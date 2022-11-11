DELETE FROM `weenie` WHERE `class_Id` = 42127923;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42127923, 'towncontrolcurrency', 51, '2021-11-03 08:49:59') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42127923,   1,     262144) /* ItemType - PromissoryNote */
     , (42127923,   5,          1) /* EncumbranceVal */
     , (42127923,   8,          1) /* Mass */
     , (42127923,   9,          0) /* ValidLocations - None */
     , (42127923,  11,        100) /* MaxStackSize */
     , (42127923,  12,          1) /* StackSize */
     , (42127923,  13,          1) /* StackUnitEncumbrance */
     , (42127923,  14,          1) /* StackUnitMass */
     , (42127923,  15,          1) /* StackUnitValue */
     , (42127923,  16,          1) /* ItemUseable - No */
     , (42127923,  18,          4) /* UI Effects Boosted Health */
     , (42127923,  19,          1) /* Value */
     , (42127923,  33,          1) /* Bonded - Bonded */
     , (42127923,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42127923,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42127923,   1, 'A Phial of Innocent Tears') /* Name */
     , (42127923,  20, 'Phials of Innocent Tears') /* PluralName */
     , (42127923,  16, 'An ancient philter, filled to the brim with tears of the Innocent.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42127923,   1, 0x020005FD) /* Setup */
     , (42127923,   3, 0x20000014) /* SoundTable */
     , (42127923,   8, 0x060033CC) /* Icon */
     , (42127923,  22, 0x3400002B) /* PhysicsEffectTable */
     , (42127923,  52,  100689805) /* IconUnderlay */;
