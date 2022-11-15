DELETE FROM `weenie` WHERE `class_Id` = 450005;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450005, 'girthraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450005,   1,          2) /* ItemType - Armor */
     , (450005,   3,          4) /* PaletteTemplate - Brown */
     , (450005,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450005,   5,        0) /* EncumbranceVal */
     , (450005,   8,         90) /* Mass */
     , (450005,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450005,  16,          1) /* ItemUseable - No */
     , (450005,  17,        218) /* RareId */
     , (450005,  19,      20) /* Value */
     , (450005,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450005,  27,          2) /* ArmorType - Leather */
     , (450005,  28,        0) /* ArmorLevel */
     , (450005,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450005, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450005,  11, True ) /* IgnoreCollisions */
     , (450005,  13, True ) /* Ethereal */
     , (450005,  14, True ) /* GravityStatus */
     , (450005,  19, True ) /* Attackable */
     , (450005,  22, True ) /* Inscribable */
     , (450005, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450005,   5,  -0.033) /* ManaRate */
     , (450005,  12,    0.66) /* Shade */
     , (450005,  13,     1.1) /* ArmorModVsSlash */
     , (450005,  14,     1.3) /* ArmorModVsPierce */
     , (450005,  15,     1.1) /* ArmorModVsBludgeon */
     , (450005,  16,     1.1) /* ArmorModVsCold */
     , (450005,  17,     0.9) /* ArmorModVsFire */
     , (450005,  18,     0.9) /* ArmorModVsAcid */
     , (450005,  19,     0.9) /* ArmorModVsElectric */
     , (450005, 110,    1.67) /* BulkMod */
     , (450005, 111,       1) /* SizeMod */
     , (450005, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450005,   1, 'Gelidite Girth') /* Name */
     , (450005,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450005,   1, 0x02001389) /* Setup */
     , (450005,   3, 0x20000014) /* SoundTable */
     , (450005,   6, 0x0400007E) /* PaletteBase */
     , (450005,   7, 0x100005ED) /* ClothingBase */
     , (450005,   8, 0x06005C1C) /* Icon */
     , (450005,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450005,  36, 0x0E000012) /* MutateFilter */
     , (450005,  46, 0x38000032) /* TsysMutationFilter */
     , (450005,  52, 0x06005B0C) /* IconUnderlay */;


