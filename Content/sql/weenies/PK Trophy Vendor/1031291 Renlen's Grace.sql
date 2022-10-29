DELETE FROM `weenie` WHERE `class_Id` = 1031291;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1031291, 'ace1031291-renlensgrace', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1031291,   1,          1) /* ItemType - MeleeWeapon */
     , (1031291,   5,          1) /* EncumbranceVal */
     , (1031291,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1031291,  19,         20) /* Value */
     , (1031291,  44,          1) /* Damage */
     , (1031291,  45,          3) /* DamageType - Slash, Pierce */
     , (1031291,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1031291,  47,          6) /* AttackType - Thrust, Slash */
     , (1031291,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1031291,  49,         40) /* WeaponTime */
     , (1031291,  51,          1) /* CombatUse - Melee */
     , (1031291,  53,        101) /* PlacementPosition - Resting */
     , (1031291,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1031291, 151,          2) /* HookType - Wall */
     , (1031291, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1031291,  11, True ) /* IgnoreCollisions */
     , (1031291,  13, True ) /* Ethereal */
     , (1031291,  14, True ) /* GravityStatus */
     , (1031291,  19, True ) /* Attackable */
     , (1031291,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1031291,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1031291,   1, 'Renlen''s Grace') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1031291,   1,   33559610) /* Setup */
     , (1031291,   3,  536870932) /* SoundTable */
     , (1031291,   8,  100687934) /* Icon */
     , (1031291,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:56:05.1321173-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
