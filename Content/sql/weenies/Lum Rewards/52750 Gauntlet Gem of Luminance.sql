DELETE FROM `weenie` WHERE `class_Id` = 52750;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (52750, 'ace52750-gauntletgemofluminance', 38, '2023-08-20 21:14:32') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (52750,   1,       2048) /* ItemType - Gem */
     , (52750,   5,         50) /* EncumbranceVal */
     , (52750,  11,         10) /* MaxStackSize */
     , (52750,  12,          1) /* StackSize */
     , (52750,  16,          1) /* ItemUseable - No */
     , (52750,  19,         15) /* Value */
     , (52750,  53,        101) /* PlacementPosition - Resting */
     , (52750,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (52750,  11, True ) /* IgnoreCollisions */
     , (52750,  13, True ) /* Ethereal */
     , (52750,  14, True ) /* GravityStatus */
     , (52750,  15, True ) /* LightsStatus */
     , (52750,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (52750,   1, 'Gauntlet Gem of Luminance') /* Name */
     , (52750,  20, 'Gauntlet Gems of Luminance') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (52750,   1, 0x02000C79) /* Setup */
     , (52750,   3, 0x20000014) /* SoundTable */
     , (52750,   8, 0x06007095) /* Icon */
     , (52750,  22, 0x3400002B) /* PhysicsEffectTable */;
