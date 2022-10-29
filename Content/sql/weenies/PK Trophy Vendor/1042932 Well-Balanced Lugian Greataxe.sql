DELETE FROM `weenie` WHERE `class_Id` = 1042932;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1042932, 'ace1042932-wellbalancedlugiangreataxe', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1042932,   1,          1) /* ItemType - MeleeWeapon */
     , (1042932,   5,          1) /* EncumbranceVal */
     , (1042932,   9,   33554432) /* ValidLocations - TwoHanded */
     , (1042932,  16,          1) /* ItemUseable - No */
     , (1042932,  18,          1) /* UiEffects - Magical */
     , (1042932,  19,         20) /* Value */
     , (1042932,  44,          1) /* Damage */
     , (1042932,  45,          3) /* DamageType - Slash, Pierce */
     , (1042932,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (1042932,  47,          6) /* AttackType - Thrust, Slash */
     , (1042932,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (1042932,  49,         35) /* WeaponTime */
     , (1042932,  51,          5) /* CombatUse - TwoHanded */
     , (1042932,  52,          1) /* ParentLocation - RightHand */
     , (1042932,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1042932,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1042932, 292,          3) /* Cleaving */
     , (1042932, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1042932,  11, True ) /* IgnoreCollisions */
     , (1042932,  13, True ) /* Ethereal */
     , (1042932,  14, True ) /* GravityStatus */
     , (1042932,  15, True ) /* LightsStatus */
     , (1042932,  19, True ) /* Attackable */
     , (1042932,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1042932,  21,       0) /* WeaponLength */
     , (1042932,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1042932,   1, 'Well-Balanced Lugian Greataxe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1042932,   1,   33558379) /* Setup */
     , (1042932,   3,  536870932) /* SoundTable */
     , (1042932,   8,  100691239) /* Icon */
     , (1042932,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T20:46:01.1235794-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
