DELETE FROM `weenie` WHERE `class_Id` = 1029821;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029821, 'ace1029821-marshsiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029821,   1,          2) /* ItemType - Armor */
     , (1029821,   3,          1) /* PaletteTemplate - AquaBlue */
     , (1029821,   4,      16384) /* ClothingPriority - Head */
     , (1029821,   5,          1) /* EncumbranceVal */
     , (1029821,   8,        250) /* Mass */
     , (1029821,   9,          1) /* ValidLocations - HeadWear */
     , (1029821,  16,          1) /* ItemUseable - No */
     , (1029821,  18,          1) /* UiEffects - Magical */
     , (1029821,  19,         20) /* Value */
     , (1029821,  27,         32) /* ArmorType - Metal */
     , (1029821,  28,          1) /* ArmorLevel */
     , (1029821,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029821, 150,        103) /* HookPlacement - Hook */
     , (1029821, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029821,  22, True ) /* Inscribable */
     , (1029821,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029821,  12, 0.6600000262260437) /* Shade */
     , (1029821, 110,       1) /* BulkMod */
     , (1029821, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029821,   1, 'Marsh Siraluun Headdress') /* Name */
     , (1029821,  16, 'A headdress plaited from the plumes of a Marsh Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029821,   1,   33557288) /* Setup */
     , (1029821,   3,  536870932) /* SoundTable */
     , (1029821,   6,   67108990) /* PaletteBase */
     , (1029821,   7,  268436237) /* ClothingBase */
     , (1029821,   8,  100677285) /* Icon */
     , (1029821,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:13:26.9181818-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
