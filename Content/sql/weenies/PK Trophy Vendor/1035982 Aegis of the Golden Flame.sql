DELETE FROM `weenie` WHERE `class_Id` = 1035982;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035982, 'ace1035982-aegisofthegoldenflame', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035982,   1,          2) /* ItemType - Armor */
     , (1035982,   5,          0) /* EncumbranceVal */
     , (1035982,   9,    2097152) /* ValidLocations - Shield */
     , (1035982,  16,          1) /* ItemUseable - No */
     , (1035982,  18,         32) /* UiEffects - Fire */
     , (1035982,  19,         20) /* Value */
     , (1035982,  28,          1) /* ArmorLevel */
     , (1035982,  51,          4) /* CombatUse - Shield */
     , (1035982,  52,          3) /* ParentLocation - Shield */
     , (1035982,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1035982, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035982,  11, True ) /* IgnoreCollisions */
     , (1035982,  13, True ) /* Ethereal */
     , (1035982,  14, True ) /* GravityStatus */
     , (1035982,  15, True ) /* LightsStatus */
     , (1035982,  19, True ) /* Attackable */
     , (1035982,  22, True ) /* Inscribable */
     , (1035982,  69, False) /* IsSellable */
     , (1035982,  85, True ) /* AppraisalHasAllowedWielder */
     , (1035982,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1035982,   5, -0.0333000011742115) /* ManaRate */
     , (1035982,  13,       1) /* ArmorModVsSlash */
     , (1035982,  14,       1) /* ArmorModVsPierce */
     , (1035982,  15,       1) /* ArmorModVsBludgeon */
     , (1035982,  16,     0.5) /* ArmorModVsCold */
     , (1035982,  17,       2) /* ArmorModVsFire */
     , (1035982,  18, 0.800000011920929) /* ArmorModVsAcid */
     , (1035982,  19, 1.2000000476837158) /* ArmorModVsElectric */
     , (1035982, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035982,   1, 'Aegis of the Golden Flame') /* Name */
     , (1035982,  16, 'A shield forged from Pure Mana and Flame.  This Aegis is the ultimate expression of the heraldry of the Knights of the Golden Flame upon Dereth.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035982,   1,   33560366) /* Setup */
     , (1035982,   3,  536870932) /* SoundTable */
     , (1035982,   8,  100689596) /* Icon */
     , (1035982,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-04-18T22:15:44.7424324-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
