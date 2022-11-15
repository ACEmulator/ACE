DELETE FROM `weenie` WHERE `class_Id` = 450501;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450501, 'ace450501-uberpenguinmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450501,   1,          4) /* ItemType - Armor */
     , (450501,   3,          4) /* PaletteTemplate - Brown */
     , (450501,   4,      16384) /* ClothingPriority - Head */
     , (450501,   5,        0) /* EncumbranceVal */
     , (450501,   8,         75) /* Mass */
     , (450501,   9,          1) /* ValidLocations - HeadWear */
     , (450501,  16,          1) /* ItemUseable - No */
     , (450501,  19,        20) /* Value */
     , (450501,  27,          2) /* ArmorType - Leather */
     , (450501,  28,         0) /* ArmorLevel */
     , (450501,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450501, 150,        103) /* HookPlacement - Hook */
     , (450501, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450501,  11, True ) /* IgnoreCollisions */
     , (450501,  13, True ) /* Ethereal */
     , (450501,  14, True ) /* GravityStatus */
     , (450501,  19, True ) /* Attackable */
     , (450501,  22, True ) /* Inscribable */
     , (450501,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450501,  12,    0.66) /* Shade */
     , (450501,  13,     0.5) /* ArmorModVsSlash */
     , (450501,  14,     0.4) /* ArmorModVsPierce */
     , (450501,  15,     0.4) /* ArmorModVsBludgeon */
     , (450501,  16,     0.6) /* ArmorModVsCold */
     , (450501,  17,     0.2) /* ArmorModVsFire */
     , (450501,  18,    0.75) /* ArmorModVsAcid */
     , (450501,  19,    0.35) /* ArmorModVsElectric */
     , (450501, 110,       1) /* BulkMod */
     , (450501, 111,       1) /* SizeMod */
     , (450501, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450501,   1, 'Uber Penguin Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450501,   1, 0x020014D8) /* Setup */
     , (450501,   3, 0x20000014) /* SoundTable */
     , (450501,   6, 0x0400007E) /* PaletteBase */
     , (450501,   7, 0x10000651) /* ClothingBase */
     , (450501,   8, 0x06006260) /* Icon */
     , (450501,  22, 0x3400002B) /* PhysicsEffectTable */;
