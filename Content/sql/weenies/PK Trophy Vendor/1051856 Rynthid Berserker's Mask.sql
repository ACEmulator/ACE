DELETE FROM `weenie` WHERE `class_Id` = 1051856;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1051856, 'ace1051856-rynthidberserkersmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1051856,   1,          2) /* ItemType - Armor */
     , (1051856,   3,         14) /* PaletteTemplate - Red */
     , (1051856,   4,      16384) /* ClothingPriority - Head */
     , (1051856,   5,          1) /* EncumbranceVal */
     , (1051856,   9,          1) /* ValidLocations - HeadWear */
     , (1051856,  16,          1) /* ItemUseable - No */
     , (1051856,  18,          1) /* UiEffects - Magical */
     , (1051856,  19,         20) /* Value */
     , (1051856,  28,          1) /* ArmorLevel */
     , (1051856,  33,          1) /* Bonded - Bonded */
     , (1051856,  53,        101) /* PlacementPosition - Resting */
     , (1051856,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1051856, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1051856,  11, True ) /* IgnoreCollisions */
     , (1051856,  13, True ) /* Ethereal */
     , (1051856,  14, True ) /* GravityStatus */
     , (1051856,  19, True ) /* Attackable */
     , (1051856,  22, True ) /* Inscribable */
     , (1051856,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1051856,   5, -0.05000000074505806) /* ManaRate */
     , (1051856,  13,       1) /* ArmorModVsSlash */
     , (1051856,  14,       1) /* ArmorModVsPierce */
     , (1051856,  15,       1) /* ArmorModVsBludgeon */
     , (1051856,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1051856,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1051856,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1051856,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1051856, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1051856,   1, 'Rynthid Berserker''s Mask') /* Name */
     , (1051856,  15, 'A mask crafted from the damaged mask of a Rynthid Berserker.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1051856,   1,   33561593) /* Setup */
     , (1051856,   3,  536870932) /* SoundTable */
     , (1051856,   7,  268437597) /* ClothingBase */
     , (1051856,   8,  100693218) /* Icon */
     , (1051856,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:50:50.6291644-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added palette template int",
  "IsDone": false
}
*/
