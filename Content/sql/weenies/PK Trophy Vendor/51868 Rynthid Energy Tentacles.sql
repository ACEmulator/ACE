DELETE FROM `weenie` WHERE `class_Id` = 51868;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51868, 'ace51868-rynthidenergytentacles', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51868,   1,          4) /* ItemType - Clothing */
     , (51868,   4,     131072) /* ClothingPriority - 131072 */
     , (51868,   5,         75) /* EncumbranceVal */
     , (51868,   9,  134217728) /* ValidLocations - Cloak */
     , (51868,  16,          1) /* ItemUseable - No */
     , (51868,  18,          1) /* UiEffects - Magical */
     , (51868,  19,         20) /* Value */
     , (51868,  28,          0) /* ArmorLevel */
     , (51868,  36,       9999) /* ResistMagic */
     , (51868,  65,        101) /* Placement - Resting */
     , (51868,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51868, 105,          8) /* ItemWorkmanship */
     , (51868, 131,          6) /* MaterialType - Silk */
     , (51868, 172,          1) /* AppraisalLongDescDecoration */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51868,   1, False) /* Stuck */
     , (51868,  11, True ) /* IgnoreCollisions */
     , (51868,  13, True ) /* Ethereal */
     , (51868,  14, True ) /* GravityStatus */
     , (51868,  19, True ) /* Attackable */
     , (51868,  22, True ) /* Inscribable */
     , (51868, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51868,  13, 0.800000011920929) /* ArmorModVsSlash */
     , (51868,  14, 0.800000011920929) /* ArmorModVsPierce */
     , (51868,  15,       1) /* ArmorModVsBludgeon */
     , (51868,  16, 0.20000000298023224) /* ArmorModVsCold */
     , (51868,  17, 0.20000000298023224) /* ArmorModVsFire */
     , (51868,  18, 0.10000000149011612) /* ArmorModVsAcid */
     , (51868,  19, 0.20000000298023224) /* ArmorModVsElectric */
     , (51868, 165,       1) /* ArmorModVsNether */
     , (51868, 8004,       5) /* PCAPRecordedWorkmanship */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51868,   1, 'Rynthid Energy Tentacles') /* Name */
     , (51868,  16, 'Cloak of Borelean') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51868,   1,   33561386) /* Setup */
     , (51868,   3,  536870932) /* SoundTable */
     , (51868,   7,  268437599) /* ClothingBase */
     , (51868,   8,  100692112) /* Icon */
     , (51868,  22,  872415275) /* PhysicsEffectTable */
     , (51868,  50,  100690998) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-02T17:58:24.9869242-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "test",
  "IsDone": false
}
*/
