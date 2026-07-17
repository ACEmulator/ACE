DELETE FROM `weenie` WHERE `class_Id` = 10000128;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10000128, 'EG Orange pants', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10000128,   1,          4) /* ItemType - Clothing */
     , (10000128,   3,         3) /* PaletteTemplate - Orange */
     , (10000128,   4,         22) /* ClothingPriority - UnderwearUpperLegs, UnderwearLowerLegs, UnderwearAbdomen */
     , (10000128,   5,        135) /* EncumbranceVal */
     , (10000128,   8,         90) /* Mass */
     , (10000128,   9,        196) /* ValidLocations - AbdomenWear, UpperLegWear, LowerLegWear */
     , (10000128,  16,          1) /* ItemUseable - No */
     , (10000128,  18,          1) /* UiEffects - Magical */
     , (10000128,  19,         30) /* Value */
     , (10000128,  27,          1) /* ArmorType - Cloth */
     , (10000128,  28,          0) /* ArmorLevel */
     , (10000128,  33,          1) /* Bonded - Bonded */
     , (10000128,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10000128, 106,        590) /* ItemSpellcraft */
     , (10000128, 107,       2000) /* ItemCurMana */
     , (10000128, 108,       1000) /* ItemMaxMana */
     , (10000128, 109,          1) /* ItemDifficulty */
     , (10000128, 169,  201326864) /* TsysMutationData */
     , (10000128, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10000128,  11, True ) /* IgnoreCollisions */
     , (10000128,  13, True ) /* Ethereal */
     , (10000128,  14, True ) /* GravityStatus */
     , (10000128,  19, True ) /* Attackable */
     , (10000128,  22, True ) /* Inscribable */
     , (10000128, 100, True ) /* Dyable */
     , (10000128, 112, True) /* cast on self*/;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10000128,  12,    1) /* Shade */
     , (10000128,  13,     0.8) /* ArmorModVsSlash */
     , (10000128,  14,     0.8) /* ArmorModVsPierce */
     , (10000128,  15,       1) /* ArmorModVsBludgeon */
     , (10000128,  16,     0.2) /* ArmorModVsCold */
     , (10000128,  17,     0.2) /* ArmorModVsFire */
     , (10000128,  18,     0.1) /* ArmorModVsAcid */
     , (10000128,  19,     0.2) /* ArmorModVsElectric */
     , (10000128, 165,       1) /* ArmorModVsNether */
     , (10000128, 156,     0.03) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10000128,   1, 'Pants') /* Name */;

     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (10000128,  55,       4335) /* ProcSpell - Dispel All level 8*/;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10000128,   1, 0x020000D7) /* Setup */
     , (10000128,   3, 0x20000014) /* SoundTable */
     , (10000128,   6, 0x0400007E) /* PaletteBase */
     , (10000128,   7, 0x1000056B) /* ClothingBase */
     , (10000128,   8, 0x06000FEA) /* Icon */
     , (10000128,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10000128,  36, 0x0E000016) /* MutateFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10000128,  6095,      2)  /* Legendary Impenetrability */
     , (10000128,  4407,      2)  /* Incantation of Impenetrability */
     , (10000128,  4391,      2)  /* Incantation of Acid Bane */
     , (10000128,  4393,      2)  /* Incantation of Blade Bane */
     , (10000128,  4397,      2)  /* Incantation of Bludgeon Bane */
     , (10000128,  4401,      2)  /* Incantation of Flame Bane */
     , (10000128,  4403,      2)  /* Incantation of Frost Bane */
     , (10000128,  4409,      2)  /* Incantation of Lightning Bane */
     , (10000128,  4412,      2)  /* Incantation of Piercing Bane */;
