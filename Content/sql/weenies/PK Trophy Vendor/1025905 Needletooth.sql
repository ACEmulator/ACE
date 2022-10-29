DELETE FROM `weenie` WHERE `class_Id` = 1025905;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1025905, 'ace1025905-needletooth', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1025905,   1,          1) /* ItemType - MeleeWeapon */
     , (1025905,   5,          1) /* EncumbranceVal */
     , (1025905,   8,         80) /* Mass */
     , (1025905,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1025905,  18,          1) /* UiEffects - Magical */
     , (1025905,  19,         20) /* Value */
     , (1025905,  44,          1) /* Damage */
     , (1025905,  45,          3) /* DamageType - Slash, Pierce */
     , (1025905,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (1025905,  47,          1) /* AttackType - Punch */
     , (1025905,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1025905,  49,         15) /* WeaponTime */
     , (1025905,  51,          1) /* CombatUse - Melee */
     , (1025905,  53,        101) /* PlacementPosition - Resting */
     , (1025905,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1025905, 150,        103) /* HookPlacement - Hook */
     , (1025905, 151,          2) /* HookType - Wall */
     , (1025905, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1025905,  11, True ) /* IgnoreCollisions */
     , (1025905,  13, True ) /* Ethereal */
     , (1025905,  14, True ) /* GravityStatus */
     , (1025905,  19, True ) /* Attackable */
     , (1025905,  22, True ) /* Inscribable */
     , (1025905,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1025905,  21, 0.3499999940395355) /* WeaponLength */
     , (1025905,  39,    1.25) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1025905,   1, 'Needletooth') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1025905,   1,   33558561) /* Setup */
     , (1025905,   3,  536870932) /* SoundTable */
     , (1025905,   8,  100675638) /* Icon */
     , (1025905,  22,  872415275) /* PhysicsEffectTable */
     , (1025905,  36,  234881044) /* MutateFilter */
     , (1025905,  37,         13) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:52:00.7408265-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": true
}
*/
