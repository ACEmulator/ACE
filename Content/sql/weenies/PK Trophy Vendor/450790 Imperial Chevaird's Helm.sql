DELETE FROM `weenie` WHERE `class_Id` = 450790;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450790, 'helmrareimperialchevairdpk', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450790,   1,          2) /* ItemType - Armor */
     , (450790,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450790,   4,      16384) /* ClothingPriority - Head */
     , (450790,   5,        0) /* EncumbranceVal */
     , (450790,   8,         90) /* Mass */
     , (450790,   9,          1) /* ValidLocations - HeadWear */
     , (450790,  16,          1) /* ItemUseable - No */
     , (450790,  19,      20) /* Value */
     , (450790,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450790,  27,          2) /* ArmorType - Leather */
     , (450790,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450790, 151,          2) /* HookType - Wall */;



INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450790,  11, True ) /* IgnoreCollisions */
     , (450790,  13, True ) /* Ethereal */
     , (450790,  14, True ) /* GravityStatus */
     , (450790,  19, True ) /* Attackable */
     , (450790,  22, True ) /* Inscribable */
     , (450790, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450790,   5,  -0.033) /* ManaRate */
     , (450790,  12,    0.66) /* Shade */
     , (450790,  13,     1.4) /* ArmorModVsSlash */
     , (450790,  14,     1.1) /* ArmorModVsPierce */
     , (450790,  15,     1.1) /* ArmorModVsBludgeon */
     , (450790,  16,     0.9) /* ArmorModVsCold */
     , (450790,  17,     0.9) /* ArmorModVsFire */
     , (450790,  18,     0.9) /* ArmorModVsAcid */
     , (450790,  19,     0.9) /* ArmorModVsElectric */
     , (450790, 110,    1.67) /* BulkMod */
     , (450790, 111,       1) /* SizeMod */
     , (450790, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450790,   1, 'Imperial Chevaird''s Helm') /* Name */
     , (450790,  16, 'The proudest warriors of the Yalain were the Imperial Chevairds. They were responsible for the King''s safety and were comprised of the kingdom''s most loyal and trusted warriors. These helms were specially designed for the Imperial Chevairds and were endowed with powerful magic.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450790,   1, 0x02001384) /* Setup */
     , (450790,   3, 0x20000014) /* SoundTable */
     , (450790,   6, 0x0400007E) /* PaletteBase */
     , (450790,   7, 0x100005E8) /* ClothingBase */
     , (450790,   8, 0x06005C12) /* Icon */
     , (450790,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450790,  36, 0x0E000012) /* MutateFilter */
     , (450790,  46, 0x38000032) /* TsysMutationFilter */
     , (450790,  52, 0x06005B0C) /* IconUnderlay */;


