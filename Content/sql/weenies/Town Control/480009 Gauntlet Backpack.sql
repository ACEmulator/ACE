DELETE FROM `weenie` WHERE `class_Id` = 480009;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480009, 'ace480009-gauntletbackpackpk', 21, '2022-05-10 03:49:02') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480009,   1,        512) /* ItemType - Container */
     , (480009,   3,         13) /* PaletteTemplate - Purple */
     , (480009,   5,         15) /* EncumbranceVal */
     , (480009,   6,         48) /* ItemsCapacity */
     , (480009,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480009,  18,         64) /* UiEffects - Lightning */
     , (480009,  19,         250) /* Value */
     , (480009,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480009,   2, False) /* Open */
     , (480009,  22, True ) /* Inscribable */
     , (480009,  34, False) /* DefaultOpen */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480009,  12,     0.5) /* Shade */
     , (480009,  39,    1.75) /* DefaultScale */
     , (480009,  54,     0.5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480009,   1, 'Gauntlet Backpack') /* Name */
     , (480009,  14, 'Use this item to close it.') /* Use */
     , (480009,  16, 'A backpack with several side pouches.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480009,   1, 0x02000151) /* Setup */
     , (480009,   3, 0x20000014) /* SoundTable */
     , (480009,   6, 0x04000BEF) /* PaletteBase */
     , (480009,   7, 0x1000019B) /* ClothingBase */
     , (480009,   8, 0x06001BB5) /* Icon */
     , (480009,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480009,  52, 0x06006902) /* IconUnderlay */;
