DELETE FROM `weenie` WHERE `class_Id` = 1023542;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1023542, 'ace1023542-staffofthenomads', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1023542,   1,          1) /* ItemType - MeleeWeapon */
     , (1023542,   3,         61) /* PaletteTemplate - White */
     , (1023542,   5,          1) /* EncumbranceVal */
     , (1023542,   8,        360) /* Mass */
     , (1023542,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1023542,  16,          1) /* ItemUseable - No */
     , (1023542,  18,          1) /* UiEffects - Magical */
     , (1023542,  19,         20) /* Value */
     , (1023542,  44,          1) /* Damage */
     , (1023542,  45,         16) /* DamageType - Fire */
     , (1023542,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1023542,  47,          6) /* AttackType - Thrust, Slash */
     , (1023542,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1023542,  49,         40) /* WeaponTime */
     , (1023542,  51,          1) /* CombatUse - Melee */
     , (1023542,  53,        101) /* PlacementPosition - Resting */
     , (1023542,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1023542, 150,        103) /* HookPlacement - Hook */
     , (1023542, 151,          2) /* HookType - Wall */
     , (1023542, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1023542,  11, True ) /* IgnoreCollisions */
     , (1023542,  13, True ) /* Ethereal */
     , (1023542,  14, True ) /* GravityStatus */
     , (1023542,  19, True ) /* Attackable */
     , (1023542,  22, True ) /* Inscribable */
     , (1023542,  23, True ) /* DestroyOnSell */
     , (1023542,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1023542,  39,    1.25) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1023542,   1, 'Staff of the Nomads') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1023542,   1,   33558224) /* Setup */
     , (1023542,   3,  536870932) /* SoundTable */
     , (1023542,   6,   67111919) /* PaletteBase */
     , (1023542,   7,  268435796) /* ClothingBase */
     , (1023542,   8,  100674090) /* Icon */
     , (1023542,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:58:45.3517504-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
