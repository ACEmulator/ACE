DELETE FROM `weenie` WHERE `class_Id` = 480053;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480053, 'ace480053-scrollofradiantbloodrecallpk', 34, '2022-12-28 05:57:21') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480053,   1,       8192) /* ItemType - Writable */
     , (480053,   5,         50) /* EncumbranceVal */
     , (480053,  16,          8) /* ItemUseable - Contained */
	 , (480053, 114,          1) /* Attuned - Attuned */
	 , (480053,  33,          1) /* Bonded - Bonded */
     , (480053,  19,          100) /* Value */
     , (480053,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480053,   1, False) /* Stuck */
     , (480053,  11, True ) /* IgnoreCollisions */
     , (480053,  13, True ) /* Ethereal */
     , (480053,  14, True ) /* GravityStatus */
     , (480053,  19, True ) /* Attackable */
     , (480053,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480053,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480053,   1, 'Scroll of Radiant Blood Recall') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480053,   1, 0x0200018A) /* Setup */
     , (480053,   8, 0x06007554) /* Icon */
     , (480053,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480053,  28,       6327) /* Spell - Radiant Blood Stronghold Recall */;
