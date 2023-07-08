DELETE FROM `weenie` WHERE `class_Id` = 480512;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480512, 'emblemmarriagenewpk', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480512,   1,          2) /* ItemType - Armor */
     , (480512,   3,          8) /* PaletteTemplate - Green */
     , (480512,   5,          0) /* EncumbranceVal */
     , (480512,   8,         11) /* Mass */
     , (480512,   9,    2097152) /* ValidLocations - Shield */
     , (480512,  16,          1) /* ItemUseable - No */
     , (480512,  19,          20) /* Value */
     , (480512,  27,          2) /* ArmorType - Leather */
     , (480512,  28,          0) /* ArmorLevel */
     , (480512,  51,          4) /* CombatUse - Shield */
     , (480512,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480512, 150,        103) /* HookPlacement - Hook */
     , (480512, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480512,  22, True ) /* Inscribable */
     , (480512,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480512,  13,       1) /* ArmorModVsSlash */
     , (480512,  14,       1) /* ArmorModVsPierce */
     , (480512,  15,       1) /* ArmorModVsBludgeon */
     , (480512,  16,       1) /* ArmorModVsCold */
     , (480512,  17,       1) /* ArmorModVsFire */
     , (480512,  18,       1) /* ArmorModVsAcid */
     , (480512,  19,       1) /* ArmorModVsElectric */
     , (480512,  39,     1) /* DefaultScale */
     , (480512, 110,       1) /* BulkMod */
     , (480512, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480512,   1, 'Emblem of Marriage') /* Name */
     , (480512,  16, 'This lightweight, shield-shaped emblem server as evidence that the bearer is married.  It is customary to inscribe the shield with the name of the beloved partner.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480512,   1, 0x02000C6C) /* Setup */
     , (480512,   3, 0x20000014) /* SoundTable */
     , (480512,   6, 0x04000BEF) /* PaletteBase */
     , (480512,   7, 0x1000037F) /* ClothingBase */
     , (480512,   8, 0x060024B8) /* Icon */
     , (480512,  22, 0x3400002B) /* PhysicsEffectTable */;
