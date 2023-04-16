DELETE FROM `weenie` WHERE `class_Id` = 10352973;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10352973, 'ace10352973-greatswordofacid', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10352973,   1,          1) /* ItemType - MeleeWeapon */
     , (10352973,   5,        500) /* EncumbranceVal */
     , (10352973,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (10352973,  16,          1) /* ItemUseable - No */
     , (10352973,  18,        256) /* UiEffects - Acid */
     , (10352973,  19,         20) /* Value */
     , (10352973,  36,       9999) /* ResistMagic */
     , (10352973,  44,          1) /* Damage */
     , (10352973,  45,         32) /* DamageType - Acid */
     , (10352973,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (10352973,  47,          6) /* AttackType - Thrust, Slash */
     , (10352973,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (10352973,  49,          1) /* WeaponTime */
     , (10352973,  51,          1) /* CombatUse - Melee */
     , (10352973,  52,          1) /* ParentLocation - RightHand */
     , (10352973,  53,          1) /* PlacementPosition - RightHandCombat */
     , (10352973,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10352973, 158,          0) /* WieldRequirements - Invalid */
     , (10352973, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10352973,  11, True ) /* IgnoreCollisions */
     , (10352973,  13, True ) /* Ethereal */
     , (10352973,  14, True ) /* GravityStatus */
     , (10352973,  19, True ) /* Attackable */
     , (10352973,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10352973,   5, -0.05000000074505806) /* ManaRate */
     , (10352973,  21,       1) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10352973,   1, 'Greatsword of Acid') /* Name */
     , (10352973,  16, 'A sword mostly composed of a white-hot flame. Lightning striking between the free floating metal sections that the flames coalesce around.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10352973,   1,   33560276) /* Setup */
     , (10352973,   3,  536870932) /* SoundTable */
     , (10352973,   8,  100689459) /* Icon */
     , (10352973,  22,  872415275) /* PhysicsEffectTable */
     , (10352973,  55,       3911) /* ProcSpell - Spiral of Souls */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T21:40:05.7711458-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Sword of Tower Guardian (35273)",
  "IsDone": false
}
*/
