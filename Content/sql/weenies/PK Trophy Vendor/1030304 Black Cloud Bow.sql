DELETE FROM `weenie` WHERE `class_Id` = 1030304;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030304, 'ace1030304-blackcloudbow', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030304,   1,        256) /* ItemType - MissileWeapon */
     , (1030304,   5,          1) /* EncumbranceVal */
     , (1030304,   8,        140) /* Mass */
     , (1030304,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1030304,  16,          1) /* ItemUseable - No */
     , (1030304,  19,         20) /* Value */
     , (1030304,  46,         16) /* DefaultCombatStyle - Bow */
     , (1030304,  49,         70) /* WeaponTime */
     , (1030304,  50,          1) /* AmmoType - Arrow */
     , (1030304,  51,          2) /* CombatUse - Missle */
     , (1030304,  52,          2) /* ParentLocation - LeftHand */
     , (1030304,  53,          3) /* PlacementPosition - LeftHand */
     , (1030304,  60,        175) /* WeaponRange */
     , (1030304,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030304, 150,        103) /* HookPlacement - Hook */
     , (1030304, 151,          2) /* HookType - Wall */
     , (1030304, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030304,  11, True ) /* IgnoreCollisions */
     , (1030304,  13, True ) /* Ethereal */
     , (1030304,  14, True ) /* GravityStatus */
     , (1030304,  19, True ) /* Attackable */
     , (1030304,  22, True ) /* Inscribable */
     , (1030304,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030304,  21,       0) /* WeaponLength */
     , (1030304,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030304,   1, 'Black Cloud Bow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030304,   1,   33559368) /* Setup */
     , (1030304,   3,  536870932) /* SoundTable */
     , (1030304,   6,   67111919) /* PaletteBase */
     , (1030304,   7,  268437600) /* ClothingBase */
     , (1030304,   8,  100686719) /* Icon */
     , (1030304,  22,  872415275) /* PhysicsEffectTable */
     , (1030304,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-07T16:25:38.7258491-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
