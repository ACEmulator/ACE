DELETE FROM `weenie` WHERE `class_Id` = 1046615;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1046615, 'ace1046615-oyoroicoat', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1046615,   1,          2) /* ItemType - Armor */
     , (1046615,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (1046615,   5,       0) /* EncumbranceVal */
     , (1046615,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (1046615,  16,          1) /* ItemUseable - No */
     , (1046615,  19,         20) /* Value */
     , (1046615,  28,        0) /* ArmorLevel */
     , (1046615,  33,          1) /* Bonded - Bonded */
     , (1046615,  53,        101) /* PlacementPosition - Resting */
     , (1046615,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1046615,  11, True ) /* IgnoreCollisions */
     , (1046615,  13, True ) /* Ethereal */
     , (1046615,  14, True ) /* GravityStatus */
     , (1046615,  19, True ) /* Attackable */
     , (1046615,  22, True ) /* Inscribable */
     , (1046615, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1046615,   5, -0.03333330154418945) /* ManaRate */
     , (1046615,  13, 1.2000000476837158) /* ArmorModVsSlash */
     , (1046615,  14,     1.5) /* ArmorModVsPierce */
     , (1046615,  15, 1.2000000476837158) /* ArmorModVsBludgeon */
     , (1046615,  16, 0.6000000238418579) /* ArmorModVsCold */
     , (1046615,  17, 0.6000000238418579) /* ArmorModVsFire */
     , (1046615,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1046615,  19, 0.6000000238418579) /* ArmorModVsElectric */
     , (1046615, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1046615,   1, 'O-Yoroi Coat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1046615,   1,   33554642) /* Setup */
     , (1046615,   3,  536870932) /* SoundTable */
     , (1046615,   6,   67108990) /* PaletteBase */
     , (1046615,   7,  268437555) /* ClothingBase */
     , (1046615,   8,  100692794) /* Icon */
     , (1046615,  22,  872415275) /* PhysicsEffectTable */;


