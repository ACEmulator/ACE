DELETE FROM `weenie` WHERE `class_Id` = 450002;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450002, 'bracersraregeliditetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450002,   1,          2) /* ItemType - Armor */
     , (450002,   3,          4) /* PaletteTemplate - Brown */
     , (450002,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (450002,   5,        0) /* EncumbranceVal */
     , (450002,   8,         90) /* Mass */
     , (450002,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (450002,  16,          1) /* ItemUseable - No */
	 , (450002,  19,      20) /* Value */
     , (450002,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450002,  27,          2) /* ArmorType - Leather */
     , (450002,  28,        0) /* ArmorLevel */
     , (450002,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450002, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450002,  11, True ) /* IgnoreCollisions */
     , (450002,  13, True ) /* Ethereal */
     , (450002,  14, True ) /* GravityStatus */
     , (450002,  19, True ) /* Attackable */
     , (450002,  22, True ) /* Inscribable */
     , (450002,  91, False) /* Retained */
     , (450002, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450002,   5,  -0.033) /* ManaRate */
     , (450002,  12,    0.66) /* Shade */
     , (450002,  13,     1.1) /* ArmorModVsSlash */
     , (450002,  14,     1.3) /* ArmorModVsPierce */
     , (450002,  15,     1.1) /* ArmorModVsBludgeon */
     , (450002,  16,     1.1) /* ArmorModVsCold */
     , (450002,  17,     0.9) /* ArmorModVsFire */
     , (450002,  18,     0.9) /* ArmorModVsAcid */
     , (450002,  19,     0.9) /* ArmorModVsElectric */
     , (450002, 110,    1.67) /* BulkMod */
     , (450002, 111,       1) /* SizeMod */
     , (450002, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450002,   1, 'Gelidite Bracers') /* Name */
     , (450002,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450002,   1, 0x02001390) /* Setup */
     , (450002,   3, 0x20000014) /* SoundTable */
     , (450002,   6, 0x0400007E) /* PaletteBase */
     , (450002,   7, 0x100005F4) /* ClothingBase */
     , (450002,   8, 0x06005C2A) /* Icon */
     , (450002,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450002,  36, 0x0E000012) /* MutateFilter */
     , (450002,  46, 0x38000032) /* TsysMutationFilter */
     , (450002,  52, 0x06005B0C) /* IconUnderlay */;
