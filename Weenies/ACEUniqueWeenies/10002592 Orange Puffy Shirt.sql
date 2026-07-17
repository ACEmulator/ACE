DELETE FROM `weenie` WHERE `class_Id` = 10002592;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10002592, 'EGOrangeshirt', 2, '2022-11-05 05:26:30') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10002592,   1,          4) /* ItemType - Clothing */
     , (10002592,   3,         76) /* PaletteTemplate -Orange */
     , (10002592,   4,        104) /* ClothingPriority - UnderwearChest, UnderwearUpperArms, UnderwearLowerArms */
     , (10002592,   5,         75) /* EncumbranceVal */
     , (10002592,   8,         50) /* Mass */
     , (10002592,   9,         30) /* ValidLocations - ChestWear, AbdomenWear, UpperArmWear, LowerArmWear */
     , (10002592,  16,          1) /* ItemUseable - No */
     , (10002592,  18,          1) /* UiEffects - Magical */
     , (10002592,  19,         15) /* Value */
     , (10002592,  27,          1) /* ArmorType - Cloth */
     , (10002592,  28,          0) /* ArmorLevel */
     , (10002592,  33,          1) /* Bonded - Bonded */
     , (10002592,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10002592, 106,        590) /* ItemSpellcraft */
     , (10002592, 107,       2000) /* ItemCurMana */
     , (10002592, 108,       1000) /* ItemMaxMana */
     , (10002592, 109,          1) /* ItemDifficulty */
     , (10002592, 169,  201328144) /* TsysMutationData */
     , (10002592, 307,          3) /* DamageRating */
     , (10002592, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10002592,  22, True ) /* Inscribable */
     , (10002592, 100, True ) /* Dyable */
            , (10002592, 112, True) /* cast on self*/;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10002592,  12,       .5647059) /* Shade */
     , (10002592,  13,     0.8) /* ArmorModVsSlash */
     , (10002592,  14,     0.8) /* ArmorModVsPierce */
     , (10002592,  15,       1) /* ArmorModVsBludgeon */
     , (10002592,  16,     0.2) /* ArmorModVsCold */
     , (10002592,  17,     0.2) /* ArmorModVsFire */
     , (10002592,  18,     0.1) /* ArmorModVsAcid */
     , (10002592,  19,     0.2) /* ArmorModVsElectric */
        , (10002592, 156,     0.2) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10002592,   1, 'Orange Puffy Shirt') /* Name */;


INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (10002592,  55,       1160) /* ProcSpell - Heal Self lvl 5*/;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10002592,   1, 0x020001C3) /* Setup */
     , (10002592,   3, 0x20000014) /* SoundTable */
     , (10002592,   6, 0x0400007E) /* PaletteBase */
     , (10002592,   7, 0x10000659) /* ClothingBase */
     , (10002592,   8, 0x06000FF3) /* Icon */
     , (10002592,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10002592,  36, 0x0E000016) /* MutateFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10002592,  6095,      2)  /* Legendary Impenetrability */
     , (10002592,  4407,      2)  /* Incantation of Impenetrability */
     , (10002592,  4391,      2)  /* Incantation of Acid Bane */
     , (10002592,  4393,      2)  /* Incantation of Blade Bane */
     , (10002592,  4397,      2)  /* Incantation of Bludgeon Bane */
     , (10002592,  4401,      2)  /* Incantation of Flame Bane */
     , (10002592,  4403,      2)  /* Incantation of Frost Bane */
     , (10002592,  4409,      2)  /* Incantation of Lightning Bane */
     , (10002592,  4412,      2)  /* Incantation of Piercing Bane */;
