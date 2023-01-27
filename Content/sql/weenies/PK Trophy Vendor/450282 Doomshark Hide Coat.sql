DELETE FROM `weenie` WHERE `class_Id` = 450282;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450282, 'coatdoomsharktsilot', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450282,   1,          2) /* ItemType - Armor */
     , (450282,   3,          4) /* PaletteTemplate - Brown */
     , (450282,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms, Head */
     , (450282,   5,       0) /* EncumbranceVal */
     , (450282,   8,        270) /* Mass */
     , (450282,   9,       512) /* ValidLocations - HeadWear, ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450282,  16,          1) /* ItemUseable - No */
     , (450282,  19,       20) /* Value */
     , (450282,  27,          2) /* ArmorType - Leather */
     , (450282,  28,        0) /* ArmorLevel */
     , (450282,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450282, 150,        103) /* HookPlacement - Hook */
     , (450282, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450282,  22, True ) /* Inscribable */
     , (450282, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450282,   5,  -0.033) /* ManaRate */
     , (450282,  12,    0.66) /* Shade */
     , (450282,  13,     0.5) /* ArmorModVsSlash */
     , (450282,  14,    0.75) /* ArmorModVsPierce */
     , (450282,  15,     0.6) /* ArmorModVsBludgeon */
     , (450282,  16,     0.4) /* ArmorModVsCold */
     , (450282,  17,    1.55) /* ArmorModVsFire */
     , (450282,  18,    1.05) /* ArmorModVsAcid */
     , (450282,  19,     0.4) /* ArmorModVsElectric */
     , (450282, 110,       1) /* BulkMod */
     , (450282, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450282,   1, 'Doomshark Hide Coat') /* Name */
     , (450282,  16, 'A hooded coat crafted from the hide of a doomshark.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450282,   1, 0x020000D4) /* Setup */
     , (450282,   3, 0x20000014) /* SoundTable */
     , (450282,   6, 0x0400007E) /* PaletteBase */
     , (450282,   7, 0x10000514) /* ClothingBase */
     , (450282,   8, 0x06003028) /* Icon */
     , (450282,  22, 0x3400002B) /* PhysicsEffectTable */;

