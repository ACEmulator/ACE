DELETE FROM `weenie` WHERE `class_Id` = 1029825;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029825, 'ace1029825-untamedsiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029825,   1,          2) /* ItemType - Armor */
     , (1029825,   3,          2) /* PaletteTemplate - Blue */
     , (1029825,   4,      16384) /* ClothingPriority - Head */
     , (1029825,   5,          1) /* EncumbranceVal */
     , (1029825,   8,        250) /* Mass */
     , (1029825,   9,          1) /* ValidLocations - HeadWear */
     , (1029825,  16,          1) /* ItemUseable - No */
     , (1029825,  18,          1) /* UiEffects - Magical */
     , (1029825,  19,         20) /* Value */
     , (1029825,  27,         32) /* ArmorType - Metal */
     , (1029825,  28,          1) /* ArmorLevel */
     , (1029825,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029825, 150,        103) /* HookPlacement - Hook */
     , (1029825, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029825,  22, True ) /* Inscribable */
     , (1029825,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029825,  12, 0.6600000262260437) /* Shade */
     , (1029825, 110,       1) /* BulkMod */
     , (1029825, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029825,   1, 'Untamed Siraluun Headdress') /* Name */
     , (1029825,  16, 'A headdress plaited from the plumes of an Untamed Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029825,   1,   33557288) /* Setup */
     , (1029825,   3,  536870932) /* SoundTable */
     , (1029825,   6,   67108990) /* PaletteBase */
     , (1029825,   7,  268436237) /* ClothingBase */
     , (1029825,   8,  100677281) /* Icon */
     , (1029825,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:15:47.4324098-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
