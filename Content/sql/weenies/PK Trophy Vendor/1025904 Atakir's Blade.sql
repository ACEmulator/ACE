DELETE FROM `weenie` WHERE `class_Id` = 1025904;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1025904, 'ace1025904-atakirsblade', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1025904,   1,          1) /* ItemType - MeleeWeapon */
     , (1025904,   5,          1) /* EncumbranceVal */
     , (1025904,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1025904,  16,          1) /* ItemUseable - No */
     , (1025904,  18,          1) /* UiEffects - Magical */
     , (1025904,  19,         20) /* Value */
     , (1025904,  44,          1) /* Damage */
     , (1025904,  45,          3) /* DamageType - Slash, Pierce */
     , (1025904,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1025904,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (1025904,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1025904,  49,         15) /* WeaponTime */
     , (1025904,  51,          1) /* CombatUse - Melee */
     , (1025904,  53,        101) /* PlacementPosition - Resting */
     , (1025904,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1025904, 150,        103) /* HookPlacement - Hook */
     , (1025904, 151,          2) /* HookType - Wall */
     , (1025904, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1025904,  11, True ) /* IgnoreCollisions */
     , (1025904,  13, True ) /* Ethereal */
     , (1025904,  14, True ) /* GravityStatus */
     , (1025904,  19, True ) /* Attackable */
     , (1025904,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1025904,  21, 0.4000000059604645) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1025904,   1, 'Atakir''s Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1025904,   1,   33558560) /* Setup */
     , (1025904,   3,  536870932) /* SoundTable */
     , (1025904,   6,   67111919) /* PaletteBase */
     , (1025904,   8,  100675637) /* Icon */
     , (1025904,  22,  872415275) /* PhysicsEffectTable */
     , (1025904,  37,         44) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:37:32.6247051-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
