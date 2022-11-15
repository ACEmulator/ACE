DELETE FROM `weenie` WHERE `class_Id` = 450025;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450025, 'ace450025-empyreanoverrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450025,   1,          4) /* ItemType - Armor */
     , (450025,   3,          2) /* PaletteTemplate - Blue */
     , (450025,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450025,   5,        0) /* EncumbranceVal */
     , (450025,   9,        512) /* ValidLocations - ChestArmor */
     , (450025,  16,          1) /* ItemUseable - No */
     , (450025,  19,     20) /* Value */
     , (450025,  27,          1) /* ArmorType - Cloth */
     , (450025,  28,        0) /* ArmorLevel */
     , (450025,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450025,  22, True ) /* Inscribable */
     , (450025,  69, False) /* IsSellable */
     , (450025,  99, False) /* Ivoryable */
     , (450025, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450025,   5,    -0.5) /* ManaRate */
     , (450025,  13,     0.6) /* ArmorModVsSlash */
     , (450025,  14,     0.6) /* ArmorModVsPierce */
     , (450025,  15,     0.6) /* ArmorModVsBludgeon */
     , (450025,  16,     0.6) /* ArmorModVsCold */
     , (450025,  17,     0.6) /* ArmorModVsFire */
     , (450025,  18,     0.6) /* ArmorModVsAcid */
     , (450025,  19,     0.6) /* ArmorModVsElectric */
     , (450025, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450025,   1, 'Empyrean Over-robe') /* Name */
     , (450025,  14, 'This robe may be tailored onto most breastplates.') /* Use */
     , (450025,  16, 'A loose-fitting Empyrean robe, designed to be worn over other armor pieces.  Embedded in the fabric are small threads of Thaumaturgic Crystal which radiate an almost palpable power.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450025,   1, 0x020001A6) /* Setup */
     , (450025,   3, 0x20000014) /* SoundTable */
     , (450025,   6, 0x0400007E) /* PaletteBase */
     , (450025,   7, 0x100006BA) /* ClothingBase */
     , (450025,   8, 0x060065D2) /* Icon */
     , (450025,  22, 0x3400002B) /* PhysicsEffectTable */;
