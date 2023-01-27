DELETE FROM `weenie` WHERE `class_Id` = 450375;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450375, 'ace450375-tthuunaegistailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450375,   1,          2) /* ItemType - Armor */
     , (450375,   5,        0) /* EncumbranceVal */
     , (450375,   9,    2097152) /* ValidLocations - Shield */
     , (450375,  16,          1) /* ItemUseable - No */
     , (450375,  19,          20) /* Value */
     , (450375,  28,         0) /* ArmorLevel */
     , (450375,  51,          4) /* CombatUse - Shield */
     , (450375,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450375, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450375,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450375,  13,     0.9) /* ArmorModVsSlash */
     , (450375,  14,     0.9) /* ArmorModVsPierce */
     , (450375,  15,     0.9) /* ArmorModVsBludgeon */
     , (450375,  16,     0.9) /* ArmorModVsCold */
     , (450375,  17,     0.9) /* ArmorModVsFire */
     , (450375,  18,     0.9) /* ArmorModVsAcid */
     , (450375,  19,     0.9) /* ArmorModVsElectric */
     , (450375,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450375,   1, 'T''thuun Aegis') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450375,   1, 0x02001880) /* Setup */
     , (450375,   3, 0x20000014) /* SoundTable */
     , (450375,   8, 0x06006970) /* Icon */
     , (450375,  22, 0x3400002B) /* PhysicsEffectTable */;
