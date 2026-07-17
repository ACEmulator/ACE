DELETE FROM `weenie` WHERE `class_Id` = 20000115;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20000115, 'CastOnStrikeFirebootsleather2', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20000115,   1,          2) /* ItemType - Armor */
     , (20000115,   3,          4) /* PaletteTemplate - Brown */
     , (20000115,   4,      65536) /* ClothingPriority - Feet */
     , (20000115,   5,        420) /* EncumbranceVal */
     , (20000115,   8,        140) /* Mass */
     , (20000115,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (20000115,  16,          1) /* ItemUseable - No */
     , (20000115,  19,       1100) /* Value */
     , (20000115,  27,          2) /* ArmorType - Leather */
     , (20000115,  28,        10) /* ArmorLevel */
     , (20000115,  44,          1) /* Damage */
     , (20000115,  45,          4) /* DamageType - Bludgeon */
     , (20000115,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (20000115, 124,          3) /* Version */
     , (20000115, 169,  185271566) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20000115,  22, True ) /* Inscribable */
     , (20000115, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20000115,  12,     0.1) /* Shade */
     , (20000115,  13,       1) /* ArmorModVsSlash */
     , (20000115,  14,     0.8) /* ArmorModVsPierce */
     , (20000115,  15,       1) /* ArmorModVsBludgeon */
     , (20000115,  16,     0.5) /* ArmorModVsCold */
     , (20000115,  17,     0.5) /* ArmorModVsFire */
     , (20000115,  18,     0.3) /* ArmorModVsAcid */
     , (20000115,  19,     0.6) /* ArmorModVsElectric */
     , (20000115,  22,    0.75) /* DamageVariance */
     , (20000115, 110,    1.67) /* BulkMod */
     , (20000115, 111,       2) /* SizeMod */
     , (20000115, 165,       1) /* ArmorModVsNether */
     , (20000115, 156,     0.4) /* ProcSpellRate */;



     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (20000115,  55,       4481) /* ProcSpell - Fire Vuln 8*/;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20000115,   1, 'Fire Vuln Leather Boots') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20000115,   1, 0x020000D0) /* Setup */
     , (20000115,   3, 0x20000014) /* SoundTable */
     , (20000115,   6, 0x0400007E) /* PaletteBase */
     , (20000115,   7, 0x10000007) /* ClothingBase */
     , (20000115,   8, 0x06000FAE) /* Icon */
     , (20000115,  22, 0x3400002B) /* PhysicsEffectTable */
     , (20000115,  36, 0x0E000012) /* MutateFilter */
     , (20000115,  46, 0x38000032) /* TsysMutationFilter */;
