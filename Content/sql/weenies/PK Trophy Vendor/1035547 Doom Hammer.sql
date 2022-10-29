DELETE FROM `weenie` WHERE `class_Id` = 1035547;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1035547, 'ace1035547-doomhammer', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1035547,   1,          1) /* ItemType - MeleeWeapon */
     , (1035547,   3,         22) /* PaletteTemplate - Aqua */
     , (1035547,   5,          1) /* EncumbranceVal */
     , (1035547,   8,        230) /* Mass */
     , (1035547,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1035547,  16,          1) /* ItemUseable - No */
     , (1035547,  18,        512) /* UiEffects - Bludgeoning */
     , (1035547,  19,         20) /* Value */
     , (1035547,  44,          1) /* Damage */
     , (1035547,  45,          4) /* DamageType - Bludgeon */
     , (1035547,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1035547,  47,          4) /* AttackType - Slash */
     , (1035547,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1035547,  49,         20) /* WeaponTime */
     , (1035547,  51,          1) /* CombatUse - Melee */
     , (1035547,  52,          1) /* ParentLocation - RightHand */
     , (1035547,  53,        101) /* PlacementPosition - Resting */
     , (1035547,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1035547, 150,        103) /* HookPlacement - Hook */
     , (1035547, 151,          2) /* HookType - Wall */
     , (1035547, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1035547,  11, True ) /* IgnoreCollisions */
     , (1035547,  13, True ) /* Ethereal */
     , (1035547,  14, True ) /* GravityStatus */
     , (1035547,  19, True ) /* Attackable */
     , (1035547,  22, True ) /* Inscribable */
     , (1035547,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1035547,  12,       0) /* Shade */
     , (1035547,  39, 1.600000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1035547,   1, 'Doom Hammer') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1035547,   1,   33559631) /* Setup */
     , (1035547,   3,  536870932) /* SoundTable */
     , (1035547,   6,   67116700) /* PaletteBase */
     , (1035547,   7,  268437032) /* ClothingBase */
     , (1035547,   8,  100688029) /* Icon */
     , (1035547,  22,  872415275) /* PhysicsEffectTable */
     , (1035547,  52,  100689403) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:38:06.4830663-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom\n",
  "IsDone": false
}
*/
