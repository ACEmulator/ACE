DELETE FROM `weenie` WHERE `class_Id` = 450000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450000, 'bootsraregeliditetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450000,   1,          2) /* ItemType - Armor */
     , (450000,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450000,   4,      65536) /* ClothingPriority - Feet */
     , (450000,   5,        300) /* EncumbranceVal */
     , (450000,   8,         90) /* Mass */
     , (450000,   9,        256) /* ValidLocations - FootWear */
     , (450000,  16,          1) /* ItemUseable - No */
     , (450000,  17,        270) /* RareId */
     , (450000,  19,      20) /* Value */
     , (450000,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450000,  27,          2) /* ArmorType - Leather */
     , (450000,  28,          0) /* ArmorLevel */
     , (450000,  44,          6) /* Damage */
     , (450000,  45,          4) /* DamageType - Bludgeon */
     , (450000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450000, 150,        103) /* HookPlacement - Hook */
     , (450000, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450000,  11, True ) /* IgnoreCollisions */
     , (450000,  13, True ) /* Ethereal */
     , (450000,  14, True ) /* GravityStatus */
     , (450000,  22, True ) /* Inscribable */
     , (450000, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450000,   5,  -0.033) /* ManaRate */
     , (450000,  13,     1.3) /* ArmorModVsSlash */
     , (450000,  14,     0.9) /* ArmorModVsPierce */
     , (450000,  15,     1.3) /* ArmorModVsBludgeon */
     , (450000,  16,       1) /* ArmorModVsCold */
     , (450000,  17,     0.9) /* ArmorModVsFire */
     , (450000,  18,     0.9) /* ArmorModVsAcid */
     , (450000,  19,     0.9) /* ArmorModVsElectric */
     , (450000, 110,    1.67) /* BulkMod */
     , (450000, 111,       1) /* SizeMod */
     , (450000, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450000,   1, 'Gelidite Boots') /* Name */
     , (450000,  16, 'After the destruction of the Great Work, some human mages who followed the Gelidite agenda returned to Frore in hopes of reviving the project. Finding only shattered fragments of the Great Work, they attempted to rebuild it by using the shards as a foundation. Years later, they had achieved little success, and the project was soon abandoned. Not wanting to waste their efforts, they took the remaining crystals and fashioned great suits of armor. This is one piece of one such suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450000,   1, 0x02001377) /* Setup */
     , (450000,   3, 0x20000014) /* SoundTable */
     , (450000,   6, 0x0400007E) /* PaletteBase */
     , (450000,   7, 0x100005F9) /* ClothingBase */
     , (450000,   8, 0x06005BED) /* Icon */
     , (450000,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450000,  36, 0x0E000012) /* MutateFilter */
     , (450000,  46, 0x38000032) /* TsysMutationFilter */
     , (450000,  52, 0x06005B0C) /* IconUnderlay */;


