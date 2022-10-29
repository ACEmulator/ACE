DELETE FROM `weenie` WHERE `class_Id` = 1030516;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030516, 'ace1030516-tassetsofleikothastears', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030516,   1,          2) /* ItemType - Armor */
     , (1030516,   3,          1) /* PaletteTemplate - AquaBlue */
     , (1030516,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (1030516,   5,          1) /* EncumbranceVal */
     , (1030516,   8,         90) /* Mass */
     , (1030516,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (1030516,  16,          1) /* ItemUseable - No */
     , (1030516,  19,         20) /* Value */
     , (1030516,  27,          2) /* ArmorType - Leather */
     , (1030516,  28,          1) /* ArmorLevel */
     , (1030516,  53,        101) /* PlacementPosition - Resting */
     , (1030516,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030516, 151,          2) /* HookType - Wall */
     , (1030516, 169,  118162702) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030516,  11, True ) /* IgnoreCollisions */
     , (1030516,  13, True ) /* Ethereal */
     , (1030516,  14, True ) /* GravityStatus */
     , (1030516,  19, True ) /* Attackable */
     , (1030516,  22, True ) /* Inscribable */
     , (1030516,  91, False) /* Retained */
     , (1030516, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030516,  12,       0) /* Shade */
     , (1030516, 110, 1.6699999570846558) /* BulkMod */
     , (1030516, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030516,   1, 'Tassets of Leikotha''s Tears') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030516,   1,   33559432) /* Setup */
     , (1030516,   3,  536870932) /* SoundTable */
     , (1030516,   6,   67108990) /* PaletteBase */
     , (1030516,   7,  268436972) /* ClothingBase */
     , (1030516,   8,  100686874) /* Icon */
     , (1030516,  22,  872415275) /* PhysicsEffectTable */
     , (1030516,  36,  234881042) /* MutateFilter */
     , (1030516,  46,  939524146) /* TsysMutationFilter */
     , (1030516,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-13T13:15:56.8857682-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Not visible on character., changed clothing DID. ",
  "IsDone": true
}
*/
