DELETE FROM `weenie` WHERE `class_Id` = 1009598;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1009598, 'ace1009598-fistofthequiddity', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1009598,   1,          1) /* ItemType - MeleeWeapon */
     , (1009598,   3,         82) /* PaletteTemplate - PinkPurple */
     , (1009598,   5,          1) /* EncumbranceVal */
     , (1009598,   8,         90) /* Mass */
     , (1009598,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1009598,  19,         20) /* Value */
     , (1009598,  45,          4) /* DamageType - Bludgeon */
     , (1009598,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (1009598,  47,          1) /* AttackType - Punch */
     , (1009598,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1009598,  52,          1) /* ParentLocation - RightHand */
     , (1009598,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1009598,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1009598, 150,        103) /* HookPlacement - Hook */
     , (1009598, 151,          2) /* HookType - Wall */
     , (1009598, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1009598,  11, True ) /* IgnoreCollisions */
     , (1009598,  13, True ) /* Ethereal */
     , (1009598,  14, True ) /* GravityStatus */
     , (1009598,  15, True ) /* LightsStatus */
     , (1009598,  19, True ) /* Attackable */
     , (1009598,  22, True ) /* Inscribable */
     , (1009598,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1009598,  12,       0) /* Shade */
     , (1009598,  39, 0.800000011920929) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1009598,   1, 'Fist of the Quiddity') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1009598,   1,   33557109) /* Setup */
     , (1009598,   3,  536870932) /* SoundTable */
     , (1009598,   7,  268436199) /* ClothingBase */
     , (1009598,   8,  100671695) /* Icon */
     , (1009598,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T23:48:01.335678-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Updated Skill - Reqs",
  "IsDone": true
}
*/
