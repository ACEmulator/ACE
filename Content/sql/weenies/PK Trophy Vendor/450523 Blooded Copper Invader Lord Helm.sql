DELETE FROM `weenie` WHERE `class_Id` = 450523;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450523, 'helminvaderlordcopperbloodedtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450523,   1,          2) /* ItemType - Armor */
     , (450523,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450523,   4,      16384) /* ClothingPriority - Head */
     , (450523,   5,        0) /* EncumbranceVal */
     , (450523,   9,          1) /* ValidLocations - HeadWear */
     , (450523,  16,          1) /* ItemUseable - No */
     , (450523,  18,          1) /* UiEffects - Magical */
     , (450523,  19,       20) /* Value */
     , (450523,  28,        0) /* ArmorLevel */
     , (450523,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450523, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450523,  11, True ) /* IgnoreCollisions */
     , (450523,  13, True ) /* Ethereal */
     , (450523,  14, True ) /* GravityStatus */
     , (450523,  19, True ) /* Attackable */
     , (450523,  22, True ) /* Inscribable */
     , (450523,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450523,   5,   -0.05) /* ManaRate */
     , (450523,  12,       0) /* Shade */
     , (450523,  13,       1) /* ArmorModVsSlash */
     , (450523,  14,       1) /* ArmorModVsPierce */
     , (450523,  15,     1.2) /* ArmorModVsBludgeon */
     , (450523,  16,     0.8) /* ArmorModVsCold */
     , (450523,  17,     0.8) /* ArmorModVsFire */
     , (450523,  18,     0.7) /* ArmorModVsAcid */
     , (450523,  19,     0.7) /* ArmorModVsElectric */
     , (450523, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450523,   1, 'Blooded Copper Invader Lord Helm') /* Name */
     , (450523,  16, 'A helm taken from one of the Knights of the Viamontian Copper Legion and fortified with royal blood.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450523,   1, 0x0200133A) /* Setup */
     , (450523,   3, 0x20000014) /* SoundTable */
     , (450523,   7, 0x100005DB) /* ClothingBase */
     , (450523,   8, 0x06005A8F) /* Icon */
     , (450523,  22, 0x3400002B) /* PhysicsEffectTable */;

