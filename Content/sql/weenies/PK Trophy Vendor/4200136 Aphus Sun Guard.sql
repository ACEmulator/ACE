DELETE FROM `weenie` WHERE `class_Id` = 4200136;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200136, '4200136-hatxuut', 4, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200136,   1,          4) /* ItemType - Clothing */
     , (4200136,   3,         87) /* PaletteTemplate - Pink */
     , (4200136,   4,      16384) /* ClothingPriority - Head */
     , (4200136,   5,        100) /* EncumbranceVal */
     , (4200136,   8,         15) /* Mass */
     , (4200136,   9,          1) /* ValidLocations - HeadWear */
     , (4200136,  16,          1) /* ItemUseable - No */
     , (4200136,  19,         20) /* Value */
     , (4200136,  27,          1) /* ArmorType - Cloth */
     , (4200136,  28,          0) /* ArmorLevel */
     , (4200136,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200136, 106,        250) /* ItemSpellcraft */
     , (4200136, 107,       1000) /* ItemCurMana */
     , (4200136, 108,       1000) /* ItemMaxMana */
     , (4200136, 109,        100) /* ItemDifficulty */
     , (4200136, 150,        103) /* HookPlacement - Hook */
     , (4200136, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200136,  22, True ) /* Inscribable */
     , (4200136, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200136,   5,  -0.025) /* ManaRate */
     , (4200136,  12,    0.66) /* Shade */
     , (4200136,  13,     0.8) /* ArmorModVsSlash */
     , (4200136,  14,     0.8) /* ArmorModVsPierce */
     , (4200136,  15,       1) /* ArmorModVsBludgeon */
     , (4200136,  16,     0.2) /* ArmorModVsCold */
     , (4200136,  17,     0.2) /* ArmorModVsFire */
     , (4200136,  18,     0.1) /* ArmorModVsAcid */
     , (4200136,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200136,   1, 'Aphus Sun Guard') /* Name */
     , (4200136,   7, 'Island Wear by Xuut') /* Inscription */
     , (4200136,   8, 'Xuut') /* ScribeName */
     , (4200136,  16, 'A wonderfully crafted hat that affords a great deal of protection from the bright sun. Thin veins of pyreal have been worked into the weave.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200136,   1, 0x02001161) /* Setup */
     , (4200136,   3, 0x20000014) /* SoundTable */
     , (4200136,   6, 0x0400007E) /* PaletteBase */
     , (4200136,   7, 0x1000056C) /* ClothingBase */
     , (4200136,   8, 0x06001357) /* Icon */
     , (4200136,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200136,  36, 0x0E000016) /* MutateFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (4200136,  1317,      2)  /* Armor Other VI */;
