DELETE FROM `weenie` WHERE `class_Id` = 450162;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450162, 'bootsrarefootmantailor2', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450162,   1,          2) /* ItemType - Armor */
     , (450162,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450162,   4,      65536) /* ClothingPriority - Feet */
     , (450162,   5,        0) /* EncumbranceVal */
     , (450162,   8,         90) /* Mass */
     , (450162,   9,        256) /* ValidLocations - FootWear */
     , (450162,  16,          1) /* ItemUseable - No */
     , (450162,  17,        226) /* RareId */
     , (450162,  19,      20) /* Value */
     , (450162,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450162,  27,          2) /* ArmorType - Leather */
     , (450162,  28,        0) /* ArmorLevel */
     , (450162,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450162,  11, True ) /* IgnoreCollisions */
     , (450162,  13, True ) /* Ethereal */
     , (450162,  14, True ) /* GravityStatus */
     , (450162,  19, True ) /* Attackable */
     , (450162,  22, True ) /* Inscribable */
     , (450162, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450162,   5,  -0.033) /* ManaRate */
     , (450162,  12,    0.66) /* Shade */
     , (450162,  13,     1.3) /* ArmorModVsSlash */
     , (450162,  14,     0.9) /* ArmorModVsPierce */
     , (450162,  15,     1.1) /* ArmorModVsBludgeon */
     , (450162,  16,     0.9) /* ArmorModVsCold */
     , (450162,  17,     0.9) /* ArmorModVsFire */
     , (450162,  18,     0.9) /* ArmorModVsAcid */
     , (450162,  19,       1) /* ArmorModVsElectric */
     , (450162, 110,    1.67) /* BulkMod */
     , (450162, 111,       1) /* SizeMod */
     , (450162, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450162,   1, 'Footman''s Boots') /* Name */
     , (450162,  16, 'Any old campaigner will tell you that while top-quality weapons and armor are very desirable, a foot soldiers best friend is a good pair of boots. This pair of boots belonged to a soldier in the Vanguard Company of the Renari Lancers in the Viamontian army, a unit that had the distinction of serving in every single battle of every single campaign for twenty years. A quartermaster attached to that unit estimated that they had marched far enough in their travels to circle Ispar twice.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450162,   1, 0x02001393) /* Setup */
     , (450162,   3, 0x20000014) /* SoundTable */
     , (450162,   6, 0x0400007E) /* PaletteBase */
     , (450162,   7, 0x100005E3) /* ClothingBase */
     , (450162,   8, 0x06005C34) /* Icon */
     , (450162,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450162,  36, 0x0E000012) /* MutateFilter */
     , (450162,  46, 0x38000032) /* TsysMutationFilter */
     , (450162,  52, 0x06005B0C) /* IconUnderlay */;
