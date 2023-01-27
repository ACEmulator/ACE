DELETE FROM `weenie` WHERE `class_Id` = 450527;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450527, 'helminvaderlordplatinumbloodedtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450527,   1,          2) /* ItemType - Armor */
     , (450527,   4,      16384) /* ClothingPriority - Head */
     , (450527,   5,        0) /* EncumbranceVal */
     , (450527,   9,          1) /* ValidLocations - HeadWear */
     , (450527,  16,          1) /* ItemUseable - No */
     , (450527,  18,          1) /* UiEffects - Magical */
     , (450527,  19,       20) /* Value */
     , (450527,  28,        0) /* ArmorLevel */
     , (450527,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450527, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450527,  11, True ) /* IgnoreCollisions */
     , (450527,  13, True ) /* Ethereal */
     , (450527,  14, True ) /* GravityStatus */
     , (450527,  19, True ) /* Attackable */
     , (450527,  22, True ) /* Inscribable */
     , (450527,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450527,   5,   -0.05) /* ManaRate */
     , (450527,  13,       1) /* ArmorModVsSlash */
     , (450527,  14,       1) /* ArmorModVsPierce */
     , (450527,  15,     1.2) /* ArmorModVsBludgeon */
     , (450527,  16,     0.8) /* ArmorModVsCold */
     , (450527,  17,     0.8) /* ArmorModVsFire */
     , (450527,  18,     0.7) /* ArmorModVsAcid */
     , (450527,  19,     0.7) /* ArmorModVsElectric */
     , (450527, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450527,   1, 'Blooded Platinum Invader Lord Helm') /* Name */
     , (450527,  16, 'A helm taken from one of the Knights of the Viamontian Platinum Legion and fortified with royal blood.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450527,   1, 0x0200133C) /* Setup */
     , (450527,   3, 0x20000014) /* SoundTable */
     , (450527,   7, 0x100005DF) /* ClothingBase */
     , (450527,   8, 0x06005A95) /* Icon */
     , (450527,  22, 0x3400002B) /* PhysicsEffectTable */;


