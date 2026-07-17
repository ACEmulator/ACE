DELETE FROM `weenie` WHERE `class_Id` = 10000127;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10000127, 'EG pants', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10000127,   1,          4) /* ItemType - Clothing */
     , (10000127,   3,         14) /* PaletteTemplate - Red */
     , (10000127,   4,         22) /* ClothingPriority - UnderwearUpperLegs, UnderwearLowerLegs, UnderwearAbdomen */
     , (10000127,   5,        135) /* EncumbranceVal */
     , (10000127,   8,         90) /* Mass */
     , (10000127,   9,        196) /* ValidLocations - AbdomenWear, UpperLegWear, LowerLegWear */
     , (10000127,  16,          1) /* ItemUseable - No */
     , (10000127,  18,          1) /* UiEffects - Magical */
     , (10000127,  19,         30) /* Value */
     , (10000127,  27,          1) /* ArmorType - Cloth */
     , (10000127,  28,          0) /* ArmorLevel */
     , (10000127,  33,          1) /* Bonded - Bonded */
     , (10000127,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10000127, 106,        590) /* ItemSpellcraft */
     , (10000127, 107,       2000) /* ItemCurMana */
     , (10000127, 108,       1000) /* ItemMaxMana */
     , (10000127, 109,          1) /* ItemDifficulty */
     , (10000127, 169,  201326864) /* TsysMutationData */
     , (10000127, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10000127,  11, True ) /* IgnoreCollisions */
     , (10000127,  13, True ) /* Ethereal */
     , (10000127,  14, True ) /* GravityStatus */
     , (10000127,  19, True ) /* Attackable */
     , (10000127,  22, True ) /* Inscribable */
     , (10000127, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10000127,  12,    0.33) /* Shade */
     , (10000127,  13,     0.8) /* ArmorModVsSlash */
     , (10000127,  14,     0.8) /* ArmorModVsPierce */
     , (10000127,  15,       1) /* ArmorModVsBludgeon */
     , (10000127,  16,     0.2) /* ArmorModVsCold */
     , (10000127,  17,     0.2) /* ArmorModVsFire */
     , (10000127,  18,     0.1) /* ArmorModVsAcid */
     , (10000127,  19,     0.2) /* ArmorModVsElectric */
     , (10000127, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10000127,   1, 'Pants') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10000127,   1, 0x020000DD) /* Setup */
     , (10000127,   3, 0x20000014) /* SoundTable */
     , (10000127,   6, 0x0400007E) /* PaletteBase */
     , (10000127,   7, 0x10000002) /* ClothingBase */
     , (10000127,   8, 0x06000FEA) /* Icon */
     , (10000127,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10000127,  36, 0x0E000016) /* MutateFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10000127,  6095,      2)  /* Legendary Impenetrability */
     , (10000127,  4407,      2)  /* Incantation of Impenetrability */
     , (10000127,  4391,      2)  /* Incantation of Acid Bane */
     , (10000127,  4393,      2)  /* Incantation of Blade Bane */
     , (10000127,  4397,      2)  /* Incantation of Bludgeon Bane */
     , (10000127,  4401,      2)  /* Incantation of Flame Bane */
     , (10000127,  4403,      2)  /* Incantation of Frost Bane */
     , (10000127,  4409,      2)  /* Incantation of Lightning Bane */
     , (10000127,  4412,      2)  /* Incantation of Piercing Bane */;
