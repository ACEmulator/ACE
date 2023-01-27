DELETE FROM `weenie` WHERE `class_Id` = 450172;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450172, 'helmrareleikothatailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450172,   1,          2) /* ItemType - Armor */
     , (450172,   3,          4) /* PaletteTemplate - Brown */
     , (450172,   4,      16384) /* ClothingPriority - Head */
     , (450172,   5,        0) /* EncumbranceVal */
     , (450172,   8,         90) /* Mass */
     , (450172,   9,          1) /* ValidLocations - HeadWear */
     , (450172,  16,          1) /* ItemUseable - No */
     , (450172,  19,      20) /* Value */
     , (450172,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450172,  27,          2) /* ArmorType - Leather */
     , (450172,  28,        0) /* ArmorLevel */
     , (450172,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450172, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450172,  11, True ) /* IgnoreCollisions */
     , (450172,  13, True ) /* Ethereal */
     , (450172,  14, True ) /* GravityStatus */
     , (450172,  19, True ) /* Attackable */
     , (450172,  22, True ) /* Inscribable */
     , (450172,  91, False) /* Retained */
     , (450172, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450172,   5,  -0.033) /* ManaRate */
     , (450172,  12,    0.66) /* Shade */
     , (450172,  13,     1.3) /* ArmorModVsSlash */
     , (450172,  14,     1.1) /* ArmorModVsPierce */
     , (450172,  15,     1.1) /* ArmorModVsBludgeon */
     , (450172,  16,       1) /* ArmorModVsCold */
     , (450172,  17,     0.9) /* ArmorModVsFire */
     , (450172,  18,     0.9) /* ArmorModVsAcid */
     , (450172,  19,     0.9) /* ArmorModVsElectric */
     , (450172, 110,    1.67) /* BulkMod */
     , (450172, 111,       1) /* SizeMod */
     , (450172, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450172,   1, 'Helm of Leikotha''s Tears') /* Name */
     , (450172,  16, 'Can the undead cry? It is said that after Leikotha, the great warrior of Haebrous, was made undead by the Sand King Nerash, she wept for thirty days and thirty nights. Each tear shed fell onto her armor, infusing Leikotha''s essence into each piece. Courage, honor, sorrow, wrath and... everlasting death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450172,   1, 0x02001384) /* Setup */
     , (450172,   3, 0x20000014) /* SoundTable */
     , (450172,   6, 0x0400007E) /* PaletteBase */
     , (450172,   7, 0x100005F8) /* ClothingBase */
     , (450172,   8, 0x06005C33) /* Icon */
     , (450172,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450172,  36, 0x0E000012) /* MutateFilter */
     , (450172,  46, 0x38000032) /* TsysMutationFilter */
     , (450172,  52, 0x06005B0C) /* IconUnderlay */;

