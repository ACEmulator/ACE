DELETE FROM `weenie` WHERE `class_Id` = 450165;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450165, 'leggingsraredusktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450165,   1,          2) /* ItemType - Armor */
     , (450165,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450165,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450165,   5,        0) /* EncumbranceVal */
     , (450165,   8,         90) /* Mass */
     , (450165,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450165,  16,          1) /* ItemUseable - No */
     , (450165,  17,        268) /* RareId */
     , (450165,  19,      20) /* Value */
     , (450165,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450165,  27,          2) /* ArmorType - Leather */
     , (450165,  28,        0) /* ArmorLevel */
     , (450165,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450165, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450165,  11, True ) /* IgnoreCollisions */
     , (450165,  13, True ) /* Ethereal */
     , (450165,  14, True ) /* GravityStatus */
     , (450165,  19, True ) /* Attackable */
     , (450165,  22, True ) /* Inscribable */
     , (450165, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450165,   5,  -0.033) /* ManaRate */
     , (450165,  12,    0.66) /* Shade */
     , (450165,  13,     1.1) /* ArmorModVsSlash */
     , (450165,  14,     1.1) /* ArmorModVsPierce */
     , (450165,  15,     1.2) /* ArmorModVsBludgeon */
     , (450165,  16,     1.1) /* ArmorModVsCold */
     , (450165,  17,     1.2) /* ArmorModVsFire */
     , (450165,  18,     1.3) /* ArmorModVsAcid */
     , (450165,  19,       1) /* ArmorModVsElectric */
     , (450165, 110,    1.67) /* BulkMod */
     , (450165, 111,       1) /* SizeMod */
     , (450165, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450165,   1, 'Dusk Leggings') /* Name */
     , (450165,  16, 'It is said that every great craftsman has a moment of inspiration, If only for a short period of time, they are possessed by a divine spirit, and they are able to create as object of such beauty and quality that they can never in thier lifetime hope to surpass. These leggings, along with the Dusk Coat, are Leyrale Sharlorn''s master work. the great tailor hung up his needle and thread after finishing the set, sold them to a wealthy nobleman, and retired to a life of fishing') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450165,   1, 0x02001394) /* Setup */
     , (450165,   3, 0x20000014) /* SoundTable */
     , (450165,   6, 0x0400007E) /* PaletteBase */
     , (450165,   7, 0x100005FA) /* ClothingBase */
     , (450165,   8, 0x06005C36) /* Icon */
     , (450165,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450165,  36, 0x0E000012) /* MutateFilter */
     , (450165,  46, 0x38000032) /* TsysMutationFilter */
     , (450165,  52, 0x06005B0C) /* IconUnderlay */;


