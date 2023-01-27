DELETE FROM `weenie` WHERE `class_Id` = 450358;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450358, 'ace450358-radiantbloodbreastplatetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450358,   1,          2) /* ItemType - Armor */
     , (450358,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450358,   5,       0) /* EncumbranceVal */
     , (450358,   9,        512) /* ValidLocations - ChestArmor */
     , (450358,  16,          1) /* ItemUseable - No */
     , (450358,  18,          1) /* UiEffects - Magical */
     , (450358,  19,      20) /* Value */
     , (450358,  27,         32) /* ArmorType - Metal */
     , (450358,  28,        0) /* ArmorLevel */
	 , (450358, 159,        289) /* WieldSkillType */
     , (450358,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450358, 150,        103) /* HookPlacement - Hook */
     , (450358, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450358,  22, True ) /* Inscribable */
     , (450358, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450358,   5,  -0.067) /* ManaRate */
     , (450358,  13,     1.3) /* ArmorModVsSlash */
     , (450358,  14,       1) /* ArmorModVsPierce */
     , (450358,  15,       1) /* ArmorModVsBludgeon */
     , (450358,  16,     0.4) /* ArmorModVsCold */
     , (450358,  17,     0.4) /* ArmorModVsFire */
     , (450358,  18,     0.6) /* ArmorModVsAcid */
     , (450358,  19,     0.4) /* ArmorModVsElectric */
     , (450358, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450358,   1, 'Radiant Blood Breastplate') /* Name */
     , (450358,  16, 'Radiant Blood Breastplate') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450358,   1, 0x020000D2) /* Setup */
     , (450358,   3, 0x20000014) /* SoundTable */
     , (450358,   7, 0x10000745) /* ClothingBase */
     , (450358,   8, 0x0600692F) /* Icon */
     , (450358,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450358,  36, 0x0E000012) /* MutateFilter */;
