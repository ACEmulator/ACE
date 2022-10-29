DELETE FROM `weenie` WHERE `class_Id` = 1033121;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1033121, 'ace1033121-redrunesilverancrossbow', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1033121,   1,        256) /* ItemType - MissileWeapon */
     , (1033121,   5,          1) /* EncumbranceVal */
     , (1033121,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1033121,  16,          1) /* ItemUseable - No */
     , (1033121,  19,         20) /* Value */
     , (1033121,  44,          1) /* Damage */
     , (1033121,  45,          0) /* DamageType - Undef */
     , (1033121,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (1033121,  48,         47) /* WeaponSkill - MissileWeapons */
     , (1033121,  50,          2) /* AmmoType - Bolt */
     , (1033121,  51,          2) /* CombatUse - Missle */
     , (1033121,  53,        101) /* PlacementPosition - Resting */
     , (1033121,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1033121, 151,          2) /* HookType - Wall */
     , (1033121, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1033121,  11, True ) /* IgnoreCollisions */
     , (1033121,  13, True ) /* Ethereal */
     , (1033121,  14, True ) /* GravityStatus */
     , (1033121,  19, True ) /* Attackable */
     , (1033121,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1033121,  21,       0) /* WeaponLength */
     , (1033121,  39,    1.25) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1033121,   1, 'Red Rune Silveran Crossbow') /* Name */
     , (1033121,  15, 'A crossbow crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1033121,   1,   33559958) /* Setup */
     , (1033121,   3,  536870932) /* SoundTable */
     , (1033121,   8,  100688933) /* Icon */
     , (1033121,  22,  872415275) /* PhysicsEffectTable */
     , (1033121,  50,  100688915) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-07-09T17:33:06.5138219-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": ".",
  "IsDone": false
}
*/
