DELETE FROM `weenie` WHERE `class_Id` = 450168;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450168, 'helmrarevalkeertailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450168,   1,          2) /* ItemType - Armor */
     , (450168,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450168,   4,      16384) /* ClothingPriority - Head */
     , (450168,   5,        0) /* EncumbranceVal */
     , (450168,   8,         90) /* Mass */
     , (450168,   9,          1) /* ValidLocations - HeadWear */
     , (450168,  16,          1) /* ItemUseable - No */
     , (450168,  19,      20) /* Value */
     , (450168,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450168,  27,          2) /* ArmorType - Leather */
     , (450168,  28,        0) /* ArmorLevel */
     , (450168,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450168, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450168,  11, True ) /* IgnoreCollisions */
     , (450168,  13, True ) /* Ethereal */
     , (450168,  14, True ) /* GravityStatus */
     , (450168,  19, True ) /* Attackable */
     , (450168,  22, True ) /* Inscribable */
     , (450168, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450168,   5,  -0.033) /* ManaRate */
     , (450168,  12,    0.66) /* Shade */
     , (450168,  13,       1) /* ArmorModVsSlash */
     , (450168,  14,     1.1) /* ArmorModVsPierce */
     , (450168,  15,       1) /* ArmorModVsBludgeon */
     , (450168,  16,     0.9) /* ArmorModVsCold */
     , (450168,  17,     0.9) /* ArmorModVsFire */
     , (450168,  18,     0.9) /* ArmorModVsAcid */
     , (450168,  19,     0.9) /* ArmorModVsElectric */
     , (450168, 110,    1.67) /* BulkMod */
     , (450168, 111,       1) /* SizeMod */
     , (450168, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450168,   1, 'Valkeer''s Helm') /* Name */
     , (450168,  16, 'Helms of this type are one of the signature pieces of armor of elite groups of Silveran warriors that called themselves the Valkeer. At first glance this helm would seem to be solely an ornamental piece. Delicate carvings cover the thin, almost fragile surface of the helm. However, its delicate nature belies its true strength.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450168,   1, 0x02001384) /* Setup */
     , (450168,   3, 0x20000014) /* SoundTable */
     , (450168,   6, 0x0400007E) /* PaletteBase */
     , (450168,   7, 0x100005F7) /* ClothingBase */
     , (450168,   8, 0x06005C31) /* Icon */
     , (450168,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450168,  36, 0x0E000012) /* MutateFilter */
     , (450168,  46, 0x38000032) /* TsysMutationFilter */
     , (450168,  52, 0x06005B0C) /* IconUnderlay */;

