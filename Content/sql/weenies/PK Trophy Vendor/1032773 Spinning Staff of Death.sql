DELETE FROM `weenie` WHERE `class_Id` = 1032773;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1032773, 'ace1032773-spinningstaffofdeath', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1032773,   1,          1) /* ItemType - MeleeWeapon */
     , (1032773,   5,          1) /* EncumbranceVal */
     , (1032773,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1032773,  16,          1) /* ItemUseable - No */
     , (1032773,  18,          1) /* UiEffects - Magical */
     , (1032773,  19,         20) /* Value */
     , (1032773,  44,          1) /* Damage */
     , (1032773,  45,          1) /* DamageType - Slash */
     , (1032773,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1032773,  47,          6) /* AttackType - Thrust, Slash */
     , (1032773,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1032773,  49,          0) /* WeaponTime */
     , (1032773,  51,          1) /* CombatUse - Melee */
     , (1032773,  53,        101) /* PlacementPosition - Resting */
     , (1032773,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1032773, 151,          2) /* HookType - Wall */
     , (1032773, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1032773,  11, True ) /* IgnoreCollisions */
     , (1032773,  13, True ) /* Ethereal */
     , (1032773,  14, True ) /* GravityStatus */
     , (1032773,  19, True ) /* Attackable */
     , (1032773,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1032773,  39, 0.6700000166893005) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1032773,   1, 'Spinning Staff of Death') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1032773,   1,   33559847) /* Setup */
     , (1032773,   3,  536870932) /* SoundTable */
     , (1032773,   8,  100688662) /* Icon */
     , (1032773,  22,  872415275) /* PhysicsEffectTable */
     , (1032773,  55,       1784) /* ProcSpell - Horizon's Blades */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:59:28.87601-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
