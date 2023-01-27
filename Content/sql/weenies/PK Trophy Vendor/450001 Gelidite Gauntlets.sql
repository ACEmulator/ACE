DELETE FROM `weenie` WHERE `class_Id` = 450001;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450001, 'gauntletsraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450001,   1,          2) /* ItemType - Armor */
     , (450001,   3,          4) /* PaletteTemplate - Brown */
     , (450001,   4,      32768) /* ClothingPriority - Hands */
     , (450001,   5,        300) /* EncumbranceVal */
     , (450001,   8,         90) /* Mass */
     , (450001,   9,         32) /* ValidLocations - HandWear */
     , (450001,  16,          1) /* ItemUseable - No */
     , (450001,  17,        211) /* RareId */
     , (450001,  19,      20) /* Value */
     , (450001,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450001,  27,          2) /* ArmorType - Leather */
     , (450001,  28,        0) /* ArmorLevel */
     , (450001,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450001, 106,        325) /* ItemSpellcraft */
     , (450001, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450001,  11, True ) /* IgnoreCollisions */
     , (450001,  13, True ) /* Ethereal */
     , (450001,  14, True ) /* GravityStatus */
     , (450001,  19, True ) /* Attackable */
     , (450001,  22, True ) /* Inscribable */
     , (450001,  91, False) /* Retained */
     , (450001, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450001,   5,  -0.033) /* ManaRate */
     , (450001,  12,    0.66) /* Shade */
     , (450001,  13,       1) /* ArmorModVsSlash */
     , (450001,  14,     1.1) /* ArmorModVsPierce */
     , (450001,  15,     1.1) /* ArmorModVsBludgeon */
     , (450001,  16,     1.1) /* ArmorModVsCold */
     , (450001,  17,     0.9) /* ArmorModVsFire */
     , (450001,  18,     0.9) /* ArmorModVsAcid */
     , (450001,  19,     0.9) /* ArmorModVsElectric */
     , (450001, 110,    1.67) /* BulkMod */
     , (450001, 111,       1) /* SizeMod */
     , (450001, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450001,   1, 'Gelidite Gauntlets') /* Name */
     , (450001,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450001,   1, 0x02001392) /* Setup */
     , (450001,   3, 0x20000014) /* SoundTable */
     , (450001,   6, 0x0400007E) /* PaletteBase */
     , (450001,   7, 0x100005F6) /* ClothingBase */
     , (450001,   8, 0x06005C2E) /* Icon */
     , (450001,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450001,  36, 0x0E000012) /* MutateFilter */
     , (450001,  46, 0x38000032) /* TsysMutationFilter */
     , (450001,  52, 0x06005B0C) /* IconUnderlay */;


