DELETE FROM `weenie` WHERE `class_Id` = 1029824;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029824, 'ace1029824-timbersiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029824,   1,          2) /* ItemType - Armor */
     , (1029824,   3,          8) /* PaletteTemplate - Green */
     , (1029824,   4,      16384) /* ClothingPriority - Head */
     , (1029824,   5,          1) /* EncumbranceVal */
     , (1029824,   8,        250) /* Mass */
     , (1029824,   9,          1) /* ValidLocations - HeadWear */
     , (1029824,  16,          1) /* ItemUseable - No */
     , (1029824,  18,          1) /* UiEffects - Magical */
     , (1029824,  19,         20) /* Value */
     , (1029824,  27,         32) /* ArmorType - Metal */
     , (1029824,  28,          1) /* ArmorLevel */
     , (1029824,  53,        101) /* PlacementPosition - Resting */
     , (1029824,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029824, 150,        103) /* HookPlacement - Hook */
     , (1029824, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029824,  11, True ) /* IgnoreCollisions */
     , (1029824,  13, True ) /* Ethereal */
     , (1029824,  14, True ) /* GravityStatus */
     , (1029824,  19, True ) /* Attackable */
     , (1029824,  22, True ) /* Inscribable */
     , (1029824,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029824,  12, 0.6600000262260437) /* Shade */
     , (1029824, 110,       1) /* BulkMod */
     , (1029824, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029824,   1, 'Timber Siraluun Headdress') /* Name */
     , (1029824,  16, 'A headdress plaited from the plumes of a Timber Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029824,   1,   33557288) /* Setup */
     , (1029824,   3,  536870932) /* SoundTable */
     , (1029824,   6,   67108990) /* PaletteBase */
     , (1029824,   7,  268436237) /* ClothingBase */
     , (1029824,   8,  100677282) /* Icon */
     , (1029824,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:15:13.4152672-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
