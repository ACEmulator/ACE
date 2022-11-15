DELETE FROM `weenie` WHERE `class_Id` = 450525;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450525, 'helminvaderlordgoldbloodedtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450525,   1,          2) /* ItemType - Armor */
     , (450525,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450525,   4,      16384) /* ClothingPriority - Head */
     , (450525,   5,        0) /* EncumbranceVal */
     , (450525,   9,          1) /* ValidLocations - HeadWear */
     , (450525,  16,          1) /* ItemUseable - No */
     , (450525,  18,          1) /* UiEffects - Magical */
     , (450525,  19,       20) /* Value */
     , (450525,  28,        0) /* ArmorLevel */
     , (450525,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450525, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450525,  11, True ) /* IgnoreCollisions */
     , (450525,  13, True ) /* Ethereal */
     , (450525,  14, True ) /* GravityStatus */
     , (450525,  19, True ) /* Attackable */
     , (450525,  22, True ) /* Inscribable */
     , (450525,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450525,   5,   -0.05) /* ManaRate */
     , (450525,  12,       0) /* Shade */
     , (450525,  13,       1) /* ArmorModVsSlash */
     , (450525,  14,       1) /* ArmorModVsPierce */
     , (450525,  15,     1.2) /* ArmorModVsBludgeon */
     , (450525,  16,     0.8) /* ArmorModVsCold */
     , (450525,  17,     0.8) /* ArmorModVsFire */
     , (450525,  18,     0.7) /* ArmorModVsAcid */
     , (450525,  19,     0.7) /* ArmorModVsElectric */
     , (450525, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450525,   1, 'Blooded Gold Invader Lord Helm') /* Name */
     , (450525,  16, 'A helm taken from one of the Knights of the Viamontian Gold Legion and fortified with royal blood.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450525,   1, 0x0200133B) /* Setup */
     , (450525,   3, 0x20000014) /* SoundTable */
     , (450525,   7, 0x100005DD) /* ClothingBase */
     , (450525,   8, 0x06005A92) /* Icon */
     , (450525,  22, 0x3400002B) /* PhysicsEffectTable */;

