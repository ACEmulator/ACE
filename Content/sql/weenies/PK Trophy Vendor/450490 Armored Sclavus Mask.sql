DELETE FROM `weenie` WHERE `class_Id` = 450490;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450490, 'ace450490-armoredsclavusmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450490,   1,          4) /* ItemType - Armor */
     , (450490,   3,          4) /* PaletteTemplate - Brown */
     , (450490,   4,      16384) /* ClothingPriority - Head */
     , (450490,   5,        0) /* EncumbranceVal */
     , (450490,   9,          1) /* ValidLocations - HeadWear */
     , (450490,  19,        20) /* Value */
     , (450490,  28,         0) /* ArmorLevel */
     , (450490, 107,          0) /* ItemCurMana */
     , (450490, 108,          0) /* ItemMaxMana */
     , (450490, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450490,  11, True ) /* IgnoreCollisions */
     , (450490,  13, True ) /* Ethereal */
     , (450490,  14, True ) /* GravityStatus */
     , (450490,  19, True ) /* Attackable */
     , (450490,  22, True ) /* Inscribable */
     , (450490,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450490,  13,     0.5) /* ArmorModVsSlash */
     , (450490,  14,   0.375) /* ArmorModVsPierce */
     , (450490,  15,    0.25) /* ArmorModVsBludgeon */
     , (450490,  16,     0.5) /* ArmorModVsCold */
     , (450490,  17,   0.375) /* ArmorModVsFire */
     , (450490,  18,   0.125) /* ArmorModVsAcid */
     , (450490,  19,   0.125) /* ArmorModVsElectric */
     , (450490, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450490,   1, 'Armored Sclavus Mask') /* Name */
     , (450490,  16, 'A mask made from one of the armored Sclavus followers of T''thuun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450490,   1, 0x0200186B) /* Setup */
     , (450490,   3, 0x20000014) /* SoundTable */
     , (450490,   6, 0x0400007E) /* PaletteBase */
     , (450490,   7, 0x1000075C) /* ClothingBase */
     , (450490,   8, 0x06006976) /* Icon */
     , (450490,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (450490,   2, 0x501038C4) /* Container */
     , (450490,   3, 0x00000000) /* Wielder */;
