DELETE FROM `weenie` WHERE `class_Id` = 1036524;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1036524, 'ace1036524-pumpkinshield', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1036524,   1,          2) /* ItemType - Armor */
     , (1036524,   5,          0) /* EncumbranceVal */
     , (1036524,   9,    2097152) /* ValidLocations - Shield */
     , (1036524,  16,          1) /* ItemUseable - No */
     , (1036524,  18,          1) /* UiEffects - Magical */
     , (1036524,  19,         20) /* Value */
     , (1036524,  28,          1) /* ArmorLevel */
     , (1036524,  51,          4) /* CombatUse - Shield */
     , (1036524,  52,          3) /* ParentLocation - Shield */
     , (1036524,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1036524, 151,          2) /* HookType - Wall */
     , (1036524, 176,         48) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1036524,  11, True ) /* IgnoreCollisions */
     , (1036524,  13, True ) /* Ethereal */
     , (1036524,  14, True ) /* GravityStatus */
     , (1036524,  19, True ) /* Attackable */
     , (1036524,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1036524,   5, -0.05000000074505806) /* ManaRate */
     , (1036524,  13,     1.5) /* ArmorModVsSlash */
     , (1036524,  14,     1.5) /* ArmorModVsPierce */
     , (1036524,  15,     1.5) /* ArmorModVsBludgeon */
     , (1036524,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1036524,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1036524,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1036524,  19, 0.800000011920929) /* ArmorModVsElectric */
     , (1036524,  39, 0.8999999761581421) /* DefaultScale */
     , (1036524, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1036524,   1, 'Pumpkin Shield') /* Name */
     , (1036524,  16, 'The thick shell of a large pumpkin. It''s surprisingly strong and lightweight.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1036524,   1,   33560395) /* Setup */
     , (1036524,   3,  536870932) /* SoundTable */
     , (1036524,   8,  100671019) /* Icon */
     , (1036524,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:22:06.4644912-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
