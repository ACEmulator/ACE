DELETE FROM `weenie` WHERE `class_Id` = 1028218;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1028218, 'ace1028218-sabletoothdirk', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1028218,   1,          1) /* ItemType - MeleeWeapon */
     , (1028218,   5,          1) /* EncumbranceVal */
     , (1028218,   8,        360) /* Mass */
     , (1028218,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1028218,  16,          1) /* ItemUseable - No */
     , (1028218,  18,          1) /* UiEffects - Magical */
     , (1028218,  19,         20) /* Value */
     , (1028218,  44,          1) /* Damage */
     , (1028218,  45,          3) /* DamageType - Slash, Pierce */
     , (1028218,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1028218,  47,          6) /* AttackType - Thrust, Slash */
     , (1028218,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1028218,  49,         35) /* WeaponTime */
     , (1028218,  51,          1) /* CombatUse - Melee */
     , (1028218,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1028218, 150,        103) /* HookPlacement - Hook */
     , (1028218, 151,          2) /* HookType - Wall */
     , (1028218, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1028218,  22, True ) /* Inscribable */
     , (1028218,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1028218,  21, 0.6200000047683716) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1028218,   1, 'Sable Tooth Dirk') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1028218,   1,   33558829) /* Setup */
     , (1028218,   3,  536870932) /* SoundTable */
     , (1028218,   8,  100676802) /* Icon */
     , (1028218,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:03:42.6465361-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
