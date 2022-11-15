DELETE FROM `weenie` WHERE `class_Id` = 450530;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450530, 'boygrubmask-xptailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450530,   1,          2) /* ItemType - Armor */
     , (450530,   3,          4) /* PaletteTemplate - Brown */
     , (450530,   4,      16384) /* ClothingPriority - Head */
     , (450530,   5,        0) /* EncumbranceVal */
     , (450530,   8,         75) /* Mass */
     , (450530,   9,          1) /* ValidLocations - HeadWear */
     , (450530,  16,          1) /* ItemUseable - No */
     , (450530,  18,          1) /* UiEffects - Magical */
     , (450530,  19,        20) /* Value */
     , (450530,  27,          2) /* ArmorType - Leather */
     , (450530,  28,        0) /* ArmorLevel */
     , (450530,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450530, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450530,  22, True ) /* Inscribable */
     , (450530,  23, True ) /* DestroyOnSell */
     , (450530,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450530,   5,  -0.033) /* ManaRate */
     , (450530,  12,    0.66) /* Shade */
     , (450530,  13,       1) /* ArmorModVsSlash */
     , (450530,  14,     1.2) /* ArmorModVsPierce */
     , (450530,  15,       1) /* ArmorModVsBludgeon */
     , (450530,  16,     0.6) /* ArmorModVsCold */
     , (450530,  17,     0.6) /* ArmorModVsFire */
     , (450530,  18,     1.5) /* ArmorModVsAcid */
     , (450530,  19,     0.6) /* ArmorModVsElectric */
     , (450530,  39,    1.25) /* DefaultScale */
     , (450530, 110,       1) /* BulkMod */
     , (450530, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450530,   1, 'Kareb Mask') /* Name */
     , (450530,   7, 'Here''s my mask! I love my mask! It''s slimming! Makes me look not so fat! Hey! Where are you going!') /* Inscription */
     , (450530,   8, 'Tekapuapuh') /* ScribeName */
     , (450530,  16, 'A mask carved in tribute to the mythical Tumerok trickster, Karab. Cloaked behind his mysterious visage, you feel capable of taking over the world! But maybe you''ll take a nice nap first.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450530,   1, 0x02000F3E) /* Setup */
     , (450530,   3, 0x20000014) /* SoundTable */
     , (450530,   6, 0x0400007E) /* PaletteBase */
     , (450530,   7, 0x10000474) /* ClothingBase */
     , (450530,   8, 0x06002AFA) /* Icon */
     , (450530,  22, 0x3400002B) /* PhysicsEffectTable */;
