DELETE FROM `weenie` WHERE `class_Id` = 2046345;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2046345, 'ace2046345-oyoroileggings', 2, '2021-12-26 05:40:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2046345,   1,          2) /* ItemType - Armor */
     , (2046345,   3,         39) /* PaletteTemplate - Black */
     , (2046345,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (2046345,   5,       0) /* EncumbranceVal */
     , (2046345,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (2046345,  16,          1) /* ItemUseable - No */
     , (2046345,  19,         20) /* Value */
     , (2046345,  28,          1) /* ArmorLevel */
     , (2046345,  33,          1) /* Bonded - Bonded */
     , (2046345,  53,        101) /* PlacementPosition - Resting */
     , (2046345,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2046345,  11, True ) /* IgnoreCollisions */
     , (2046345,  13, True ) /* Ethereal */
     , (2046345,  14, True ) /* GravityStatus */
     , (2046345,  19, True ) /* Attackable */
     , (2046345,  22, True ) /* Inscribable */
     , (2046345, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2046345,   5,  -0.033) /* ManaRate */
     , (2046345,  12,    0.25) /* Shade */
     , (2046345,  13,     1.2) /* ArmorModVsSlash */
     , (2046345,  14,     1.5) /* ArmorModVsPierce */
     , (2046345,  15,     1.2) /* ArmorModVsBludgeon */
     , (2046345,  16,     0.6) /* ArmorModVsCold */
     , (2046345,  17,     0.6) /* ArmorModVsFire */
     , (2046345,  18,     0.8) /* ArmorModVsAcid */
     , (2046345,  19,     0.6) /* ArmorModVsElectric */
     , (2046345, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2046345,   1, 'O-Yoroi Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2046345,   1, 0x020001A8) /* Setup */
     , (2046345,   3, 0x20000014) /* SoundTable */
     , (2046345,   6, 0x0400007E) /* PaletteBase */
     , (2046345,   7, 0x1000082B) /* ClothingBase */
     , (2046345,   8, 0x06007358) /* Icon */
     , (2046345,  22, 0x3400002B) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2021-12-26T05:36:07.4595754Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-26T05:30:04.1541386Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-26T05:36:07.4591522Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
