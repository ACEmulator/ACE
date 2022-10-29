DELETE FROM `weenie` WHERE `class_Id` = 1029820;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029820, 'ace1029820-littoralsiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029820,   1,          2) /* ItemType - Armor */
     , (1029820,   3,         17) /* PaletteTemplate - Yellow */
     , (1029820,   4,      16384) /* ClothingPriority - Head */
     , (1029820,   5,          1) /* EncumbranceVal */
     , (1029820,   8,        250) /* Mass */
     , (1029820,   9,          1) /* ValidLocations - HeadWear */
     , (1029820,  16,          1) /* ItemUseable - No */
     , (1029820,  18,          1) /* UiEffects - Magical */
     , (1029820,  19,         20) /* Value */
     , (1029820,  27,         32) /* ArmorType - Metal */
     , (1029820,  28,          1) /* ArmorLevel */
     , (1029820,  53,        101) /* PlacementPosition - Resting */
     , (1029820,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029820, 150,        103) /* HookPlacement - Hook */
     , (1029820, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029820,  11, True ) /* IgnoreCollisions */
     , (1029820,  13, True ) /* Ethereal */
     , (1029820,  14, True ) /* GravityStatus */
     , (1029820,  19, True ) /* Attackable */
     , (1029820,  22, True ) /* Inscribable */
     , (1029820,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029820,  12, 0.6600000262260437) /* Shade */
     , (1029820, 110,       1) /* BulkMod */
     , (1029820, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029820,   1, 'Littoral Siraluun Headdress') /* Name */
     , (1029820,  16, 'A headdress plaited from the plumes of a Littoral Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029820,   1,   33557288) /* Setup */
     , (1029820,   3,  536870932) /* SoundTable */
     , (1029820,   6,   67108990) /* PaletteBase */
     , (1029820,   7,  268436237) /* ClothingBase */
     , (1029820,   8,  100677286) /* Icon */
     , (1029820,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:08:00.1943388-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
