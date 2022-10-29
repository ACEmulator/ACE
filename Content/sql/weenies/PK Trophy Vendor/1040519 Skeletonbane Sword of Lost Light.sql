DELETE FROM `weenie` WHERE `class_Id` = 1040519;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1040519, 'ace1040519-skeletonbaneswordoflostlight', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1040519,   1,          1) /* ItemType - MeleeWeapon */
     , (1040519,   5,          1) /* EncumbranceVal */
     , (1040519,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1040519,  16,          1) /* ItemUseable - No */
     , (1040519,  18,          1) /* UiEffects - Magical */
     , (1040519,  19,         20) /* Value */
     , (1040519,  44,          1) /* Damage */
     , (1040519,  45,          1) /* DamageType - Slash */
     , (1040519,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1040519,  47,          4) /* AttackType - Slash */
     , (1040519,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1040519,  49,         30) /* WeaponTime */
     , (1040519,  51,          1) /* CombatUse - Melee */
     , (1040519,  52,          1) /* ParentLocation - RightHand */
     , (1040519,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1040519,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1040519, 151,          2) /* HookType - Wall */
     , (1040519, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1040519,  11, True ) /* IgnoreCollisions */
     , (1040519,  13, True ) /* Ethereal */
     , (1040519,  14, True ) /* GravityStatus */
     , (1040519,  19, True ) /* Attackable */
     , (1040519,  22, True ) /* Inscribable */
     , (1040519,  69, False) /* IsSellable */
     , (1040519,  85, True ) /* AppraisalHasAllowedWielder */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1040519,  21,       0) /* WeaponLength */
     , (1040519,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1040519,   1, 'Skeletonbane Sword of Lost Light') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1040519,   1,   33560711) /* Setup */
     , (1040519,   3,  536870932) /* SoundTable */
     , (1040519,   8,  100674513) /* Icon */
     , (1040519,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:43:44.0193789-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
