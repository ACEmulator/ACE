DELETE FROM `weenie` WHERE `class_Id` = 480020;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480020, 'ace480020-scrollofcelestialhandrecallpk', 34, '2022-12-28 05:57:21') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480020,   1,       8192) /* ItemType - Writable */
     , (480020,   5,         50) /* EncumbranceVal */
     , (480020,  16,          8) /* ItemUseable - Contained */
     , (480020,  19,          100) /* Value */
	 , (480020,  33,         1) /* Bonded - Slippery */
	 , (480020, 114,          1) /* Attuned - Attuned */
     , (480020,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480020,   1, False) /* Stuck */
     , (480020,  11, True ) /* IgnoreCollisions */
     , (480020,  13, True ) /* Ethereal */
     , (480020,  14, True ) /* GravityStatus */
     , (480020,  19, True ) /* Attackable */
     , (480020,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480020,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480020,   1, 'Scroll of Celestial Hand Recall') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480020,   1, 0x0200018A) /* Setup */
     , (480020,   8, 0x06007554) /* Icon */
     , (480020,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480020,  28,       6325) /* Spell - Celestial Hand Stronghold Recall */;
