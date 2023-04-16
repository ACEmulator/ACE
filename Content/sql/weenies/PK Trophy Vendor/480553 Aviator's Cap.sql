DELETE FROM `weenie` WHERE `class_Id` = 480553;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480553, 'aviatorscappk', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480553,   1,          4) /* ItemType - Armor */
     , (480553,   3,          4) /* PaletteTemplate - Brown */
     , (480553,   4,      16384) /* ClothingPriority - Head */
     , (480553,   5,        0) /* EncumbranceVal */
     , (480553,   8,        200) /* Mass */
     , (480553,   9,          1) /* ValidLocations - HeadWear */
     , (480553,  16,          1) /* ItemUseable - No */
     , (480553,  19,       20) /* Value */
     , (480553,  27,         32) /* ArmorType - Metal */
     , (480553,  28,        0) /* ArmorLevel */
     , (480553,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480553, 150,        103) /* HookPlacement - Hook */
     , (480553, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480553,  11, True ) /* IgnoreCollisions */
     , (480553,  13, True ) /* Ethereal */
     , (480553,  14, True ) /* GravityStatus */
     , (480553,  19, True ) /* Attackable */
     , (480553,  22, True ) /* Inscribable */
     , (480553,  23, True ) /* DestroyOnSell */
     , (480553,  99, True ) /* Ivoryable */
     , (480553, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480553,   5,  -0.033) /* ManaRate */
     , (480553,  12,    0.66) /* Shade */
     , (480553,  13,       1) /* ArmorModVsSlash */
     , (480553,  14,     0.8) /* ArmorModVsPierce */
     , (480553,  15,       1) /* ArmorModVsBludgeon */
     , (480553,  16,     0.8) /* ArmorModVsCold */
     , (480553,  17,     0.5) /* ArmorModVsFire */
     , (480553,  18,     0.5) /* ArmorModVsAcid */
     , (480553,  19,     0.7) /* ArmorModVsElectric */
     , (480553, 110,       1) /* BulkMod */
     , (480553, 111,       1) /* SizeMod */
     , (480553, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480553,   1, 'Aviator''s Cap') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480553,   1, 0x020012AB) /* Setup */
     , (480553,   3, 0x20000014) /* SoundTable */
     , (480553,   6, 0x0400007E) /* PaletteBase */
     , (480553,   7, 0x1000059E) /* ClothingBase */
     , (480553,   8, 0x06003710) /* Icon */
     , (480553,  22, 0x3400002B) /* PhysicsEffectTable */;

