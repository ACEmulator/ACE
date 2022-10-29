DELETE FROM `weenie` WHERE `class_Id` = 1029822;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029822, 'ace1029822-strandsiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029822,   1,          2) /* ItemType - Armor */
     , (1029822,   3,         10) /* PaletteTemplate - LightBlue */
     , (1029822,   4,      16384) /* ClothingPriority - Head */
     , (1029822,   5,          1) /* EncumbranceVal */
     , (1029822,   8,        250) /* Mass */
     , (1029822,   9,          1) /* ValidLocations - HeadWear */
     , (1029822,  16,          1) /* ItemUseable - No */
     , (1029822,  18,          1) /* UiEffects - Magical */
     , (1029822,  19,         20) /* Value */
     , (1029822,  27,         32) /* ArmorType - Metal */
     , (1029822,  28,          1) /* ArmorLevel */
     , (1029822,  53,        101) /* PlacementPosition - Resting */
     , (1029822,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029822, 150,        103) /* HookPlacement - Hook */
     , (1029822, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029822,  11, True ) /* IgnoreCollisions */
     , (1029822,  13, True ) /* Ethereal */
     , (1029822,  14, True ) /* GravityStatus */
     , (1029822,  19, True ) /* Attackable */
     , (1029822,  22, True ) /* Inscribable */
     , (1029822,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029822,  12, 0.6600000262260437) /* Shade */
     , (1029822, 110,       1) /* BulkMod */
     , (1029822, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029822,   1, 'Strand Siraluun Headdress') /* Name */
     , (1029822,  16, 'A headdress plaited from the plumes of a Strand Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029822,   1,   33557288) /* Setup */
     , (1029822,   3,  536870932) /* SoundTable */
     , (1029822,   6,   67108990) /* PaletteBase */
     , (1029822,   7,  268436237) /* ClothingBase */
     , (1029822,   8,  100677284) /* Icon */
     , (1029822,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:14:06.7378772-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
