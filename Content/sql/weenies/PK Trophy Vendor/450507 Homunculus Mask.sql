DELETE FROM `weenie` WHERE `class_Id` = 450507;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450507, 'ace450507-homunculusmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450507,   1,          4) /* ItemType - Armor */
     , (450507,   3,          4) /* PaletteTemplate - Brown */
     , (450507,   4,      16384) /* ClothingPriority - Head */
     , (450507,   5,        0) /* EncumbranceVal */
     , (450507,   9,          1) /* ValidLocations - HeadWear */
     , (450507,  16,          1) /* ItemUseable - No */
     , (450507,  19,        20) /* Value */
     , (450507,  28,         0) /* ArmorLevel */
     , (450507,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450507, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450507,   1, False) /* Stuck */
     , (450507,  11, True ) /* IgnoreCollisions */
     , (450507,  13, True ) /* Ethereal */
     , (450507,  14, True ) /* GravityStatus */
     , (450507,  19, True ) /* Attackable */
     , (450507,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450507,  12,       0) /* Shade */
     , (450507,  13,     0.5) /* ArmorModVsSlash */
     , (450507,  14,     0.4) /* ArmorModVsPierce */
     , (450507,  15,     0.4) /* ArmorModVsBludgeon */
     , (450507,  16,     0.6) /* ArmorModVsCold */
     , (450507,  17,     0.2) /* ArmorModVsFire */
     , (450507,  18,     0.8) /* ArmorModVsAcid */
     , (450507,  19,     0.3) /* ArmorModVsElectric */
     , (450507, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450507,   1, 'Homunculus Mask') /* Name */
     , (450507,  16, 'A mask crafted after the visage of the sinister Homunculus. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450507,   1, 0x020014D6) /* Setup */
     , (450507,   3, 0x20000014) /* SoundTable */
     , (450507,   7, 0x1000064F) /* ClothingBase */
     , (450507,   8, 0x06006232) /* Icon */
     , (450507,  22, 0x3400002B) /* PhysicsEffectTable */;
