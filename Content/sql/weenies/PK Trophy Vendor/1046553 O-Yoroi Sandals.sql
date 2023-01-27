DELETE FROM `weenie` WHERE `class_Id` = 1046553;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1046553, 'ace1046553-oyoroisandals', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1046553,   1,          2) /* ItemType - Armor */
     , (1046553,   4,      65536) /* ClothingPriority - Feet */
     , (1046553,   5,        0) /* EncumbranceVal */
     , (1046553,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (1046553,  16,          1) /* ItemUseable - No */
     , (1046553,  19,         20) /* Value */
     , (1046553,  28,          1) /* ArmorLevel */
     , (1046553,  33,          1) /* Bonded - Bonded */
     , (1046553,  53,        101) /* PlacementPosition - Resting */
     , (1046553,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1046553,  11, True ) /* IgnoreCollisions */
     , (1046553,  13, True ) /* Ethereal */
     , (1046553,  14, True ) /* GravityStatus */
     , (1046553,  19, True ) /* Attackable */
     , (1046553,  22, True ) /* Inscribable */
     , (1046553, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1046553,   5, -0.03333330154418945) /* ManaRate */
     , (1046553,  13, 1.2000000476837158) /* ArmorModVsSlash */
     , (1046553,  14,     1.5) /* ArmorModVsPierce */
     , (1046553,  15, 1.2000000476837158) /* ArmorModVsBludgeon */
     , (1046553,  16, 0.6000000238418579) /* ArmorModVsCold */
     , (1046553,  17, 0.6000000238418579) /* ArmorModVsFire */
     , (1046553,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1046553,  19, 0.6000000238418579) /* ArmorModVsElectric */
     , (1046553, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1046553,   1, 'O-Yoroi Sandals') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1046553,   1,   33554654) /* Setup */
     , (1046553,   3,  536870932) /* SoundTable */
     , (1046553,   6,   67108990) /* PaletteBase */
     , (1046553,   7,  268437552) /* ClothingBase */
     , (1046553,   8,  100676025) /* Icon */
     , (1046553,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T17:59:47.1874686-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": false
}
*/
