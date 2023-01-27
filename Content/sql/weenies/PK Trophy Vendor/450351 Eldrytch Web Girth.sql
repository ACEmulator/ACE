DELETE FROM `weenie` WHERE `class_Id` = 450351;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450351, 'ace450351-eldrytchwebgirthtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450351,   1,          2) /* ItemType - Armor */
     , (450351,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450351,   5,        0) /* EncumbranceVal */
     , (450351,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450351,  16,          1) /* ItemUseable - No */
     , (450351,  18,          1) /* UiEffects - Magical */
     , (450351,  19,       20) /* Value */
     , (450351,  27,         32) /* ArmorType - Metal */
     , (450351,  28,        0) /* ArmorLevel */
     , (450351,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450351, 150,        103) /* HookPlacement - Hook */
     , (450351, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450351,  22, True ) /* Inscribable */
     , (450351, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450351,   5,  -0.067) /* ManaRate */
     , (450351,  13,     1.3) /* ArmorModVsSlash */
     , (450351,  14,       1) /* ArmorModVsPierce */
     , (450351,  15,       1) /* ArmorModVsBludgeon */
     , (450351,  16,     0.4) /* ArmorModVsCold */
     , (450351,  17,     0.4) /* ArmorModVsFire */
     , (450351,  18,     0.6) /* ArmorModVsAcid */
     , (450351,  19,     0.4) /* ArmorModVsElectric */
     , (450351, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450351,   1, 'Eldrytch Web Girth') /* Name */
     , (450351,  16, 'Eldrytch Web Girth') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450351,   1, 0x020000D7) /* Setup */
     , (450351,   3, 0x20000014) /* SoundTable */
     , (450351,   7, 0x10000750) /* ClothingBase */
     , (450351,   8, 0x06006946) /* Icon */
     , (450351,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450351,  36, 0x0E000012) /* MutateFilter */;
