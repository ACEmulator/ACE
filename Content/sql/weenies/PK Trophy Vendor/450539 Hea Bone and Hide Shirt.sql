DELETE FROM `weenie` WHERE `class_Id` = 450539;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450539, 'ace450539-heaboneandhideshirttailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450539,   1,          2) /* ItemType - Armor */
     , (450539,   3,          4) /* PaletteTemplate - Brown */
     , (450539,   4,      15360) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450539,   5,       0) /* EncumbranceVal */
     , (450539,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450539,  16,          1) /* ItemUseable - No */
     , (450539,  19,       20) /* Value */
     , (450539,  28,        0) /* ArmorLevel */
     , (450539,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450539,  11, True ) /* IgnoreCollisions */
     , (450539,  13, True ) /* Ethereal */
     , (450539,  14, True ) /* GravityStatus */
     , (450539,  19, True ) /* Attackable */
     , (450539,  22, True ) /* Inscribable */
     , (450539,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450539,   5,  -0.033) /* ManaRate */
     , (450539,  12,       0) /* Shade */
     , (450539,  13,     1.1) /* ArmorModVsSlash */
     , (450539,  14,       1) /* ArmorModVsPierce */
     , (450539,  15,     1.4) /* ArmorModVsBludgeon */
     , (450539,  16,     0.9) /* ArmorModVsCold */
     , (450539,  17,     0.6) /* ArmorModVsFire */
     , (450539,  18,     0.8) /* ArmorModVsAcid */
     , (450539,  19,     0.8) /* ArmorModVsElectric */
     , (450539, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450539,   1, 'Hea Bone and Hide Shirt') /* Name */
     , (450539,  16, 'An armored shirt of bones and hide, made by the Hea Hunter Kassoka.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450539,   1, 0x020000D4) /* Setup */
     , (450539,   3, 0x20000014) /* SoundTable */
     , (450539,   6, 0x04001EA7) /* PaletteBase */
     , (450539,   7, 0x10000630) /* ClothingBase */
     , (450539,   8, 0x06006103) /* Icon */;


