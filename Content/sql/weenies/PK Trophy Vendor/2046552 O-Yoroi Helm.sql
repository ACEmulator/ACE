DELETE FROM `weenie` WHERE `class_Id` = 2046552;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2046552, 'ace2046552-oyoroihelm', 2, '2021-12-26 05:40:57') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2046552,   1,          2) /* ItemType - Armor */
     , (2046552,   3,         39) /* PaletteTemplate - Black */
     , (2046552,   4,      16384) /* ClothingPriority - Head */
     , (2046552,   5,        533) /* EncumbranceVal */
     , (2046552,   9,          1) /* ValidLocations - HeadWear */
     , (2046552,  16,          1) /* ItemUseable - No */
     , (2046552,  19,         20) /* Value */
     , (2046552,  28,          0) /* ArmorLevel */
     , (2046552,  33,          1) /* Bonded - Bonded */
     , (2046552,  53,        101) /* PlacementPosition - Resting */
     , (2046552,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2046552, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2046552,  11, True ) /* IgnoreCollisions */
     , (2046552,  13, True ) /* Ethereal */
     , (2046552,  14, True ) /* GravityStatus */
     , (2046552,  19, True ) /* Attackable */
     , (2046552,  22, True ) /* Inscribable */
     , (2046552, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2046552,   5,  -0.033) /* ManaRate */
     , (2046552,  12,    0.25) /* Shade */
     , (2046552,  13,     2.9) /* ArmorModVsSlash */
     , (2046552,  14,     3.2) /* ArmorModVsPierce */
     , (2046552,  15,     2.9) /* ArmorModVsBludgeon */
     , (2046552,  16,     2.3) /* ArmorModVsCold */
     , (2046552,  17,     2.3) /* ArmorModVsFire */
     , (2046552,  18,     2.5) /* ArmorModVsAcid */
     , (2046552,  19,     2.3) /* ArmorModVsElectric */
     , (2046552, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2046552,   1, 'O-Yoroi Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2046552,   1, 0x02000330) /* Setup */
     , (2046552,   3, 0x20000014) /* SoundTable */
     , (2046552,   6, 0x0400007E) /* PaletteBase */
     , (2046552,   7, 0x1000082F) /* ClothingBase */
     , (2046552,   8, 0x0600734E) /* Icon */
     , (2046552,  22, 0x3400002B) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2021-12-26T05:35:56.6780934Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-26T05:30:20.4233322Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-26T05:35:56.6774333Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
