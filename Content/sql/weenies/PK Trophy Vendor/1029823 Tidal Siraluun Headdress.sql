DELETE FROM `weenie` WHERE `class_Id` = 1029823;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029823, 'ace1029823-tidalsiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029823,   1,          2) /* ItemType - Armor */
     , (1029823,   3,          9) /* PaletteTemplate - Grey */
     , (1029823,   4,      16384) /* ClothingPriority - Head */
     , (1029823,   5,          1) /* EncumbranceVal */
     , (1029823,   8,        250) /* Mass */
     , (1029823,   9,          1) /* ValidLocations - HeadWear */
     , (1029823,  16,          1) /* ItemUseable - No */
     , (1029823,  18,          1) /* UiEffects - Magical */
     , (1029823,  19,         20) /* Value */
     , (1029823,  27,         32) /* ArmorType - Metal */
     , (1029823,  28,          1) /* ArmorLevel */
     , (1029823,  53,        101) /* PlacementPosition - Resting */
     , (1029823,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029823, 150,        103) /* HookPlacement - Hook */
     , (1029823, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029823,  11, True ) /* IgnoreCollisions */
     , (1029823,  13, True ) /* Ethereal */
     , (1029823,  14, True ) /* GravityStatus */
     , (1029823,  19, True ) /* Attackable */
     , (1029823,  22, True ) /* Inscribable */
     , (1029823,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029823,  12, 0.6600000262260437) /* Shade */
     , (1029823, 110,       1) /* BulkMod */
     , (1029823, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029823,   1, 'Tidal Siraluun Headdress') /* Name */
     , (1029823,  16, 'A headdress plaited from the plumes of a Tidal Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029823,   1,   33557288) /* Setup */
     , (1029823,   3,  536870932) /* SoundTable */
     , (1029823,   6,   67108990) /* PaletteBase */
     , (1029823,   7,  268436237) /* ClothingBase */
     , (1029823,   8,  100677283) /* Icon */
     , (1029823,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:14:39.5549439-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
