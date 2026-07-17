DELETE FROM `weenie` WHERE `class_Id` = 2000036;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2000036, 'Destruction8bracersleather', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2000036,   1,          2) /* ItemType - Armor */
     , (2000036,   3,          4) /* PaletteTemplate - Brown */
     , (2000036,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (2000036,   5,        270) /* EncumbranceVal */
     , (2000036,   8,         90) /* Mass */
     , (2000036,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (2000036,  16,          1) /* ItemUseable - No */
     , (2000036,  19,       1100) /* Value */
     , (2000036,  27,          2) /* ArmorType - Leather */
     , (2000036,  28,         10) /* ArmorLevel */
     , (2000036, 106,        480) /* ItemSpellcraft */
     , (2000036,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2000036, 124,          3) /* Version */
     , (2000036, 169,  118162702) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2000036,  22, True ) /* Inscribable */
     , (2000036, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2000036,  12,    0.66) /* Shade */
     , (2000036,  13,       1) /* ArmorModVsSlash */
     , (2000036,  14,     0.8) /* ArmorModVsPierce */
     , (2000036,  15,       1) /* ArmorModVsBludgeon */
     , (2000036,  16,     0.5) /* ArmorModVsCold */
     , (2000036,  17,     0.5) /* ArmorModVsFire */
     , (2000036,  18,     0.3) /* ArmorModVsAcid */
     , (2000036,  19,     0.6) /* ArmorModVsElectric */
     , (2000036, 165,       1) /* ArmorModVsNether */
        , (2000036, 156,     0.4) /* ProcSpellRate */;

     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (2000036,  55,       5338) /* ProcSpell - Destruction level 8*/;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2000036,   1, 'Leather Bracers') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2000036,   1, 0x020000D1) /* Setup */
     , (2000036,   3, 0x20000014) /* SoundTable */
     , (2000036,   6, 0x0400007E) /* PaletteBase */
     , (2000036,   7, 0x1000000C) /* ClothingBase */
     , (2000036,   8, 0x06000FE4) /* Icon */
     , (2000036,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2000036,  36, 0x0E000012) /* MutateFilter */
     , (2000036,  46, 0x38000032) /* TsysMutationFilter */;
