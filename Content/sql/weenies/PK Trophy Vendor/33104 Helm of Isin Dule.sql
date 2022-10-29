DELETE FROM `weenie` WHERE `class_Id` = 33104;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33104, 'ace33104-helmofisindule', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33104,   1,          2) /* ItemType - Armor */
     , (33104,   3,         39) /* PaletteTemplate - Black */
     , (33104,   4,      16384) /* ClothingPriority - Head */
     , (33104,   5,        400) /* EncumbranceVal */
     , (33104,   9,          1) /* ValidLocations - HeadWear */
     , (33104,  16,          1) /* ItemUseable - No */
     , (33104,  19,         20) /* Value */
     , (33104,  28,          1) /* ArmorLevel */
     , (33104,  33,          0) /* Bonded - Normal */
     , (33104,  53,        101) /* PlacementPosition - Resting */
     , (33104,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (33104, 106,        450) /* ItemSpellcraft */
     , (33104, 107,       7993) /* ItemCurMana */
     , (33104, 108,       8000) /* ItemMaxMana */
     , (33104, 109,        250) /* ItemDifficulty */
     , (33104, 114,          0) /* Attuned - Normal */
     , (33104, 151,          2) /* HookType - Wall */
     , (33104, 158,          7) /* WieldRequirements - Level */
     , (33104, 159,          1) /* WieldSkillType - Axe */
     , (33104, 160,        150) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33104,  11, True ) /* IgnoreCollisions */
     , (33104,  13, True ) /* Ethereal */
     , (33104,  14, True ) /* GravityStatus */
     , (33104,  19, True ) /* Attackable */
     , (33104,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33104,   5, -0.05000000074505806) /* ManaRate */
     , (33104,  13, 1.399999976158142) /* ArmorModVsSlash */
     , (33104,  14, 1.2000000476837158) /* ArmorModVsPierce */
     , (33104,  15, 1.2000000476837158) /* ArmorModVsBludgeon */
     , (33104,  16, 0.800000011920929) /* ArmorModVsCold */
     , (33104,  17, 1.399999976158142) /* ArmorModVsFire */
     , (33104,  18,       1) /* ArmorModVsAcid */
     , (33104,  19, 0.800000011920929) /* ArmorModVsElectric */
     , (33104, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33104,   1, 'Helm of Isin Dule') /* Name */
     , (33104,  16, 'A black, crystalline helm created by the Shadow, Isin Dule.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33104,   1,   33559922) /* Setup */
     , (33104,   3,  536870932) /* SoundTable */
     , (33104,   7,  268437120) /* ClothingBase */
     , (33104,   8,  100688917) /* Icon */
     , (33104,  22,  872415275) /* PhysicsEffectTable */;

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
