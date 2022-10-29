DELETE FROM `weenie` WHERE `class_Id` = 1052699;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1052699, 'ace1052699-woodentop', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1052699,   1,      32768) /* ItemType - Caster */
     , (1052699,   5,         50) /* EncumbranceVal */
     , (1052699,   9,   16777216) /* ValidLocations - Held */
     , (1052699,  16,          1) /* ItemUseable - No */
     , (1052699,  19,         20) /* Value */
     , (1052699,  46,        512) /* DefaultCombatStyle - Magic */
     , (1052699,  52,          1) /* ParentLocation - RightHand */
     , (1052699,  93,      66580) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, HasPhysicsBSP */
     , (1052699,  94,         16) /* TargetType - Creature */
     , (1052699, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1052699,  11, True ) /* IgnoreCollisions */
     , (1052699,  13, True ) /* Ethereal */
     , (1052699,  14, True ) /* GravityStatus */
     , (1052699,  19, True ) /* Attackable */
     , (1052699,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1052699,  29,       1) /* WeaponDefense */
     , (1052699, 144,       0) /* ManaConversionMod */
     , (1052699, 152, 1.0800000429153442) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1052699,   1, 'Wooden Top') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1052699,   1,   33561643) /* Setup */
     , (1052699,   3,  536870932) /* SoundTable */
     , (1052699,   8,  100693302) /* Icon */
     , (1052699,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-07T17:34:04.4163016-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom for tailor vendor",
  "IsDone": false
}
*/
