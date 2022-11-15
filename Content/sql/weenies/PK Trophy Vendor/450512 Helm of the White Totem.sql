DELETE FROM `weenie` WHERE `class_Id` = 450512;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450512, 'ace450512-helmofthewhitetotemtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450512,   1,          2) /* ItemType - Armor */
     , (450512,   3,         90) /* PaletteTemplate - DyeWinterSilver */
     , (450512,   4,      16384) /* ClothingPriority - Head */
     , (450512,   5,        0) /* EncumbranceVal */
     , (450512,   9,          1) /* ValidLocations - HeadWear */
     , (450512,  18,          1) /* UiEffects - Magical */
     , (450512,  19,       20) /* Value */
     , (450512,  28,        0) /* ArmorLevel */
     , (450512, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450512,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450512,   5,   -0.05) /* ManaRate */
     , (450512,  12,     0.5) /* Shade */
     , (450512,  13,       1) /* ArmorModVsSlash */
     , (450512,  14,       1) /* ArmorModVsPierce */
     , (450512,  15,       1) /* ArmorModVsBludgeon */
     , (450512,  16,     1.2) /* ArmorModVsCold */
     , (450512,  17,     0.8) /* ArmorModVsFire */
     , (450512,  18,     0.8) /* ArmorModVsAcid */
     , (450512,  19,     0.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450512,   1, 'Helm of the White Totem') /* Name */
     , (450512,  16, 'A helm of powerful enchantments, granted as a reward for the destruction of the White Totem of Grael.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450512,   1, 0x02000978) /* Setup */
     , (450512,   3, 0x20000014) /* SoundTable */
     , (450512,   6, 0x0400007E) /* PaletteBase */
     , (450512,   7, 0x10000644) /* ClothingBase */
     , (450512,   8, 0x0600617D) /* Icon */
     , (450512,  22, 0x3400002B) /* PhysicsEffectTable */;

