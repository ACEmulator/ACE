DELETE FROM `weenie` WHERE `class_Id` = 450529;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450529, 'ace450529-viamontianlordshelmtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450529,   1,          2) /* ItemType - Armor */
     , (450529,   3,         13) /* PaletteTemplate - Purple */
     , (450529,   4,      16384) /* ClothingPriority - Head */
     , (450529,   5,        0) /* EncumbranceVal */
     , (450529,   9,          1) /* ValidLocations - HeadWear */
     , (450529,  16,          1) /* ItemUseable - No */
     , (450529,  19,       20) /* Value */
     , (450529,  28,        0) /* ArmorLevel */
     , (450529,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450529, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450529,  11, True ) /* IgnoreCollisions */
     , (450529,  13, True ) /* Ethereal */
     , (450529,  14, True ) /* GravityStatus */
     , (450529,  19, True ) /* Attackable */
     , (450529,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450529,   5,   -0.05) /* ManaRate */
     , (450529,  12,       0) /* Shade */
     , (450529,  13,     1.4) /* ArmorModVsSlash */
     , (450529,  14,     1.4) /* ArmorModVsPierce */
     , (450529,  15,     1.2) /* ArmorModVsBludgeon */
     , (450529,  16,     1.2) /* ArmorModVsCold */
     , (450529,  17,     0.8) /* ArmorModVsFire */
     , (450529,  18,     1.2) /* ArmorModVsAcid */
     , (450529,  19,     0.8) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450529,   1, 'Viamontian Lord''s Helm') /* Name */
     , (450529,  16, 'The fitted helm of a Viamontian Lord.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450529,   1, 0x020014F4) /* Setup */
     , (450529,   3, 0x20000014) /* SoundTable */
     , (450529,   7, 0x10000662) /* ClothingBase */
     , (450529,   8, 0x06006282) /* Icon */;


