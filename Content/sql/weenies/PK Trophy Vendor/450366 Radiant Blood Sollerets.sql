DELETE FROM `weenie` WHERE `class_Id` = 450366;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450366, 'ace450366-radiantbloodsolleretstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450366,   1,          2) /* ItemType - Armor */
     , (450366,   4,      65536) /* ClothingPriority - Feet */
     , (450366,   5,        0) /* EncumbranceVal */
     , (450366,   9,        256) /* ValidLocations - FootWear */
     , (450366,  16,          1) /* ItemUseable - No */
     , (450366,  18,          1) /* UiEffects - Magical */
     , (450366,  19,       20) /* Value */
     , (450366,  27,         32) /* ArmorType - Metal */
     , (450366,  28,        0) /* ArmorLevel */
     , (450366,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450366, 150,        103) /* HookPlacement - Hook */
     , (450366, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450366,  22, True ) /* Inscribable */
     , (450366, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450366,   5,  -0.067) /* ManaRate */
     , (450366,  13,     1.3) /* ArmorModVsSlash */
     , (450366,  14,       1) /* ArmorModVsPierce */
     , (450366,  15,       1) /* ArmorModVsBludgeon */
     , (450366,  16,     0.4) /* ArmorModVsCold */
     , (450366,  17,     0.4) /* ArmorModVsFire */
     , (450366,  18,     0.6) /* ArmorModVsAcid */
     , (450366,  19,     0.4) /* ArmorModVsElectric */
     , (450366, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450366,   1, 'Radiant Blood Sollerets') /* Name */
     , (450366,  16, 'Radiant Blood Sollerets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450366,   1, 0x020000DE) /* Setup */
     , (450366,   3, 0x20000014) /* SoundTable */
     , (450366,   7, 0x1000074B) /* ClothingBase */
     , (450366,   8, 0x06006934) /* Icon */
     , (450366,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450366,  36, 0x0E000012) /* MutateFilter */;
