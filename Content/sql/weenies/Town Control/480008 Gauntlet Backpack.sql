DELETE FROM `weenie` WHERE `class_Id` = 480008;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480008, 'ace480008-gauntletbackpackpk', 21, '2022-05-10 03:49:02') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480008,   1,        512) /* ItemType - Container */
     , (480008,   3,          2) /* PaletteTemplate - Blue */
     , (480008,   5,         15) /* EncumbranceVal */
     , (480008,   6,         48) /* ItemsCapacity */
     , (480008,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480008,  18,          8) /* UiEffects - BoostMana */
     , (480008,  19,         250) /* Value */
     , (480008,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480008,   2, False) /* Open */
     , (480008,  22, True ) /* Inscribable */
     , (480008,  34, False) /* DefaultOpen */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480008,  12,     0.5) /* Shade */
     , (480008,  39,    1.75) /* DefaultScale */
     , (480008,  54,     0.5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480008,   1, 'Gauntlet Backpack') /* Name */
     , (480008,  14, 'Use this item to close it.') /* Use */
     , (480008,  16, 'A backpack with several side pouches.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480008,   1, 0x02000151) /* Setup */
     , (480008,   3, 0x20000014) /* SoundTable */
     , (480008,   6, 0x04000BEF) /* PaletteBase */
     , (480008,   7, 0x1000019B) /* ClothingBase */
     , (480008,   8, 0x06001BB3) /* Icon */
     , (480008,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480008,  52, 0x06006901) /* IconUnderlay */;
