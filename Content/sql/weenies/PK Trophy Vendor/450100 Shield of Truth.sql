DELETE FROM `weenie` WHERE `class_Id` = 450100;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450100, 'ace450100-shieldoftruthtailor', 1, '2021-11-29 06:19:28') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450100,   1,          2) /* ItemType - Armor */
     , (450100,   3,          8) /* PaletteTemplate - Green */
     , (450100,   5,        0) /* EncumbranceVal */
     , (450100,   9,    2097152) /* ValidLocations - Shield */
     , (450100,  16,          1) /* ItemUseable - No */
     , (450100,  19,       20) /* Value */
     , (450100,  28,        0) /* ArmorLevel */
     , (450100,  51,          4) /* CombatUse - Shield */
     , (450100,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450100, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450100,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450100,   5,  -0.033) /* ManaRate */
     , (450100,  13,       2) /* ArmorModVsSlash */
     , (450100,  14,       2) /* ArmorModVsPierce */
     , (450100,  15,       2) /* ArmorModVsBludgeon */
     , (450100,  16,     1.3) /* ArmorModVsCold */
     , (450100,  17,     1.3) /* ArmorModVsFire */
     , (450100,  18,     1.3) /* ArmorModVsAcid */
     , (450100,  19,     1.3) /* ArmorModVsElectric */
     , (450100,  39,     0.7) /* DefaultScale */
     , (450100,  76,     0.3) /* Translucency */
     , (450100, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450100,   1, 'Shield of Truth') /* Name */
     , (450100,  16, '"My teaching is the shield of truth, which, if held by a true seeker, will stop the deadly sword of ignorance."     -Master Jojii') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450100,   1, 0x02000162) /* Setup */
     , (450100,   3, 0x20000014) /* SoundTable */
     , (450100,   6, 0x04000BEF) /* PaletteBase */
     , (450100,   7, 0x1000015F) /* ClothingBase */
     , (450100,   8, 0x06001426) /* Icon */
     , (450100,  22, 0x3400002B) /* PhysicsEffectTable */;

