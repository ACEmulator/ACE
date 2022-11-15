DELETE FROM `weenie` WHERE `class_Id` = 450084;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450084, 'shieldpowertailor', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450084,   1,          2) /* ItemType - Armor */
     , (450084,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450084,   5,        0) /* EncumbranceVal */
     , (450084,   8,        230) /* Mass */
     , (450084,   9,    2097152) /* ValidLocations - Shield */
     , (450084,  16,          1) /* ItemUseable - No */
     , (450084,  19,      20) /* Value */
     , (450084,  27,          2) /* ArmorType - Leather */
     , (450084,  28,        0) /* ArmorLevel */
     , (450084,  51,          4) /* CombatUse - Shield */
     , (450084,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450084, 150,        103) /* HookPlacement - Hook */
     , (450084, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450084,  22, True ) /* Inscribable */
     , (450084,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450084,   5,  -0.033) /* ManaRate */
     , (450084,  13,       1) /* ArmorModVsSlash */
     , (450084,  14,       1) /* ArmorModVsPierce */
     , (450084,  15,       1) /* ArmorModVsBludgeon */
     , (450084,  16,     0.6) /* ArmorModVsCold */
     , (450084,  17,       2) /* ArmorModVsFire */
     , (450084,  18,       2) /* ArmorModVsAcid */
     , (450084,  19,     0.6) /* ArmorModVsElectric */
     , (450084,  39,       1) /* DefaultScale */
     , (450084, 110,       1) /* BulkMod */
     , (450084, 111,    1.33) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450084,   1, 'Shield of Power') /* Name */
     , (450084,  16, 'The shield is a boon of strength and a symbol of the adherents to the path of the Dragon. The symbol looks as though it can be changed with the right materials.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450084,   1, 0x0200101B) /* Setup */
     , (450084,   3, 0x20000014) /* SoundTable */
     , (450084,   6, 0x040016F0) /* PaletteBase */
     , (450084,   7, 0x100004FE) /* ClothingBase */
     , (450084,   8, 0x06002FF5) /* Icon */
     , (450084,  22, 0x3400002B) /* PhysicsEffectTable */;


