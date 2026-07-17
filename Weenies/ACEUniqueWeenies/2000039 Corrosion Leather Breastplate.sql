DELETE FROM `weenie` WHERE `class_Id` = 2000039;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2000039, 'CorrosionCastonStrikebreastplateleather', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2000039,   1,          2) /* ItemType - Armor */
     , (2000039,   3,          4) /* PaletteTemplate - Brown */
     , (2000039,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (2000039,   5,        420) /* EncumbranceVal */
     , (2000039,   8,        140) /* Mass */
     , (2000039,   9,        512) /* ValidLocations - ChestArmor */
     , (2000039,  16,          1) /* ItemUseable - No */
     , (2000039,  19,       1400) /* Value */
     , (2000039,  27,          2) /* ArmorType - Leather */
     , (2000039,  28,         10) /* ArmorLevel */
     , (2000039,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2000039, 124,          3) /* Version */
     , (2000039, 169,  118163214) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2000039,  22, True ) /* Inscribable */
     , (2000039, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2000039,  12,    0.66) /* Shade */
     , (2000039,  13,       1) /* ArmorModVsSlash */
     , (2000039,  14,     0.8) /* ArmorModVsPierce */
     , (2000039,  15,       1) /* ArmorModVsBludgeon */
     , (2000039,  16,     0.5) /* ArmorModVsCold */
     , (2000039,  17,     0.5) /* ArmorModVsFire */
     , (2000039,  18,     0.3) /* ArmorModVsAcid */
     , (2000039,  19,     0.6) /* ArmorModVsElectric */
     , (2000039, 110,    1.67) /* BulkMod */
     , (2000039, 111,     2.5) /* SizeMod */
     , (2000039, 165,       1) /* ArmorModVsNether */
     , (2000039, 156,     0.2) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2000039,   1, 'Corrosion Leather Breastplate') /* Name */;

     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (2000039,  55,       5394	) /* ProcSpell - Corruption*/;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2000039,   1, 0x020000D2) /* Setup */
     , (2000039,   3, 0x20000014) /* SoundTable */
     , (2000039,   6, 0x0400007E) /* PaletteBase */
     , (2000039,   7, 0x10000055) /* ClothingBase */
     , (2000039,   8, 0x06000FD6) /* Icon */
     , (2000039,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2000039,  36, 0x0E000012) /* MutateFilter */
     , (2000039,  46, 0x38000032) /* TsysMutationFilter */;
