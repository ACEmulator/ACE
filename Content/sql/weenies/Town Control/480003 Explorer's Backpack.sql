DELETE FROM `weenie` WHERE `class_Id` = 480003;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480003, 'ace480003-explorersbackpacktailor', 21, '2021-11-01 00:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480003,   1,        512) /* ItemType - Container */
     , (480003,   3,         39) /* PaletteTemplate - Black */
     , (480003,   5,         15) /* EncumbranceVal */
     , (480003,   6,         48) /* ItemsCapacity */
     , (480003,   8,        200) /* Mass */
     , (480003,   9,          0) /* ValidLocations - None */
     , (480003,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480003,  18,          1) /* UiEffects - Magical */
     , (480003,  19,          250) /* Value */
     , (480003,  33,          1) /* Bonded - Bonded */
     , (480003,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480003,  96,       2000) /* EncumbranceCapacity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480003,  22, True ) /* Inscribable */
     , (480003,  23, True ) /* DestroyOnSell */
     , (480003,  69, False) /* IsSellable */
     , (480003,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480003,  39,    1.75) /* DefaultScale */
     , (480003,  54,     0.5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480003,   1, 'Explorer''s Backpack') /* Name */
     , (480003,   7, 'A gift from one Elite Explorer to another.  Your skills at exploration are an inspiration to many.') /* Inscription */
     , (480003,   8, 'Sean the Speedy') /* ScribeName */
     , (480003,  14, 'Use this item to close it.') /* Use */
     , (480003,  16, 'An excellent backpack with extra storage for long hauls.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480003,   1, 0x02000151) /* Setup */
     , (480003,   3, 0x20000014) /* SoundTable */
     , (480003,   6, 0x04000BEF) /* PaletteBase */
     , (480003,   7, 0x1000019B) /* ClothingBase */
     , (480003,   8, 0x06001BB4) /* Icon */
     , (480003,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480003,  50, 0x06006A78) /* IconOverlay */
     , (480003,  52, 0x06006700) /* IconUnderlay */;
