DELETE FROM `weenie` WHERE `class_Id` = 450261;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450261, 'helmluminredtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450261,   1,          2) /* ItemType - Armor */
     , (450261,   3,         14) /* PaletteTemplate - Red */
     , (450261,   4,      16384) /* ClothingPriority - Head */
     , (450261,   5,        265) /* EncumbranceVal */
     , (450261,   8,        125) /* Mass */
     , (450261,   9,          1) /* ValidLocations - HeadWear */
     , (450261,  16,          1) /* ItemUseable - No */
     , (450261,  19,       20) /* Value */
     , (450261,  27,         32) /* ArmorType - Metal */
     , (450261,  28,        0) /* ArmorLevel */
     , (450261,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450261,  22, True ) /* Inscribable */
     , (450261,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450261,   5,    -0.5) /* ManaRate */
     , (450261,  12,    0.66) /* Shade */
     , (450261,  13,    0.75) /* ArmorModVsSlash */
     , (450261,  14,    0.75) /* ArmorModVsPierce */
     , (450261,  15,    0.75) /* ArmorModVsBludgeon */
     , (450261,  16,    0.75) /* ArmorModVsCold */
     , (450261,  17,       1) /* ArmorModVsFire */
     , (450261,  18,       1) /* ArmorModVsAcid */
     , (450261,  19,    0.75) /* ArmorModVsElectric */
     , (450261, 110,       1) /* BulkMod */
     , (450261, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450261,   1, 'Luminescent Runic Helm') /* Name */
     , (450261,  15, 'This helm was worn by Sentinels of Perfect Light. They are modeled after the helms worn by the armies of Lord Atlan against the swelling darkness. The helm has a single setting for an orb.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450261,   1, 0x02000EFA) /* Setup */
     , (450261,   3, 0x20000014) /* SoundTable */
     , (450261,   6, 0x0400007E) /* PaletteBase */
     , (450261,   7, 0x10000451) /* ClothingBase */
     , (450261,   8, 0x06002A5A) /* Icon */
     , (450261,  22, 0x3400002B) /* PhysicsEffectTable */;

