DELETE FROM `weenie` WHERE `class_Id` = 450173;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450173, 'breastplaterareleikothatailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450173,   1,          2) /* ItemType - Armor */
     , (450173,   3,          4) /* PaletteTemplate - Brown */
     , (450173,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450173,   5,       0) /* EncumbranceVal */
     , (450173,   8,         90) /* Mass */
     , (450173,   9,        512) /* ValidLocations - ChestArmor */
     , (450173,  16,          1) /* ItemUseable - No */
     , (450173,  17,        217) /* RareId */
     , (450173,  19,      20) /* Value */
     , (450173,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450173,  27,          2) /* ArmorType - Leather */
     , (450173,  28,        0) /* ArmorLevel */
     , (450173,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450173, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450173,  11, True ) /* IgnoreCollisions */
     , (450173,  13, True ) /* Ethereal */
     , (450173,  14, True ) /* GravityStatus */
     , (450173,  19, True ) /* Attackable */
     , (450173,  22, True ) /* Inscribable */
     , (450173,  91, False) /* Retained */
     , (450173, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450173,   5,  -0.033) /* ManaRate */
     , (450173,  12,    0.66) /* Shade */
     , (450173,  13,     1.4) /* ArmorModVsSlash */
     , (450173,  14,     1.1) /* ArmorModVsPierce */
     , (450173,  15,     1.1) /* ArmorModVsBludgeon */
     , (450173,  16,       1) /* ArmorModVsCold */
     , (450173,  17,     0.9) /* ArmorModVsFire */
     , (450173,  18,     0.9) /* ArmorModVsAcid */
     , (450173,  19,     0.9) /* ArmorModVsElectric */
     , (450173, 110,    1.67) /* BulkMod */
     , (450173, 111,       1) /* SizeMod */
     , (450173, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450173,   1, 'Breastplate of Leikotha''s Tears') /* Name */
     , (450173,  16, 'Can the undead cry? It is said that after Leikotha, the great warrior of Haebrous, was made undead by the Sand King Nerash, she wept for thirty days and thirty nights. Each tear shed fell onto her armor, infusing Leikotha''s essence into each piece. Courage, honor, sorrow, wrath and... everlasting death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450173,   1, 0x0200138C) /* Setup */
     , (450173,   3, 0x20000014) /* SoundTable */
     , (450173,   6, 0x0400007E) /* PaletteBase */
     , (450173,   7, 0x100005F0) /* ClothingBase */
     , (450173,   8, 0x06005C22) /* Icon */
     , (450173,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450173,  36, 0x0E000012) /* MutateFilter */
     , (450173,  46, 0x38000032) /* TsysMutationFilter */
     , (450173,  52, 0x06005B0C) /* IconUnderlay */;
