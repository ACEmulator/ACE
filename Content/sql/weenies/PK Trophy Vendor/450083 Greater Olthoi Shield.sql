DELETE FROM `weenie` WHERE `class_Id` = 450083;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450083, 'shieldolthoiextremerot2tailor', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450083,   1,          2) /* ItemType - Armor */
     , (450083,   5,       0) /* EncumbranceVal */
     , (450083,   8,        500) /* Mass */
     , (450083,   9,    2097152) /* ValidLocations - Shield */
     , (450083,  16,          1) /* ItemUseable - No */
     , (450083,  18,          0) /* UiEffects - Undef */
     , (450083,  19,       20) /* Value */
     , (450083,  27,          2) /* ArmorType - Leather */
     , (450083,  28,        0) /* ArmorLevel */
     , (450083,  36,       9999) /* ResistMagic */
     , (450083,  51,          4) /* CombatUse - Shield */
     , (450083,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450083, 150,        103) /* HookPlacement - Hook */
     , (450083, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450083,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450083,  13,     1.7) /* ArmorModVsSlash */
     , (450083,  14,     1.3) /* ArmorModVsPierce */
     , (450083,  15,     1.3) /* ArmorModVsBludgeon */
     , (450083,  16,     1.5) /* ArmorModVsCold */
     , (450083,  17,     1.5) /* ArmorModVsFire */
     , (450083,  18,       2) /* ArmorModVsAcid */
     , (450083,  19,     1.6) /* ArmorModVsElectric */
     , (450083,  39,       1) /* DefaultScale */
     , (450083, 110,       1) /* BulkMod */
     , (450083, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450083,   1, 'Greater Olthoi Shield') /* Name */
     , (450083,  16, 'A shield created from the abdomen of an Olthoi Sentinel.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450083,   1, 0x02000161) /* Setup */
     , (450083,   3, 0x20000014) /* SoundTable */
     , (450083,   6, 0x04001661) /* PaletteBase */
     , (450083,   7, 0x100004DB) /* ClothingBase */
     , (450083,   8, 0x06002DE4) /* Icon */
     , (450083,  22, 0x3400002B) /* PhysicsEffectTable */;
