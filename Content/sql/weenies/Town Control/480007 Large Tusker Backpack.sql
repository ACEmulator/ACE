DELETE FROM `weenie` WHERE `class_Id` = 480007;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480007, 'backpacktuskerlargepk', 21, '2005-02-09 10:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480007,   1,        512) /* ItemType - Container */
     , (480007,   3,         21) /* PaletteTemplate - Gold */
     , (480007,   5,          1) /* EncumbranceVal */
     , (480007,   6,         48) /* ItemsCapacity */
     , (480007,   7,          0) /* ContainersCapacity */
     , (480007,   8,          0) /* Mass */
     , (480007,   9,          0) /* ValidLocations - None */
     , (480007,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480007,  19,        250) /* Value */
     , (480007,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480007,  96,       2000) /* EncumbranceCapacity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480007,  22, True ) /* Inscribable */
     , (480007,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480007,  39,     1.6) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480007,   1, 'Large Tusker Backpack') /* Name */
     , (480007,  15, 'A light Tusker backpack.') /* ShortDesc */
     , (480007,  16, 'This tusker was a veritable genius, judging by the size of it''s cranium.  It''s a good thing you stopped it from breeding, or their might have been a whole new breed of Tusker...') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480007,   1, 0x0200095A) /* Setup */
     , (480007,   3, 0x20000014) /* SoundTable */
     , (480007,   6, 0x0400102F) /* PaletteBase */
     , (480007,   7, 0x1000032E) /* ClothingBase */
     , (480007,   8, 0x060022AF) /* Icon */
     , (480007,  22, 0x3400002B) /* PhysicsEffectTable */;
