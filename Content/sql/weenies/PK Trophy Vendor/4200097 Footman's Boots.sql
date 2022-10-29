DELETE FROM `weenie` WHERE `class_Id` = 4200097;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200097, 'bootsrarefootmantailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200097,   1,          2) /* ItemType - Armor */
     , (4200097,   3,          1) /* PaletteTemplate - AquaBlue */
     , (4200097,   4,      65536) /* ClothingPriority - Feet */
     , (4200097,   5,          1) /* EncumbranceVal */
     , (4200097,   8,         90) /* Mass */
     , (4200097,   9,        256) /* ValidLocations - FootWear */
     , (4200097,  16,          1) /* ItemUseable - No */
     , (4200097,  19,         20) /* Value */
     , (4200097,  27,          2) /* ArmorType - Leather */
     , (4200097,  28,          1) /* ArmorLevel */
     , (4200097,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200097, 151,          2) /* HookType - Wall */
     , (4200097, 169,  118162702) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200097,  11, True ) /* IgnoreCollisions */
     , (4200097,  13, True ) /* Ethereal */
     , (4200097,  14, True ) /* GravityStatus */
     , (4200097,  19, True ) /* Attackable */
     , (4200097,  22, True ) /* Inscribable */
     , (4200097, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200097,   5,  -0.033) /* ManaRate */
     , (4200097,  12,    0.66) /* Shade */
     , (4200097,  13,     1.3) /* ArmorModVsSlash */
     , (4200097,  14,     0.9) /* ArmorModVsPierce */
     , (4200097,  15,     1.1) /* ArmorModVsBludgeon */
     , (4200097,  16,     0.9) /* ArmorModVsCold */
     , (4200097,  17,     0.9) /* ArmorModVsFire */
     , (4200097,  18,     0.9) /* ArmorModVsAcid */
     , (4200097,  19,       1) /* ArmorModVsElectric */
     , (4200097, 110,    1.67) /* BulkMod */
     , (4200097, 111,       1) /* SizeMod */
     , (4200097, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200097,   1, 'Footman''s Boots') /* Name */
     , (4200097,  16, 'Any old campaigner will tell you that while top-quality weapons and armor are very desirable, a foot soldiers best friend is a good pair of boots. This pair of boots belonged to a soldier in the Vanguard Company of the Renari Lancers in the Viamontian army, a unit that had the distinction of serving in every single battle of every single campaign for twenty years. A quartermaster attached to that unit estimated that they had marched far enough in their travels to circle Ispar twice.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200097,   1, 0x02001393) /* Setup */
     , (4200097,   3, 0x20000014) /* SoundTable */
     , (4200097,   6, 0x0400007E) /* PaletteBase */
     , (4200097,   7, 0x100005E3) /* ClothingBase */
     , (4200097,   8, 0x06005C34) /* Icon */
     , (4200097,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200097,  36, 0x0E000012) /* MutateFilter */
     , (4200097,  46, 0x38000032) /* TsysMutationFilter */
     , (4200097,  52, 0x06005B0C) /* IconUnderlay */;

