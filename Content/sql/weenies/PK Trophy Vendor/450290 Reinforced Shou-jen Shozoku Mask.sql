DELETE FROM `weenie` WHERE `class_Id` = 450290;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450290, 'ace450290-reinforcedshoujenshozokumasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450290,   1,          2) /* ItemType - Armor */
     , (450290,   3,          9) /* PaletteTemplate - Grey */
     , (450290,   4,      16384) /* ClothingPriority - Head */
     , (450290,   5,        0) /* EncumbranceVal */
     , (450290,   9,          1) /* ValidLocations - HeadWear */
     , (450290,  16,          1) /* ItemUseable - No */
     , (450290,  18,          1) /* UiEffects - Magical */
     , (450290,  19,      20) /* Value */
     , (450290,  28,        0) /* ArmorLevel */
     , (450290,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450290, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450290,  11, True ) /* IgnoreCollisions */
     , (450290,  13, True ) /* Ethereal */
     , (450290,  14, True ) /* GravityStatus */
     , (450290,  19, True ) /* Attackable */
     , (450290,  22, True ) /* Inscribable */
     , (450290, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450290,   5,  -0.017) /* ManaRate */
     , (450290,  12,       0) /* Shade */
     , (450290,  13,     0.6) /* ArmorModVsSlash */
     , (450290,  14,     0.6) /* ArmorModVsPierce */
     , (450290,  15,     0.6) /* ArmorModVsBludgeon */
     , (450290,  16,     1.4) /* ArmorModVsCold */
     , (450290,  17,     0.7) /* ArmorModVsFire */
     , (450290,  18,     1.2) /* ArmorModVsAcid */
     , (450290,  19,     1.4) /* ArmorModVsElectric */
     , (450290, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450290,   1, 'Reinforced Shou-jen Shozoku Mask') /* Name */
     , (450290,  33, 'HoshinoFortArmorPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450290,   1, 0x02001620) /* Setup */
     , (450290,   3, 0x20000014) /* SoundTable */
     , (450290,   6, 0x0400007E) /* PaletteBase */
     , (450290,   7, 0x10000838) /* ClothingBase */
     , (450290,   8, 0x060064CD) /* Icon */
     , (450290,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (450290,  4020,      2)  /* Epic Deception Prowess */
     , (450290,  4329,      2)  /* Incantation of Willpower Self */
     , (450290,  4391,      2)  /* Incantation of Acid Bane */
     , (450290,  4393,      2)  /* Incantation of Blade Bane */
     , (450290,  4397,      2)  /* Incantation of Bludgeon Bane */
     , (450290,  4401,      2)  /* Incantation of Flame Bane */
     , (450290,  4403,      2)  /* Incantation of Frost Bane */
     , (450290,  4407,      2)  /* Incantation of Impenetrability */
     , (450290,  4409,      2)  /* Incantation of Lightning Bane */
     , (450290,  4412,      2)  /* Incantation of Piercing Bane */
     , (450290,  4542,      2)  /* Incantation of Deception Mastery Self */;
