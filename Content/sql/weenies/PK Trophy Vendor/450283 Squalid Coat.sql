DELETE FROM `weenie` WHERE `class_Id` = 450283;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450283, 'ace450283-squalidcoattailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450283,   1,          2) /* ItemType - Armor */
     , (450283,   3,         21) /* PaletteTemplate - Gold */
     , (450283,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450283,   5,       0) /* EncumbranceVal */
     , (450283,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450283,  16,          1) /* ItemUseable - No */
     , (450283,  19,       20) /* Value */
     , (450283,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450283,  28,        0) /* ArmorLevel */
     , (450283,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450283, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450283,  11, True ) /* IgnoreCollisions */
     , (450283,  13, True ) /* Ethereal */
     , (450283,  14, True ) /* GravityStatus */
     , (450283,  19, True ) /* Attackable */
     , (450283,  22, True ) /* Inscribable */
     , (450283,  69, True ) /* IsSellable */
     , (450283, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450283,   5,   -0.05) /* ManaRate */
     , (450283,  13,       1) /* ArmorModVsSlash */
     , (450283,  14,       1) /* ArmorModVsPierce */
     , (450283,  15,     1.2) /* ArmorModVsBludgeon */
     , (450283,  16,     0.8) /* ArmorModVsCold */
     , (450283,  17,     1.2) /* ArmorModVsFire */
     , (450283,  18,       1) /* ArmorModVsAcid */
     , (450283,  19,     0.8) /* ArmorModVsElectric */
     , (450283, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450283,   1, 'Squalid Coat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450283,   1, 0x020000D2) /* Setup */
     , (450283,   3, 0x20000014) /* SoundTable */
     , (450283,   6, 0x0400007E) /* PaletteBase */
     , (450283,   7, 0x10000616) /* ClothingBase */
     , (450283,   8, 0x06005F9A) /* Icon */
     , (450283,  22, 0x3400002B) /* PhysicsEffectTable */;


