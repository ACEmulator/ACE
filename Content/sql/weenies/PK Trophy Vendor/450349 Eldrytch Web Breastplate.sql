DELETE FROM `weenie` WHERE `class_Id` = 450349;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450349, 'ace450349-eldrytchwebbreastplatetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450349,   1,          2) /* ItemType - Armor */
     , (450349,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450349,   5,       0) /* EncumbranceVal */
     , (450349,   9,        512) /* ValidLocations - ChestArmor */
     , (450349,  16,          1) /* ItemUseable - No */
     , (450349,  18,          1) /* UiEffects - Magical */
     , (450349,  19,      20) /* Value */
     , (450349,  27,         32) /* ArmorType - Metal */
     , (450349,  28,        0) /* ArmorLevel */
     , (450349,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450349, 150,        103) /* HookPlacement - Hook */
     , (450349, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450349,  22, True ) /* Inscribable */
     , (450349, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450349,   5,  -0.067) /* ManaRate */
     , (450349,  13,     1.3) /* ArmorModVsSlash */
     , (450349,  14,       1) /* ArmorModVsPierce */
     , (450349,  15,       1) /* ArmorModVsBludgeon */
     , (450349,  16,     0.4) /* ArmorModVsCold */
     , (450349,  17,     0.4) /* ArmorModVsFire */
     , (450349,  18,     0.6) /* ArmorModVsAcid */
     , (450349,  19,     0.4) /* ArmorModVsElectric */
     , (450349, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450349,   1, 'Eldrytch Web Breastplate') /* Name */
     , (450349,  16, 'Eldrytch Web Breastplate') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450349,   1, 0x020000D2) /* Setup */
     , (450349,   3, 0x20000014) /* SoundTable */
     , (450349,   7, 0x1000074E) /* ClothingBase */
     , (450349,   8, 0x06006945) /* Icon */
     , (450349,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450349,  36, 0x0E000012) /* MutateFilter */;
