DELETE FROM `weenie` WHERE `class_Id` = 1030866;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030866, 'ace1030866-hammerofthefallen', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030866,   1,          1) /* ItemType - MeleeWeapon */
     , (1030866,   5,          1) /* EncumbranceVal */
     , (1030866,   8,        340) /* Mass */
     , (1030866,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030866,  16,          1) /* ItemUseable - No */
     , (1030866,  19,         20) /* Value */
     , (1030866,  44,          1) /* Damage */
     , (1030866,  45,          4) /* DamageType - Bludgeon */
     , (1030866,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030866,  47,          4) /* AttackType - Slash */
     , (1030866,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1030866,  49,         60) /* WeaponTime */
     , (1030866,  51,          1) /* CombatUse - Melee */
     , (1030866,  53,        101) /* PlacementPosition - Resting */
     , (1030866,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030866, 150,        103) /* HookPlacement - Hook */
     , (1030866, 151,          2) /* HookType - Wall */
     , (1030866, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030866,  11, True ) /* IgnoreCollisions */
     , (1030866,  13, True ) /* Ethereal */
     , (1030866,  14, True ) /* GravityStatus */
     , (1030866,  19, True ) /* Attackable */
     , (1030866,  22, True ) /* Inscribable */
     , (1030866,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030866,  21, 0.949999988079071) /* WeaponLength */
     , (1030866,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030866,   1, 'Hammer of the Fallen') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030866,   1,   33559273) /* Setup */
     , (1030866,   3,  536870932) /* SoundTable */
     , (1030866,   8,  100677505) /* Icon */
     , (1030866,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:05:46.8422388-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
