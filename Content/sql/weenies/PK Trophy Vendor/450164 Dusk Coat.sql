DELETE FROM `weenie` WHERE `class_Id` = 450164;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450164, 'coatraredusktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450164,   1,          2) /* ItemType - Armor */
     , (450164,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450164,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450164,   5,        0) /* EncumbranceVal */
     , (450164,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450164,  16,          1) /* ItemUseable - No */
     , (450164,  19,      20) /* Value */
     , (450164,  27,          4) /* ArmorType - StuddedLeather */
     , (450164,  28,        0) /* ArmorLevel */
     , (450164,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450164, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450164,  11, True ) /* IgnoreCollisions */
     , (450164,  13, True ) /* Ethereal */
     , (450164,  14, True ) /* GravityStatus */
     , (450164,  19, True ) /* Attackable */
     , (450164,  22, True ) /* Inscribable */
     , (450164,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450164,   5,  -0.033) /* ManaRate */
     , (450164,  13,     1.1) /* ArmorModVsSlash */
     , (450164,  14,     1.1) /* ArmorModVsPierce */
     , (450164,  15,     1.2) /* ArmorModVsBludgeon */
     , (450164,  16,     1.1) /* ArmorModVsCold */
     , (450164,  17,     1.2) /* ArmorModVsFire */
     , (450164,  18,     1.3) /* ArmorModVsAcid */
     , (450164,  19,       1) /* ArmorModVsElectric */
     , (450164, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450164,   1, 'Dusk Coat') /* Name */
     , (450164,  16, 'It is said that every great craftsman has a moment of inspiration. If only for a short period of time, they are possessed by a divine spirit, and they are able to create an object of such beauty and quality that they can never in their lifetime hope to surpass. This coat, along with the Dusk Leggings, is Leyrale Shalorn''s master work. The great tailor hung up his needle and thread after finishing the set, sold them to a wealthy nobleman and retired to a life of fishing.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450164,   1, 0x02001395) /* Setup */
     , (450164,   3, 0x20000014) /* SoundTable */
     , (450164,   6, 0x04000BEF) /* PaletteBase */
     , (450164,   7, 0x100005FC) /* ClothingBase */
     , (450164,   8, 0x06005C39) /* Icon */
     , (450164,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450164,  52, 0x06005B0C) /* IconUnderlay */;

