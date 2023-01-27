DELETE FROM `weenie` WHERE `class_Id` = 2046551;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2046551, 'ace2046551-oyoroigauntlets', 2, '2021-12-26 05:40:54') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2046551,   1,          2) /* ItemType - Armor */
     , (2046551,   3,         39) /* PaletteTemplate - Black */
     , (2046551,   4,      32768) /* ClothingPriority - Hands */
     , (2046551,   5,        0) /* EncumbranceVal */
     , (2046551,   9,         32) /* ValidLocations - HandWear */
     , (2046551,  16,          1) /* ItemUseable - No */
     , (2046551,  19,         20) /* Value */
     , (2046551,  27,          0) /* ArmorType - None */
     , (2046551,  28,          0) /* ArmorLevel */
     , (2046551,  33,          1) /* Bonded - Bonded */
     , (2046551,  53,        101) /* PlacementPosition - Resting */
     , (2046551,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2046551,  11, True ) /* IgnoreCollisions */
     , (2046551,  13, True ) /* Ethereal */
     , (2046551,  14, True ) /* GravityStatus */
     , (2046551,  19, True ) /* Attackable */
     , (2046551,  22, True ) /* Inscribable */
     , (2046551, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2046551,   5,  -0.033) /* ManaRate */
     , (2046551,  12,    0.25) /* Shade */
     , (2046551,  13,     2.9) /* ArmorModVsSlash */
     , (2046551,  14,     3.2) /* ArmorModVsPierce */
     , (2046551,  15,     2.9) /* ArmorModVsBludgeon */
     , (2046551,  16,     2.3) /* ArmorModVsCold */
     , (2046551,  17,     2.3) /* ArmorModVsFire */
     , (2046551,  18,     2.5) /* ArmorModVsAcid */
     , (2046551,  19,     2.3) /* ArmorModVsElectric */
     , (2046551, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2046551,   1, 'O-Yoroi Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2046551,   1, 0x020000D8) /* Setup */
     , (2046551,   3, 0x20000014) /* SoundTable */
     , (2046551,   6, 0x0400007E) /* PaletteBase */
     , (2046551,   7, 0x1000082E) /* ClothingBase */
     , (2046551,   8, 0x06003193) /* Icon */
     , (2046551,  22, 0x3400002B) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2021-12-26T05:35:58.6947173Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-26T05:30:17.4789902Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-26T05:35:58.6942828Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
