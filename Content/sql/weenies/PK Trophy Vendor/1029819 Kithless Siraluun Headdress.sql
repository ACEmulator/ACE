DELETE FROM `weenie` WHERE `class_Id` = 1029819;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029819, 'ace1029819-kithlesssiraluunheaddress', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029819,   1,          2) /* ItemType - Armor */
     , (1029819,   3,         14) /* PaletteTemplate - Red */
     , (1029819,   4,      16384) /* ClothingPriority - Head */
     , (1029819,   5,          1) /* EncumbranceVal */
     , (1029819,   8,        250) /* Mass */
     , (1029819,   9,          1) /* ValidLocations - HeadWear */
     , (1029819,  16,          1) /* ItemUseable - No */
     , (1029819,  18,          1) /* UiEffects - Magical */
     , (1029819,  19,         20) /* Value */
     , (1029819,  27,         32) /* ArmorType - Metal */
     , (1029819,  28,          1) /* ArmorLevel */
     , (1029819,  53,        101) /* PlacementPosition - Resting */
     , (1029819,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029819, 150,        103) /* HookPlacement - Hook */
     , (1029819, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029819,  11, True ) /* IgnoreCollisions */
     , (1029819,  13, True ) /* Ethereal */
     , (1029819,  14, True ) /* GravityStatus */
     , (1029819,  19, True ) /* Attackable */
     , (1029819,  22, True ) /* Inscribable */
     , (1029819,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029819,   5, -0.029999999329447746) /* ManaRate */
     , (1029819,  12, 0.6600000262260437) /* Shade */
     , (1029819, 110,       1) /* BulkMod */
     , (1029819, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029819,   1, 'Kithless Siraluun Headdress') /* Name */
     , (1029819,  16, 'A headdress plaited from the plumes of a Kithless Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029819,   1,   33557288) /* Setup */
     , (1029819,   3,  536870932) /* SoundTable */
     , (1029819,   6,   67108990) /* PaletteBase */
     , (1029819,   7,  268436237) /* ClothingBase */
     , (1029819,   8,  100671999) /* Icon */
     , (1029819,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T10:06:55.9460439-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
