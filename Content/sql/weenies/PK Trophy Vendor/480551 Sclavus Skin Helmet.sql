DELETE FROM `weenie` WHERE `class_Id` = 480551;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480551, 'helmetsclavuspk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480551,   1,          2) /* ItemType - Armor */
     , (480551,   3,          8) /* PaletteTemplate - Green */
     , (480551,   4,      16384) /* ClothingPriority - Head */
     , (480551,   5,        0) /* EncumbranceVal */
     , (480551,   8,        125) /* Mass */
     , (480551,   9,          1) /* ValidLocations - HeadWear */
     , (480551,  16,          1) /* ItemUseable - No */
     , (480551,  19,       20) /* Value */
     , (480551,  27,          4) /* ArmorType - StuddedLeather */
     , (480551,  28,        0) /* ArmorLevel */
     , (480551,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480551, 150,        103) /* HookPlacement - Hook */
     , (480551, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480551,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480551,  12,     0.5) /* Shade */
     , (480551,  13,     1.4) /* ArmorModVsSlash */
     , (480551,  14,       1) /* ArmorModVsPierce */
     , (480551,  15,     0.7) /* ArmorModVsBludgeon */
     , (480551,  16,     1.4) /* ArmorModVsCold */
     , (480551,  17,       1) /* ArmorModVsFire */
     , (480551,  18,     0.5) /* ArmorModVsAcid */
     , (480551,  19,     0.5) /* ArmorModVsElectric */
     , (480551, 110,       1) /* BulkMod */
     , (480551, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480551,   1, 'Sclavus Skin Helmet') /* Name */
     , (480551,  16, 'A helmet reinforced by the skin shed from a potent Sclavus.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480551,   1, 0x02000A00) /* Setup */
     , (480551,   3, 0x20000014) /* SoundTable */
     , (480551,   6, 0x0400007E) /* PaletteBase */
     , (480551,   7, 0x100002B3) /* ClothingBase */
     , (480551,   8, 0x06001353) /* Icon */
     , (480551,  22, 0x3400002B) /* PhysicsEffectTable */;
