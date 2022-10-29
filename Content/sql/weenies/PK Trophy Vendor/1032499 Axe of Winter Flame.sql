DELETE FROM `weenie` WHERE `class_Id` = 1032499;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1032499, 'ace1032499-axeofwinterflame', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1032499,   1,          1) /* ItemType - MeleeWeapon */
     , (1032499,   5,          1) /* EncumbranceVal */
     , (1032499,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1032499,  16,          1) /* ItemUseable - No */
     , (1032499,  18,          1) /* UiEffects - Magical */
     , (1032499,  19,         20) /* Value */
     , (1032499,  44,          1) /* Damage */
     , (1032499,  45,         16) /* DamageType - Fire */
     , (1032499,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1032499,  47,          4) /* AttackType - Slash */
     , (1032499,  48,         45) /* WeaponSkill - LightWeapons */
     , (1032499,  49,         50) /* WeaponTime */
     , (1032499,  51,          1) /* CombatUse - Melee */
     , (1032499,  53,        101) /* PlacementPosition - Resting */
     , (1032499,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1032499, 151,          2) /* HookType - Wall */
     , (1032499, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1032499,  11, True ) /* IgnoreCollisions */
     , (1032499,  13, True ) /* Ethereal */
     , (1032499,  14, True ) /* GravityStatus */
     , (1032499,  19, True ) /* Attackable */
     , (1032499,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1032499,   1, 'Axe of Winter Flame') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1032499,   1,   33559805) /* Setup */
     , (1032499,   3,  536870932) /* SoundTable */
     , (1032499,   6,   67111919) /* PaletteBase */
     , (1032499,   8,  100688525) /* Icon */
     , (1032499,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:07:51.3369081-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom\n",
  "IsDone": true
}
*/
