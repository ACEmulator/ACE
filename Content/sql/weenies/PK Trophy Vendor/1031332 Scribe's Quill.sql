DELETE FROM `weenie` WHERE `class_Id` = 1031332;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1031332, 'ace1031332-scribesquill', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1031332,   1,      32768) /* ItemType - Caster */
     , (1031332,   5,         10) /* EncumbranceVal */
     , (1031332,   9,   16777216) /* ValidLocations - Held */
     , (1031332,  16,          1) /* ItemUseable - No */
     , (1031332,  19,         20) /* Value */
     , (1031332,  33,          0) /* Bonded - Normal */
     , (1031332,  46,      66048) /* DefaultCombatStyle - Magic, StubbornMagic */
     , (1031332,  53,        101) /* PlacementPosition - Resting */
     , (1031332,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1031332,  94,         16) /* TargetType - Creature */
     , (1031332, 106,        350) /* ItemSpellcraft */
     , (1031332, 107,      12000) /* ItemCurMana */
     , (1031332, 108,      12000) /* ItemMaxMana */
     , (1031332, 114,          0) /* Attuned - Normal */
     , (1031332, 115,        400) /* ItemSkillLevelLimit */
     , (1031332, 151,          2) /* HookType - Wall */
     , (1031332, 176,         18) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1031332,  11, True ) /* IgnoreCollisions */
     , (1031332,  13, True ) /* Ethereal */
     , (1031332,  14, True ) /* GravityStatus */
     , (1031332,  19, True ) /* Attackable */
     , (1031332,  22, True ) /* Inscribable */
     , (1031332,  69, True ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1031332,   5, -0.05000000074505806) /* ManaRate */
     , (1031332,  29,       1) /* WeaponDefense */
     , (1031332, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1031332,   1, 'Scribe''s Quill') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1031332,   1,   33559614) /* Setup */
     , (1031332,   3,  536870932) /* SoundTable */
     , (1031332,   8,  100687943) /* Icon */
     , (1031332,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T11:03:05.9664069-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added default combat style to enter combat",
  "IsDone": true
}
*/
