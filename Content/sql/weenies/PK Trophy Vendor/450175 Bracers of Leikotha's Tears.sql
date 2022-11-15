DELETE FROM `weenie` WHERE `class_Id` = 450175;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450175, 'bracersrareleikothatailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450175,   1,          2) /* ItemType - Armor */
     , (450175,   3,          4) /* PaletteTemplate - Brown */
     , (450175,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (450175,   5,        0) /* EncumbranceVal */
     , (450175,   8,         90) /* Mass */
     , (450175,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (450175,  16,          1) /* ItemUseable - No */
     , (450175,  19,      20) /* Value */
     , (450175,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450175,  27,          2) /* ArmorType - Leather */
     , (450175,  28,        0) /* ArmorLevel */
     , (450175,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450175, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450175,  11, True ) /* IgnoreCollisions */
     , (450175,  13, True ) /* Ethereal */
     , (450175,  14, True ) /* GravityStatus */
     , (450175,  19, True ) /* Attackable */
     , (450175,  22, True ) /* Inscribable */
     , (450175,  91, False) /* Retained */
     , (450175, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450175,   5,  -0.033) /* ManaRate */
     , (450175,  12,    0.66) /* Shade */
     , (450175,  13,     1.3) /* ArmorModVsSlash */
     , (450175,  14,     1.1) /* ArmorModVsPierce */
     , (450175,  15,     1.1) /* ArmorModVsBludgeon */
     , (450175,  16,     0.9) /* ArmorModVsCold */
     , (450175,  17,     0.9) /* ArmorModVsFire */
     , (450175,  18,     0.9) /* ArmorModVsAcid */
     , (450175,  19,       1) /* ArmorModVsElectric */
     , (450175, 110,    1.67) /* BulkMod */
     , (450175, 111,       1) /* SizeMod */
     , (450175, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450175,   1, 'Bracers of Leikotha''s Tears') /* Name */
     , (450175,  16, 'Can the undead cry? It is said that after Leikotha, the great warrior of Haebrous, was made undead by the Sand King Nerash, she wept for thirty days and thirty nights. Each tear shed fell onto her armor, infusing Leikotha''s essence into each piece. Courage, honor, sorrow, wrath and... everlasting death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450175,   1, 0x0200138F) /* Setup */
     , (450175,   3, 0x20000014) /* SoundTable */
     , (450175,   6, 0x0400007E) /* PaletteBase */
     , (450175,   7, 0x100005F3) /* ClothingBase */
     , (450175,   8, 0x06005C28) /* Icon */
     , (450175,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450175,  36, 0x0E000012) /* MutateFilter */
     , (450175,  46, 0x38000032) /* TsysMutationFilter */
     , (450175,  52, 0x06005B0C) /* IconUnderlay */;

