DELETE FROM `weenie` WHERE `class_Id` = 1030863;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030863, 'ace1030863-banishedspear', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030863,   1,          1) /* ItemType - MeleeWeapon */
     , (1030863,   5,          1) /* EncumbranceVal */
     , (1030863,   8,        340) /* Mass */
     , (1030863,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030863,  16,          1) /* ItemUseable - No */
     , (1030863,  19,         20) /* Value */
     , (1030863,  44,          1) /* Damage */
     , (1030863,  45,         64) /* DamageType - Electric */
     , (1030863,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030863,  47,          2) /* AttackType - Thrust */
     , (1030863,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1030863,  49,         30) /* WeaponTime */
     , (1030863,  51,          1) /* CombatUse - Melee */
     , (1030863,  52,          1) /* ParentLocation - RightHand */
     , (1030863,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1030863,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030863, 150,        103) /* HookPlacement - Hook */
     , (1030863, 151,          2) /* HookType - Wall */
     , (1030863, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030863,  11, True ) /* IgnoreCollisions */
     , (1030863,  13, True ) /* Ethereal */
     , (1030863,  14, True ) /* GravityStatus */
     , (1030863,  19, True ) /* Attackable */
     , (1030863,  22, True ) /* Inscribable */
     , (1030863,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030863,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030863,   1, 'Banished Spear') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030863,   1,   33559259) /* Setup */
     , (1030863,   3,  536870932) /* SoundTable */
     , (1030863,   8,  100677487) /* Icon */
     , (1030863,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:42:41.3686007-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
