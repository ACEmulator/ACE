DELETE FROM `weenie` WHERE `class_Id` = 450171;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450171, 'gauntletsrarecrimsonstartailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450171,   1,          2) /* ItemType - Armor */
     , (450171,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450171,   4,      32768) /* ClothingPriority - Hands */
     , (450171,   5,        0) /* EncumbranceVal */
     , (450171,   8,         90) /* Mass */
     , (450171,   9,         32) /* ValidLocations - HandWear */
     , (450171,  16,          1) /* ItemUseable - No */
     , (450171,  19,      20) /* Value */
     , (450171,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450171,  27,          2) /* ArmorType - Leather */
     , (450171,  28,        0) /* ArmorLevel */
     , (450171,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450171, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450171,  11, True ) /* IgnoreCollisions */
     , (450171,  13, True ) /* Ethereal */
     , (450171,  14, True ) /* GravityStatus */
     , (450171,  19, True ) /* Attackable */
     , (450171,  22, True ) /* Inscribable */
     , (450171,  91, False) /* Retained */
     , (450171, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450171,   5,  -0.033) /* ManaRate */
     , (450171,  12,    0.66) /* Shade */
     , (450171,  13,     1.1) /* ArmorModVsSlash */
     , (450171,  14,     1.1) /* ArmorModVsPierce */
     , (450171,  15,     1.1) /* ArmorModVsBludgeon */
     , (450171,  16,     0.9) /* ArmorModVsCold */
     , (450171,  17,     1.3) /* ArmorModVsFire */
     , (450171,  18,     0.9) /* ArmorModVsAcid */
     , (450171,  19,     0.9) /* ArmorModVsElectric */
     , (450171, 110,    1.67) /* BulkMod */
     , (450171, 111,       1) /* SizeMod */
     , (450171, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450171,   1, 'Gauntlets of the Crimson Star') /* Name */
     , (450171,  16, 'Using a combination of chain, scale, and plate armor, these gauntlets are the ultimate in comfort and protection. They were built to protect the wearer from fire.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450171,   1, 0x02001397) /* Setup */
     , (450171,   3, 0x20000014) /* SoundTable */
     , (450171,   6, 0x0400007E) /* PaletteBase */
     , (450171,   7, 0x100005FE) /* ClothingBase */
     , (450171,   8, 0x06005C3F) /* Icon */
     , (450171,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450171,  36, 0x0E000012) /* MutateFilter */
     , (450171,  46, 0x38000032) /* TsysMutationFilter */
     , (450171,  52, 0x06005B0C) /* IconUnderlay */;


