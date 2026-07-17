DELETE FROM `weenie` WHERE `class_Id` = 2000086;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2000086, 'Corruptionpauldronsleather', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2000086,   1,          2) /* ItemType - Armor */
     , (2000086,   3,          4) /* PaletteTemplate - Brown */
     , (2000086,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (2000086,   5,        420) /* EncumbranceVal */
     , (2000086,   8,        140) /* Mass */
     , (2000086,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (2000086,  16,          1) /* ItemUseable - No */
     , (2000086,  19,       1250) /* Value */
     , (2000086,  27,          2) /* ArmorType - Leather */
     , (2000086, 106,        480) /* ItemSpellcraft */
     , (2000086,  28,         90) /* ArmorLevel */
     , (2000086,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2000086, 124,          3) /* Version */
     , (2000086, 169,  118161678) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2000086,  22, True ) /* Inscribable */
     , (2000086, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2000086,  12,    0.66) /* Shade */
     , (2000086,  13,       1) /* ArmorModVsSlash */
     , (2000086,  14,     0.8) /* ArmorModVsPierce */
     , (2000086,  15,       1) /* ArmorModVsBludgeon */
     , (2000086,  16,     0.5) /* ArmorModVsCold */
     , (2000086,  17,     0.5) /* ArmorModVsFire */
     , (2000086,  18,     0.3) /* ArmorModVsAcid */
     , (2000086,  19,     0.6) /* ArmorModVsElectric */
     , (2000086,  39,     1.1) /* DefaultScale */
     , (2000086, 110,    1.67) /* BulkMod */
     , (2000086, 111,       1) /* SizeMod */
     , (2000086, 165,       1) /* ArmorModVsNether */
     , (2000086, 156,     0.2) /* ProcSpellRate */;


INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2000086,   1, 'Corruption Leather Pauldrons') /* Name */;

     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (2000086,  55,       5402	) /* ProcSpell - Corruption*/;


INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2000086,   1, 0x020000D1) /* Setup */
     , (2000086,   3, 0x20000014) /* SoundTable */
     , (2000086,   6, 0x0400007E) /* PaletteBase */
     , (2000086,   7, 0x1000004F) /* ClothingBase */
     , (2000086,   8, 0x0600130B) /* Icon */
     , (2000086,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2000086,  36, 0x0E000012) /* MutateFilter */
     , (2000086,  46, 0x38000032) /* TsysMutationFilter */;
