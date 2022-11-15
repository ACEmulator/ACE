DELETE FROM `weenie` WHERE `class_Id` = 2046553;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2046553, 'ace2046553-oyoroisandals', 2, '2021-12-26 05:41:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2046553,   1,          2) /* ItemType - Armor */
     , (2046553,   3,         39) /* PaletteTemplate - Black */
     , (2046553,   4,      65536) /* ClothingPriority - Feet */
     , (2046553,   5,        0) /* EncumbranceVal */
     , (2046553,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (2046553,  16,          1) /* ItemUseable - No */
     , (2046553,  19,         20) /* Value */
     , (2046553,  28,          0) /* ArmorLevel */
     , (2046553,  33,          1) /* Bonded - Bonded */
     , (2046553,  53,        101) /* PlacementPosition - Resting */
     , (2046553,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2046553,  11, True ) /* IgnoreCollisions */
     , (2046553,  13, True ) /* Ethereal */
     , (2046553,  14, True ) /* GravityStatus */
     , (2046553,  19, True ) /* Attackable */
     , (2046553,  22, True ) /* Inscribable */
     , (2046553, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2046553,   5,  -0.033) /* ManaRate */
     , (2046553,  12,    0.25) /* Shade */
     , (2046553,  13,     1.2) /* ArmorModVsSlash */
     , (2046553,  14,     1.5) /* ArmorModVsPierce */
     , (2046553,  15,     1.2) /* ArmorModVsBludgeon */
     , (2046553,  16,     0.6) /* ArmorModVsCold */
     , (2046553,  17,     0.6) /* ArmorModVsFire */
     , (2046553,  18,     0.8) /* ArmorModVsAcid */
     , (2046553,  19,     0.6) /* ArmorModVsElectric */
     , (2046553, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2046553,   1, 'O-Yoroi Sandals') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2046553,   1, 0x020000DE) /* Setup */
     , (2046553,   3, 0x20000014) /* SoundTable */
     , (2046553,   6, 0x0400007E) /* PaletteBase */
     , (2046553,   7, 0x10000830) /* ClothingBase */
     , (2046553,   8, 0x060031B9) /* Icon */
     , (2046553,  22, 0x3400002B) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2021-12-26T05:35:50.7726417Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-26T05:30:24.0686398Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-26T05:35:50.7721243Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
