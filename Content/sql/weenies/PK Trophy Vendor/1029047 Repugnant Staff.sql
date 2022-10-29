DELETE FROM `weenie` WHERE `class_Id` = 1029047;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1029047, 'ace1029047-repugnantstaff', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1029047,   1,          1) /* ItemType - MeleeWeapon */
     , (1029047,   5,          1) /* EncumbranceVal */
     , (1029047,   8,       2560) /* Mass */
     , (1029047,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1029047,  16,          1) /* ItemUseable - No */
     , (1029047,  19,         20) /* Value */
     , (1029047,  44,          1) /* Damage */
     , (1029047,  45,          4) /* DamageType - Bludgeon */
     , (1029047,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1029047,  47,          6) /* AttackType - Thrust, Slash */
     , (1029047,  48,         45) /* WeaponSkill - LightWeapons */
     , (1029047,  49,         60) /* WeaponTime */
     , (1029047,  51,          1) /* CombatUse - Melee */
     , (1029047,  52,          1) /* ParentLocation - RightHand */
     , (1029047,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1029047,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1029047, 150,        103) /* HookPlacement - Hook */
     , (1029047, 151,          2) /* HookType - Wall */
     , (1029047, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1029047,  11, True ) /* IgnoreCollisions */
     , (1029047,  13, True ) /* Ethereal */
     , (1029047,  14, True ) /* GravityStatus */
     , (1029047,  19, True ) /* Attackable */
     , (1029047,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1029047,  21,     1.5) /* WeaponLength */
     , (1029047,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1029047,   1, 'Repugnant Staff') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1029047,   1,   33558883) /* Setup */
     , (1029047,   3,  536870932) /* SoundTable */
     , (1029047,   8,  100677030) /* Icon */
     , (1029047,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:15:55.5002391-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
