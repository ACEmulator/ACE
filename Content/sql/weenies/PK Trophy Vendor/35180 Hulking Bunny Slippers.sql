DELETE FROM `weenie` WHERE `class_Id` = 35180;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (35180, 'ace35180-hulkingbunnyslippers', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (35180,   1,          4) /* ItemType - Clothing */
     , (35180,   3,          6) /* PaletteTemplate - DeepBrown */
     , (35180,   4,      65536) /* ClothingPriority - Feet */
     , (35180,   5,        500) /* EncumbranceVal */
     , (35180,   9,        256) /* ValidLocations - FootWear */
     , (35180,  16,          1) /* ItemUseable - No */
     , (35180,  19,         20) /* Value */
     , (35180,  28,         50) /* ArmorLevel */
     , (35180,  53,        101) /* PlacementPosition - Resting */
     , (35180,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (35180, 106,         30) /* ItemSpellcraft */
     , (35180, 107,        397) /* ItemCurMana */
     , (35180, 108,        500) /* ItemMaxMana */
     , (35180, 109,        225) /* ItemDifficulty */
     , (35180, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (35180,  11, True ) /* IgnoreCollisions */
     , (35180,  13, True ) /* Ethereal */
     , (35180,  14, True ) /* GravityStatus */
     , (35180,  19, True ) /* Attackable */
     , (35180,  22, True ) /* Inscribable */
     , (35180,  69, False) /* IsSellable */
     , (35180, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (35180,   5, -0.033330000936985016) /* ManaRate */
     , (35180,  13,     0.5) /* ArmorModVsSlash */
     , (35180,  14,     0.5) /* ArmorModVsPierce */
     , (35180,  15,     0.5) /* ArmorModVsBludgeon */
     , (35180,  16, 1.2999999523162842) /* ArmorModVsCold */
     , (35180,  17, 0.4000000059604645) /* ArmorModVsFire */
     , (35180,  18, 0.4000000059604645) /* ArmorModVsAcid */
     , (35180,  19, 0.4000000059604645) /* ArmorModVsElectric */
     , (35180,  39,       2) /* DefaultScale */
     , (35180, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (35180,   1, 'Hulking Bunny Slippers') /* Name */
     , (35180,  16, 'A pair of hulking bunny slippers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (35180,   1,   33557435) /* Setup */
     , (35180,   6,   67108990) /* PaletteBase */
     , (35180,   7,  268437202) /* ClothingBase */
     , (35180,   8,  100672378) /* Icon */
     , (35180,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (35180,  2257,      2)  /* Jahannan's Blessing */
     , (35180,  2301,      2)  /* Saladur's Blessing */
     , (35180,  2529,      2)  /* Major Sprint */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T12:40:00.7353868-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Marked Done",
  "IsDone": true
}
*/
