DELETE FROM `weenie` WHERE `class_Id` = 1051988;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1051988, 'ace1051988-rynthidtentaclebow', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1051988,   1,        256) /* ItemType - MissileWeapon */
     , (1051988,   5,        950) /* EncumbranceVal */
     , (1051988,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1051988,  16,          1) /* ItemUseable - No */
     , (1051988,  18,          1) /* UiEffects - Magical */
     , (1051988,  19,         20) /* Value */
     , (1051988,  33,          1) /* Bonded - Bonded */
     , (1051988,  44,          1) /* Damage */
     , (1051988,  46,         16) /* DefaultCombatStyle - Bow */
     , (1051988,  49,          1) /* WeaponTime */
     , (1051988,  50,          1) /* AmmoType - Arrow */
     , (1051988,  51,          2) /* CombatUse - Missle */
     , (1051988,  52,          2) /* ParentLocation - LeftHand */
     , (1051988,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1051988, 114,          0) /* Attuned - Normal */
     , (1051988, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1051988,  11, True ) /* IgnoreCollisions */
     , (1051988,  13, True ) /* Ethereal */
     , (1051988,  14, True ) /* GravityStatus */
     , (1051988,  19, True ) /* Attackable */
     , (1051988,  22, True ) /* Inscribable */
     , (1051988,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1051988,   5, -0.032999999821186066) /* ManaRate */
     , (1051988,  21,       0) /* WeaponLength */
     , (1051988,  22,       0) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1051988,   1, 'Rynthid Tentacle Bow') /* Name */
     , (1051988,  16, 'A bow crafted from enchanted obsidian and Rynthid tentacles.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1051988,   1,   33561601) /* Setup */
     , (1051988,   3,  536870932) /* SoundTable */
     , (1051988,   6,   67111919) /* PaletteBase */
     , (1051988,   8,  100693229) /* Icon */
     , (1051988,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T20:57:52.8052229-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
