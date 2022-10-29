DELETE FROM `weenie` WHERE `class_Id` = 1011329;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1011329, 'ace1011329-auntanuaswartaiaha', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1011329,   1,          1) /* ItemType - MeleeWeapon */
     , (1011329,   5,          1) /* EncumbranceVal */
     , (1011329,   8,        500) /* Mass */
     , (1011329,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1011329,  16,          1) /* ItemUseable - No */
     , (1011329,  18,          1) /* UiEffects - Magical */
     , (1011329,  19,         20) /* Value */
     , (1011329,  44,          1) /* Damage */
     , (1011329,  45,          2) /* DamageType - Pierce */
     , (1011329,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1011329,  47,          2) /* AttackType - Thrust */
     , (1011329,  48,         45) /* WeaponSkill - LightWeapons */
     , (1011329,  49,         25) /* WeaponTime */
     , (1011329,  51,          1) /* CombatUse - Melee */
     , (1011329,  53,        101) /* PlacementPosition - Resting */
     , (1011329,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1011329, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1011329,  11, True ) /* IgnoreCollisions */
     , (1011329,  13, True ) /* Ethereal */
     , (1011329,  14, True ) /* GravityStatus */
     , (1011329,  19, True ) /* Attackable */
     , (1011329,  22, True ) /* Inscribable */
     , (1011329,  23, True ) /* DestroyOnSell */
     , (1011329,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1011329,  21, 1.2999999523162842) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1011329,   1, 'Aun Tanua''s War Taiaha') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1011329,   1,   33557310) /* Setup */
     , (1011329,   3,  536870932) /* SoundTable */
     , (1011329,   8,  100672030) /* Icon */
     , (1011329,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:14:08.4142415-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
