DELETE FROM `weenie` WHERE `class_Id` = 10352972;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10352972, 'ace10352972-greatswordoflightning', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10352972,   1,          1) /* ItemType - MeleeWeapon */
     , (10352972,   5,        500) /* EncumbranceVal */
     , (10352972,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (10352972,  16,          1) /* ItemUseable - No */
     , (10352972,  18,         64) /* UiEffects - Lightning */
     , (10352972,  19,         20) /* Value */
     , (10352972,  36,       9999) /* ResistMagic */
     , (10352972,  44,          1) /* Damage */
     , (10352972,  45,         64) /* DamageType - Electric */
     , (10352972,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (10352972,  47,          6) /* AttackType - Thrust, Slash */
     , (10352972,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (10352972,  49,          1) /* WeaponTime */
     , (10352972,  51,          1) /* CombatUse - Melee */
     , (10352972,  52,          1) /* ParentLocation - RightHand */
     , (10352972,  53,          1) /* PlacementPosition - RightHandCombat */
     , (10352972,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10352972, 158,          0) /* WieldRequirements - Invalid */
     , (10352972, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10352972,  11, True ) /* IgnoreCollisions */
     , (10352972,  13, True ) /* Ethereal */
     , (10352972,  14, True ) /* GravityStatus */
     , (10352972,  19, True ) /* Attackable */
     , (10352972,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10352972,   5, -0.05000000074505806) /* ManaRate */
     , (10352972,  21,       1) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10352972,   1, 'Greatsword of Lightning') /* Name */
     , (10352972,  16, 'A sword mostly composed of a white-hot flame. Lightning striking between the free floating metal sections that the flames coalesce around.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10352972,   1,   33560276) /* Setup */
     , (10352972,   3,  536870932) /* SoundTable */
     , (10352972,   8,  100689459) /* Icon */
     , (10352972,  22,  872415275) /* PhysicsEffectTable */
     , (10352972,  55,       3911) /* ProcSpell - Spiral of Souls */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T21:38:19.0253228-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Sword of Tower Guardian (35273)",
  "IsDone": false
}
*/
