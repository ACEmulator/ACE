DELETE FROM `weenie` WHERE `class_Id` = 2046615;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2046615, 'ace2046615-oyoroicoat', 2, '2021-12-26 05:40:42') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2046615,   1,          2) /* ItemType - Armor */
     , (2046615,   3,         39) /* PaletteTemplate - Black */
     , (2046615,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (2046615,   5,       0) /* EncumbranceVal */
     , (2046615,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (2046615,  16,          1) /* ItemUseable - No */
     , (2046615,  19,         20) /* Value */
     , (2046615,  28,          0) /* ArmorLevel */
     , (2046615,  33,          1) /* Bonded - Bonded */
     , (2046615,  53,        101) /* PlacementPosition - Resting */
     , (2046615,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2046615,  11, True ) /* IgnoreCollisions */
     , (2046615,  13, True ) /* Ethereal */
     , (2046615,  14, True ) /* GravityStatus */
     , (2046615,  19, True ) /* Attackable */
     , (2046615,  22, True ) /* Inscribable */
     , (2046615, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (2046615,   5,  -0.033) /* ManaRate */
     , (2046615,  12,    0.25) /* Shade */
     , (2046615,  13,     1.2) /* ArmorModVsSlash */
     , (2046615,  14,     1.5) /* ArmorModVsPierce */
     , (2046615,  15,     1.2) /* ArmorModVsBludgeon */
     , (2046615,  16,     0.6) /* ArmorModVsCold */
     , (2046615,  17,     0.6) /* ArmorModVsFire */
     , (2046615,  18,     0.8) /* ArmorModVsAcid */
     , (2046615,  19,     0.6) /* ArmorModVsElectric */
     , (2046615, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2046615,   1, 'O-Yoroi Coat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2046615,   1, 0x020000D2) /* Setup */
     , (2046615,   3, 0x20000014) /* SoundTable */
     , (2046615,   6, 0x0400007E) /* PaletteBase */
     , (2046615,   7, 0x10000833) /* ClothingBase */
     , (2046615,   8, 0x0600733A) /* Icon */
     , (2046615,  22, 0x3400002B) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2021-12-26T05:36:03.6596003Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-26T05:30:11.2363364Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-26T05:36:03.659208Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
