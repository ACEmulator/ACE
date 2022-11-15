DELETE FROM `weenie` WHERE `class_Id` = 450089;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450089, 'shieldkamshirlowtailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450089,   1,          2) /* ItemType - Armor */
     , (450089,   5,       0) /* EncumbranceVal */
     , (450089,   9,    2097152) /* ValidLocations - Shield */
     , (450089,  16,          1) /* ItemUseable - No */
     , (450089,  19,       20) /* Value */
     , (450089,  28,         0) /* ArmorLevel */
     , (450089,  51,          4) /* CombatUse - Shield */
     , (450089,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450089,  11, True ) /* IgnoreCollisions */
     , (450089,  13, True ) /* Ethereal */
     , (450089,  14, True ) /* GravityStatus */
     , (450089,  19, True ) /* Attackable */
     , (450089,  22, True ) /* Inscribable */
     , (450089,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450089,   5,   -0.05) /* ManaRate */
     , (450089,  13,       1) /* ArmorModVsSlash */
     , (450089,  14,     0.8) /* ArmorModVsPierce */
     , (450089,  15,     0.8) /* ArmorModVsBludgeon */
     , (450089,  16,     1.2) /* ArmorModVsCold */
     , (450089,  17,     0.6) /* ArmorModVsFire */
     , (450089,  18,     0.6) /* ArmorModVsAcid */
     , (450089,  19,     0.6) /* ArmorModVsElectric */
     , (450089,  39,     0.8) /* DefaultScale */
     , (450089, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450089,   1, 'Kam''shir') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450089,   1, 0x02001335) /* Setup */
     , (450089,   3, 0x20000014) /* SoundTable */
     , (450089,   8, 0x06005A40) /* Icon */
     , (450089,  22, 0x3400002B) /* PhysicsEffectTable */;

