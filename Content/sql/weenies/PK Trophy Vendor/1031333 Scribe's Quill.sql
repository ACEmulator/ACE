DELETE FROM `weenie` WHERE `class_Id` = 1031333;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1031333, 'ace1031333-scribesquill', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1031333,   1,      32768) /* ItemType - Caster */
     , (1031333,   5,         10) /* EncumbranceVal */
     , (1031333,   9,   16777216) /* ValidLocations - Held */
     , (1031333,  16,          1) /* ItemUseable - No */
     , (1031333,  19,         20) /* Value */
     , (1031333,  33,          0) /* Bonded - Normal */
     , (1031333,  46,      66048) /* DefaultCombatStyle - Magic, StubbornMagic */
     , (1031333,  53,        101) /* PlacementPosition - Resting */
     , (1031333,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1031333,  94,         16) /* TargetType - Creature */
     , (1031333, 106,        350) /* ItemSpellcraft */
     , (1031333, 107,      12000) /* ItemCurMana */
     , (1031333, 108,      12000) /* ItemMaxMana */
     , (1031333, 114,          0) /* Attuned - Normal */
     , (1031333, 115,        400) /* ItemSkillLevelLimit */
     , (1031333, 151,          2) /* HookType - Wall */
     , (1031333, 176,         29) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1031333,  11, True ) /* IgnoreCollisions */
     , (1031333,  13, True ) /* Ethereal */
     , (1031333,  14, True ) /* GravityStatus */
     , (1031333,  19, True ) /* Attackable */
     , (1031333,  22, True ) /* Inscribable */
     , (1031333,  69, True ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1031333,   5, -0.05000000074505806) /* ManaRate */
     , (1031333,  29,       1) /* WeaponDefense */
     , (1031333, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1031333,   1, 'Scribe''s Quill') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1031333,   1,   33559616) /* Setup */
     , (1031333,   3,  536870932) /* SoundTable */
     , (1031333,   8,  100687945) /* Icon */
     , (1031333,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T11:03:16.6677861-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added default combat style to enter combat",
  "IsDone": true
}
*/
