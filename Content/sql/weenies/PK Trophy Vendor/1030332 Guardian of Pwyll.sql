DELETE FROM `weenie` WHERE `class_Id` = 1030332;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030332, 'ace1030332-guardianofpwyll', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030332,   1,          1) /* ItemType - MeleeWeapon */
     , (1030332,   5,          1) /* EncumbranceVal */
     , (1030332,   8,         90) /* Mass */
     , (1030332,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030332,  16,          1) /* ItemUseable - No */
     , (1030332,  19,         20) /* Value */
     , (1030332,  44,          1) /* Damage */
     , (1030332,  45,          2) /* DamageType - Pierce */
     , (1030332,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030332,  47,          2) /* AttackType - Thrust */
     , (1030332,  48,         45) /* WeaponSkill - LightWeapons */
     , (1030332,  49,         50) /* WeaponTime */
     , (1030332,  51,          1) /* CombatUse - Melee */
     , (1030332,  52,          1) /* ParentLocation - RightHand */
     , (1030332,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030332, 151,          2) /* HookType - Wall */
     , (1030332, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030332,  11, True ) /* IgnoreCollisions */
     , (1030332,  13, True ) /* Ethereal */
     , (1030332,  14, True ) /* GravityStatus */
     , (1030332,  19, True ) /* Attackable */
     , (1030332,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030332,  21,       0) /* WeaponLength */
     , (1030332,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030332,   1, 'Guardian of Pwyll') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030332,   1,   33559396) /* Setup */
     , (1030332,   3,  536870932) /* SoundTable */
     , (1030332,   6,   67111919) /* PaletteBase */
     , (1030332,   7,  268437600) /* ClothingBase */
     , (1030332,   8,  100686775) /* Icon */
     , (1030332,  22,  872415275) /* PhysicsEffectTable */
     , (1030332,  36,  234881042) /* MutateFilter */
     , (1030332,  46,  939524146) /* TsysMutationFilter */
     , (1030332,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:18:37.9067587-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
