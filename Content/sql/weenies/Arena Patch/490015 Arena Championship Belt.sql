DELETE FROM `weenie` WHERE `class_Id` = 490015;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490015, 'ace490015-darkbeatbackpacktailor', 21, '2021-11-01 00:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490015,   1,        512) /* ItemType - Container */
     , (490015,   3,         39) /* PaletteTemplate - Black */
     , (490015,   5,         15) /* EncumbranceVal */
     , (490015,   6,         96) /* ItemsCapacity */
     , (490015,   8,        200) /* Mass */
     , (490015,   9,          0) /* ValidLocations - None */
     , (490015,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (490015,  18,          1) /* UiEffects - Magical */
     , (490015,  19,          50) /* Value */
     , (490015,  33,          1) /* Bonded - Bonded */
     , (490015,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490015,  96,       2000) /* EncumbranceCapacity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490015,  22, True ) /* Inscribable */
     , (490015,  23, True ) /* DestroyOnSell */
     , (490015,  69, False) /* IsSellable */
     , (490015,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490015,  39,    1.75) /* DefaultScale */
     , (490015,  54,     0.5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490015,   1, 'Arena Championship Belt') /* Name */
     , (490015,  14, 'Use this item to close it.') /* Use */
     , (490015,  16, 'A finely made belt made for champions of the arena.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490015,   1, 0x02001097) /* Setup */
     , (490015,   3, 0x20000014) /* SoundTable */
     , (490015,   6, 0x0400007E) /* PaletteBase */
     , (490015,   7, 0x10000553) /* ClothingBase */
     , (490015,   8, 0x06003339) /* Icon */
     , (490015,  52, 0x06005B0C) /* IconUnderlay */;
