DELETE FROM `weenie` WHERE `class_Id` = 450170;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450170, 'helmrareimperialchevairdtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450170,   1,          2) /* ItemType - Armor */
     , (450170,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450170,   4,      16384) /* ClothingPriority - Head */
     , (450170,   5,        0) /* EncumbranceVal */
     , (450170,   8,         90) /* Mass */
     , (450170,   9,          1) /* ValidLocations - HeadWear */
     , (450170,  16,          1) /* ItemUseable - No */
     , (450170,  19,      20) /* Value */
     , (450170,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450170,  27,          2) /* ArmorType - Leather */
     , (450170,  28,        0) /* ArmorLevel */
     , (450170,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450170, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450170,  11, True ) /* IgnoreCollisions */
     , (450170,  13, True ) /* Ethereal */
     , (450170,  14, True ) /* GravityStatus */
     , (450170,  19, True ) /* Attackable */
     , (450170,  22, True ) /* Inscribable */
     , (450170, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450170,   5,  -0.033) /* ManaRate */
     , (450170,  12,    0.66) /* Shade */
     , (450170,  13,     1.4) /* ArmorModVsSlash */
     , (450170,  14,     1.1) /* ArmorModVsPierce */
     , (450170,  15,     1.1) /* ArmorModVsBludgeon */
     , (450170,  16,     0.9) /* ArmorModVsCold */
     , (450170,  17,     0.9) /* ArmorModVsFire */
     , (450170,  18,     0.9) /* ArmorModVsAcid */
     , (450170,  19,     0.9) /* ArmorModVsElectric */
     , (450170, 110,    1.67) /* BulkMod */
     , (450170, 111,       1) /* SizeMod */
     , (450170, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450170,   1, 'Imperial Chevaird''s Helm') /* Name */
     , (450170,  16, 'The proudest warriors of the Yalain were the Imperial Chevairds. They were responsible for the King''s safety and were comprised of the kingdom''s most loyal and trusted warriors. These helms were specially designed for the Imperial Chevairds and were endowed with powerful magic.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450170,   1, 0x02001384) /* Setup */
     , (450170,   3, 0x20000014) /* SoundTable */
     , (450170,   6, 0x0400007E) /* PaletteBase */
     , (450170,   7, 0x100005E8) /* ClothingBase */
     , (450170,   8, 0x06005C12) /* Icon */
     , (450170,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450170,  36, 0x0E000012) /* MutateFilter */
     , (450170,  46, 0x38000032) /* TsysMutationFilter */
     , (450170,  52, 0x06005B0C) /* IconUnderlay */;
