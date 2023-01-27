DELETE FROM `weenie` WHERE `class_Id` = 450506;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450506, 'ace450506-twoheadedsnowmanmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450506,   1,          4) /* ItemType - Armor */
     , (450506,   3,          4) /* PaletteTemplate - Brown */
     , (450506,   4,      16384) /* ClothingPriority - Head */
     , (450506,   5,        0) /* EncumbranceVal */
     , (450506,   9,          1) /* ValidLocations - HeadWear */
     , (450506,  16,          1) /* ItemUseable - No */
     , (450506,  19,        20) /* Value */
     , (450506,  28,         0) /* ArmorLevel */
     , (450506,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450506, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450506,   1, False) /* Stuck */
     , (450506,  11, True ) /* IgnoreCollisions */
     , (450506,  13, True ) /* Ethereal */
     , (450506,  14, True ) /* GravityStatus */
     , (450506,  19, True ) /* Attackable */
     , (450506,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450506,  12,       0) /* Shade */
     , (450506,  13,     0.5) /* ArmorModVsSlash */
     , (450506,  14,     0.4) /* ArmorModVsPierce */
     , (450506,  15,     0.4) /* ArmorModVsBludgeon */
     , (450506,  16,     0.6) /* ArmorModVsCold */
     , (450506,  17,     0.2) /* ArmorModVsFire */
     , (450506,  18,    0.75) /* ArmorModVsAcid */
     , (450506,  19,    0.35) /* ArmorModVsElectric */
     , (450506, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450506,   1, 'Two Headed Snowman Mask') /* Name */
     , (450506,  16, 'A mask crafted from the hollowed-out heads of a Two Headed Snowman.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450506,   1, 0x020014DB) /* Setup */
     , (450506,   3, 0x20000014) /* SoundTable */
     , (450506,   7, 0x10000654) /* ClothingBase */
     , (450506,   8, 0x0600622F) /* Icon */
     , (450506,  22, 0x3400002B) /* PhysicsEffectTable */;
