DELETE FROM `weenie` WHERE `class_Id` = 450177;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450177, 'girthrareleikothatailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450177,   1,          2) /* ItemType - Armor */
     , (450177,   3,          4) /* PaletteTemplate - Brown */
     , (450177,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450177,   5,        0) /* EncumbranceVal */
     , (450177,   8,         90) /* Mass */
     , (450177,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450177,  16,          1) /* ItemUseable - No */
     , (450177,  19,      20) /* Value */
     , (450177,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450177,  27,          2) /* ArmorType - Leather */
     , (450177,  28,        0) /* ArmorLevel */
     , (450177,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450177, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450177,  11, True ) /* IgnoreCollisions */
     , (450177,  13, True ) /* Ethereal */
     , (450177,  14, True ) /* GravityStatus */
     , (450177,  19, True ) /* Attackable */
     , (450177,  22, True ) /* Inscribable */
     , (450177,  91, False) /* Retained */
     , (450177, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450177,   5,  -0.033) /* ManaRate */
     , (450177,  12,    0.66) /* Shade */
     , (450177,  13,     1.4) /* ArmorModVsSlash */
     , (450177,  14,     1.1) /* ArmorModVsPierce */
     , (450177,  15,     1.1) /* ArmorModVsBludgeon */
     , (450177,  16,       1) /* ArmorModVsCold */
     , (450177,  17,     0.9) /* ArmorModVsFire */
     , (450177,  18,     0.9) /* ArmorModVsAcid */
     , (450177,  19,       1) /* ArmorModVsElectric */
     , (450177, 110,    1.67) /* BulkMod */
     , (450177, 111,       1) /* SizeMod */
     , (450177, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450177,   1, 'Girth of Leikotha''s Tears') /* Name */
     , (450177,  16, 'Can the undead cry? It is said that after Leikotha, the great warrior of Haebrous, was made undead by the Sand King Nerash, she wept for thirty days and thirty nights. Each tear shed fell onto her armor, infusing Leikotha''s essence into each piece. Courage, honor, sorrow, wrath and... everlasting death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450177,   1, 0x0200138A) /* Setup */
     , (450177,   3, 0x20000014) /* SoundTable */
     , (450177,   6, 0x0400007E) /* PaletteBase */
     , (450177,   7, 0x100005EE) /* ClothingBase */
     , (450177,   8, 0x06005C1E) /* Icon */
     , (450177,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450177,  36, 0x0E000012) /* MutateFilter */
     , (450177,  46, 0x38000032) /* TsysMutationFilter */
     , (450177,  52, 0x06005B0C) /* IconUnderlay */;
