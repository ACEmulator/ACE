DELETE FROM `weenie` WHERE `class_Id` = 1030372;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030372, 'ace1030372-shieldofengorgement', 1, '2021-11-20 00:19:18') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030372,   1,          2) /* ItemType - Armor */
     , (1030372,   3,          4) /* PaletteTemplate - Brown */
     , (1030372,   4,      65536) /* ClothingPriority - Feet */
     , (1030372,   5,          0) /* EncumbranceVal */
     , (1030372,   8,         90) /* Mass */
     , (1030372,   9,    2097152) /* ValidLocations - Shield */
     , (1030372,  16,          1) /* ItemUseable - No */
     , (1030372,  19,         20) /* Value */
     , (1030372,  27,          2) /* ArmorType - Leather */
     , (1030372,  28,          1) /* ArmorLevel */
     , (1030372,  51,          4) /* CombatUse - Shield */
     , (1030372,  52,          3) /* ParentLocation - Shield */
     , (1030372,  53,        101) /* PlacementPosition - Resting */
     , (1030372,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030372, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (1030372,   4,          0) /* ItemTotalXp */
     , (1030372,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030372,  11, True ) /* IgnoreCollisions */
     , (1030372,  13, True ) /* Ethereal */
     , (1030372,  14, True ) /* GravityStatus */
     , (1030372,  19, True ) /* Attackable */
     , (1030372,  22, True ) /* Inscribable */
     , (1030372,  91, False) /* Retained */
     , (1030372, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030372,   5, -0.03333330154418945) /* ManaRate */
     , (1030372,  12, 0.6600000262260437) /* Shade */
     , (1030372,  13, 0.8999999761581421) /* ArmorModVsSlash */
     , (1030372,  14, 0.8999999761581421) /* ArmorModVsPierce */
     , (1030372,  15, 0.8999999761581421) /* ArmorModVsBludgeon */
     , (1030372,  16, 1.100000023841858) /* ArmorModVsCold */
     , (1030372,  17, 1.399999976158142) /* ArmorModVsFire */
     , (1030372,  18, 1.2999999523162842) /* ArmorModVsAcid */
     , (1030372,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1030372, 110, 1.6699999570846558) /* BulkMod */
     , (1030372, 111,       1) /* SizeMod */
     , (1030372, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030372,   1, 'Shield of Engorgement') /* Name */
     , (1030372,  16, 'At first glance this shield seems to be of primitive make. But when exposed to magic, the emblems on the shield writhe and glow, helping to resist any magic. Its very presence on the arm seems to make the bearer more able to withstand magical attacks.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030372,   1,   33559420) /* Setup */
     , (1030372,   3,  536870932) /* SoundTable */
     , (1030372,   6,   67108990) /* PaletteBase */
     , (1030372,   8,  100686841) /* Icon */
     , (1030372,  22,  872415275) /* PhysicsEffectTable */
     , (1030372,  36,  234881042) /* MutateFilter */
     , (1030372,  46,  939524146) /* TsysMutationFilter */
     , (1030372,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:25:26.4374983-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "-correcting weenie_type to 1 - generic (standard for shields instead of clothing)\n\ncustom",
  "IsDone": true
}
*/
