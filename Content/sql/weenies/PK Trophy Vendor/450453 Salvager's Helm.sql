DELETE FROM `weenie` WHERE `class_Id` = 450453;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450453, 'helmsalvagingboss0205tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450453,   1,          2) /* ItemType - Armor */
     , (450453,   3,         14) /* PaletteTemplate - Red */
     , (450453,   4,      16384) /* ClothingPriority - Head */
     , (450453,   5,        0) /* EncumbranceVal */
     , (450453,   8,        200) /* Mass */
     , (450453,   9,          1) /* ValidLocations - HeadWear */
     , (450453,  16,          1) /* ItemUseable - No */
     , (450453,  19,      20) /* Value */
     , (450453,  27,         32) /* ArmorType - Metal */
     , (450453,  28,         0) /* ArmorLevel */
     , (450453,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450453, 150,        103) /* HookPlacement - Hook */
     , (450453, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450453,  22, True ) /* Inscribable */
     , (450453,  23, True ) /* DestroyOnSell */
     , (450453, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450453,   5,  -0.033) /* ManaRate */
     , (450453,  12,    0.66) /* Shade */
     , (450453,  13,     0.2) /* ArmorModVsSlash */
     , (450453,  14,     0.2) /* ArmorModVsPierce */
     , (450453,  15,     0.2) /* ArmorModVsBludgeon */
     , (450453,  16,     0.2) /* ArmorModVsCold */
     , (450453,  17,     0.2) /* ArmorModVsFire */
     , (450453,  18,     0.2) /* ArmorModVsAcid */
     , (450453,  19,     0.2) /* ArmorModVsElectric */
     , (450453, 110,       1) /* BulkMod */
     , (450453, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450453,   1, 'Salvager''s Helm') /* Name */
     , (450453,  33, 'BossHelmAcquired0205') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450453,   1, 0x02000330) /* Setup */
     , (450453,   3, 0x20000014) /* SoundTable */
     , (450453,   6, 0x0400007E) /* PaletteBase */
     , (450453,   7, 0x100000AD) /* ClothingBase */
     , (450453,   8, 0x06000FCF) /* Icon */
     , (450453,  22, 0x3400002B) /* PhysicsEffectTable */;

