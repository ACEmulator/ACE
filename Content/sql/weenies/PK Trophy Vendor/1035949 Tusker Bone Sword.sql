DELETE FROM `weenie` WHERE `class_Id` = 1035949;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035949, 'ace1035949-tuskerbonesword', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035949,   1,          1) /* ItemType - MeleeWeapon */
     , (1035949,   5,          1) /* EncumbranceVal */
     , (1035949,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1035949,  18,          1) /* UiEffects - Magical */
     , (1035949,  19,         20) /* Value */
     , (1035949,  44,          1) /* Damage */
     , (1035949,  45,          3) /* DamageType - Slash, Pierce */
     , (1035949,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1035949,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1035949,  51,          1) /* CombatUse - Melee */
     , (1035949,  53,        101) /* PlacementPosition - Resting */
     , (1035949,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1035949, 151,          2) /* HookType - Wall */
     , (1035949, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035949,  11, True ) /* IgnoreCollisions */
     , (1035949,  13, True ) /* Ethereal */
     , (1035949,  14, True ) /* GravityStatus */
     , (1035949,  19, True ) /* Attackable */
     , (1035949,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035949,   1, 'Tusker Bone Sword') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035949,   1,   33560347) /* Setup */
     , (1035949,   3,  536870932) /* SoundTable */
     , (1035949,   8,  100689574) /* Icon */
     , (1035949,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:55:19.8759669-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
