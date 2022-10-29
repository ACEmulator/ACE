DELETE FROM `weenie` WHERE `class_Id` = 1023522;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1023522, 'ace1023522-overlordssword', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1023522,   1,          1) /* ItemType - MeleeWeapon */
     , (1023522,   3,         61) /* PaletteTemplate - White */
     , (1023522,   5,          1) /* EncumbranceVal */
     , (1023522,   8,        420) /* Mass */
     , (1023522,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1023522,  16,          1) /* ItemUseable - No */
     , (1023522,  19,         20) /* Value */
     , (1023522,  44,          1) /* Damage */
     , (1023522,  45,         64) /* DamageType - Electric */
     , (1023522,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1023522,  47,          6) /* AttackType - Thrust, Slash */
     , (1023522,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1023522,  49,         50) /* WeaponTime */
     , (1023522,  51,          1) /* CombatUse - Melee */
     , (1023522,  52,          8) /* ParentLocation - LeftWeapon */
     , (1023522,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1023522,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1023522, 150,        103) /* HookPlacement - Hook */
     , (1023522, 151,          2) /* HookType - Wall */
     , (1023522, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1023522,  11, True ) /* IgnoreCollisions */
     , (1023522,  13, True ) /* Ethereal */
     , (1023522,  14, True ) /* GravityStatus */
     , (1023522,  19, True ) /* Attackable */
     , (1023522,  22, True ) /* Inscribable */
     , (1023522,  23, True ) /* DestroyOnSell */
     , (1023522,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1023522,  21, 0.949999988079071) /* WeaponLength */
     , (1023522,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1023522,   1, 'Overlord''s Sword') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1023522,   1,   33558185) /* Setup */
     , (1023522,   3,  536870932) /* SoundTable */
     , (1023522,   6,   67111092) /* PaletteBase */
     , (1023522,   8,  100674032) /* Icon */
     , (1023522,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:44:20.4311754-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
