DELETE FROM `weenie` WHERE `class_Id` = 1031331;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1031331, 'ace1031331-scribesquill', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1031331,   1,      32768) /* ItemType - Caster */
     , (1031331,   5,         10) /* EncumbranceVal */
     , (1031331,   9,   16777216) /* ValidLocations - Held */
     , (1031331,  16,          1) /* ItemUseable - No */
     , (1031331,  19,         20) /* Value */
     , (1031331,  33,          0) /* Bonded - Normal */
     , (1031331,  46,      66048) /* DefaultCombatStyle - Magic, StubbornMagic */
     , (1031331,  53,        101) /* PlacementPosition - Resting */
     , (1031331,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1031331,  94,         16) /* TargetType - Creature */
     , (1031331, 106,        350) /* ItemSpellcraft */
     , (1031331, 107,      12000) /* ItemCurMana */
     , (1031331, 108,      12000) /* ItemMaxMana */
     , (1031331, 114,          0) /* Attuned - Normal */
     , (1031331, 115,        400) /* ItemSkillLevelLimit */
     , (1031331, 151,          2) /* HookType - Wall */
     , (1031331, 176,         28) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1031331,  11, True ) /* IgnoreCollisions */
     , (1031331,  13, True ) /* Ethereal */
     , (1031331,  14, True ) /* GravityStatus */
     , (1031331,  19, True ) /* Attackable */
     , (1031331,  22, True ) /* Inscribable */
     , (1031331,  69, True ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1031331,   5, -0.05000000074505806) /* ManaRate */
     , (1031331,  29,       1) /* WeaponDefense */
     , (1031331, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1031331,   1, 'Scribe''s Quill') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1031331,   1,   33559612) /* Setup */
     , (1031331,   3,  536870932) /* SoundTable */
     , (1031331,   8,  100687946) /* Icon */
     , (1031331,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T11:02:48.8039751-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added default combat style to enter combat\n",
  "IsDone": true
}
*/
