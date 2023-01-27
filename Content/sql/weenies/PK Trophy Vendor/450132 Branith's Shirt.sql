DELETE FROM `weenie` WHERE `class_Id` = 450132;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450132, 'shirtstuddedleatherbranithtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450132,   1,          2) /* ItemType - Armor */
     , (450132,   3,          4) /* PaletteTemplate - Brown */
     , (450132,   4,       1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms */
     , (450132,   5,        0) /* EncumbranceVal */
     , (450132,   8,        300) /* Mass */
     , (450132,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor */
     , (450132,  16,          1) /* ItemUseable - No */
     , (450132,  18,          1) /* UiEffects - Magical */
     , (450132,  19,       20) /* Value */
     , (450132,  27,          4) /* ArmorType - StuddedLeather */
     , (450132,  28,        0) /* ArmorLevel */
     , (450132,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450132, 150,        103) /* HookPlacement - Hook */
     , (450132, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450132,  22, True ) /* Inscribable */
     , (450132,  69, False) /* IsSellable */
     , (450132,  84, True ) /* IgnoreCloIcons */
     , (450132, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450132,   5,  -0.025) /* ManaRate */
     , (450132,  12,    0.66) /* Shade */
     , (450132,  13,     1.4) /* ArmorModVsSlash */
     , (450132,  14,     1.3) /* ArmorModVsPierce */
     , (450132,  15,     1.2) /* ArmorModVsBludgeon */
     , (450132,  16,     0.6) /* ArmorModVsCold */
     , (450132,  17,     1.2) /* ArmorModVsFire */
     , (450132,  18,     0.6) /* ArmorModVsAcid */
     , (450132,  19,     0.6) /* ArmorModVsElectric */
     , (450132, 110,       1) /* BulkMod */
     , (450132, 111,       1) /* SizeMod */
     , (450132, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450132,   1, 'Branith''s Shirt') /* Name */
     , (450132,  16, 'A well-mended leather shirt. Inside the collar is a small tag which reads: Branith.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450132,   1, 0x02000F0F) /* Setup */
     , (450132,   3, 0x20000014) /* SoundTable */
     , (450132,   6, 0x0400007E) /* PaletteBase */
     , (450132,   7, 0x1000046D) /* ClothingBase */
     , (450132,   8, 0x06002AC0) /* Icon */
     , (450132,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450132,  37,         34) /* ItemSkillLimit - WarMagic */;
