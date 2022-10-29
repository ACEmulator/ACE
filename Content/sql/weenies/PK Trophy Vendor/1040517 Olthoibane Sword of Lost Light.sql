DELETE FROM `weenie` WHERE `class_Id` = 1040517;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1040517, 'ace1040517-olthoibaneswordoflostlight', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1040517,   1,          1) /* ItemType - MeleeWeapon */
     , (1040517,   5,        450) /* EncumbranceVal */
     , (1040517,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1040517,  16,          1) /* ItemUseable - No */
     , (1040517,  18,          1) /* UiEffects - Magical */
     , (1040517,  19,         20) /* Value */
     , (1040517,  44,          1) /* Damage */
     , (1040517,  45,          3) /* DamageType - Slash, Pierce */
     , (1040517,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1040517,  47,          2) /* AttackType - Thrust */
     , (1040517,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1040517,  49,          1) /* WeaponTime */
     , (1040517,  51,          1) /* CombatUse - Melee */
     , (1040517,  52,          1) /* ParentLocation - RightHand */
     , (1040517,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1040517,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1040517, 151,          2) /* HookType - Wall */
     , (1040517, 159,         44) /* WieldSkillType - HeavyWeapons */
     , (1040517, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1040517,  11, True ) /* IgnoreCollisions */
     , (1040517,  13, True ) /* Ethereal */
     , (1040517,  14, True ) /* GravityStatus */
     , (1040517,  19, True ) /* Attackable */
     , (1040517,  22, True ) /* Inscribable */
     , (1040517,  69, False) /* IsSellable */
     , (1040517,  85, True ) /* AppraisalHasAllowedWielder */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1040517,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1040517,   1, 'Olthoibane Sword of Lost Light') /* Name */
     , (1040517,  16, 'The Empowered Sword of Lost Light, infused with the power of the Paradox-touched Olthoi, which is deadly to normal Olthoi.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1040517,   1,   33560709) /* Setup */
     , (1040517,   3,  536870932) /* SoundTable */
     , (1040517,   8,  100674513) /* Icon */
     , (1040517,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T05:58:58.7280341-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
