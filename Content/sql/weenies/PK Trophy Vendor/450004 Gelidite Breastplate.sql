DELETE FROM `weenie` WHERE `class_Id` = 450004;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450004, 'breastplateraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450004,   1,          2) /* ItemType - Armor */
     , (450004,   3,          4) /* PaletteTemplate - Brown */
     , (450004,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450004,   5,       1825) /* EncumbranceVal */
     , (450004,   8,         90) /* Mass */
     , (450004,   9,        512) /* ValidLocations - ChestArmor */
     , (450004,  16,          1) /* ItemUseable - No */
     , (450004,  17,        216) /* RareId */
     , (450004,  19,      20) /* Value */
     , (450004,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450004,  27,          2) /* ArmorType - Leather */
     , (450004,  28,        0) /* ArmorLevel */
     , (450004,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450004, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450004,  11, True ) /* IgnoreCollisions */
     , (450004,  13, True ) /* Ethereal */
     , (450004,  14, True ) /* GravityStatus */
     , (450004,  19, True ) /* Attackable */
     , (450004,  22, True ) /* Inscribable */
     , (450004,  91, False) /* Retained */
     , (450004, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450004,   5,  -0.033) /* ManaRate */
     , (450004,  12,    0.66) /* Shade */
     , (450004,  13,     1.1) /* ArmorModVsSlash */
     , (450004,  14,     1.3) /* ArmorModVsPierce */
     , (450004,  15,     1.1) /* ArmorModVsBludgeon */
     , (450004,  16,     1.1) /* ArmorModVsCold */
     , (450004,  17,     0.9) /* ArmorModVsFire */
     , (450004,  18,     0.9) /* ArmorModVsAcid */
     , (450004,  19,     0.9) /* ArmorModVsElectric */
     , (450004, 110,    1.67) /* BulkMod */
     , (450004, 111,       1) /* SizeMod */
     , (450004, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450004,   1, 'Gelidite Breastplate') /* Name */
     , (450004,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450004,   1, 0x0200138B) /* Setup */
     , (450004,   3, 0x20000014) /* SoundTable */
     , (450004,   6, 0x0400007E) /* PaletteBase */
     , (450004,   7, 0x100005EF) /* ClothingBase */
     , (450004,   8, 0x06005C20) /* Icon */
     , (450004,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450004,  36, 0x0E000012) /* MutateFilter */
     , (450004,  46, 0x38000032) /* TsysMutationFilter */
     , (450004,  52, 0x06005B0C) /* IconUnderlay */;


