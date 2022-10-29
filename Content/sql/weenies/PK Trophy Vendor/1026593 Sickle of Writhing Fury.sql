DELETE FROM `weenie` WHERE `class_Id` = 1026593;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1026593, 'ace1026593-sickleofwrithingfury', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1026593,   1,          1) /* ItemType - MeleeWeapon */
     , (1026593,   3,         39) /* PaletteTemplate - Black */
     , (1026593,   5,          1) /* EncumbranceVal */
     , (1026593,   8,        320) /* Mass */
     , (1026593,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1026593,  16,          1) /* ItemUseable - No */
     , (1026593,  19,         20) /* Value */
     , (1026593,  44,          1) /* Damage */
     , (1026593,  45,          1) /* DamageType - Slash */
     , (1026593,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1026593,  47,          4) /* AttackType - Slash */
     , (1026593,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1026593,  49,         65) /* WeaponTime */
     , (1026593,  51,          1) /* CombatUse - Melee */
     , (1026593,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1026593, 150,        103) /* HookPlacement - Hook */
     , (1026593, 151,          2) /* HookType - Wall */
     , (1026593, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1026593,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1026593,  21,    0.75) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1026593,   1, 'Sickle of Writhing Fury') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1026593,   1,   33558593) /* Setup */
     , (1026593,   3,  536870932) /* SoundTable */
     , (1026593,   6,   67114956) /* PaletteBase */
     , (1026593,   7,  268436792) /* ClothingBase */
     , (1026593,   8,  100675777) /* Icon */
     , (1026593,  22,  872415275) /* PhysicsEffectTable */
     , (1026593,  30,         87) /* PhysicsScript - BreatheLightning */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:33:35.2038319-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
