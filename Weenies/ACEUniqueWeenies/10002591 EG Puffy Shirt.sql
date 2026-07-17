DELETE FROM `weenie` WHERE `class_Id` = 10002591;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10002591, 'EGshirt', 2, '2022-11-05 05:26:30') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10002591,   1,          4) /* ItemType - Clothing */
     , (10002591,   3,          13) /* PaletteTemplate - Green */
     , (10002591,   307,          3) /* Damage Rating */
     , (10002591,   4,        104) /* ClothingPriority - UnderwearChest, UnderwearUpperArms, UnderwearLowerArms */
     , (10002591,   5,         75) /* EncumbranceVal */
     , (10002591,   8,         50) /* Mass */
     , (10002591,   9,         30) /* ValidLocations - ChestWear, AbdomenWear, UpperArmWear, LowerArmWear */
     , (10002591,  16,          1) /* ItemUseable - No */
     , (10002591,  18,          1) /* Magical UI */
     , (10002591,  19,         15) /* Value */
     , (10002591,  27,          1) /* ArmorType - Cloth */
     , (10002591,  28,          0) /* ArmorLevel */
     , (10002591,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10002591, 169,  201328144) /* TsysMutationData */
      , (10002591,  33,          1) /* Bonded - Bonded */
     , (10002591, 106,        590) /* ItemSpellcraft */
     , (10002591, 107,        2000) /* ItemCurMana */
     , (10002591, 108,       1000) /* ItemMaxMana */
     , (10002591, 109,        1) /* ItemDifficulty */
     , (10002591,  370,       3) /* Dmg rating */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10002591,  22, True ) /* Inscribable */
     , (10002591, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10002591,  12,    1) /* Shade */
     , (10002591,  13,     0.8) /* ArmorModVsSlash */
     , (10002591,  14,     0.8) /* ArmorModVsPierce */
     , (10002591,  15,       1) /* ArmorModVsBludgeon */
     , (10002591,  16,     0.2) /* ArmorModVsCold */
     , (10002591,  17,     0.2) /* ArmorModVsFire */
     , (10002591,  18,     0.1) /* ArmorModVsAcid */
     , (10002591,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10002591,   1, 'Puffy Shirt') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10002591,   1, 0x020001A6) /* Setup */
     , (10002591,   3, 0x20000014) /* SoundTable */
     , (10002591,   6, 0x0400007E) /* PaletteBase */
     , (10002591,   7, 0x100005B3) /* ClothingBase */
     , (10002591,   8, 0x06004A89) /* Icon */
     , (10002591,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10002591,  36, 0x0E000016) /* MutateFilter */;


INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES
(10002591,   6095,      2)  /* Legendary Impenetrability */
,(10002591,   4407,      2)  /* Impen level 9 */
,(10002591,   4391,      2)  /* Acid bane level 8 */
,(10002591,   4393	,      2)  /* Blade bane 9 */
,(10002591,   4397,      2)  /* Bludg bane 9 */
,(10002591,   4401,      2)  /* Fire bane 9 */
,(10002591,   4403	,      2)  /* Frost bane 9 */
,(10002591,   4409,      2)  /* Lightning bane 9 */
,(10002591,   4412,      2)  /* Piercing bane 9 */
