DELETE FROM `weenie` WHERE `class_Id` = 450176;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450176, 'gauntletsrareleikothatailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450176,   1,          2) /* ItemType - Armor */
     , (450176,   3,          4) /* PaletteTemplate - Brown */
     , (450176,   4,      32768) /* ClothingPriority - Hands */
     , (450176,   5,        0) /* EncumbranceVal */
     , (450176,   8,         90) /* Mass */
     , (450176,   9,         32) /* ValidLocations - HandWear */
     , (450176,  16,          1) /* ItemUseable - No */
     , (450176,  19,      20) /* Value */
     , (450176,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450176,  27,          2) /* ArmorType - Leather */
     , (450176,  28,        0) /* ArmorLevel */
     , (450176,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450176, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (450176,   4,          0) /* ItemTotalXp */
     , (450176,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450176,  11, True ) /* IgnoreCollisions */
     , (450176,  13, True ) /* Ethereal */
     , (450176,  14, True ) /* GravityStatus */
     , (450176,  19, True ) /* Attackable */
     , (450176,  22, True ) /* Inscribable */
     , (450176,  91, False) /* Retained */
     , (450176, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450176,   5,  -0.033) /* ManaRate */
     , (450176,  12,    0.66) /* Shade */
     , (450176,  13,     1.4) /* ArmorModVsSlash */
     , (450176,  14,     1.1) /* ArmorModVsPierce */
     , (450176,  15,     1.1) /* ArmorModVsBludgeon */
     , (450176,  16,       1) /* ArmorModVsCold */
     , (450176,  17,     0.9) /* ArmorModVsFire */
     , (450176,  18,     0.9) /* ArmorModVsAcid */
     , (450176,  19,       1) /* ArmorModVsElectric */
     , (450176, 110,    1.67) /* BulkMod */
     , (450176, 111,       1) /* SizeMod */
     , (450176, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450176,   1, 'Gauntlets of Leikotha''s Tears') /* Name */
     , (450176,  16, 'Can the undead cry? It is said that after Leikotha, the great warrior of Haebrous, was made undead by the Sand King Nerash, she wept for thirty days and thirty nights. Each tear shed fell onto her armor, infusing Leikotha''s essence into each piece. Courage, honor, sorrow, wrath and... everlasting death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450176,   1, 0x02001390) /* Setup */
     , (450176,   3, 0x20000014) /* SoundTable */
     , (450176,   6, 0x0400007E) /* PaletteBase */
     , (450176,   7, 0x100005F5) /* ClothingBase */
     , (450176,   8, 0x06005C2C) /* Icon */
     , (450176,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450176,  36, 0x0E000012) /* MutateFilter */
     , (450176,  46, 0x38000032) /* TsysMutationFilter */
     , (450176,  52, 0x06005B0C) /* IconUnderlay */;

