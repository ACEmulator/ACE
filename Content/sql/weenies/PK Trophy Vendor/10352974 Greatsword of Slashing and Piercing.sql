DELETE FROM `weenie` WHERE `class_Id` = 10352974;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10352974, 'ace10352974-greatswordofslashingandpiercing', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10352974,   1,          1) /* ItemType - MeleeWeapon */
     , (10352974,   5,        500) /* EncumbranceVal */
     , (10352974,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (10352974,  16,          1) /* ItemUseable - No */
     , (10352974,  18,       3072) /* UiEffects - Slashing, Piercing */
     , (10352974,  19,         20) /* Value */
     , (10352974,  36,       9999) /* ResistMagic */
     , (10352974,  44,          1) /* Damage */
     , (10352974,  45,          3) /* DamageType - Slash, Pierce */
     , (10352974,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (10352974,  47,          6) /* AttackType - Thrust, Slash */
     , (10352974,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (10352974,  49,          1) /* WeaponTime */
     , (10352974,  51,          1) /* CombatUse - Melee */
     , (10352974,  52,          1) /* ParentLocation - RightHand */
     , (10352974,  53,          1) /* PlacementPosition - RightHandCombat */
     , (10352974,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10352974, 158,          0) /* WieldRequirements - Invalid */
     , (10352974, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10352974,  11, True ) /* IgnoreCollisions */
     , (10352974,  13, True ) /* Ethereal */
     , (10352974,  14, True ) /* GravityStatus */
     , (10352974,  19, True ) /* Attackable */
     , (10352974,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10352974,   5, -0.05000000074505806) /* ManaRate */
     , (10352974,  21,       1) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10352974,   1, 'Greatsword of Slashing and Piercing') /* Name */
     , (10352974,  16, 'A sword mostly composed of a white-hot flame. Lightning striking between the free floating metal sections that the flames coalesce around.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10352974,   1,   33560276) /* Setup */
     , (10352974,   3,  536870932) /* SoundTable */
     , (10352974,   8,  100689459) /* Icon */
     , (10352974,  22,  872415275) /* PhysicsEffectTable */
     , (10352974,  55,       3911) /* ProcSpell - Spiral of Souls */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T21:40:33.8417447-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Sword of Tower Guardian (35273)",
  "IsDone": false
}
*/
