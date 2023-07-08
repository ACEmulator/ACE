DELETE FROM `weenie` WHERE `class_Id` = 1000003;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1000003, 'bloodytearstrophy', 51, '2021-11-03 08:49:59') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1000003,   1,     128) /* ItemType - PromissoryNote */
     , (1000003,   5,          1) /* EncumbranceVal */
     , (1000003,   8,          1) /* Mass */
     , (1000003,   9,          0) /* ValidLocations - None */
     , (1000003,  11,        1000) /* MaxStackSize */
     , (1000003,  12,          1) /* StackSize */
     , (1000003,  13,          1) /* StackUnitEncumbrance */
     , (1000003,  14,          1) /* StackUnitMass */
     , (1000003,  15,          1) /* StackUnitValue */
     , (1000003,  16,          1) /* ItemUseable - No */
     , (1000003,  19,          1) /* Value */
     , (1000003,  33,          1) /* Bonded - Bonded */
	 , (1000003, 114,          1) /* Attuned - Attuned */
     , (1000003,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

     INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1000003,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1000003,   1, 'Phial of Bloody Tears') /* Name */
     , (1000003,  20, 'Phials of Bloody Tears') /* PluralName */
	 , (1000003,  16, 'An ancient philter, filled to the brim with tears of blood collected from a fallen foe.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1000003,  8, 0x060033CC) /* Icon */
     , (1000003,  52, 0x06005B0C) /* IconUnderlay */;

