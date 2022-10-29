DELETE FROM `weenie` WHERE `class_Id` = 1033058;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1033058, 'ace1033058-redrunesilveranaxe', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1033058,   1,          1) /* ItemType - MeleeWeapon */
     , (1033058,   5,          1) /* EncumbranceVal */
     , (1033058,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1033058,  16,          1) /* ItemUseable - No */
     , (1033058,  19,         20) /* Value */
     , (1033058,  44,          1) /* Damage */
     , (1033058,  45,          1) /* DamageType - Slash */
     , (1033058,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1033058,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1033058,  51,          1) /* CombatUse - Melee */
     , (1033058,  52,          1) /* ParentLocation - RightHand */
     , (1033058,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1033058,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1033058, 151,          2) /* HookType - Wall */
     , (1033058, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1033058,  11, True ) /* IgnoreCollisions */
     , (1033058,  13, True ) /* Ethereal */
     , (1033058,  14, True ) /* GravityStatus */
     , (1033058,  19, True ) /* Attackable */
     , (1033058,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1033058,  77,       1) /* PhysicsScriptIntensity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1033058,   1, 'Red Rune Silveran Axe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1033058,   1,   33559896) /* Setup */
     , (1033058,   3,  536870932) /* SoundTable */
     , (1033058,   8,  100688899) /* Icon */
     , (1033058,  19,         88) /* ActivationAnimation */
     , (1033058,  22,  872415275) /* PhysicsEffectTable */
     , (1033058,  30,         88) /* PhysicsScript - Create */
     , (1033058,  50,  100688915) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:07:32.7035502-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
