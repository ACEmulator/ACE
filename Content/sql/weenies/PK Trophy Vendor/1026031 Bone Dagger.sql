DELETE FROM `weenie` WHERE `class_Id` = 1026031;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1026031, 'ace1026031-bonedagger', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1026031,   1,          1) /* ItemType - MeleeWeapon */
     , (1026031,   5,          1) /* EncumbranceVal */
     , (1026031,   8,         90) /* Mass */
     , (1026031,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1026031,  16,          1) /* ItemUseable - No */
     , (1026031,  19,         20) /* Value */
     , (1026031,  44,          1) /* Damage */
     , (1026031,  45,         32) /* DamageType - Acid */
     , (1026031,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1026031,  47,        486) /* AttackType - Thrust, Slash, DoubleSlash, TripleSlash, DoubleThrust, TripleThrust */
     , (1026031,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1026031,  49,          1) /* WeaponTime */
     , (1026031,  51,          1) /* CombatUse - Melee */
     , (1026031,  52,          1) /* ParentLocation - RightHand */
     , (1026031,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1026031,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1026031, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1026031,  11, True ) /* IgnoreCollisions */
     , (1026031,  13, True ) /* Ethereal */
     , (1026031,  14, True ) /* GravityStatus */
     , (1026031,  19, True ) /* Attackable */
     , (1026031,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1026031,  21, 0.4000000059604645) /* WeaponLength */
     , (1026031,  22,    0.75) /* DamageVariance */
     , (1026031,  29,       1) /* WeaponDefense */
     , (1026031,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1026031,   1, 'Bone Dagger') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1026031,   1,   33558584) /* Setup */
     , (1026031,   3,  536870932) /* SoundTable */
     , (1026031,   8,  100675766) /* Icon */
     , (1026031,  22,  872415275) /* PhysicsEffectTable */
     , (1026031,  36,  234881044) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:04:33.6758647-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
