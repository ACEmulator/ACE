DELETE FROM `weenie` WHERE `class_Id` = 2000056;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2000056, 'imperilgauntletsleather2', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2000056,   1,          2) /* ItemType - Armor */
     , (2000056,   3,          4) /* PaletteTemplate - Brown */
     , (2000056,   4,      32768) /* ClothingPriority - Hands */
     , (2000056,   5,        270) /* EncumbranceVal */
     , (2000056,   8,         90) /* Mass */
     , (2000056,   9,         32) /* ValidLocations - HandWear */
     , (2000056,  16,          1) /* ItemUseable - No */
     , (2000056,  19,       1100) /* Value */
     , (2000056,  27,          2) /* ArmorType - Leather */
     , (2000056,  28,        10) /* ArmorLevel */
     , (2000056,  44,          0) /* Damage */
     , (2000056, 106,        480) /* ItemSpellcraft */
     , (2000056,  45,          4) /* DamageType - Bludgeon */
     , (2000056,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2000056, 124,          3) /* Version */
     , (2000056, 169,  151717134) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2000056,  22, True ) /* Inscribable */
     , (2000056, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2000056,  12,    0.66) /* Shade */
     , (2000056,  13,       1) /* ArmorModVsSlash */
     , (2000056,  14,     0.8) /* ArmorModVsPierce */
     , (2000056,  15,       1) /* ArmorModVsBludgeon */
     , (2000056,  16,     0.5) /* ArmorModVsCold */
     , (2000056,  17,     0.5) /* ArmorModVsFire */
     , (2000056,  18,     0.3) /* ArmorModVsAcid */
     , (2000056,  19,     0.6) /* ArmorModVsElectric */
     , (2000056,  22,    0.75) /* DamageVariance */
     , (2000056, 110,    1.67) /* BulkMod */
     , (2000056, 111,       1) /* SizeMod */
     , (2000056, 165,       1) /* ArmorModVsNether */
     , (2000056, 156,     0.4) /* ProcSpellRate */;

     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (2000056,  55,       3091) /* ProcSpell - Imperil level 8*/;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2000056,   1, 'Imperil Leather Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2000056,   1, 0x020000D8) /* Setup */
     , (2000056,   3, 0x20000014) /* SoundTable */
     , (2000056,   6, 0x0400007E) /* PaletteBase */
     , (2000056,   7, 0x10000008) /* ClothingBase */
     , (2000056,   8, 0x06000FCC) /* Icon */
     , (2000056,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2000056,  36, 0x0E000012) /* MutateFilter */
     , (2000056,  46, 0x38000032) /* TsysMutationFilter */;
