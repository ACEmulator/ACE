DELETE FROM `weenie` WHERE `class_Id` = 450163;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450163, 'bootsrarereinforcedtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450163,   1,          2) /* ItemType - Armor */
     , (450163,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450163,   4,      65536) /* ClothingPriority - Feet */
     , (450163,   5,        0) /* EncumbranceVal */
     , (450163,   8,         90) /* Mass */
     , (450163,   9,        256) /* ValidLocations - FootWear */
     , (450163,  16,          1) /* ItemUseable - No */
     , (450163,  17,        269) /* RareId */
     , (450163,  19,      20) /* Value */
     , (450163,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450163,  27,          2) /* ArmorType - Leather */
     , (450163,  28,        0) /* ArmorLevel */
     , (450163,  44,          6) /* Damage */
     , (450163,  45,          4) /* DamageType - Bludgeon */
     , (450163,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450163,  22, True ) /* Inscribable */
     , (450163, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450163,   5,  -0.033) /* ManaRate */
     , (450163,  13,     1.3) /* ArmorModVsSlash */
     , (450163,  14,     0.9) /* ArmorModVsPierce */
     , (450163,  15,     1.3) /* ArmorModVsBludgeon */
     , (450163,  16,       1) /* ArmorModVsCold */
     , (450163,  17,     0.9) /* ArmorModVsFire */
     , (450163,  18,     0.9) /* ArmorModVsAcid */
     , (450163,  19,     0.9) /* ArmorModVsElectric */
     , (450163, 110,    1.67) /* BulkMod */
     , (450163, 111,       1) /* SizeMod */
     , (450163, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450163,   1, 'Steel Wall Boots') /* Name */
     , (450163,  16, 'These leather boots have had small plates of metal built in for added protection. They also have heavy-duty armor built into the toes, making them formidable melee weapons for those whole like to kick.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450163,   1, 0x02001378) /* Setup */
     , (450163,   3, 0x20000014) /* SoundTable */
     , (450163,   6, 0x0400007E) /* PaletteBase */
     , (450163,   7, 0x100005E4) /* ClothingBase */
     , (450163,   8, 0x06005BEF) /* Icon */
     , (450163,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450163,  36, 0x0E000012) /* MutateFilter */
     , (450163,  46, 0x38000032) /* TsysMutationFilter */
     , (450163,  52, 0x06005B0C) /* IconUnderlay */;


