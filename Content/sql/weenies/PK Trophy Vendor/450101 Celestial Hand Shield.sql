DELETE FROM `weenie` WHERE `class_Id` = 450101;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450101, 'ace450101-celestialhandshieldtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450101,   1,          2) /* ItemType - Armor */
     , (450101,   5,        0) /* EncumbranceVal */
     , (450101,   9,    2097152) /* ValidLocations - Shield */
     , (450101,  16,          1) /* ItemUseable - No */
     , (450101,  19,       20) /* Value */
     , (450101,  28,        0) /* ArmorLevel */
     , (450101,  51,          4) /* CombatUse - Shield */
     , (450101,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450101,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450101,  13,     0.9) /* ArmorModVsSlash */
     , (450101,  14,     0.9) /* ArmorModVsPierce */
     , (450101,  15,     0.9) /* ArmorModVsBludgeon */
     , (450101,  16,     0.9) /* ArmorModVsCold */
     , (450101,  17,     0.9) /* ArmorModVsFire */
     , (450101,  18,     0.9) /* ArmorModVsAcid */
     , (450101,  19,     0.9) /* ArmorModVsElectric */
     , (450101,  39,     1.3) /* DefaultScale */
     , (450101, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450101,   1, 'Celestial Hand Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450101,   1, 0x02001857) /* Setup */
     , (450101,   3, 0x20000014) /* SoundTable */
     , (450101,   8, 0x06006953) /* Icon */
     , (450101,  22, 0x3400002B) /* PhysicsEffectTable */;
