DELETE FROM `weenie` WHERE `class_Id` = 1037579;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1037579, 'ace1037579-soulboundcrossbow', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1037579,   1,        256) /* ItemType - MissileWeapon */
     , (1037579,   3,         20) /* PaletteTemplate - Silver */
     , (1037579,   5,          1) /* EncumbranceVal */
     , (1037579,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1037579,  16,          1) /* ItemUseable - No */
     , (1037579,  18,          1) /* UiEffects - Magical */
     , (1037579,  19,         20) /* Value */
     , (1037579,  44,          1) /* Damage */
     , (1037579,  45,          0) /* DamageType - Undef */
     , (1037579,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (1037579,  48,         47) /* WeaponSkill - MissileWeapons */
     , (1037579,  52,          2) /* ParentLocation - LeftHand */
     , (1037579,  53,          3) /* PlacementPosition - LeftHand */
     , (1037579,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1037579, 151,          2) /* HookType - Wall */
     , (1037579, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1037579,  11, True ) /* IgnoreCollisions */
     , (1037579,  13, True ) /* Ethereal */
     , (1037579,  14, True ) /* GravityStatus */
     , (1037579,  15, True ) /* LightsStatus */
     , (1037579,  19, True ) /* Attackable */
     , (1037579,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1037579,  39,    1.25) /* DefaultScale */
     , (1037579,  76, 0.699999988079071) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1037579,   1, 'Soul Bound Crossbow') /* Name */
     , (1037579,  16, 'A ghostly blue crossbow, bound to your soul.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1037579,   1,   33560580) /* Setup */
     , (1037579,   3,  536870932) /* SoundTable */
     , (1037579,   7,  268436428) /* ClothingBase */
     , (1037579,   8,  100673202) /* Icon */
     , (1037579,  22,  872415275) /* PhysicsEffectTable */
     , (1037579,  52,  100689896) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-07-09T17:10:22.240179-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "add clothing base returned to eor stats\nadd float 136 crit multiplier 2, removed biting strike ",
  "IsDone": false
}
*/
