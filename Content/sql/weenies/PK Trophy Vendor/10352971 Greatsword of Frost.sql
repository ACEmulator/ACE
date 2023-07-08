DELETE FROM `weenie` WHERE `class_Id` = 10352971;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10352971, 'ace10352971-greatswordoffrost', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10352971,   1,          1) /* ItemType - MeleeWeapon */
     , (10352971,   5,        500) /* EncumbranceVal */
     , (10352971,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (10352971,  16,          1) /* ItemUseable - No */
     , (10352971,  18,        128) /* UiEffects - Frost */
     , (10352971,  19,         20) /* Value */
     , (10352971,  36,       9999) /* ResistMagic */
     , (10352971,  44,          1) /* Damage */
     , (10352971,  45,          8) /* DamageType - Cold */
     , (10352971,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (10352971,  47,          6) /* AttackType - Thrust, Slash */
     , (10352971,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (10352971,  49,          1) /* WeaponTime */
     , (10352971,  51,          1) /* CombatUse - Melee */
     , (10352971,  52,          1) /* ParentLocation - RightHand */
     , (10352971,  53,          1) /* PlacementPosition - RightHandCombat */
     , (10352971,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10352971, 158,          0) /* WieldRequirements - Invalid */
     , (10352971, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10352971,  11, True ) /* IgnoreCollisions */
     , (10352971,  13, True ) /* Ethereal */
     , (10352971,  14, True ) /* GravityStatus */
     , (10352971,  19, True ) /* Attackable */
     , (10352971,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10352971,   5, -0.05000000074505806) /* ManaRate */
     , (10352971,  21,       1) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10352971,   1, 'Greatsword of Frost') /* Name */
     , (10352971,  16, 'A sword mostly composed of a white-hot flame. Lightning striking between the free floating metal sections that the flames coalesce around.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10352971,   1,   33560276) /* Setup */
     , (10352971,   3,  536870932) /* SoundTable */
     , (10352971,   8,  100689459) /* Icon */
     , (10352971,  22,  872415275) /* PhysicsEffectTable */
     , (10352971,  55,       3911) /* ProcSpell - Spiral of Souls */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T21:38:47.3580246-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Sword of Tower Guardian (35273)",
  "IsDone": false
}
*/
