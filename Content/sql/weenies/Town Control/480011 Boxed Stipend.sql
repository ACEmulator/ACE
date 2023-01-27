DELETE FROM `weenie` WHERE `class_Id` = 480011;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480011, 'ace480011-boxedstipendpk', 38, '2022-12-28 05:57:21') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480011,   1,        128) /* ItemType - Misc */
     , (480011,   5,          1) /* EncumbranceVal */
     , (480011,  11,       1) /* MaxStackSize */
     , (480011,  12,          1) /* StackSize */
     , (480011,  13,          1) /* StackUnitEncumbrance */
     , (480011,  15,          100) /* StackUnitValue */
     , (480011,  16,          8) /* ItemUseable - No */
     , (480011,  19,          100) /* Value */
     , (480011,  33,          1) /* Bonded - Bonded */
     , (480011,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480011, 114,          1) /* Attuned - Attuned */
	 , (480011,  94,         16) /* TargetType - Creature */
     , (480011, 269,         1) /* UseCreateQuantity */;;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480011,   1, False) /* Stuck */
     , (480011,  11, True ) /* IgnoreCollisions */
     , (480011,  13, True ) /* Ethereal */
     , (480011,  14, True ) /* GravityStatus */
     , (480011,  19, True ) /* Attackable */
     , (480011,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480011,   1, 'Boxed Stipend') /* Name */
     , (480011,  14, 'Use this crate to retrieve its contents.') /* Use */
     , (480011,  16, 'A box containing a stipend for service to Dereth. Stipends may be used to purchase special items at a stipend vendor.') /* LongDesc */;


INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480011,   1, 0x0200011E) /* Setup */
     , (480011,   3, 0x20000014) /* SoundTable */
	, (480011,   8, 0x060012F8) /* Icon */
     , (480011,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480011,  38,      46423) /* UseCreateItem - Promissory Note */;