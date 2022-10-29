DELETE FROM `weenie` WHERE `class_Id` = 1029818;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029818, 'ace1029818-badlandssiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029818,   1,          2) /* ItemType - Armor */
     , (1029818,   3,         16) /* PaletteTemplate - Rose */
     , (1029818,   4,      16384) /* ClothingPriority - Head */
     , (1029818,   5,          1) /* EncumbranceVal */
     , (1029818,   8,        250) /* Mass */
     , (1029818,   9,          1) /* ValidLocations - HeadWear */
     , (1029818,  16,          1) /* ItemUseable - No */
     , (1029818,  18,          1) /* UiEffects - Magical */
     , (1029818,  19,         20) /* Value */
     , (1029818,  27,         32) /* ArmorType - Metal */
     , (1029818,  28,          1) /* ArmorLevel */
     , (1029818,  53,        101) /* PlacementPosition - Resting */
     , (1029818,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029818, 150,        103) /* HookPlacement - Hook */
     , (1029818, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029818,  11, True ) /* IgnoreCollisions */
     , (1029818,  13, True ) /* Ethereal */
     , (1029818,  14, True ) /* GravityStatus */
     , (1029818,  19, True ) /* Attackable */
     , (1029818,  22, True ) /* Inscribable */
     , (1029818,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029818,  12, 0.6600000262260437) /* Shade */
     , (1029818, 110,       1) /* BulkMod */
     , (1029818, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029818,   1, 'Badlands Siraluun Headdress') /* Name */
     , (1029818,  16, 'A headdress plaited from the plumes of a Badlands Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029818,   1,   33557288) /* Setup */
     , (1029818,   3,  536870932) /* SoundTable */
     , (1029818,   6,   67108990) /* PaletteBase */
     , (1029818,   7,  268436237) /* ClothingBase */
     , (1029818,   8,  100677287) /* Icon */
     , (1029818,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:03:03.6678682-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
