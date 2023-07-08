DELETE FROM `weenie` WHERE `class_Id` = 1000002;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1000002, 'pktrophy', 51, '2021-11-03 08:49:59') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1000002,   1,     128) /* ItemType - PromissoryNote */
     , (1000002,   5,          1) /* EncumbranceVal */
     , (1000002,   8,          1) /* Mass */
     , (1000002,   9,          0) /* ValidLocations - None */
     , (1000002,  11,        1000) /* MaxStackSize */
     , (1000002,  12,          1) /* StackSize */
     , (1000002,  13,          1) /* StackUnitEncumbrance */
     , (1000002,  14,          1) /* StackUnitMass */
     , (1000002,  15,          1) /* StackUnitValue */
     , (1000002,  16,          1) /* ItemUseable - No */
     , (1000002,  19,          1) /* Value */
     , (1000002,  33,          1) /* Bonded - Bonded */
     , (1000002,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

     INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1000002,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1000002,   1, 'PK Trophy') /* Name */
     , (1000002,  20, 'PK Trophies') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1000002,   8,  100686488) /* Icon */;

