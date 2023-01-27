DELETE FROM `weenie` WHERE `class_Id` = 450103;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450103, 'ace450103-eldrytchwebshieldtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450103,   1,          2) /* ItemType - Armor */
     , (450103,   5,        0) /* EncumbranceVal */
     , (450103,   9,    2097152) /* ValidLocations - Shield */
     , (450103,  16,          1) /* ItemUseable - No */
     , (450103,  19,       20) /* Value */
     , (450103,  28,        0) /* ArmorLevel */
     , (450103,  51,          4) /* CombatUse - Shield */
     , (450103,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450103,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450103,  13,     0.9) /* ArmorModVsSlash */
     , (450103,  14,     0.9) /* ArmorModVsPierce */
     , (450103,  15,     0.9) /* ArmorModVsBludgeon */
     , (450103,  16,     0.9) /* ArmorModVsCold */
     , (450103,  17,     0.9) /* ArmorModVsFire */
     , (450103,  18,     0.9) /* ArmorModVsAcid */
     , (450103,  19,     0.9) /* ArmorModVsElectric */
     , (450103,  39,     1.3) /* DefaultScale */
     , (450103, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450103,   1, 'Eldrytch Web Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450103,   1, 0x02001858) /* Setup */
     , (450103,   3, 0x20000014) /* SoundTable */
     , (450103,   8, 0x06006954) /* Icon */
     , (450103,  22, 0x3400002B) /* PhysicsEffectTable */;
