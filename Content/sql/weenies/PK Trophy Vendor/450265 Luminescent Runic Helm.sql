DELETE FROM `weenie` WHERE `class_Id` = 450265;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450265, 'helmlumingreentailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450265,   1,          2) /* ItemType - Armor */
     , (450265,   3,          8) /* PaletteTemplate - Green */
     , (450265,   4,      16384) /* ClothingPriority - Head */
     , (450265,   5,        0) /* EncumbranceVal */
     , (450265,   8,        125) /* Mass */
     , (450265,   9,          1) /* ValidLocations - HeadWear */
     , (450265,  16,          1) /* ItemUseable - No */
     , (450265,  19,       20) /* Value */
     , (450265,  27,         32) /* ArmorType - Metal */
     , (450265,  28,        0) /* ArmorLevel */
     , (450265,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450265,  22, True ) /* Inscribable */
     , (450265,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450265,   5,    -0.5) /* ManaRate */
     , (450265,  12,    0.66) /* Shade */
     , (450265,  13,    0.75) /* ArmorModVsSlash */
     , (450265,  14,    0.75) /* ArmorModVsPierce */
     , (450265,  15,    0.75) /* ArmorModVsBludgeon */
     , (450265,  16,    0.75) /* ArmorModVsCold */
     , (450265,  17,       1) /* ArmorModVsFire */
     , (450265,  18,       1) /* ArmorModVsAcid */
     , (450265,  19,    0.75) /* ArmorModVsElectric */
     , (450265, 110,       1) /* BulkMod */
     , (450265, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450265,   1, 'Luminescent Runic Helm') /* Name */
     , (450265,  15, 'This helm was worn by Sentinels of Perfect Light. They are modeled after the helms worn by the armies of Lord Atlan against the swelling darkness. The helm has a single setting for an orb.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450265,   1, 0x02000EFA) /* Setup */
     , (450265,   3, 0x20000014) /* SoundTable */
     , (450265,   6, 0x0400007E) /* PaletteBase */
     , (450265,   7, 0x10000451) /* ClothingBase */
     , (450265,   8, 0x06002A5B) /* Icon */
     , (450265,  22, 0x3400002B) /* PhysicsEffectTable */;


