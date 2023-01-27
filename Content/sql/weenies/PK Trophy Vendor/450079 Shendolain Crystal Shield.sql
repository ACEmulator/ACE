DELETE FROM `weenie` WHERE `class_Id` = 450079;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450079, 'shieldcrystalshentailor', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450079,   1,          2) /* ItemType - Armor */
     , (450079,   3,         13) /* PaletteTemplate - Purple */
     , (450079,   5,       0) /* EncumbranceVal */
     , (450079,   8,        460) /* Mass */
     , (450079,   9,    2097152) /* ValidLocations - Shield */
     , (450079,  16,          1) /* ItemUseable - No */
     , (450079,  18,          1) /* UiEffects - Magical */
     , (450079,  19,       20) /* Value */
     , (450079,  27,          2) /* ArmorType - Leather */
     , (450079,  28,        0) /* ArmorLevel */
     , (450079,  36,       9999) /* ResistMagic */
     , (450079,  51,          4) /* CombatUse - Shield */
     , (450079,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450079, 150,        103) /* HookPlacement - Hook */
     , (450079, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450079,  22, True ) /* Inscribable */
     , (450079,  23, True ) /* DestroyOnSell */
     , (450079,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450079,   5,   -0.05) /* ManaRate */
     , (450079,  13,       1) /* ArmorModVsSlash */
     , (450079,  14,     0.8) /* ArmorModVsPierce */
     , (450079,  15,     1.2) /* ArmorModVsBludgeon */
     , (450079,  16,     0.6) /* ArmorModVsCold */
     , (450079,  17,     0.6) /* ArmorModVsFire */
     , (450079,  18,       1) /* ArmorModVsAcid */
     , (450079,  19,     0.6) /* ArmorModVsElectric */
     , (450079,  39,    1.25) /* DefaultScale */
     , (450079,  76,     0.5) /* Translucency */
     , (450079, 110,       1) /* BulkMod */
     , (450079, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450079,   1, 'Shendolain Crystal Shield') /* Name */
     , (450079,  15, 'A shield imbued with the power of the Shendolain Crystal.') /* ShortDesc */
     , (450079,  16, 'A shield imbued with the power of the Shendolain Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450079,   1, 0x02000162) /* Setup */
     , (450079,   3, 0x20000014) /* SoundTable */
     , (450079,   6, 0x04000BEF) /* PaletteBase */
     , (450079,   7, 0x10000245) /* ClothingBase */
     , (450079,   8, 0x06001E02) /* Icon */
     , (450079,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450079,  37,          6) /* ItemSkillLimit - MeleeDefense */;


