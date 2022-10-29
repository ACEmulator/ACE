DELETE FROM `weenie` WHERE `class_Id` = 1028067;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1028067, 'ace1028067-superiorashbane', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1028067,   1,          1) /* ItemType - MeleeWeapon */
     , (1028067,   5,          1) /* EncumbranceVal */
     , (1028067,   8,        180) /* Mass */
     , (1028067,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1028067,  16,          1) /* ItemUseable - No */
     , (1028067,  18,         32) /* UiEffects - Fire */
     , (1028067,  19,         20) /* Value */
     , (1028067,  44,          1) /* Damage */
     , (1028067,  45,         16) /* DamageType - Fire */
     , (1028067,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1028067,  47,          6) /* AttackType - Thrust, Slash */
     , (1028067,  48,         45) /* WeaponSkill - LightWeapons */
     , (1028067,  49,         20) /* WeaponTime */
     , (1028067,  51,          1) /* CombatUse - Melee */
     , (1028067,  53,        101) /* PlacementPosition - Resting */
     , (1028067,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1028067, 114,          1) /* Attuned - Attuned */
     , (1028067, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1028067,  11, True ) /* IgnoreCollisions */
     , (1028067,  13, True ) /* Ethereal */
     , (1028067,  14, True ) /* GravityStatus */
     , (1028067,  19, True ) /* Attackable */
     , (1028067,  22, True ) /* Inscribable */
     , (1028067,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1028067,  21, 0.949999988079071) /* WeaponLength */
     , (1028067,  39, 1.2000000476837158) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1028067,   1, 'Superior Ashbane') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1028067,   1,   33558823) /* Setup */
     , (1028067,   3,  536870932) /* SoundTable */
     , (1028067,   8,  100671001) /* Icon */
     , (1028067,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:16:56.3710215-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
