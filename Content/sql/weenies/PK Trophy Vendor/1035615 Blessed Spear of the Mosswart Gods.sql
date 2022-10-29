DELETE FROM `weenie` WHERE `class_Id` = 1035615;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035615, 'ace1035615-blessedspearofthemosswartgods', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035615,   1,          1) /* ItemType - MeleeWeapon */
     , (1035615,   5,          1) /* EncumbranceVal */
     , (1035615,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1035615,  16,          1) /* ItemUseable - No */
     , (1035615,  18,         32) /* UiEffects - Fire */
     , (1035615,  19,         20) /* Value */
     , (1035615,  44,          1) /* Damage */
     , (1035615,  45,         32) /* DamageType - Acid */
     , (1035615,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1035615,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1035615,  51,          1) /* CombatUse - Melee */
     , (1035615,  53,        101) /* PlacementPosition - Resting */
     , (1035615,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1035615, 151,          2) /* HookType - Wall */
     , (1035615, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035615,  11, True ) /* IgnoreCollisions */
     , (1035615,  13, True ) /* Ethereal */
     , (1035615,  14, True ) /* GravityStatus */
     , (1035615,  19, True ) /* Attackable */
     , (1035615,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035615,   1, 'Blessed Spear of the Mosswart Gods') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035615,   1,   33556901) /* Setup */
     , (1035615,   3,  536870932) /* SoundTable */
     , (1035615,   6,   67111919) /* PaletteBase */
     , (1035615,   8,  100671208) /* Icon */
     , (1035615,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:42:14.4291617-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
