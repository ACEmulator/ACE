DELETE FROM `weenie` WHERE `class_Id` = 30936;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30936, 'backpackrare', 21, '2021-11-17 16:56:08') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30936,   1,        512) /* ItemType - Container */
     , (30936,   5,         15) /* EncumbranceVal */
     , (30936,   6,         48) /* ItemsCapacity */
     , (30936,   8,         15) /* Mass */
     , (30936,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (30936,  17,        186) /* RareId */
     , (30936,  18,          1) /* UiEffects - Magical */
     , (30936,  19,         65) /* Value */
     , (30936,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30936,   2, False) /* Open */
     , (30936,  11, True ) /* IgnoreCollisions */
     , (30936,  13, True ) /* Ethereal */
     , (30936,  14, True ) /* GravityStatus */
     , (30936,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30936,  39,    1.75) /* DefaultScale */
     , (30936,  54,     0.5) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30936,   1, 'Pack') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30936,   1, 0x02000151) /* Setup */
     , (30936,   3, 0x20000014) /* SoundTable */
     , (30936,   6, 0x04000BEF) /* PaletteBase */
     , (30936,   8, 0x06005D15) /* Icon */
     , (30936,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30936,  52, 0x06005B0C) /* IconUnderlay */;
