DELETE FROM `weenie` WHERE `class_Id` = 1041794;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1041794, 'ace1041794-greatbladeofthequiddity', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1041794,   1,          1) /* ItemType - MeleeWeapon */
     , (1041794,   5,          1) /* EncumbranceVal */
     , (1041794,   8,        320) /* Mass */
     , (1041794,   9,   33554432) /* ValidLocations - TwoHanded */
     , (1041794,  16,          1) /* ItemUseable - No */
     , (1041794,  18,          1) /* UiEffects - Magical */
     , (1041794,  19,         20) /* Value */
     , (1041794,  44,          1) /* Damage */
     , (1041794,  45,          1) /* DamageType - Slash */
     , (1041794,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (1041794,  47,          4) /* AttackType - Slash */
     , (1041794,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (1041794,  49,         25) /* WeaponTime */
     , (1041794,  51,          1) /* CombatUse - Melee */
     , (1041794,  52,          1) /* ParentLocation - RightHand */
     , (1041794,  53,        101) /* PlacementPosition - Resting */
     , (1041794,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1041794, 150,        103) /* HookPlacement - Hook */
     , (1041794, 151,          2) /* HookType - Wall */
     , (1041794, 292,          2) /* Cleaving */
     , (1041794, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1041794,  11, True ) /* IgnoreCollisions */
     , (1041794,  13, True ) /* Ethereal */
     , (1041794,  14, True ) /* GravityStatus */
     , (1041794,  15, True ) /* LightsStatus */
     , (1041794,  19, True ) /* Attackable */
     , (1041794,  22, True ) /* Inscribable */
     , (1041794,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1041794,  21,       1) /* WeaponLength */
     , (1041794,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1041794,   1, 'Greatblade of the Quiddity') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1041794,   1,   33557106) /* Setup */
     , (1041794,   3,  536870932) /* SoundTable */
     , (1041794,   8,  100690837) /* Icon */
     , (1041794,  22,  872415275) /* PhysicsEffectTable */
     , (1041794,  36,  234881044) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T20:48:20.221955-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
