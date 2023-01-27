DELETE FROM `weenie` WHERE `class_Id` = 450174;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450174, 'pauldronsrareleikothatailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450174,   1,          2) /* ItemType - Armor */
     , (450174,   3,          4) /* PaletteTemplate - Brown */
     , (450174,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (450174,   5,        0) /* EncumbranceVal */
     , (450174,   8,         90) /* Mass */
     , (450174,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (450174,  16,          1) /* ItemUseable - No */
     , (450174,  19,      20) /* Value */
     , (450174,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450174,  27,          2) /* ArmorType - Leather */
     , (450174,  28,        0) /* ArmorLevel */
     , (450174,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450174, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450174,  11, True ) /* IgnoreCollisions */
     , (450174,  13, True ) /* Ethereal */
     , (450174,  14, True ) /* GravityStatus */
     , (450174,  19, True ) /* Attackable */
     , (450174,  22, True ) /* Inscribable */
     , (450174,  91, False) /* Retained */
     , (450174, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450174,   5,  -0.033) /* ManaRate */
     , (450174,  12,    0.66) /* Shade */
     , (450174,  13,     1.3) /* ArmorModVsSlash */
     , (450174,  14,     1.1) /* ArmorModVsPierce */
     , (450174,  15,     1.1) /* ArmorModVsBludgeon */
     , (450174,  16,       1) /* ArmorModVsCold */
     , (450174,  17,     0.9) /* ArmorModVsFire */
     , (450174,  18,     0.9) /* ArmorModVsAcid */
     , (450174,  19,       1) /* ArmorModVsElectric */
     , (450174, 110,    1.67) /* BulkMod */
     , (450174, 111,       1) /* SizeMod */
     , (450174, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450174,   1, 'Pauldrons of Leikotha''s Tears') /* Name */
     , (450174,  16, 'Can the undead cry? It is said that after Leikotha, the great warrior of Haebrous, was made undead by the Sand King Nerash, she wept for thirty days and thirty nights. Each tear shed fell onto her armor, infusing Leikotha''s essence into each piece. Courage, honor, sorrow, wrath and... everlasting death. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450174,   1, 0x0200138D) /* Setup */
     , (450174,   3, 0x20000014) /* SoundTable */
     , (450174,   6, 0x0400007E) /* PaletteBase */
     , (450174,   7, 0x100005F1) /* ClothingBase */
     , (450174,   8, 0x06005C24) /* Icon */
     , (450174,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450174,  36, 0x0E000012) /* MutateFilter */
     , (450174,  46, 0x38000032) /* TsysMutationFilter */
     , (450174,  52, 0x06005B0C) /* IconUnderlay */;


