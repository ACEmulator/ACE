DELETE FROM `weenie` WHERE `class_Id` = 1030342;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030342, 'ace1030342-countrenarisequalizer', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030342,   1,          1) /* ItemType - MeleeWeapon */
     , (1030342,   5,          1) /* EncumbranceVal */
     , (1030342,   8,         90) /* Mass */
     , (1030342,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030342,  16,          1) /* ItemUseable - No */
     , (1030342,  19,         20) /* Value */
     , (1030342,  44,          1) /* Damage */
     , (1030342,  45,          1) /* DamageType - Slash */
     , (1030342,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030342,  47,          4) /* AttackType - Slash */
     , (1030342,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1030342,  49,         65) /* WeaponTime */
     , (1030342,  51,          1) /* CombatUse - Melee */
     , (1030342,  52,          1) /* ParentLocation - RightHand */
     , (1030342,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030342, 151,          2) /* HookType - Wall */
     , (1030342, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030342,  11, True ) /* IgnoreCollisions */
     , (1030342,  13, True ) /* Ethereal */
     , (1030342,  14, True ) /* GravityStatus */
     , (1030342,  19, True ) /* Attackable */
     , (1030342,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030342,  21,       0) /* WeaponLength */
     , (1030342,  39, 0.8500000238418579) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030342,   1, 'Count Renari''s Equalizer') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030342,   1,   33559406) /* Setup */
     , (1030342,   3,  536870932) /* SoundTable */
     , (1030342,   6,   67111919) /* PaletteBase */
     , (1030342,   7,  268437600) /* ClothingBase */
     , (1030342,   8,  100686795) /* Icon */
     , (1030342,  22,  872415275) /* PhysicsEffectTable */
     , (1030342,  36,  234881042) /* MutateFilter */
     , (1030342,  46,  939524146) /* TsysMutationFilter */
     , (1030342,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:50:37.4845228-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
