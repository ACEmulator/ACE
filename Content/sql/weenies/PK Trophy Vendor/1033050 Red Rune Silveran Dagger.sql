DELETE FROM `weenie` WHERE `class_Id` = 1033050;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1033050, 'ace1033050-redrunesilverandagger', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1033050,   1,          1) /* ItemType - MeleeWeapon */
     , (1033050,   5,          1) /* EncumbranceVal */
     , (1033050,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1033050,  16,          1) /* ItemUseable - No */
     , (1033050,  19,         20) /* Value */
     , (1033050,  44,          1) /* Damage */
     , (1033050,  45,          3) /* DamageType - Slash, Pierce */
     , (1033050,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1033050,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1033050,  49,          0) /* WeaponTime */
     , (1033050,  51,          1) /* CombatUse - Melee */
     , (1033050,  53,        101) /* PlacementPosition - Resting */
     , (1033050,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1033050, 151,          2) /* HookType - Wall */
     , (1033050, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1033050,  11, True ) /* IgnoreCollisions */
     , (1033050,  13, True ) /* Ethereal */
     , (1033050,  14, True ) /* GravityStatus */
     , (1033050,  19, True ) /* Attackable */
     , (1033050,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1033050,   1, 'Red Rune Silveran Dagger') /* Name */
     , (1033050,  15, 'A dagger crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1033050,   1,   33559888) /* Setup */
     , (1033050,   3,  536870932) /* SoundTable */
     , (1033050,   8,  100688896) /* Icon */
     , (1033050,  22,  872415275) /* PhysicsEffectTable */
     , (1033050,  50,  100688915) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T05:42:12.9243611-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
