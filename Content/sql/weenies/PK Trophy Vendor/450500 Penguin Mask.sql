DELETE FROM `weenie` WHERE `class_Id` = 450500;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450500, 'ace450500-penguinmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450500,   1,          4) /* ItemType - Armor */
     , (450500,   3,          4) /* PaletteTemplate - Brown */
     , (450500,   4,      16384) /* ClothingPriority - Head */
     , (450500,   5,        0) /* EncumbranceVal */
     , (450500,   9,          1) /* ValidLocations - HeadWear */
     , (450500,  16,          1) /* ItemUseable - No */
     , (450500,  19,        20) /* Value */
     , (450500,  28,         0) /* ArmorLevel */
     , (450500,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450500, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450500,   1, False) /* Stuck */
     , (450500,  11, True ) /* IgnoreCollisions */
     , (450500,  13, True ) /* Ethereal */
     , (450500,  14, True ) /* GravityStatus */
     , (450500,  19, True ) /* Attackable */
     , (450500,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450500,  12,       0) /* Shade */
     , (450500,  13,     0.5) /* ArmorModVsSlash */
     , (450500,  14,     0.4) /* ArmorModVsPierce */
     , (450500,  15,     0.4) /* ArmorModVsBludgeon */
     , (450500,  16,     0.6) /* ArmorModVsCold */
     , (450500,  17,     0.2) /* ArmorModVsFire */
     , (450500,  18,    0.75) /* ArmorModVsAcid */
     , (450500,  19,    0.35) /* ArmorModVsElectric */
     , (450500, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450500,   1, 'Penguin Mask') /* Name */
     , (450500,  16, 'A mask crafted to resemble the head of the noble Penguin.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450500,   1, 0x020014D7) /* Setup */
     , (450500,   3, 0x20000014) /* SoundTable */
     , (450500,   7, 0x10000650) /* ClothingBase */
     , (450500,   8, 0x0600625F) /* Icon */
     , (450500,  22, 0x3400002B) /* PhysicsEffectTable */;
