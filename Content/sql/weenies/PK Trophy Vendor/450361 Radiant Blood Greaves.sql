DELETE FROM `weenie` WHERE `class_Id` = 450361;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450361, 'ace450361-radiantbloodgreavestailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450361,   1,          2) /* ItemType - Armor */
     , (450361,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (450361,   5,        0) /* EncumbranceVal */
     , (450361,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (450361,  16,          1) /* ItemUseable - No */
     , (450361,  18,          1) /* UiEffects - Magical */
     , (450361,  19,       20) /* Value */
     , (450361,  27,         32) /* ArmorType - Metal */
     , (450361,  28,        0) /* ArmorLevel */
     , (450361,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450361, 150,        103) /* HookPlacement - Hook */
     , (450361, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450361,  22, True ) /* Inscribable */
     , (450361, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450361,   5,  -0.067) /* ManaRate */
     , (450361,  13,     1.3) /* ArmorModVsSlash */
     , (450361,  14,       1) /* ArmorModVsPierce */
     , (450361,  15,       1) /* ArmorModVsBludgeon */
     , (450361,  16,     0.4) /* ArmorModVsCold */
     , (450361,  17,     0.4) /* ArmorModVsFire */
     , (450361,  18,     0.6) /* ArmorModVsAcid */
     , (450361,  19,     0.4) /* ArmorModVsElectric */
     , (450361,  39,    1.33) /* DefaultScale */
     , (450361, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450361,   1, 'Radiant Blood Greaves') /* Name */
     , (450361,  16, 'Radiant Blood Greaves') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450361,   1, 0x020000D1) /* Setup */
     , (450361,   3, 0x20000014) /* SoundTable */
     , (450361,   7, 0x10000748) /* ClothingBase */
     , (450361,   8, 0x06006931) /* Icon */
     , (450361,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450361,  36, 0x0E000012) /* MutateFilter */;
