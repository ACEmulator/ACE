DELETE FROM `weenie` WHERE `class_Id` = 1043197;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1043197, 'ace1043197-apostategranddirectorsmask', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1043197,   1,          2) /* ItemType - Armor */
     , (1043197,   3,         14) /* PaletteTemplate - Red */
     , (1043197,   4,      16384) /* ClothingPriority - Head */
     , (1043197,   5,          1) /* EncumbranceVal */
     , (1043197,   9,          1) /* ValidLocations - HeadWear */
     , (1043197,  16,          1) /* ItemUseable - No */
     , (1043197,  18,          1) /* UiEffects - Magical */
     , (1043197,  19,         20) /* Value */
     , (1043197,  28,          1) /* ArmorLevel */
     , (1043197,  33,          1) /* Bonded - Bonded */
     , (1043197,  53,        101) /* PlacementPosition - Resting */
     , (1043197,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1043197, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1043197,  11, True ) /* IgnoreCollisions */
     , (1043197,  13, True ) /* Ethereal */
     , (1043197,  14, True ) /* GravityStatus */
     , (1043197,  19, True ) /* Attackable */
     , (1043197,  22, True ) /* Inscribable */
     , (1043197,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1043197,   5, -0.05000000074505806) /* ManaRate */
     , (1043197,  13,       1) /* ArmorModVsSlash */
     , (1043197,  14,       1) /* ArmorModVsPierce */
     , (1043197,  15,       1) /* ArmorModVsBludgeon */
     , (1043197,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1043197,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1043197,  18, 0.6000000238418579) /* ArmorModVsAcid */
     , (1043197,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1043197, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1043197,   1, 'Apostate Grand Director''s Mask') /* Name */
     , (1043197,  15, 'A mask crafted from a shard of the mask of the Apostate Grand Director.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1043197,   1,   33561077) /* Setup */
     , (1043197,   3,  536870932) /* SoundTable */
     , (1043197,   6,   67108990) /* PaletteBase */
     , (1043197,   7,  268437424) /* ClothingBase */
     , (1043197,   8,  100691483) /* Icon */
     , (1043197,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-21T20:53:32.1013044-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
