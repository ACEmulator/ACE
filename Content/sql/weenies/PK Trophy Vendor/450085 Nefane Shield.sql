DELETE FROM `weenie` WHERE `class_Id` = 450085;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450085, 'shieldnefanetailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450085,   1,          2) /* ItemType - Armor */
     , (450085,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450085,   5,       0) /* EncumbranceVal */
     , (450085,   8,        140) /* Mass */
     , (450085,   9,    2097152) /* ValidLocations - Shield */
     , (450085,  16,          1) /* ItemUseable - No */
     , (450085,  19,      20) /* Value */
     , (450085,  27,          2) /* ArmorType - Leather */
     , (450085,  28,        0) /* ArmorLevel */
     , (450085,  51,          4) /* CombatUse - Shield */
     , (450085,  56,        0) /* ShieldValue */
     , (450085,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450085, 106,        400) /* ItemSpellcraft */
     , (450085, 150,        103) /* HookPlacement - Hook */
     , (450085, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450085,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450085,   5,  -0.033) /* ManaRate */
     , (450085,  13,     0.6) /* ArmorModVsSlash */
     , (450085,  14,       1) /* ArmorModVsPierce */
     , (450085,  15,       1) /* ArmorModVsBludgeon */
     , (450085,  16,    0.25) /* ArmorModVsCold */
     , (450085,  17,       1) /* ArmorModVsFire */
     , (450085,  18,       1) /* ArmorModVsAcid */
     , (450085,  19,    0.25) /* ArmorModVsElectric */
     , (450085,  39,     1.3) /* DefaultScale */
     , (450085, 110,       1) /* BulkMod */
     , (450085, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450085,   1, 'Nefane Shield') /* Name */
     , (450085,  15, 'This shield was carved from the shell of a corrupted Nefane. Some elements of the natural magic of the creature remains within the shield.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450085,   1, 0x0200101C) /* Setup */
     , (450085,   3, 0x20000014) /* SoundTable */
     , (450085,   6, 0x04001723) /* PaletteBase */
     , (450085,   7, 0x10000515) /* ClothingBase */
     , (450085,   8, 0x06003029) /* Icon */
     , (450085,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450085,  37,         48) /* ItemSkillLimit - Shield */;


