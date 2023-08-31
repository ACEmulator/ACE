DELETE FROM `weenie` WHERE `class_Id` = 480051;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480051, 'ace480051-scrollofeldrytchwebrecallpktrophy', 34, '2022-12-28 05:57:21') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480051,   1,       8192) /* ItemType - Writable */
     , (480051,   5,         50) /* EncumbranceVal */
     , (480051,  16,          8) /* ItemUseable - Contained */
	 , (480051, 114,          1) /* Attuned - Attuned */
	 , (480051,  33,          1) /* Bonded - Bonded */
     , (480051,  19,          100) /* Value */
     , (480051,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480051,   1, False) /* Stuck */
     , (480051,  11, True ) /* IgnoreCollisions */
     , (480051,  13, True ) /* Ethereal */
     , (480051,  14, True ) /* GravityStatus */
     , (480051,  19, True ) /* Attackable */
     , (480051,  22, True ) /* Inscribable */
	 , (480051,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480051,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480051,   1, 'Scroll of Eldrytch Web Recall') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480051,   1, 0x0200018A) /* Setup */
     , (480051,   8, 0x06007554) /* Icon */
     , (480051,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480051,  28,       6326) /* Spell - Eldrytch Web Stronghold Recall */;
