DELETE FROM `weenie` WHERE `class_Id` = 2000065;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2000065, 'ColdVulngreavesleather', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2000065,   1,          2) /* ItemType - Armor */
     , (2000065,   3,          4) /* PaletteTemplate - Brown */
     , (2000065,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (2000065,   5,        420) /* EncumbranceVal */
     , (2000065,   8,        140) /* Mass */
     , (2000065,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (2000065,  16,          1) /* ItemUseable - No */
     , (2000065,  19,       1200) /* Value */
     , (2000065,  27,          2) /* ArmorType - Leather */
     , (2000065,  28,         10) /* ArmorLevel */
     , (2000065, 106,        480) /* ItemSpellcraft */
     , (2000065,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2000065, 124,          3) /* Version */
     , (2000065, 169,  252379406) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2000065,  22, True ) /* Inscribable */
     , (2000065, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2000065,  12,    0.66) /* Shade */
     , (2000065,  13,       1) /* ArmorModVsSlash */
     , (2000065,  14,     0.8) /* ArmorModVsPierce */
     , (2000065,  15,       1) /* ArmorModVsBludgeon */
     , (2000065,  16,     0.5) /* ArmorModVsCold */
     , (2000065,  17,     0.5) /* ArmorModVsFire */
     , (2000065,  18,     0.3) /* ArmorModVsAcid */
     , (2000065,  19,     0.6) /* ArmorModVsElectric */
     , (2000065,  39,    1.33) /* DefaultScale */
     , (2000065, 110,    1.67) /* BulkMod */
     , (2000065, 111,       1) /* SizeMod */
     , (2000065, 165,       1) /* ArmorModVsNether */
     , (2000065, 156,     0.4) /* ProcSpellRate */;


INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (2000065,  55,       4479) /* ProcSpell - Cold Vuln level 8*/;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2000065,   1, 'Leather Greaves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2000065,   1, 0x020000D1) /* Setup */
     , (2000065,   3, 0x20000014) /* SoundTable */
     , (2000065,   6, 0x0400007E) /* PaletteBase */
     , (2000065,   7, 0x10000057) /* ClothingBase */
     , (2000065,   8, 0x060012DA) /* Icon */
     , (2000065,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2000065,  36, 0x0E000012) /* MutateFilter */
     , (2000065,  46, 0x38000032) /* TsysMutationFilter */;
