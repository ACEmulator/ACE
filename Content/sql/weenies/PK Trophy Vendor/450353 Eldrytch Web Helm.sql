DELETE FROM `weenie` WHERE `class_Id` = 450353;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450353, 'ace450353-eldrytchwebhelmtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450353,   1,          2) /* ItemType - Armor */
     , (450353,   4,      16384) /* ClothingPriority - Head */
     , (450353,   5,        0) /* EncumbranceVal */
     , (450353,   9,          1) /* ValidLocations - HeadWear */
     , (450353,  16,          1) /* ItemUseable - No */
     , (450353,  18,          1) /* UiEffects - Magical */
     , (450353,  19,       20) /* Value */
     , (450353,  27,         32) /* ArmorType - Metal */
     , (450353,  28,        0) /* ArmorLevel */
     , (450353,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450353, 150,        103) /* HookPlacement - Hook */
     , (450353, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450353,  22, True ) /* Inscribable */
     , (450353, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450353,   5,  -0.067) /* ManaRate */
     , (450353,  13,     1.3) /* ArmorModVsSlash */
     , (450353,  14,       1) /* ArmorModVsPierce */
     , (450353,  15,       1) /* ArmorModVsBludgeon */
     , (450353,  16,     0.4) /* ArmorModVsCold */
     , (450353,  17,     0.4) /* ArmorModVsFire */
     , (450353,  18,     0.6) /* ArmorModVsAcid */
     , (450353,  19,     0.4) /* ArmorModVsElectric */
     , (450353, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450353,   1, 'Eldrytch Web Helm') /* Name */
     , (450353,  16, 'Eldrytch Web Helm') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450353,   1, 0x02001630) /* Setup */
     , (450353,   3, 0x20000014) /* SoundTable */
     , (450353,   7, 0x10000752) /* ClothingBase */
     , (450353,   8, 0x06006948) /* Icon */
     , (450353,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450353,  36, 0x0E000012) /* MutateFilter */;
