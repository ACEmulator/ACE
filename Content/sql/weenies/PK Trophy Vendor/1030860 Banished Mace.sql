DELETE FROM `weenie` WHERE `class_Id` = 1030860;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030860, 'ace1030860-banishedmace', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030860,   1,          1) /* ItemType - MeleeWeapon */
     , (1030860,   5,          1) /* EncumbranceVal */
     , (1030860,   8,        340) /* Mass */
     , (1030860,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030860,  16,          1) /* ItemUseable - No */
     , (1030860,  19,         20) /* Value */
     , (1030860,  44,          1) /* Damage */
     , (1030860,  45,          8) /* DamageType - Cold */
     , (1030860,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030860,  47,          4) /* AttackType - Slash */
     , (1030860,  48,         45) /* WeaponSkill - LightWeapons */
     , (1030860,  49,         40) /* WeaponTime */
     , (1030860,  51,          1) /* CombatUse - Melee */
     , (1030860,  53,        101) /* PlacementPosition - Resting */
     , (1030860,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030860, 150,        103) /* HookPlacement - Hook */
     , (1030860, 151,          2) /* HookType - Wall */
     , (1030860, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030860,  11, True ) /* IgnoreCollisions */
     , (1030860,  13, True ) /* Ethereal */
     , (1030860,  14, True ) /* GravityStatus */
     , (1030860,  19, True ) /* Attackable */
     , (1030860,  22, True ) /* Inscribable */
     , (1030860,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030860,  21, 0.6200000047683716) /* WeaponLength */
     , (1030860,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030860,   1, 'Banished Mace') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030860,   1,   33559263) /* Setup */
     , (1030860,   3,  536870932) /* SoundTable */
     , (1030860,   8,  100677480) /* Icon */
     , (1030860,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:12:13.6542639-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
