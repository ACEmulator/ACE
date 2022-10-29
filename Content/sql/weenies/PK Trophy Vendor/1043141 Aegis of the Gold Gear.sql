DELETE FROM `weenie` WHERE `class_Id` = 1043141;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1043141, 'ace1043141-aegisofthegoldgear', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1043141,   1,          2) /* ItemType - Armor */
     , (1043141,   5,          0) /* EncumbranceVal */
     , (1043141,   9,    2097152) /* ValidLocations - Shield */
     , (1043141,  16,          1) /* ItemUseable - No */
     , (1043141,  18,          1) /* UiEffects - Magical */
     , (1043141,  19,         20) /* Value */
     , (1043141,  28,          1) /* ArmorLevel */
     , (1043141,  51,          4) /* CombatUse - Shield */
     , (1043141,  52,          3) /* ParentLocation - Shield */
     , (1043141,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1043141, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1043141,  11, True ) /* IgnoreCollisions */
     , (1043141,  13, True ) /* Ethereal */
     , (1043141,  14, True ) /* GravityStatus */
     , (1043141,  19, True ) /* Attackable */
     , (1043141,  22, True ) /* Inscribable */
     , (1043141,  91, False) /* Retained */
     , (1043141, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1043141,   5, -0.02500000037252903) /* ManaRate */
     , (1043141,  13, 1.2999999523162842) /* ArmorModVsSlash */
     , (1043141,  14, 1.2999999523162842) /* ArmorModVsPierce */
     , (1043141,  15, 1.2999999523162842) /* ArmorModVsBludgeon */
     , (1043141,  16, 0.800000011920929) /* ArmorModVsCold */
     , (1043141,  17,       1) /* ArmorModVsFire */
     , (1043141,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1043141,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1043141, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1043141,   1, 'Aegis of the Gold Gear') /* Name */
     , (1043141,  16, 'An Aegis, crafted in the Gear Knight style out of a chunk of Aetherium that has been augmented with small amounts of Chorizite.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1043141,   1,   33561097) /* Setup */
     , (1043141,   3,  536870932) /* SoundTable */
     , (1043141,   8,  100691463) /* Icon */
     , (1043141,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:16:52.1331322-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
