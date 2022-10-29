DELETE FROM `weenie` WHERE `class_Id` = 1025906;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1025906, 'ace1025906-maceofdissonance', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1025906,   1,          1) /* ItemType - MeleeWeapon */
     , (1025906,   5,          1) /* EncumbranceVal */
     , (1025906,   8,        360) /* Mass */
     , (1025906,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1025906,  16,          1) /* ItemUseable - No */
     , (1025906,  18,          1) /* UiEffects - Magical */
     , (1025906,  19,         20) /* Value */
     , (1025906,  44,          1) /* Damage */
     , (1025906,  45,          4) /* DamageType - Bludgeon */
     , (1025906,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1025906,  47,          4) /* AttackType - Slash */
     , (1025906,  48,         45) /* WeaponSkill - LightWeapons */
     , (1025906,  49,         40) /* WeaponTime */
     , (1025906,  51,          1) /* CombatUse - Melee */
     , (1025906,  53,        101) /* PlacementPosition - Resting */
     , (1025906,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1025906, 150,        103) /* HookPlacement - Hook */
     , (1025906, 151,          2) /* HookType - Wall */
     , (1025906, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1025906,  11, True ) /* IgnoreCollisions */
     , (1025906,  13, True ) /* Ethereal */
     , (1025906,  14, True ) /* GravityStatus */
     , (1025906,  19, True ) /* Attackable */
     , (1025906,  22, True ) /* Inscribable */
     , (1025906,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1025906,  21, 0.6200000047683716) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1025906,   1, 'Mace of Dissonance') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1025906,   1,   33558559) /* Setup */
     , (1025906,   3,  536870932) /* SoundTable */
     , (1025906,   8,  100675636) /* Icon */
     , (1025906,  22,  872415275) /* PhysicsEffectTable */
     , (1025906,  36,  234881044) /* MutateFilter */
     , (1025906,  37,          5) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:09:42.5543261-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": true
}
*/
