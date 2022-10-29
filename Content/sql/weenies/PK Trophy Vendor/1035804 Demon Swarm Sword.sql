DELETE FROM `weenie` WHERE `class_Id` = 1035804;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035804, 'ace1035804-demonswarmsword', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035804,   1,          1) /* ItemType - MeleeWeapon */
     , (1035804,   5,          1) /* EncumbranceVal */
     , (1035804,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1035804,  16,          1) /* ItemUseable - No */
     , (1035804,  19,         20) /* Value */
     , (1035804,  44,          1) /* Damage */
     , (1035804,  45,          1) /* DamageType - Slash */
     , (1035804,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1035804,  47,          6) /* AttackType - Thrust, Slash */
     , (1035804,  48,         45) /* WeaponSkill - LightWeapons */
     , (1035804,  49,          0) /* WeaponTime */
     , (1035804,  51,          1) /* CombatUse - Melee */
     , (1035804,  52,          8) /* ParentLocation - LeftWeapon */
     , (1035804,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1035804,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1035804, 151,          2) /* HookType - Wall */
     , (1035804, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035804,  11, True ) /* IgnoreCollisions */
     , (1035804,  13, True ) /* Ethereal */
     , (1035804,  14, True ) /* GravityStatus */
     , (1035804,  19, True ) /* Attackable */
     , (1035804,  22, True ) /* Inscribable */
     , (1035804,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1035804,  21,       0) /* WeaponLength */
     , (1035804,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035804,   1, 'Demon Swarm Sword') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035804,   1,   33556589) /* Setup */
     , (1035804,   3,  536870932) /* SoundTable */
     , (1035804,   6,   67109311) /* PaletteBase */
     , (1035804,   8,  100670666) /* Icon */
     , (1035804,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:16:26.8883227-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
