DELETE FROM `weenie` WHERE `class_Id` = 450514;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450514, 'ace450514-helmoftheabyssaltotemtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450514,   1,          2) /* ItemType - Armor */
     , (450514,   3,         13) /* PaletteTemplate - Purple */
     , (450514,   4,      16384) /* ClothingPriority - Head */
     , (450514,   5,        0) /* EncumbranceVal */
     , (450514,   9,          1) /* ValidLocations - HeadWear */
     , (450514,  16,          1) /* ItemUseable - No */
     , (450514,  18,          1) /* UiEffects - Magical */
     , (450514,  19,      20) /* Value */
     , (450514,  28,        0) /* ArmorLevel */
     , (450514,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450514, 114,          0) /* Attuned - Normal */
     , (450514, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450514,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450514,   5,   -0.05) /* ManaRate */
     , (450514,  13,       1) /* ArmorModVsSlash */
     , (450514,  14,       1) /* ArmorModVsPierce */
     , (450514,  15,       1) /* ArmorModVsBludgeon */
     , (450514,  16,     1.2) /* ArmorModVsCold */
     , (450514,  17,     0.6) /* ArmorModVsFire */
     , (450514,  18,     0.8) /* ArmorModVsAcid */
     , (450514,  19,     0.8) /* ArmorModVsElectric */
     , (450514, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450514,   1, 'Helm of the Abyssal Totem') /* Name */
     , (450514,  16, 'A helm of powerful enchantments, granted as a reward for the destruction of the Abyssal Totem of Grael.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450514,   1, 0x02000978) /* Setup */
     , (450514,   3, 0x20000014) /* SoundTable */
     , (450514,   6, 0x0400007E) /* PaletteBase */
     , (450514,   7, 0x10000644) /* ClothingBase */
     , (450514,   8, 0x0600617B) /* Icon */
     , (450514,  22, 0x3400002B) /* PhysicsEffectTable */;

