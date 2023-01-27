DELETE FROM `weenie` WHERE `class_Id` = 450260;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450260, 'helmluminbluetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450260,   1,          2) /* ItemType - Armor */
     , (450260,   3,          2) /* PaletteTemplate - Blue */
     , (450260,   4,      16384) /* ClothingPriority - Head */
     , (450260,   5,        0) /* EncumbranceVal */
     , (450260,   8,        125) /* Mass */
     , (450260,   9,          1) /* ValidLocations - HeadWear */
     , (450260,  16,          1) /* ItemUseable - No */
     , (450260,  19,       20) /* Value */
     , (450260,  27,         32) /* ArmorType - Metal */
     , (450260,  28,        0) /* ArmorLevel */
     , (450260,  33,          1) /* Bonded - Bonded */
     , (450260,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450260,  22, True ) /* Inscribable */
     , (450260,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450260,   5,    -0.5) /* ManaRate */
     , (450260,  12,    0.66) /* Shade */
     , (450260,  13,    0.75) /* ArmorModVsSlash */
     , (450260,  14,    0.75) /* ArmorModVsPierce */
     , (450260,  15,    0.75) /* ArmorModVsBludgeon */
     , (450260,  16,    0.75) /* ArmorModVsCold */
     , (450260,  17,       1) /* ArmorModVsFire */
     , (450260,  18,       1) /* ArmorModVsAcid */
     , (450260,  19,    0.75) /* ArmorModVsElectric */
     , (450260, 110,       1) /* BulkMod */
     , (450260, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450260,   1, 'Luminescent Runic Helm') /* Name */
     , (450260,  15, 'This helm was worn by Sentinels of Perfect Light. They are modeled after the helms worn by the armies of Lord Atlan against the swelling darkness. The helm has a single setting for an orb.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450260,   1, 0x02000EFA) /* Setup */
     , (450260,   3, 0x20000014) /* SoundTable */
     , (450260,   6, 0x0400007E) /* PaletteBase */
     , (450260,   7, 0x10000451) /* ClothingBase */
     , (450260,   8, 0x06002A5D) /* Icon */
     , (450260,  22, 0x3400002B) /* PhysicsEffectTable */;


