DELETE FROM `weenie` WHERE `class_Id` = 450178;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450178, 'greavesrareleikothatailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450178,   1,          2) /* ItemType - Armor */
     , (450178,   3,          4) /* PaletteTemplate - Brown */
     , (450178,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (450178,   5,        0) /* EncumbranceVal */
     , (450178,   8,         90) /* Mass */
     , (450178,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (450178,  16,          1) /* ItemUseable - No */
     , (450178,  19,      20) /* Value */
     , (450178,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450178,  27,          2) /* ArmorType - Leather */
     , (450178,  28,        0) /* ArmorLevel */
     , (450178,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450178, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450178,  11, True ) /* IgnoreCollisions */
     , (450178,  13, True ) /* Ethereal */
     , (450178,  14, True ) /* GravityStatus */
     , (450178,  19, True ) /* Attackable */
     , (450178,  22, True ) /* Inscribable */
     , (450178,  91, False) /* Retained */
     , (450178, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450178,   5,  -0.033) /* ManaRate */
     , (450178,  12,    0.66) /* Shade */
     , (450178,  13,     1.3) /* ArmorModVsSlash */
     , (450178,  14,     1.1) /* ArmorModVsPierce */
     , (450178,  15,     1.1) /* ArmorModVsBludgeon */
     , (450178,  16,       1) /* ArmorModVsCold */
     , (450178,  17,     0.9) /* ArmorModVsFire */
     , (450178,  18,     0.9) /* ArmorModVsAcid */
     , (450178,  19,       1) /* ArmorModVsElectric */
     , (450178, 110,    1.67) /* BulkMod */
     , (450178, 111,       1) /* SizeMod */
     , (450178, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450178,   1, 'Greaves of Leikotha''s Tears') /* Name */
     , (450178,  16, 'Can the undead cry? It is said that after Leikotha, the great warrior of Haebrous, was made undead by the Sand King Nerash, she wept for thirty days and thirty nights. Each tear shed fell onto her armor, infusing Leikotha''s essence into each piece. Courage, honor, sorrow, wrath and... everlasting death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450178,   1, 0x02001385) /* Setup */
     , (450178,   3, 0x20000014) /* SoundTable */
     , (450178,   6, 0x0400007E) /* PaletteBase */
     , (450178,   7, 0x100005E9) /* ClothingBase */
     , (450178,   8, 0x06005C14) /* Icon */
     , (450178,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450178,  36, 0x0E000012) /* MutateFilter */
     , (450178,  46, 0x38000032) /* TsysMutationFilter */
     , (450178,  52, 0x06005B0C) /* IconUnderlay */;

