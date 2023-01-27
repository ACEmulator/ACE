DELETE FROM `weenie` WHERE `class_Id` = 450080;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450080, 'shieldsimulacraimbuedtailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450080,   1,          2) /* ItemType - Armor */
     , (450080,   3,          5) /* PaletteTemplate - DarkBlue */
     , (450080,   5,       0) /* EncumbranceVal */
     , (450080,   8,        500) /* Mass */
     , (450080,   9,    2097152) /* ValidLocations - Shield */
     , (450080,  16,          1) /* ItemUseable - No */
     , (450080,  18,          1) /* UiEffects - Magical */
     , (450080,  19,       20) /* Value */
     , (450080,  27,          2) /* ArmorType - Leather */
     , (450080,  28,        0) /* ArmorLevel */
     , (450080,  51,          4) /* CombatUse - Shield */
     , (450080,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450080, 150,        103) /* HookPlacement - Hook */
     , (450080, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450080,  22, True ) /* Inscribable */
     , (450080,  23, True ) /* DestroyOnSell */
     , (450080,  69, False) /* IsSellable */
     , (450080,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450080,   5,   -0.05) /* ManaRate */
     , (450080,  13,       1) /* ArmorModVsSlash */
     , (450080,  14,     0.8) /* ArmorModVsPierce */
     , (450080,  15,     1.2) /* ArmorModVsBludgeon */
     , (450080,  16,     0.6) /* ArmorModVsCold */
     , (450080,  17,     0.6) /* ArmorModVsFire */
     , (450080,  18,       1) /* ArmorModVsAcid */
     , (450080,  19,     0.6) /* ArmorModVsElectric */
     , (450080,  39,    1.25) /* DefaultScale */
     , (450080, 110,       1) /* BulkMod */
     , (450080, 111,       1) /* SizeMod */
     , (450080, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450080,   1, 'Imbued Shield of the Simulacra') /* Name */
     , (450080,  16, 'A shield imbued with the power of the Asteliary Gem.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450080,   1, 0x02000B64) /* Setup */
     , (450080,   3, 0x20000014) /* SoundTable */
     , (450080,   6, 0x04000BEF) /* PaletteBase */
     , (450080,   7, 0x10000097) /* ClothingBase */
     , (450080,   8, 0x06002287) /* Icon */
     , (450080,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450080,  37,         48) /* ItemSkillLimit - Shield */;


