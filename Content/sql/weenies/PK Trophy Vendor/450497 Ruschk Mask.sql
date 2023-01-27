DELETE FROM `weenie` WHERE `class_Id` = 450497;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450497, 'ace450497-ruschkmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450497,   1,          4) /* ItemType - Armor */
     , (450497,   3,          4) /* PaletteTemplate - Brown */
     , (450497,   4,      16384) /* ClothingPriority - Head */
     , (450497,   5,        0) /* EncumbranceVal */
     , (450497,   9,          1) /* ValidLocations - HeadWear */
     , (450497,  16,          1) /* ItemUseable - No */
     , (450497,  19,        20) /* Value */
     , (450497,  28,         0) /* ArmorLevel */
     , (450497,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450497, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450497,   1, False) /* Stuck */
     , (450497,  11, True ) /* IgnoreCollisions */
     , (450497,  13, True ) /* Ethereal */
     , (450497,  14, True ) /* GravityStatus */
     , (450497,  19, True ) /* Attackable */
     , (450497,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450497,  12,       0) /* Shade */
     , (450497,  13,     0.5) /* ArmorModVsSlash */
     , (450497,  14,     0.4) /* ArmorModVsPierce */
     , (450497,  15,     0.4) /* ArmorModVsBludgeon */
     , (450497,  16,     0.6) /* ArmorModVsCold */
     , (450497,  17,     0.2) /* ArmorModVsFire */
     , (450497,  18,    0.75) /* ArmorModVsAcid */
     , (450497,  19,    0.35) /* ArmorModVsElectric */
     , (450497, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450497,   1, 'Ruschk Mask') /* Name */
     , (450497,  16, 'A fearsome mask made from the head of a barbaric Ruschk.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450497,   1, 0x020014D9) /* Setup */
     , (450497,   3, 0x20000014) /* SoundTable */
     , (450497,   7, 0x10000652) /* ClothingBase */
     , (450497,   8, 0x06006233) /* Icon */
     , (450497,  22, 0x3400002B) /* PhysicsEffectTable */;
