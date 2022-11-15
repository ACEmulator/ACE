DELETE FROM `weenie` WHERE `class_Id` = 450360;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450360, 'ace450360-radiantbloodgirthtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450360,   1,          2) /* ItemType - Armor */
     , (450360,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450360,   5,        0) /* EncumbranceVal */
     , (450360,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450360,  16,          1) /* ItemUseable - No */
     , (450360,  18,          1) /* UiEffects - Magical */
     , (450360,  19,       20) /* Value */
     , (450360,  27,         32) /* ArmorType - Metal */
     , (450360,  28,        0) /* ArmorLevel */
     , (450360,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450360, 150,        103) /* HookPlacement - Hook */
     , (450360, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450360,  22, True ) /* Inscribable */
     , (450360, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450360,   5,  -0.067) /* ManaRate */
     , (450360,  13,     1.3) /* ArmorModVsSlash */
     , (450360,  14,       1) /* ArmorModVsPierce */
     , (450360,  15,       1) /* ArmorModVsBludgeon */
     , (450360,  16,     0.4) /* ArmorModVsCold */
     , (450360,  17,     0.4) /* ArmorModVsFire */
     , (450360,  18,     0.6) /* ArmorModVsAcid */
     , (450360,  19,     0.4) /* ArmorModVsElectric */
     , (450360, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450360,   1, 'Radiant Blood Girth') /* Name */
     , (450360,  16, 'Radiant Blood Girth') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450360,   1, 0x020000D7) /* Setup */
     , (450360,   3, 0x20000014) /* SoundTable */
     , (450360,   7, 0x10000747) /* ClothingBase */
     , (450360,   8, 0x06006930) /* Icon */
     , (450360,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450360,  36, 0x0E000012) /* MutateFilter */;
