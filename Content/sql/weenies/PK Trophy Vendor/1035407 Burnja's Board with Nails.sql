DELETE FROM `weenie` WHERE `class_Id` = 1035407;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035407, 'ace1035407-burnjasboardwithnails', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035407,   1,          1) /* ItemType - MeleeWeapon */
     , (1035407,   5,          1) /* EncumbranceVal */
     , (1035407,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1035407,  16,          1) /* ItemUseable - No */
     , (1035407,  18,         32) /* UiEffects - Fire */
     , (1035407,  19,         20) /* Value */
     , (1035407,  44,          1) /* Damage */
     , (1035407,  45,         16) /* DamageType - Fire */
     , (1035407,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1035407,  47,          4) /* AttackType - Slash */
     , (1035407,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1035407,  49,         30) /* WeaponTime */
     , (1035407,  51,          1) /* CombatUse - Melee */
     , (1035407,  53,        101) /* PlacementPosition - Resting */
     , (1035407,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1035407, 151,          2) /* HookType - Wall */
     , (1035407, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035407,  11, True ) /* IgnoreCollisions */
     , (1035407,  13, True ) /* Ethereal */
     , (1035407,  14, True ) /* GravityStatus */
     , (1035407,  19, True ) /* Attackable */
     , (1035407,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1035407,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035407,   1, 'Burnja''s Board with Nails') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035407,   1,   33560302) /* Setup */
     , (1035407,   3,  536870932) /* SoundTable */
     , (1035407,   8,  100689512) /* Icon */
     , (1035407,  22,  872415275) /* PhysicsEffectTable */
     , (1035407,  55,         27) /* ProcSpell - Flame Bolt I */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:39:04.4009729-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
