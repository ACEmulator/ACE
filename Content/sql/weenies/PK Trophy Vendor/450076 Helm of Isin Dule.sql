DELETE FROM `weenie` WHERE `class_Id` = 450076;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450076, 'ace450076-helmofisinduletailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450076,   1,          2) /* ItemType - Armor */
     , (450076,   3,         39) /* PaletteTemplate - Black */
     , (450076,   4,      16384) /* ClothingPriority - Head */
     , (450076,   5,        0) /* EncumbranceVal */
     , (450076,   9,          1) /* ValidLocations - HeadWear */
     , (450076,  16,          1) /* ItemUseable - No */
     , (450076,  19,         20) /* Value */
     , (450076,  28,          1) /* ArmorLevel */
     , (450076,  33,          0) /* Bonded - Normal */
     , (450076,  53,        101) /* PlacementPosition - Resting */
     , (450076,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450076, 109,        250) /* ItemDifficulty */
     , (450076, 114,          0) /* Attuned - Normal */
     , (450076, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450076,  11, True ) /* IgnoreCollisions */
     , (450076,  13, True ) /* Ethereal */
     , (450076,  14, True ) /* GravityStatus */
     , (450076,  19, True ) /* Attackable */
     , (450076,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450076,   5, -0.05000000074505806) /* ManaRate */
     , (450076,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (450076,  14, 1.2000000476837158) /* ArmorModVsPierce */
     , (450076,  15, 1.2000000476837158) /* ArmorModVsBludgeon */
     , (450076,  16, 0.800000011920929) /* ArmorModVsCold */
     , (450076,  17, 1.399999976158142) /* ArmorModVsFire */
     , (450076,  18,       1) /* ArmorModVsAcid */
     , (450076,  19, 0.800000011920929) /* ArmorModVsElectric */
     , (450076, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450076,   1, 'Helm of Isin Dule') /* Name */
     , (450076,  16, 'A black, crystalline helm created by the Shadow, Isin Dule.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450076,   1,   33559922) /* Setup */
     , (450076,   3,  536870932) /* SoundTable */
     , (450076,   7,  268437120) /* ClothingBase */
     , (450076,   8,  100688917) /* Icon */
     , (450076,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2019-07-30T12:01:23.9594919-04:00",
  "ModifiedBy": "Scribble",
  "Changelog": [
    {
      "created": "2019-07-30T12:01:28.6941083-04:00",
      "author": "Scribble",
      "comment": "Added int 3"
    }
  ],
  "UserChangeSummary": "Added int 3",
  "IsDone": false
}
*/
