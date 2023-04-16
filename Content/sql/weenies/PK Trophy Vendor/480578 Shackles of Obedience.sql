DELETE FROM `weenie` WHERE `class_Id` = 480578;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480578, 'bracersmanaclespk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480578,   1,          2) /* ItemType - Armor */
     , (480578,   3,         20) /* PaletteTemplate - Silver */
     , (480578,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (480578,   5,        0) /* EncumbranceVal */
     , (480578,   8,        270) /* Mass */
     , (480578,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (480578,  16,          1) /* ItemUseable - No */
     , (480578,  18,          1) /* UiEffects - Magical */
     , (480578,  19,          20) /* Value */
     , (480578,  27,         32) /* ArmorType - Metal */
     , (480578,  28,        0) /* ArmorLevel */
     , (480578,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480578, 106,        230) /* ItemSpellcraft */
     , (480578, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480578,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480578,   5,   -0.05) /* ManaRate */
     , (480578,  12,    0.33) /* Shade */
     , (480578,  13,     1.2) /* ArmorModVsSlash */
     , (480578,  14,       1) /* ArmorModVsPierce */
     , (480578,  15,       1) /* ArmorModVsBludgeon */
     , (480578,  16,     0.5) /* ArmorModVsCold */
     , (480578,  17,     0.5) /* ArmorModVsFire */
     , (480578,  18,     0.7) /* ArmorModVsAcid */
     , (480578,  19,     0.5) /* ArmorModVsElectric */
     , (480578, 110,       1) /* BulkMod */
     , (480578, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480578,   1, 'Shackles of Obedience') /* Name */
     , (480578,  16, 'Shackles used to bind the wrists.  They have a long chain between them, allowing for freedom of movement.') /* LongDesc */
     , (480578,  33, 'VirindiManacles') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480578,   1, 0x020000D1) /* Setup */
     , (480578,   3, 0x20000014) /* SoundTable */
     , (480578,   6, 0x0400007E) /* PaletteBase */
     , (480578,   7, 0x100002DF) /* ClothingBase */
     , (480578,   8, 0x06000FC3) /* Icon */
     , (480578,  22, 0x3400002B) /* PhysicsEffectTable */;

