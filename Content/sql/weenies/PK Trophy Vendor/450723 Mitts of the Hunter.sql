DELETE FROM `weenie` WHERE `class_Id` = 450723;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450723, 'ace450723-mittsofthehuntertailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450723,   1,          2) /* ItemType - Armor */
     , (450723,   3,          4) /* PaletteTemplate - Brown */
     , (450723,   4,      32768) /* ClothingPriority - Hands */
     , (450723,   5,        0) /* EncumbranceVal */
     , (450723,   9,         32) /* ValidLocations - HandWear */
     , (450723,  16,          1) /* ItemUseable - No */
     , (450723,  19,       20) /* Value */
     , (450723,  28,        0) /* ArmorLevel */
     , (450723,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450723,  11, True ) /* IgnoreCollisions */
     , (450723,  13, True ) /* Ethereal */
     , (450723,  14, True ) /* GravityStatus */
     , (450723,  19, True ) /* Attackable */
     , (450723,  22, True ) /* Inscribable */
     , (450723,  69, False) /* IsSellable */
     , (450723, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450723,   5,  -0.025) /* ManaRate */
     , (450723,  12,       0) /* Shade */
     , (450723,  13,       1) /* ArmorModVsSlash */
     , (450723,  14,       1) /* ArmorModVsPierce */
     , (450723,  15,       2) /* ArmorModVsBludgeon */
     , (450723,  16,       2) /* ArmorModVsCold */
     , (450723,  17,       1) /* ArmorModVsFire */
     , (450723,  18,       1) /* ArmorModVsAcid */
     , (450723,  19,       1) /* ArmorModVsElectric */
     , (450723, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450723,   1, 'Mitts of the Hunter') /* Name */
     , (450723,  15, 'A set of simple dark leather and metal gauntlets. The emblem of the Order of the Raven Hand is embossed on the back of each hand.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450723,   1, 0x02001443) /* Setup */
     , (450723,   3, 0x20000014) /* SoundTable */
     , (450723,   6, 0x0400007E) /* PaletteBase */
     , (450723,   7, 0x10000624) /* ClothingBase */
     , (450723,   8, 0x06006061) /* Icon */
     , (450723,  22, 0x3400002B) /* PhysicsEffectTable */;


