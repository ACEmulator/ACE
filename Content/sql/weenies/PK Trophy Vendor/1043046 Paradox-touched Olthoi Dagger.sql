DELETE FROM `weenie` WHERE `class_Id` = 1043046;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1043046, 'ace1043046-paradoxtouchedolthoidagger', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1043046,   1,          1) /* ItemType - MeleeWeapon */
     , (1043046,   5,          1) /* EncumbranceVal */
     , (1043046,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1043046,  16,          1) /* ItemUseable - No */
     , (1043046,  18,          1) /* UiEffects - Magical */
     , (1043046,  19,         20) /* Value */
     , (1043046,  44,          1) /* Damage */
     , (1043046,  45,          3) /* DamageType - Slash, Pierce */
     , (1043046,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1043046,  51,          1) /* CombatUse - Melee */
     , (1043046,  52,          1) /* ParentLocation - RightHand */
     , (1043046,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1043046,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1043046, 151,          2) /* HookType - Wall */
     , (1043046, 159,         44) /* WieldSkillType - HeavyWeapons */
     , (1043046, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1043046,  11, True ) /* IgnoreCollisions */
     , (1043046,  13, True ) /* Ethereal */
     , (1043046,  14, True ) /* GravityStatus */
     , (1043046,  19, True ) /* Attackable */
     , (1043046,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1043046,  39,    0.75) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1043046,   1, 'Paradox-touched Olthoi Dagger') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1043046,   1,   33561083) /* Setup */
     , (1043046,   3,  536870932) /* SoundTable */
     , (1043046,   8,  100691350) /* Icon */
     , (1043046,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T06:23:46.616094-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
