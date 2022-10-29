DELETE FROM `weenie` WHERE `class_Id` = 1024557;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1024557, 'ace1024557-quadruplebladedaxe', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1024557,   1,          1) /* ItemType - MeleeWeapon */
     , (1024557,   5,          1) /* EncumbranceVal */
     , (1024557,   8,        320) /* Mass */
     , (1024557,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1024557,  16,          1) /* ItemUseable - No */
     , (1024557,  18,          1) /* UiEffects - Magical */
     , (1024557,  19,         20) /* Value */
     , (1024557,  44,          1) /* Damage */
     , (1024557,  45,          1) /* DamageType - Slash */
     , (1024557,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1024557,  47,          4) /* AttackType - Slash */
     , (1024557,  48,         45) /* WeaponSkill - LightWeapons */
     , (1024557,  49,         55) /* WeaponTime */
     , (1024557,  51,          1) /* CombatUse - Melee */
     , (1024557,  53,        101) /* PlacementPosition - Resting */
     , (1024557,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1024557, 150,        103) /* HookPlacement - Hook */
     , (1024557, 151,          2) /* HookType - Wall */
     , (1024557, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1024557,  11, True ) /* IgnoreCollisions */
     , (1024557,  13, True ) /* Ethereal */
     , (1024557,  14, True ) /* GravityStatus */
     , (1024557,  19, True ) /* Attackable */
     , (1024557,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1024557,  21,    0.75) /* WeaponLength */
     , (1024557,  39, 1.399999976158142) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1024557,   1, 'Quadruple-bladed Axe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1024557,   1,   33558379) /* Setup */
     , (1024557,   3,  536870932) /* SoundTable */
     , (1024557,   6,   67111919) /* PaletteBase */
     , (1024557,   8,  100674408) /* Icon */
     , (1024557,  19,         88) /* ActivationAnimation */
     , (1024557,  22,  872415275) /* PhysicsEffectTable */
     , (1024557,  30,         87) /* PhysicsScript - BreatheLightning */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:08:44.8941908-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
