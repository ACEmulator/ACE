DELETE FROM `weenie` WHERE `class_Id` = 450104;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450104, 'ace450104-radiantbloodshieldtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450104,   1,          2) /* ItemType - Armor */
     , (450104,   5,        0) /* EncumbranceVal */
     , (450104,   9,    2097152) /* ValidLocations - Shield */
     , (450104,  16,          1) /* ItemUseable - No */
     , (450104,  19,       20) /* Value */
     , (450104,  28,        0) /* ArmorLevel */
     , (450104,  51,          4) /* CombatUse - Shield */
     , (450104,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450104,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450104,  13,     0.9) /* ArmorModVsSlash */
     , (450104,  14,     0.9) /* ArmorModVsPierce */
     , (450104,  15,     0.9) /* ArmorModVsBludgeon */
     , (450104,  16,     0.9) /* ArmorModVsCold */
     , (450104,  17,     0.9) /* ArmorModVsFire */
     , (450104,  18,     0.9) /* ArmorModVsAcid */
     , (450104,  19,     0.9) /* ArmorModVsElectric */
     , (450104,  39,     1.3) /* DefaultScale */
     , (450104, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450104,   1, 'Radiant Blood Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450104,   1, 0x02001859) /* Setup */
     , (450104,   3, 0x20000014) /* SoundTable */
     , (450104,   8, 0x06006955) /* Icon */
     , (450104,  22, 0x3400002B) /* PhysicsEffectTable */;
