DELETE FROM `weenie` WHERE `class_Id` = 450007;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450007, 'greavesraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450007,   1,          2) /* ItemType - Armor */
     , (450007,   3,          4) /* PaletteTemplate - Brown */
     , (450007,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (450007,   5,        0) /* EncumbranceVal */
     , (450007,   8,         90) /* Mass */
     , (450007,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (450007,  16,          1) /* ItemUseable - No */
     , (450007,  17,        223) /* RareId */
     , (450007,  19,      20) /* Value */
     , (450007,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450007,  27,          2) /* ArmorType - Leather */
     , (450007,  28,        0) /* ArmorLevel */
     , (450007,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450007, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450007,  11, True ) /* IgnoreCollisions */
     , (450007,  13, True ) /* Ethereal */
     , (450007,  14, True ) /* GravityStatus */
     , (450007,  19, True ) /* Attackable */
     , (450007,  22, True ) /* Inscribable */
     , (450007, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450007,   5,  -0.033) /* ManaRate */
     , (450007,  12,    0.66) /* Shade */
     , (450007,  13,       1) /* ArmorModVsSlash */
     , (450007,  14,     1.1) /* ArmorModVsPierce */
     , (450007,  15,     1.1) /* ArmorModVsBludgeon */
     , (450007,  16,       1) /* ArmorModVsCold */
     , (450007,  17,     0.9) /* ArmorModVsFire */
     , (450007,  18,     0.9) /* ArmorModVsAcid */
     , (450007,  19,     0.9) /* ArmorModVsElectric */
     , (450007, 110,    1.67) /* BulkMod */
     , (450007, 111,       1) /* SizeMod */
     , (450007, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450007,   1, 'Gelidite Greaves') /* Name */
     , (450007,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450007,   1, 0x02001386) /* Setup */
     , (450007,   3, 0x20000014) /* SoundTable */
     , (450007,   6, 0x0400007E) /* PaletteBase */
     , (450007,   7, 0x100005EA) /* ClothingBase */
     , (450007,   8, 0x06005C16) /* Icon */
     , (450007,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450007,  36, 0x0E000012) /* MutateFilter */
     , (450007,  46, 0x38000032) /* TsysMutationFilter */
     , (450007,  52, 0x06005B0C) /* IconUnderlay */;

