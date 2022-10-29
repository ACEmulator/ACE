DELETE FROM `weenie` WHERE `class_Id` = 1036229;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1036229, 'ace1036229-riftorb', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1036229,   1,      32768) /* ItemType - Caster */
     , (1036229,   5,         50) /* EncumbranceVal */
     , (1036229,   9,   16777216) /* ValidLocations - Held */
     , (1036229,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1036229,  19,         20) /* Value */
     , (1036229,  46,        512) /* DefaultCombatStyle - Magic */
     , (1036229,  53,        101) /* PlacementPosition - Resting */
     , (1036229,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1036229,  94,         16) /* TargetType - Creature */
     , (1036229, 106,        425) /* ItemSpellcraft */
     , (1036229, 107,       1918) /* ItemCurMana */
     , (1036229, 108,       2000) /* ItemMaxMana */
     , (1036229, 109,          0) /* ItemDifficulty */
     , (1036229, 115,        350) /* ItemSkillLevelLimit */
     , (1036229, 151,          2) /* HookType - Wall */
     , (1036229, 176,         32) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1036229,  11, True ) /* IgnoreCollisions */
     , (1036229,  13, True ) /* Ethereal */
     , (1036229,  14, True ) /* GravityStatus */
     , (1036229,  19, True ) /* Attackable */
     , (1036229,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1036229,   5, -0.03333330154418945) /* ManaRate */
     , (1036229,  39, 0.6000000238418579) /* DefaultScale */
     , (1036229, 144, 0.0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1036229,   1, 'Rift Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1036229,   1,   33560377) /* Setup */
     , (1036229,   3,  536870932) /* SoundTable */
     , (1036229,   8,  100689612) /* Icon */
     , (1036229,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T11:00:44.1750314-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
