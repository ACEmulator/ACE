DELETE FROM `weenie` WHERE `class_Id` = 480554;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480554, 'ace480554-falatacotabbessmaskpk', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480554,   1,          4) /* ItemType - Armor */
     , (480554,   3,         69) /* PaletteTemplate - YellowSlime */
     , (480554,   4,      16384) /* ClothingPriority - Head */
     , (480554,   5,        150) /* EncumbranceVal */
     , (480554,   9,          1) /* ValidLocations - HeadWear */
     , (480554,  16,          1) /* ItemUseable - No */
     , (480554,  19,        20) /* Value */
     , (480554,  27,          2) /* ArmorType - Leather */
     , (480554,  28,         0) /* ArmorLevel */
     , (480554,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480554, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480554,  11, True ) /* IgnoreCollisions */
     , (480554,  13, True ) /* Ethereal */
     , (480554,  14, True ) /* GravityStatus */
     , (480554,  19, True ) /* Attackable */
     , (480554,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480554,  13,     0.5) /* ArmorModVsSlash */
     , (480554,  14,     0.4) /* ArmorModVsPierce */
     , (480554,  15,     0.4) /* ArmorModVsBludgeon */
     , (480554,  16,     0.6) /* ArmorModVsCold */
     , (480554,  17,     0.2) /* ArmorModVsFire */
     , (480554,  18,     0.8) /* ArmorModVsAcid */
     , (480554,  19,     0.3) /* ArmorModVsElectric */
     , (480554, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480554,   1, 'Falatacot Abbess Mask') /* Name */
     , (480554,  15, 'A gruesome mask, crafted from the head of an Undead Falatacot.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480554,   1, 0x02001626) /* Setup */
     , (480554,   3, 0x20000014) /* SoundTable */
     , (480554,   6, 0x0400007E) /* PaletteBase */
     , (480554,   7, 0x1000069F) /* ClothingBase */
     , (480554,   8, 0x060064E4) /* Icon */
     , (480554,  22, 0x3400002B) /* PhysicsEffectTable */;
