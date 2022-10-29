DELETE FROM `weenie` WHERE `class_Id` = 1024027;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1024027, 'ace1024027-scepterofthunderousmight', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1024027,   1,          1) /* ItemType - MeleeWeapon */
     , (1024027,   5,          1) /* EncumbranceVal */
     , (1024027,   8,        360) /* Mass */
     , (1024027,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1024027,  16,          1) /* ItemUseable - No */
     , (1024027,  18,          1) /* UiEffects - Magical */
     , (1024027,  19,         20) /* Value */
     , (1024027,  44,          1) /* Damage */
     , (1024027,  45,         64) /* DamageType - Electric */
     , (1024027,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1024027,  47,          4) /* AttackType - Slash */
     , (1024027,  48,         45) /* WeaponSkill - LightWeapons */
     , (1024027,  49,         60) /* WeaponTime */
     , (1024027,  51,          1) /* CombatUse - Melee */
     , (1024027,  53,        101) /* PlacementPosition - Resting */
     , (1024027,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1024027, 150,        103) /* HookPlacement - Hook */
     , (1024027, 151,          2) /* HookType - Wall */
     , (1024027, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1024027,  11, True ) /* IgnoreCollisions */
     , (1024027,  13, True ) /* Ethereal */
     , (1024027,  14, True ) /* GravityStatus */
     , (1024027,  15, True ) /* LightsStatus */
     , (1024027,  19, True ) /* Attackable */
     , (1024027,  22, True ) /* Inscribable */
     , (1024027,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1024027,  21, 0.6200000047683716) /* WeaponLength */
     , (1024027,  39, 0.30000001192092896) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1024027,   1, 'Scepter of Thunderous Might') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1024027,   1,   33558265) /* Setup */
     , (1024027,   3,  536870932) /* SoundTable */
     , (1024027,   8,  100674149) /* Icon */
     , (1024027,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:11:43.4611934-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
