DELETE FROM `weenie` WHERE `class_Id` = 450003;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450003, 'pauldronsraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450003,   1,          2) /* ItemType - Armor */
     , (450003,   3,          4) /* PaletteTemplate - Brown */
     , (450003,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (450003,   5,        0) /* EncumbranceVal */
     , (450003,   8,         90) /* Mass */
     , (450003,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (450003,  16,          1) /* ItemUseable - No */
     , (450003,  17,        215) /* RareId */
     , (450003,  19,      20) /* Value */
     , (450003,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450003,  27,          2) /* ArmorType - Leather */
     , (450003,  28,        0) /* ArmorLevel */
     , (450003,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450003, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450003,  11, True ) /* IgnoreCollisions */
     , (450003,  13, True ) /* Ethereal */
     , (450003,  14, True ) /* GravityStatus */
     , (450003,  19, True ) /* Attackable */
     , (450003,  22, True ) /* Inscribable */
     , (450003,  91, False) /* Retained */
     , (450003, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450003,   5,  -0.033) /* ManaRate */
     , (450003,  12,    0.66) /* Shade */
     , (450003,  13,     1.1) /* ArmorModVsSlash */
     , (450003,  14,     1.3) /* ArmorModVsPierce */
     , (450003,  15,     1.1) /* ArmorModVsBludgeon */
     , (450003,  16,     1.1) /* ArmorModVsCold */
     , (450003,  17,     0.9) /* ArmorModVsFire */
     , (450003,  18,     0.9) /* ArmorModVsAcid */
     , (450003,  19,     0.9) /* ArmorModVsElectric */
     , (450003, 110,    1.67) /* BulkMod */
     , (450003, 111,       1) /* SizeMod */
     , (450003, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450003,   1, 'Gelidite Pauldrons') /* Name */
     , (450003,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450003,   1, 0x0200138E) /* Setup */
     , (450003,   3, 0x20000014) /* SoundTable */
     , (450003,   6, 0x0400007E) /* PaletteBase */
     , (450003,   7, 0x100005F2) /* ClothingBase */
     , (450003,   8, 0x06005C26) /* Icon */
     , (450003,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450003,  36, 0x0E000012) /* MutateFilter */
     , (450003,  46, 0x38000032) /* TsysMutationFilter */
     , (450003,  52, 0x06005B0C) /* IconUnderlay */;


