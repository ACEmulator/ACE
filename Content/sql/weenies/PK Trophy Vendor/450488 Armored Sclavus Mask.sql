DELETE FROM `weenie` WHERE `class_Id` = 450488;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450488, 'ace450488-armoredsclavusmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450488,   1,          4) /* ItemType - Armor */
     , (450488,   3,          4) /* PaletteTemplate - Brown */
     , (450488,   4,      16384) /* ClothingPriority - Head */
     , (450488,   5,        0) /* EncumbranceVal */
     , (450488,   9,          1) /* ValidLocations - HeadWear */
     , (450488,  19,        20) /* Value */
     , (450488,  28,         0) /* ArmorLevel */
     , (450488, 107,          0) /* ItemCurMana */
     , (450488, 108,          0) /* ItemMaxMana */
     , (450488, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450488,  11, True ) /* IgnoreCollisions */
     , (450488,  13, True ) /* Ethereal */
     , (450488,  14, True ) /* GravityStatus */
     , (450488,  19, True ) /* Attackable */
     , (450488,  22, True ) /* Inscribable */
     , (450488,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450488,  13,     0.5) /* ArmorModVsSlash */
     , (450488,  14,   0.375) /* ArmorModVsPierce */
     , (450488,  15,    0.25) /* ArmorModVsBludgeon */
     , (450488,  16,     0.5) /* ArmorModVsCold */
     , (450488,  17,   0.375) /* ArmorModVsFire */
     , (450488,  18,   0.125) /* ArmorModVsAcid */
     , (450488,  19,   0.125) /* ArmorModVsElectric */
     , (450488, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450488,   1, 'Armored Sclavus Mask') /* Name */
     , (450488,  16, 'A mask made from one of the armored Sclavus followers of T''thuun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450488,   1, 0x02001869) /* Setup */
     , (450488,   3, 0x20000014) /* SoundTable */
     , (450488,   6, 0x0400007E) /* PaletteBase */
     , (450488,   7, 0x1000075A) /* ClothingBase */
     , (450488,   8, 0x06006974) /* Icon */
     , (450488,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (450488,   2, 0x501038C4) /* Container */
     , (450488,   3, 0x00000000) /* Wielder */;
