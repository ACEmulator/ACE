DELETE FROM `weenie` WHERE `class_Id` = 450373;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450373, 'ace450373-corruptedaegistailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450373,   1,          2) /* ItemType - Armor */
     , (450373,   3,         39) /* PaletteTemplate - Black */
     , (450373,   5,        0) /* EncumbranceVal */
     , (450373,   9,    2097152) /* ValidLocations - Shield */
     , (450373,  16,          1) /* ItemUseable - No */
     , (450373,  19,        20) /* Value */
     , (450373,  28,          0) /* ArmorLevel */
     , (450373,  51,          4) /* CombatUse - Shield */
     , (450373,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450373,  19, True ) /* Attackable */
     , (450373,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450373,   1, 'Corrupted Aegis') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450373,   1, 0x02000576) /* Setup */
     , (450373,   3, 0x20000014) /* SoundTable */
     , (450373,   6, 0x04000BEF) /* PaletteBase */
     , (450373,   7, 0x10000155) /* ClothingBase */
     , (450373,   8, 0x060018DA) /* Icon */
     , (450373,  22, 0x3400002B) /* PhysicsEffectTable */;
