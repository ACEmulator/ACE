DELETE FROM `weenie` WHERE `class_Id` = 450167;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450167, 'leggingsrarepatriarchtwilighttailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450167,   1,          2) /* ItemType - Armor */
     , (450167,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450167,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450167,   5,        0) /* EncumbranceVal */
     , (450167,   8,         90) /* Mass */
     , (450167,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450167,  16,          1) /* ItemUseable - No */
     , (450167,  19,      20) /* Value */
     , (450167,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450167,  27,          2) /* ArmorType - Leather */
     , (450167,  28,        0) /* ArmorLevel */
     , (450167,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450167, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450167,  11, True ) /* IgnoreCollisions */
     , (450167,  13, True ) /* Ethereal */
     , (450167,  14, True ) /* GravityStatus */
     , (450167,  19, True ) /* Attackable */
     , (450167,  22, True ) /* Inscribable */
     , (450167, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450167,   5,  -0.033) /* ManaRate */
     , (450167,  12,    0.66) /* Shade */
     , (450167,  13,     1.1) /* ArmorModVsSlash */
     , (450167,  14,     0.9) /* ArmorModVsPierce */
     , (450167,  15,     1.1) /* ArmorModVsBludgeon */
     , (450167,  16,     1.3) /* ArmorModVsCold */
     , (450167,  17,     0.9) /* ArmorModVsFire */
     , (450167,  18,     0.9) /* ArmorModVsAcid */
     , (450167,  19,     0.9) /* ArmorModVsElectric */
     , (450167, 110,    1.67) /* BulkMod */
     , (450167, 111,       1) /* SizeMod */
     , (450167, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450167,   1, 'Patriarch''s Twilight Tights') /* Name */
     , (450167,  16, 'Made of the finest silks and embroidered with the most expensive gold thread and jewels, these tights are the pinnacle of excess. They compliment the Patriarch''s Twilight Coat perfectly. Just wearing these leggings may make the wearer feel more confident.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450167,   1, 0x02001336) /* Setup */
     , (450167,   3, 0x20000014) /* SoundTable */
     , (450167,   6, 0x0400007E) /* PaletteBase */
     , (450167,   7, 0x100005FB) /* ClothingBase */
     , (450167,   8, 0x06005C38) /* Icon */
     , (450167,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450167,  36, 0x0E000012) /* MutateFilter */
     , (450167,  46, 0x38000032) /* TsysMutationFilter */
     , (450167,  52, 0x06005B0C) /* IconUnderlay */;

