DELETE FROM `weenie` WHERE `class_Id` = 1029910;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029910, 'ace1029910-marshsiraluunwaaika', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029910,   1,          1) /* ItemType - MeleeWeapon */
     , (1029910,   5,          1) /* EncumbranceVal */
     , (1029910,   8,        300) /* Mass */
     , (1029910,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1029910,  16,          1) /* ItemUseable - No */
     , (1029910,  18,          1) /* UiEffects - Magical */
     , (1029910,  19,         20) /* Value */
     , (1029910,  44,          1) /* Damage */
     , (1029910,  45,          1) /* DamageType - Slash */
     , (1029910,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1029910,  47,          4) /* AttackType - Slash */
     , (1029910,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1029910,  49,         45) /* WeaponTime */
     , (1029910,  51,          1) /* CombatUse - Melee */
     , (1029910,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029910, 150,        103) /* HookPlacement - Hook */
     , (1029910, 151,          2) /* HookType - Wall */
     , (1029910, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029910,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029910,   1, 'Marsh Siraluun Waaika') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029910,   1,   33559108) /* Setup */
     , (1029910,   3,  536870932) /* SoundTable */
     , (1029910,   8,  100677337) /* Icon */
     , (1029910,  22,  872415275) /* PhysicsEffectTable */
     , (1029910,  37,          5) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:02:19.6216323-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
