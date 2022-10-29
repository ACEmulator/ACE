DELETE FROM `weenie` WHERE `class_Id` = 1028490;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1028490, 'ace1028490-noblewarmaul', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1028490,   1,          1) /* ItemType - MeleeWeapon */
     , (1028490,   5,          1) /* EncumbranceVal */
     , (1028490,   8,        350) /* Mass */
     , (1028490,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1028490,  16,          1) /* ItemUseable - No */
     , (1028490,  18,          1) /* UiEffects - Magical */
     , (1028490,  19,         20) /* Value */
     , (1028490,  44,          1) /* Damage */
     , (1028490,  45,          2) /* DamageType - Pierce */
     , (1028490,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1028490,  47,          4) /* AttackType - Slash */
     , (1028490,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1028490,  49,         65) /* WeaponTime */
     , (1028490,  51,          1) /* CombatUse - Melee */
     , (1028490,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1028490, 150,        103) /* HookPlacement - Hook */
     , (1028490, 151,          2) /* HookType - Wall */
     , (1028490, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1028490,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1028490,  21,    0.75) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1028490,   1, 'Noble War Maul') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1028490,   1,   33558861) /* Setup */
     , (1028490,   3,  536870932) /* SoundTable */
     , (1028490,   8,  100676975) /* Icon */
     , (1028490,  22,  872415275) /* PhysicsEffectTable */
     , (1028490,  30,         87) /* PhysicsScript - BreatheLightning */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:33:12.5406815-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
