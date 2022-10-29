DELETE FROM `weenie` WHERE `class_Id` = 1030634;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030634, 'ace1030634-cyphissuldowshalfmoonspear', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030634,   1,          1) /* ItemType - MeleeWeapon */
     , (1030634,   5,          1) /* EncumbranceVal */
     , (1030634,   8,        140) /* Mass */
     , (1030634,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030634,  16,          1) /* ItemUseable - No */
     , (1030634,  19,         20) /* Value */
     , (1030634,  44,          1) /* Damage */
     , (1030634,  45,          8) /* DamageType - Cold */
     , (1030634,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030634,  47,          2) /* AttackType - Thrust */
     , (1030634,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1030634,  49,         30) /* WeaponTime */
     , (1030634,  51,          1) /* CombatUse - Melee */
     , (1030634,  53,        101) /* PlacementPosition - Resting */
     , (1030634,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030634, 150,        103) /* HookPlacement - Hook */
     , (1030634, 151,          2) /* HookType - Wall */
     , (1030634, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030634,  11, True ) /* IgnoreCollisions */
     , (1030634,  13, True ) /* Ethereal */
     , (1030634,  14, True ) /* GravityStatus */
     , (1030634,  19, False) /* Attackable */
     , (1030634,  22, True ) /* Inscribable */
     , (1030634,  23, True ) /* DestroyOnSell */
     , (1030634,  69, True ) /* IsSellable */
     , (1030634,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030634,  21,     1.5) /* WeaponLength */
     , (1030634,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030634,   1, 'Cyphis Suldow''s Half Moon Spear') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030634,   1,   33559207) /* Setup */
     , (1030634,   3,  536870932) /* SoundTable */
     , (1030634,   8,  100677384) /* Icon */
     , (1030634,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:00:11.6946587-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
