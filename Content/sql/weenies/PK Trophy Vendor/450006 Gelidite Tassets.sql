DELETE FROM `weenie` WHERE `class_Id` = 450006;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450006, 'tassetsraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450006,   1,          2) /* ItemType - Armor */
     , (450006,   3,          4) /* PaletteTemplate - Brown */
     , (450006,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (450006,   5,        0) /* EncumbranceVal */
     , (450006,   8,         90) /* Mass */
     , (450006,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (450006,  16,          1) /* ItemUseable - No */
     , (450006,  17,        222) /* RareId */
     , (450006,  19,      20) /* Value */
     , (450006,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450006,  27,          2) /* ArmorType - Leather */
     , (450006,  28,        0) /* ArmorLevel */
     , (450006,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450006, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450006,  11, True ) /* IgnoreCollisions */
     , (450006,  13, True ) /* Ethereal */
     , (450006,  14, True ) /* GravityStatus */
     , (450006,  19, True ) /* Attackable */
     , (450006,  22, True ) /* Inscribable */
     , (450006, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450006,   5,  -0.033) /* ManaRate */
     , (450006,  12,    0.66) /* Shade */
     , (450006,  13,       1) /* ArmorModVsSlash */
     , (450006,  14,     1.1) /* ArmorModVsPierce */
     , (450006,  15,     1.1) /* ArmorModVsBludgeon */
     , (450006,  16,     1.1) /* ArmorModVsCold */
     , (450006,  17,     0.9) /* ArmorModVsFire */
     , (450006,  18,     0.9) /* ArmorModVsAcid */
     , (450006,  19,       1) /* ArmorModVsElectric */
     , (450006, 110,    1.67) /* BulkMod */
     , (450006, 111,       1) /* SizeMod */
     , (450006, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450006,   1, 'Gelidite Tassets') /* Name */
     , (450006,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450006,   1, 0x02001387) /* Setup */
     , (450006,   3, 0x20000014) /* SoundTable */
     , (450006,   6, 0x0400007E) /* PaletteBase */
     , (450006,   7, 0x100005EB) /* ClothingBase */
     , (450006,   8, 0x06005C18) /* Icon */
     , (450006,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450006,  36, 0x0E000012) /* MutateFilter */
     , (450006,  46, 0x38000032) /* TsysMutationFilter */
     , (450006,  52, 0x06005B0C) /* IconUnderlay */;

