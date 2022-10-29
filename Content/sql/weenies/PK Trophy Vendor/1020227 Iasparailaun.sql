DELETE FROM `weenie` WHERE `class_Id` = 1020227;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1020227, 'ace1020227-iasparailaun', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1020227,   1,          1) /* ItemType - MeleeWeapon */
     , (1020227,   5,          1) /* EncumbranceVal */
     , (1020227,   8,        180) /* Mass */
     , (1020227,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1020227,  18,         32) /* UiEffects - Fire */
     , (1020227,  19,         20) /* Value */
     , (1020227,  44,          1) /* Damage */
     , (1020227,  45,         16) /* DamageType - Fire */
     , (1020227,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1020227,  47,          2) /* AttackType - Thrust */
     , (1020227,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1020227,  49,         30) /* WeaponTime */
     , (1020227,  51,          1) /* CombatUse - Melee */
     , (1020227,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1020227, 150,        103) /* HookPlacement - Hook */
     , (1020227, 151,          2) /* HookType - Wall */
     , (1020227, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1020227,  22, True ) /* Inscribable */
     , (1020227,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1020227,  21, 0.949999988079071) /* WeaponLength */
     , (1020227,  39, 1.2000000476837158) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1020227,   1, 'Iasparailaun') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1020227,   1,   33557926) /* Setup */
     , (1020227,   3,  536870932) /* SoundTable */
     , (1020227,   8,  100673479) /* Icon */
     , (1020227,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:43:25.3646797-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
