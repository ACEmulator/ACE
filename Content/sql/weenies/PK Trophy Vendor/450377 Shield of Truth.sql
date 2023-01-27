DELETE FROM `weenie` WHERE `class_Id` = 450377;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450377, 'ace450377-shieldoftruthtailor', 1, '2021-11-29 06:19:28') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450377,   1,          2) /* ItemType - Armor */
     , (450377,   3,          8) /* PaletteTemplate - Green */
     , (450377,   5,        0) /* EncumbranceVal */
     , (450377,   9,    2097152) /* ValidLocations - Shield */
     , (450377,  16,          1) /* ItemUseable - No */
     , (450377,  19,       20) /* Value */
     , (450377,  28,        0) /* ArmorLevel */
     , (450377,  51,          4) /* CombatUse - Shield */
     , (450377,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450377, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450377,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450377,   5,  -0.033) /* ManaRate */
     , (450377,  13,       2) /* ArmorModVsSlash */
     , (450377,  14,       2) /* ArmorModVsPierce */
     , (450377,  15,       2) /* ArmorModVsBludgeon */
     , (450377,  16,     1.3) /* ArmorModVsCold */
     , (450377,  17,     1.3) /* ArmorModVsFire */
     , (450377,  18,     1.3) /* ArmorModVsAcid */
     , (450377,  19,     1.3) /* ArmorModVsElectric */
     , (450377,  39,     0.7) /* DefaultScale */
     , (450377,  76,     0.3) /* Translucency */
     , (450377, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450377,   1, 'Shield of Truth') /* Name */
     , (450377,  16, '"My teaching is the shield of truth, which, if held by a true seeker, will stop the deadly sword of ignorance."     -Master Jojii') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450377,   1, 0x02000162) /* Setup */
     , (450377,   3, 0x20000014) /* SoundTable */
     , (450377,   6, 0x04000BEF) /* PaletteBase */
     , (450377,   7, 0x1000015F) /* ClothingBase */
     , (450377,   8, 0x06001426) /* Icon */
     , (450377,  22, 0x3400002B) /* PhysicsEffectTable */;

