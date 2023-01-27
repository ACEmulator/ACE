DELETE FROM `weenie` WHERE `class_Id` = 450008;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450008, 'helmraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450008,   1,          2) /* ItemType - Armor */
     , (450008,   3,          4) /* PaletteTemplate - Brown */
     , (450008,   4,      16384) /* ClothingPriority - Head */
     , (450008,   5,        0) /* EncumbranceVal */
     , (450008,   8,         90) /* Mass */
     , (450008,   9,          1) /* ValidLocations - HeadWear */
     , (450008,  16,          1) /* ItemUseable - No */
     , (450008,  17,        262) /* RareId */
     , (450008,  19,      20) /* Value */
     , (450008,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450008,  27,          2) /* ArmorType - Leather */
     , (450008,  28,        0) /* ArmorLevel */
     , (450008,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450008, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450008,  11, True ) /* IgnoreCollisions */
     , (450008,  13, True ) /* Ethereal */
     , (450008,  14, True ) /* GravityStatus */
     , (450008,  19, True ) /* Attackable */
     , (450008,  22, True ) /* Inscribable */
     , (450008,  91, False) /* Retained */
     , (450008, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450008,   5,  -0.033) /* ManaRate */
     , (450008,  12,    0.66) /* Shade */
     , (450008,  13,     1.1) /* ArmorModVsSlash */
     , (450008,  14,     1.3) /* ArmorModVsPierce */
     , (450008,  15,     1.1) /* ArmorModVsBludgeon */
     , (450008,  16,     1.1) /* ArmorModVsCold */
     , (450008,  17,     0.9) /* ArmorModVsFire */
     , (450008,  18,     0.9) /* ArmorModVsAcid */
     , (450008,  19,     0.9) /* ArmorModVsElectric */
     , (450008, 110,    1.67) /* BulkMod */
     , (450008, 111,       1) /* SizeMod */
     , (450008, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450008,   1, 'Gelidite Mitre') /* Name */
     , (450008,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450008,   1, 0x02001384) /* Setup */
     , (450008,   3, 0x20000014) /* SoundTable */
     , (450008,   6, 0x0400007E) /* PaletteBase */
     , (450008,   7, 0x100005E7) /* ClothingBase */
     , (450008,   8, 0x06005C0F) /* Icon */
     , (450008,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450008,  36, 0x0E000012) /* MutateFilter */
     , (450008,  46, 0x38000032) /* TsysMutationFilter */
     , (450008,  52, 0x06005B0C) /* IconUnderlay */;


