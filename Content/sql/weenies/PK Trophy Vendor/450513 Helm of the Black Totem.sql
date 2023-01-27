DELETE FROM `weenie` WHERE `class_Id` = 450513;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450513, 'ace450513-helmoftheblacktotemtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450513,   1,          2) /* ItemType - Armor */
     , (450513,   3,         39) /* PaletteTemplate - Black */
     , (450513,   4,      16384) /* ClothingPriority - Head */
     , (450513,   5,        0) /* EncumbranceVal */
     , (450513,   9,          1) /* ValidLocations - HeadWear */
     , (450513,  13,         39) /* StackUnitEncumbrance */
     , (450513,  18,          1) /* UiEffects - Magical */
     , (450513,  19,       20) /* Value */
     , (450513,  28,        0) /* ArmorLevel */
     , (450513,  33,          0) /* Bonded - Normal */
     , (450513,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450513, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450513,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450513,   5,   -0.05) /* ManaRate */
     , (450513,  12,     0.5) /* Shade */
     , (450513,  13,       1) /* ArmorModVsSlash */
     , (450513,  14,       1) /* ArmorModVsPierce */
     , (450513,  15,       1) /* ArmorModVsBludgeon */
     , (450513,  16,     1.2) /* ArmorModVsCold */
     , (450513,  17,     0.6) /* ArmorModVsFire */
     , (450513,  18,     0.8) /* ArmorModVsAcid */
     , (450513,  19,     0.8) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450513,   1, 'Helm of the Black Totem') /* Name */
     , (450513,  16, 'A helm of powerful enchantments, granted as a reward for the destruction of the Black Totem of Grael.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450513,   1, 0x02000978) /* Setup */
     , (450513,   3, 0x20000014) /* SoundTable */
     , (450513,   6, 0x0400007E) /* PaletteBase */
     , (450513,   7, 0x10000644) /* ClothingBase */
     , (450513,   8, 0x06006176) /* Icon */
     , (450513,  22, 0x3400002B) /* PhysicsEffectTable */;


