DELETE FROM `weenie` WHERE `class_Id` = 4200104;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200104, 'regaliashohitailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200104,   1,          2) /* ItemType - Armor */
     , (4200104,   3,          4) /* PaletteTemplate - Brown */
     , (4200104,   4,      16384) /* ClothingPriority - Head */
     , (4200104,   5,          1) /* EncumbranceVal */
     , (4200104,   8,         75) /* Mass */
     , (4200104,   9,          1) /* ValidLocations - HeadWear */
     , (4200104,  16,          1) /* ItemUseable - No */
     , (4200104,  19,         20) /* Value */
     , (4200104,  27,          2) /* ArmorType - Leather */
     , (4200104,  28,          1) /* ArmorLevel */
     , (4200104,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200104, 150,        103) /* HookPlacement - Hook */
     , (4200104, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200104,  22, True ) /* Inscribable */
     , (4200104,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200104,   5,  -0.033) /* ManaRate */
     , (4200104,  12,    0.66) /* Shade */
     , (4200104,  13,     1.3) /* ArmorModVsSlash */
     , (4200104,  14,     1.3) /* ArmorModVsPierce */
     , (4200104,  15,       1) /* ArmorModVsBludgeon */
     , (4200104,  16,     1.5) /* ArmorModVsCold */
     , (4200104,  17,       1) /* ArmorModVsFire */
     , (4200104,  18,     1.5) /* ArmorModVsAcid */
     , (4200104,  19,     1.2) /* ArmorModVsElectric */
     , (4200104, 110,       1) /* BulkMod */
     , (4200104, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200104,   1, 'Koji''s Beast') /* Name */
     , (4200104,  16, 'A mask made with masterful craftsmanship. It has been sculpted into the face of a deadly Ogre Magi that Koji once encountered on her travels.') /* LongDesc */
     , (4200104,  19, 'Sho') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200104,   1, 0x02000B8A) /* Setup */
     , (4200104,   3, 0x20000014) /* SoundTable */
     , (4200104,   6, 0x0400007E) /* PaletteBase */
     , (4200104,   7, 0x10000341) /* ClothingBase */
     , (4200104,   8, 0x060022DA) /* Icon */
     , (4200104,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200104,  37,         13) /* ItemSkillLimit - UnarmedCombat */;
