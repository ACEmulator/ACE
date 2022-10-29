DELETE FROM `weenie` WHERE `class_Id` = 1034024;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1034024, 'ace1034024-silifiofcrimsonnight', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1034024,   1,          1) /* ItemType - MeleeWeapon */
     , (1034024,   5,          1) /* EncumbranceVal */
     , (1034024,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1034024,  16,          1) /* ItemUseable - No */
     , (1034024,  18,          1) /* UiEffects - Magical */
     , (1034024,  19,         20) /* Value */
     , (1034024,  44,          1) /* Damage */
     , (1034024,  45,         64) /* DamageType - Electric */
     , (1034024,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1034024,  47,          4) /* AttackType - Slash */
     , (1034024,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1034024,  49,         50) /* WeaponTime */
     , (1034024,  51,          1) /* CombatUse - Melee */
     , (1034024,  52,          1) /* ParentLocation - RightHand */
     , (1034024,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1034024,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1034024, 151,          2) /* HookType - Wall */
     , (1034024, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1034024,  11, True ) /* IgnoreCollisions */
     , (1034024,  13, True ) /* Ethereal */
     , (1034024,  14, True ) /* GravityStatus */
     , (1034024,  19, True ) /* Attackable */
     , (1034024,  22, True ) /* Inscribable */
     , (1034024,  69, False) /* IsSellable */
     , (1034024,  85, True ) /* AppraisalHasAllowedWielder */
     , (1034024,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1034024,  21,       0) /* WeaponLength */
     , (1034024,  39,    1.25) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1034024,   1, 'Silifi of Crimson Night') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1034024,   1,   33556553) /* Setup */
     , (1034024,   3,  536870932) /* SoundTable */
     , (1034024,   6,   67111919) /* PaletteBase */
     , (1034024,   8,  100670613) /* Icon */
     , (1034024,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:28:38.3168967-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
