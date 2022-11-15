DELETE FROM `weenie` WHERE `class_Id` = 450480;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450480, 'helmbutchertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450480,   1,          2) /* ItemType - Armor */
     , (450480,   3,          4) /* PaletteTemplate - Brown */
     , (450480,   4,      16384) /* ClothingPriority - Head */
     , (450480,   5,        0) /* EncumbranceVal */
     , (450480,   8,        150) /* Mass */
     , (450480,   9,          1) /* ValidLocations - HeadWear */
     , (450480,  16,          1) /* ItemUseable - No */
     , (450480,  19,       20) /* Value */
     , (450480,  27,         32) /* ArmorType - Metal */
     , (450480,  28,        0) /* ArmorLevel */
     , (450480,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450480, 150,        103) /* HookPlacement - Hook */
     , (450480, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450480,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450480,  12,     0.3) /* Shade */
     , (450480,  13,     1.1) /* ArmorModVsSlash */
     , (450480,  14,     0.8) /* ArmorModVsPierce */
     , (450480,  15,     0.8) /* ArmorModVsBludgeon */
     , (450480,  16,       1) /* ArmorModVsCold */
     , (450480,  17,     1.1) /* ArmorModVsFire */
     , (450480,  18,     1.1) /* ArmorModVsAcid */
     , (450480,  19,     1.1) /* ArmorModVsElectric */
     , (450480, 110,       1) /* BulkMod */
     , (450480, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450480,   1, 'Olthoi Helm') /* Name */
     , (450480,  15, 'A helm crafted from the head of an olthoi Eviscerator.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450480,   1, 0x02000E09) /* Setup */
     , (450480,   3, 0x20000014) /* SoundTable */
     , (450480,   6, 0x0400007E) /* PaletteBase */
     , (450480,   7, 0x100003FF) /* ClothingBase */
     , (450480,   8, 0x06002889) /* Icon */
     , (450480,  22, 0x3400002B) /* PhysicsEffectTable */;
