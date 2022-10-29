DELETE FROM `weenie` WHERE `class_Id` = 1035295;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035295, 'ace1035295-shieldofperfectlight', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035295,   1,          2) /* ItemType - Armor */
     , (1035295,   5,          0) /* EncumbranceVal */
     , (1035295,   9,    2097152) /* ValidLocations - Shield */
     , (1035295,  16,          1) /* ItemUseable - No */
     , (1035295,  18,          1) /* UiEffects - Magical */
     , (1035295,  19,         20) /* Value */
     , (1035295,  28,          1) /* ArmorLevel */
     , (1035295,  51,          4) /* CombatUse - Shield */
     , (1035295,  52,          3) /* ParentLocation - Shield */
     , (1035295,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1035295, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035295,  11, True ) /* IgnoreCollisions */
     , (1035295,  13, True ) /* Ethereal */
     , (1035295,  14, True ) /* GravityStatus */
     , (1035295,  19, True ) /* Attackable */
     , (1035295,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1035295,   5, -0.02500000037252903) /* ManaRate */
     , (1035295,  13, 1.7999999523162842) /* ArmorModVsSlash */
     , (1035295,  14,       1) /* ArmorModVsPierce */
     , (1035295,  15, 1.7999999523162842) /* ArmorModVsBludgeon */
     , (1035295,  16,       2) /* ArmorModVsCold */
     , (1035295,  17, 0.800000011920929) /* ArmorModVsFire */
     , (1035295,  18,       2) /* ArmorModVsAcid */
     , (1035295,  19, 0.800000011920929) /* ArmorModVsElectric */
     , (1035295, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035295,   1, 'Shield of Perfect Light') /* Name */
     , (1035295,  16, 'A shield glowing with a brilliant light. Although the shield looks insubstantial it strongly resists your efforts to penetrate the magical barrier it contains.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035295,   1,   33560269) /* Setup */
     , (1035295,   3,  536870932) /* SoundTable */
     , (1035295,   8,  100689429) /* Icon */
     , (1035295,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:24:22.0297281-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
