DELETE FROM `weenie` WHERE `class_Id` = 1023539;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1023539, 'ace1023539-serpentsfang', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1023539,   1,          1) /* ItemType - MeleeWeapon */
     , (1023539,   5,          1) /* EncumbranceVal */
     , (1023539,   8,        500) /* Mass */
     , (1023539,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1023539,  16,          1) /* ItemUseable - No */
     , (1023539,  18,          1) /* UiEffects - Magical */
     , (1023539,  19,         20) /* Value */
     , (1023539,  44,          1) /* Damage */
     , (1023539,  45,         32) /* DamageType - Acid */
     , (1023539,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1023539,  47,          2) /* AttackType - Thrust */
     , (1023539,  48,         45) /* WeaponSkill - LightWeapons */
     , (1023539,  49,         30) /* WeaponTime */
     , (1023539,  51,          1) /* CombatUse - Melee */
     , (1023539,  53,        101) /* PlacementPosition - Resting */
     , (1023539,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1023539, 150,        103) /* HookPlacement - Hook */
     , (1023539, 151,          2) /* HookType - Wall */
     , (1023539, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1023539,  11, True ) /* IgnoreCollisions */
     , (1023539,  13, True ) /* Ethereal */
     , (1023539,  14, True ) /* GravityStatus */
     , (1023539,  19, True ) /* Attackable */
     , (1023539,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1023539,  21, 1.2999999523162842) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1023539,   1, 'Serpent''s Fang') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1023539,   1,   33557337) /* Setup */
     , (1023539,   3,  536870932) /* SoundTable */
     , (1023539,   8,  100674087) /* Icon */
     , (1023539,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:13:26.6512556-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
