DELETE FROM `weenie` WHERE `class_Id` = 450179;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450179, 'ace450179-tassetsofleikothastearstailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450179,   1,          2) /* ItemType - Armor */
     , (450179,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450179,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (450179,   5,          1) /* EncumbranceVal */
     , (450179,   8,         90) /* Mass */
     , (450179,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (450179,  16,          1) /* ItemUseable - No */
     , (450179,  19,         20) /* Value */
     , (450179,  27,          2) /* ArmorType - Leather */
     , (450179,  28,          1) /* ArmorLevel */
     , (450179,  53,        101) /* PlacementPosition - Resting */
     , (450179,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450179, 151,          2) /* HookType - Wall */
     , (450179, 169,  118162702) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450179,  11, True ) /* IgnoreCollisions */
     , (450179,  13, True ) /* Ethereal */
     , (450179,  14, True ) /* GravityStatus */
     , (450179,  19, True ) /* Attackable */
     , (450179,  22, True ) /* Inscribable */
     , (450179,  91, False) /* Retained */
     , (450179, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450179,  12,       0) /* Shade */
     , (450179, 110, 1.6699999570846558) /* BulkMod */
     , (450179, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450179,   1, 'Tassets of Leikotha''s Tears') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450179,   1,   33559432) /* Setup */
     , (450179,   3,  536870932) /* SoundTable */
     , (450179,   6,   67108990) /* PaletteBase */
     , (450179,   7,  268436972) /* ClothingBase */
     , (450179,   8,  100686874) /* Icon */
     , (450179,  22,  872415275) /* PhysicsEffectTable */
     , (450179,  36,  234881042) /* MutateFilter */
     , (450179,  46,  939524146) /* TsysMutationFilter */
     , (450179,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-13T13:15:56.8857682-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Not visible on character., changed clothing DID. ",
  "IsDone": true
}
*/
