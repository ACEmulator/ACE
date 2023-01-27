DELETE FROM `weenie` WHERE `class_Id` = 450097;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450097, 'ace450097-shieldofsilyuntailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450097,   1,          2) /* ItemType - Armor */
     , (450097,   5,        0) /* EncumbranceVal */
     , (450097,   9,    2097152) /* ValidLocations - Shield */
     , (450097,  16,          1) /* ItemUseable - No */
     , (450097,  18,          1) /* UiEffects - Magical */
     , (450097,  19,       20) /* Value */
     , (450097,  28,        0) /* ArmorLevel */
     , (450097,  51,          4) /* CombatUse - Shield */
     , (450097,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450097, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450097,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450097,   5,   -0.05) /* ManaRate */
     , (450097,  13,       1) /* ArmorModVsSlash */
     , (450097,  14,     1.1) /* ArmorModVsPierce */
     , (450097,  15,       1) /* ArmorModVsBludgeon */
     , (450097,  16,     0.5) /* ArmorModVsCold */
     , (450097,  17,     0.8) /* ArmorModVsFire */
     , (450097,  18,     0.8) /* ArmorModVsAcid */
     , (450097,  19,     0.5) /* ArmorModVsElectric */
     , (450097,  39,     0.9) /* DefaultScale */
     , (450097, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450097,   1, 'Shield of Silyun') /* Name */
     , (450097,  16, 'A shield given by Lucari di Bellenesse, for valor in the retrieval of the lost Sword of Bellenesse.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450097,   1, 0x02001523) /* Setup */
     , (450097,   3, 0x20000014) /* SoundTable */
     , (450097,   7, 0x10000804) /* ClothingBase */
     , (450097,   8, 0x06006301) /* Icon */
     , (450097,  22, 0x3400002B) /* PhysicsEffectTable */;


