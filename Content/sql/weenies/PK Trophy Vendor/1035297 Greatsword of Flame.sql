DELETE FROM `weenie` WHERE `class_Id` = 1035297;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035297, 'ace1035297-greatswordofflame', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035297,   1,          1) /* ItemType - MeleeWeapon */
     , (1035297,   5,        500) /* EncumbranceVal */
     , (1035297,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1035297,  16,          1) /* ItemUseable - No */
     , (1035297,  18,         32) /* UiEffects - Fire */
     , (1035297,  19,         20) /* Value */
     , (1035297,  36,       9999) /* ResistMagic */
     , (1035297,  44,          1) /* Damage */
     , (1035297,  45,         16) /* DamageType - Fire */
     , (1035297,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1035297,  47,          6) /* AttackType - Thrust, Slash */
     , (1035297,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1035297,  49,          1) /* WeaponTime */
     , (1035297,  51,          1) /* CombatUse - Melee */
     , (1035297,  52,          1) /* ParentLocation - RightHand */
     , (1035297,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1035297,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1035297, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035297,  11, True ) /* IgnoreCollisions */
     , (1035297,  13, True ) /* Ethereal */
     , (1035297,  14, True ) /* GravityStatus */
     , (1035297,  19, True ) /* Attackable */
     , (1035297,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1035297,   5, -0.05000000074505806) /* ManaRate */
     , (1035297,  21,       1) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035297,   1, 'Greatsword of Flame') /* Name */
     , (1035297,  16, 'A sword mostly composed of a white-hot flame. Lightning striking between the free floating metal sections that the flames coalesce around.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035297,   1,   33560276) /* Setup */
     , (1035297,   3,  536870932) /* SoundTable */
     , (1035297,   8,  100689459) /* Icon */
     , (1035297,  22,  872415275) /* PhysicsEffectTable */
     , (1035297,  55,       3911) /* ProcSpell - Spiral of Souls */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T21:09:21.9712453-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Sword of Tower Guardian (35273)",
  "IsDone": false
}
*/
