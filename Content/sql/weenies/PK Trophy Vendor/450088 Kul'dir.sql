DELETE FROM `weenie` WHERE `class_Id` = 450088;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450088, 'shieldkuldirlowtailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450088,   1,          2) /* ItemType - Armor */
     , (450088,   5,       0) /* EncumbranceVal */
     , (450088,   9,    2097152) /* ValidLocations - Shield */
     , (450088,  16,          1) /* ItemUseable - No */
     , (450088,  19,       20) /* Value */
     , (450088,  28,         0) /* ArmorLevel */
     , (450088,  51,          4) /* CombatUse - Shield */
     , (450088,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450088,  22, True ) /* Inscribable */
     , (450088,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450088,   5,   -0.05) /* ManaRate */
     , (450088,  13,       1) /* ArmorModVsSlash */
     , (450088,  14,     0.8) /* ArmorModVsPierce */
     , (450088,  15,     0.8) /* ArmorModVsBludgeon */
     , (450088,  16,     0.6) /* ArmorModVsCold */
     , (450088,  17,     1.2) /* ArmorModVsFire */
     , (450088,  18,     0.6) /* ArmorModVsAcid */
     , (450088,  19,     0.6) /* ArmorModVsElectric */
     , (450088, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450088,   1, 'Kul''dir') /* Name */
     , (450088,  33, 'eleonorasheart') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450088,   1, 0x02001333) /* Setup */
     , (450088,   3, 0x20000014) /* SoundTable */
     , (450088,   8, 0x06005A33) /* Icon */
     , (450088,  22, 0x3400002B) /* PhysicsEffectTable */;

