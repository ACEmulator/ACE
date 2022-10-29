DELETE FROM `weenie` WHERE `class_Id` = 1032160;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1032160, 'ace1032160-uberpenguinmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1032160,   1,          2) /* ItemType - Armor */
     , (1032160,   3,          4) /* PaletteTemplate - Brown */
     , (1032160,   4,      16384) /* ClothingPriority - Head */
     , (1032160,   5,          1) /* EncumbranceVal */
     , (1032160,   8,         75) /* Mass */
     , (1032160,   9,          1) /* ValidLocations - HeadWear */
     , (1032160,  16,          1) /* ItemUseable - No */
     , (1032160,  19,         20) /* Value */
     , (1032160,  27,         32) /* ArmorType - Metal */
     , (1032160,  28,          1) /* ArmorLevel */
     , (1032160,  53,        101) /* PlacementPosition - Resting */
     , (1032160,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1032160, 150,        103) /* HookPlacement - Hook */
     , (1032160, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1032160,  11, True ) /* IgnoreCollisions */
     , (1032160,  13, True ) /* Ethereal */
     , (1032160,  14, True ) /* GravityStatus */
     , (1032160,  19, True ) /* Attackable */
     , (1032160,  22, True ) /* Inscribable */
     , (1032160,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1032160,  12, 0.6600000262260437) /* Shade */
     , (1032160, 110,       1) /* BulkMod */
     , (1032160, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1032160,   1, 'Uber Penguin Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1032160,   1,   33559768) /* Setup */
     , (1032160,   3,  536870932) /* SoundTable */
     , (1032160,   6,   67108990) /* PaletteBase */
     , (1032160,   7,  268437073) /* ClothingBase */
     , (1032160,   8,  100688480) /* Icon */
     , (1032160,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T23:43:12.0014582-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Updated EoR",
  "IsDone": false
}
*/
