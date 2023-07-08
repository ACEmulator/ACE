DELETE FROM `weenie` WHERE `class_Id` = 480010;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480010, 'ace480010-gauntletbackpackpk', 21, '2022-05-10 03:49:02') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480010,   1,        512) /* ItemType - Container */
     , (480010,   3,         14) /* PaletteTemplate - Red */
     , (480010,   5,         15) /* EncumbranceVal */
     , (480010,   6,         48) /* ItemsCapacity */
     , (480010,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480010,  18,          4) /* UiEffects - BoostHealth */
     , (480010,  19,         250) /* Value */
     , (480010,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480010,   2, False) /* Open */
     , (480010,  22, True ) /* Inscribable */
     , (480010,  34, False) /* DefaultOpen */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480010,  12,     0.5) /* Shade */
     , (480010,  39,    1.75) /* DefaultScale */
     , (480010,  54,     0.5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480010,   1, 'Gauntlet Backpack') /* Name */
     , (480010,  14, 'Use this item to close it.') /* Use */
     , (480010,  16, 'A backpack with several side pouches.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480010,   1, 0x02000151) /* Setup */
     , (480010,   3, 0x20000014) /* SoundTable */
     , (480010,   6, 0x04000BEF) /* PaletteBase */
     , (480010,   7, 0x1000019B) /* ClothingBase */
     , (480010,   8, 0x06001BB0) /* Icon */
     , (480010,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480010,  52, 0x06006903) /* IconUnderlay */;
