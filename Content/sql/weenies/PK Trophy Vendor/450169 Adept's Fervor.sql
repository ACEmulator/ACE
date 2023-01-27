DELETE FROM `weenie` WHERE `class_Id` = 450169;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450169, 'gauntletsrareadeptsfervortailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450169,   1,          2) /* ItemType - Armor */
     , (450169,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450169,   4,      32768) /* ClothingPriority - Hands */
     , (450169,   5,        0) /* EncumbranceVal */
     , (450169,   8,         90) /* Mass */
     , (450169,   9,         32) /* ValidLocations - HandWear */
     , (450169,  16,          1) /* ItemUseable - No */
     , (450169,  19,      20) /* Value */
     , (450169,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450169,  27,          2) /* ArmorType - Leather */
     , (450169,  28,        0) /* ArmorLevel */
     , (450169,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450169, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450169,  11, True ) /* IgnoreCollisions */
     , (450169,  13, True ) /* Ethereal */
     , (450169,  14, True ) /* GravityStatus */
     , (450169,  19, True ) /* Attackable */
     , (450169,  22, True ) /* Inscribable */
     , (450169, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450169,   5,  -0.033) /* ManaRate */
     , (450169,  12,    0.66) /* Shade */
     , (450169,  13,     1.1) /* ArmorModVsSlash */
     , (450169,  14,     0.9) /* ArmorModVsPierce */
     , (450169,  15,     1.1) /* ArmorModVsBludgeon */
     , (450169,  16,     0.9) /* ArmorModVsCold */
     , (450169,  17,     0.9) /* ArmorModVsFire */
     , (450169,  18,     0.9) /* ArmorModVsAcid */
     , (450169,  19,     0.9) /* ArmorModVsElectric */
     , (450169, 110,    1.67) /* BulkMod */
     , (450169, 111,       1) /* SizeMod */
     , (450169, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450169,   1, 'Adept''s Fervor') /* Name */
     , (450169,  16, 'These gauntlets are built for mages, finely crafted from lightweight metals and put together with jeweler''s precision. A mage can easily manipulate objects and spell components as if wearing no gloves at all. Two large bloodstones help to serve as a magnifier for all life magic. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450169,   1, 0x02001383) /* Setup */
     , (450169,   3, 0x20000014) /* SoundTable */
     , (450169,   6, 0x0400007E) /* PaletteBase */
     , (450169,   7, 0x100005E6) /* ClothingBase */
     , (450169,   8, 0x06005C0D) /* Icon */
     , (450169,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450169,  36, 0x0E000012) /* MutateFilter */
     , (450169,  46, 0x38000032) /* TsysMutationFilter */
     , (450169,  52, 0x06005B0C) /* IconUnderlay */;


