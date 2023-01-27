DELETE FROM `weenie` WHERE `class_Id` = 450363;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450363, 'ace450363-radiantbloodpauldronstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450363,   1,          2) /* ItemType - Armor */
     , (450363,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (450363,   5,        0) /* EncumbranceVal */
     , (450363,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (450363,  16,          1) /* ItemUseable - No */
     , (450363,  18,          1) /* UiEffects - Magical */
     , (450363,  19,       20) /* Value */
     , (450363,  27,         32) /* ArmorType - Metal */
     , (450363,  28,        0) /* ArmorLevel */
     , (450363,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450363, 150,        103) /* HookPlacement - Hook */
     , (450363, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450363,  22, True ) /* Inscribable */
     , (450363, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450363,   5,  -0.067) /* ManaRate */
     , (450363,  13,     1.3) /* ArmorModVsSlash */
     , (450363,  14,       1) /* ArmorModVsPierce */
     , (450363,  15,       1) /* ArmorModVsBludgeon */
     , (450363,  16,     0.4) /* ArmorModVsCold */
     , (450363,  17,     0.4) /* ArmorModVsFire */
     , (450363,  18,     0.6) /* ArmorModVsAcid */
     , (450363,  19,     0.4) /* ArmorModVsElectric */
     , (450363,  39,     1.1) /* DefaultScale */
     , (450363, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450363,   1, 'Radiant Blood Pauldrons') /* Name */
     , (450363,  16, 'Radiant Blood Pauldrons') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450363,   1, 0x020000D1) /* Setup */
     , (450363,   3, 0x20000014) /* SoundTable */
     , (450363,   7, 0x1000074A) /* ClothingBase */
     , (450363,   8, 0x06006933) /* Icon */
     , (450363,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450363,  36, 0x0E000012) /* MutateFilter */;
