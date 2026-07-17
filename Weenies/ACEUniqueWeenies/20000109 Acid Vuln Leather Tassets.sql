DELETE FROM `weenie` WHERE `class_Id` = 20000109;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20000109, 'acidvulntassetsleather', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20000109,   1,          2) /* ItemType - Armor */
     , (20000109,   3,          4) /* PaletteTemplate - Brown */
     , (20000109,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (20000109,   5,        420) /* EncumbranceVal */
     , (20000109,   8,        140) /* Mass */
     , (20000109,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (20000109,  16,          1) /* ItemUseable - No */
     , (20000109,  19,       1100) /* Value */
     , (20000109,  27,          2) /* ArmorType - Leather */
     , (20000109, 106,        480) /* ItemSpellcraft */
     , (20000109,  28,         10) /* ArmorLevel */
     , (20000109,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (20000109, 124,          3) /* Version */
     , (20000109, 169,  252379406) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20000109,  22, True ) /* Inscribable */
     , (20000109, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20000109,  12,    0.66) /* Shade */
     , (20000109,  13,       1) /* ArmorModVsSlash */
     , (20000109,  14,     0.8) /* ArmorModVsPierce */
     , (20000109,  15,       1) /* ArmorModVsBludgeon */
     , (20000109,  16,     0.5) /* ArmorModVsCold */
     , (20000109,  17,     0.5) /* ArmorModVsFire */
     , (20000109,  18,     0.3) /* ArmorModVsAcid */
     , (20000109,  19,     0.6) /* ArmorModVsElectric */
     , (20000109,  39,    1.33) /* DefaultScale */
     , (20000109, 110,    1.67) /* BulkMod */
     , (20000109, 111,       1) /* SizeMod */
     , (20000109, 165,       1) /* ArmorModVsNether */
      , (20000109, 156,     0.4) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20000109,   1, 'Acid Vuln Leather Tassets') /* Name */;


     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (20000109,  55,       4473) /* ProcSpell - Acid Vuln 8*/;


INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20000109,   1, 0x020000E0) /* Setup */
     , (20000109,   3, 0x20000014) /* SoundTable */
     , (20000109,   6, 0x0400007E) /* PaletteBase */
     , (20000109,   7, 0x100003D4) /* ClothingBase */
     , (20000109,   8, 0x06002737) /* Icon */
     , (20000109,  22, 0x3400002B) /* PhysicsEffectTable */
     , (20000109,  36, 0x0E000012) /* MutateFilter */
     , (20000109,  46, 0x38000032) /* TsysMutationFilter */;
