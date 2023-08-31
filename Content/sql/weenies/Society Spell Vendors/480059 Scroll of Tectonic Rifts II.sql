DELETE FROM `weenie` WHERE `class_Id` = 480059;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480059, 'ace480059-scrolloftectonicriftsiipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480059,   1,       8192) /* ItemType - Writable */
     , (480059,   5,         30) /* EncumbranceVal */
     , (480059,  16,          8) /* ItemUseable - Contained */
	 , (480059,  33,         1) /* Bonded - Slippery */
	 , (480059, 114,          1) /* Attuned - Attuned */
     , (480059,  19,        100) /* Value */
     , (480059,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480059,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480059,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480059,   1, 'Scroll of Tectonic Rifts II') /* Name */
     , (480059,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480059,  16, 'Inscribed spell: Tectonic Rifts II
Shoots eight shock waves outward from the caster. Each wave does 49-96 points of bludgeoning damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480059,   1, 0x0200018A) /* Setup */
     , (480059,   8, 0x06003592) /* Icon */
     , (480059,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480059,  28,       6196) /* Spell - Tectonic Rifts II */;
