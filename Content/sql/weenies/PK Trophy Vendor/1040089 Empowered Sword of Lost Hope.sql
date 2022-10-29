DELETE FROM `weenie` WHERE `class_Id` = 1040089;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1040089, 'ace1040089-empoweredswordoflosthope', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1040089,   1,          1) /* ItemType - MeleeWeapon */
     , (1040089,   5,          1) /* EncumbranceVal */
     , (1040089,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1040089,  16,          1) /* ItemUseable - No */
     , (1040089,  18,          1) /* UiEffects - Magical */
     , (1040089,  19,         20) /* Value */
     , (1040089,  44,          1) /* Damage */
     , (1040089,  45,         32) /* DamageType - Acid */
     , (1040089,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1040089,  47,          6) /* AttackType - Thrust, Slash */
     , (1040089,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1040089,  49,         30) /* WeaponTime */
     , (1040089,  51,          1) /* CombatUse - Melee */
     , (1040089,  53,        101) /* PlacementPosition - Resting */
     , (1040089,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1040089, 151,          2) /* HookType - Wall */
     , (1040089, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1040089,  11, True ) /* IgnoreCollisions */
     , (1040089,  13, True ) /* Ethereal */
     , (1040089,  14, True ) /* GravityStatus */
     , (1040089,  15, True ) /* LightsStatus */
     , (1040089,  19, True ) /* Attackable */
     , (1040089,  22, True ) /* Inscribable */
     , (1040089,  69, False) /* IsSellable */
     , (1040089,  85, True ) /* AppraisalHasAllowedWielder */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1040089,  21,       0) /* WeaponLength */
     , (1040089,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1040089,   1, 'Empowered Sword of Lost Hope') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1040089,   1,   33558420) /* Setup */
     , (1040089,   3,  536870932) /* SoundTable */
     , (1040089,   8,  100671325) /* Icon */
     , (1040089,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:43:13.4634703-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
