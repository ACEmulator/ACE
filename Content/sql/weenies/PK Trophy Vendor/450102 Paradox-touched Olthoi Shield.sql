DELETE FROM `weenie` WHERE `class_Id` = 450102;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450102, 'ace450102-paradoxtouchedolthoishieldtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450102,   1,          2) /* ItemType - Armor */
     , (450102,   3,         39) /* PaletteTemplate - Black */
     , (450102,   5,        0) /* EncumbranceVal */
     , (450102,   9,    2097152) /* ValidLocations - Shield */
     , (450102,  16,          1) /* ItemUseable - No */
     , (450102,  18,          1) /* UiEffects - Magical */
     , (450102,  19,      20) /* Value */
     , (450102,  28,        0) /* ArmorLevel */
     , (450102,  51,          4) /* CombatUse - Shield */
     , (450102,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450102, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450102,  22, True ) /* Inscribable */
     , (450102,  69, False) /* IsSellable */
     , (450102,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450102,   5,  -0.033) /* ManaRate */
     , (450102,  13,       1) /* ArmorModVsSlash */
     , (450102,  14,     1.1) /* ArmorModVsPierce */
     , (450102,  15,     0.8) /* ArmorModVsBludgeon */
     , (450102,  16,     0.5) /* ArmorModVsCold */
     , (450102,  17,     0.8) /* ArmorModVsFire */
     , (450102,  18,     1.1) /* ArmorModVsAcid */
     , (450102,  19,     0.5) /* ArmorModVsElectric */
     , (450102, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450102,   1, 'Paradox-touched Olthoi Shield') /* Name */
     , (450102,  16, 'A shield, crafted from the remains of the stronger Paradox-touched Olthoi.  Something about the nature of these creatures makes this shield empowered versus the attack types oft used by Olthoi.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450102,   1, 0x0200170F) /* Setup */
     , (450102,   3, 0x20000014) /* SoundTable */
     , (450102,   6, 0x04001661) /* PaletteBase */
     , (450102,   7, 0x100004DB) /* ClothingBase */
     , (450102,   8, 0x06006697) /* Icon */
     , (450102,  22, 0x3400002B) /* PhysicsEffectTable */;


