DELETE FROM `weenie` WHERE `class_Id` = 1034582;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1034582, 'ace1034582-bonecrossbow', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1034582,   1,        256) /* ItemType - MissileWeapon */
     , (1034582,   5,          1) /* EncumbranceVal */
     , (1034582,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1034582,  16,          1) /* ItemUseable - No */
     , (1034582,  19,         20) /* Value */
     , (1034582,  44,          0) /* Damage */
     , (1034582,  45,          0) /* DamageType - Undef */
     , (1034582,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (1034582,  48,         47) /* WeaponSkill - MissileWeapons */
     , (1034582,  50,          2) /* AmmoType - Bolt */
     , (1034582,  51,          2) /* CombatUse - Missle */
     , (1034582,  53,        101) /* PlacementPosition - Resting */
     , (1034582,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1034582, 151,          2) /* HookType - Wall */
     , (1034582, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1034582,  11, True ) /* IgnoreCollisions */
     , (1034582,  13, True ) /* Ethereal */
     , (1034582,  14, True ) /* GravityStatus */
     , (1034582,  19, True ) /* Attackable */
     , (1034582,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1034582,  21,       0) /* WeaponLength */
     , (1034582,  39,    1.25) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1034582,   1, 'Bone Crossbow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1034582,   1,   33560184) /* Setup */
     , (1034582,   3,  536870932) /* SoundTable */
     , (1034582,   8,  100689317) /* Icon */
     , (1034582,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-07-09T17:40:36.8896521-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": ".",
  "IsDone": false
}
*/
