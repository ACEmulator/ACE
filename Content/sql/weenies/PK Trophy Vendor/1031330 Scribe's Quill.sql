DELETE FROM `weenie` WHERE `class_Id` = 1031330;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1031330, 'ace1031330-scribesquill', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1031330,   1,      32768) /* ItemType - Caster */
     , (1031330,   5,         10) /* EncumbranceVal */
     , (1031330,   9,   16777216) /* ValidLocations - Held */
     , (1031330,  16,          1) /* ItemUseable - No */
     , (1031330,  19,         20) /* Value */
     , (1031330,  33,          0) /* Bonded - Normal */
     , (1031330,  46,      66048) /* DefaultCombatStyle - Magic, StubbornMagic */
     , (1031330,  53,        101) /* PlacementPosition - Resting */
     , (1031330,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1031330,  94,         16) /* TargetType - Creature */
     , (1031330, 106,        350) /* ItemSpellcraft */
     , (1031330, 107,      12000) /* ItemCurMana */
     , (1031330, 108,      12000) /* ItemMaxMana */
     , (1031330, 114,          0) /* Attuned - Normal */
     , (1031330, 115,        400) /* ItemSkillLevelLimit */
     , (1031330, 151,          2) /* HookType - Wall */
     , (1031330, 176,         18) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1031330,  11, True ) /* IgnoreCollisions */
     , (1031330,  13, True ) /* Ethereal */
     , (1031330,  14, True ) /* GravityStatus */
     , (1031330,  19, True ) /* Attackable */
     , (1031330,  22, True ) /* Inscribable */
     , (1031330,  69, True ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1031330,   5, -0.05000000074505806) /* ManaRate */
     , (1031330,  29,       1) /* WeaponDefense */
     , (1031330, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1031330,   1, 'Scribe''s Quill') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1031330,   1,   33559615) /* Setup */
     , (1031330,   3,  536870932) /* SoundTable */
     , (1031330,   8,  100687944) /* Icon */
     , (1031330,  22,  872415275) /* PhysicsEffectTable */
     , (1031330,  57,    1000002) /* AlternateCurrency */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T11:20:18.5007068-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Added default combat style to enter combat",
  "IsDone": false
}
*/
