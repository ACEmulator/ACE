DELETE FROM `weenie` WHERE `class_Id` = 1036230;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1036230, 'ace1036230-riftorb', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1036230,   1,      32768) /* ItemType - Caster */
     , (1036230,   5,         50) /* EncumbranceVal */
     , (1036230,   9,   16777216) /* ValidLocations - Held */
     , (1036230,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1036230,  19,         20) /* Value */
     , (1036230,  46,        512) /* DefaultCombatStyle - Magic */
     , (1036230,  52,          1) /* ParentLocation - RightHand */
     , (1036230,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1036230,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1036230,  94,         16) /* TargetType - Creature */
     , (1036230, 106,        425) /* ItemSpellcraft */
     , (1036230, 107,       1980) /* ItemCurMana */
     , (1036230, 108,       2000) /* ItemMaxMana */
     , (1036230, 109,          0) /* ItemDifficulty */
     , (1036230, 115,        350) /* ItemSkillLevelLimit */
     , (1036230, 151,          2) /* HookType - Wall */
     , (1036230, 176,         33) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1036230,  11, True ) /* IgnoreCollisions */
     , (1036230,  13, True ) /* Ethereal */
     , (1036230,  14, True ) /* GravityStatus */
     , (1036230,  19, True ) /* Attackable */
     , (1036230,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1036230,   5, -0.03333330154418945) /* ManaRate */
     , (1036230,  39, 0.6000000238418579) /* DefaultScale */
     , (1036230, 144, 0.0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1036230,   1, 'Rift Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1036230,   1,   33560376) /* Setup */
     , (1036230,   3,  536870932) /* SoundTable */
     , (1036230,   6,   67111919) /* PaletteBase */
     , (1036230,   8,  100689611) /* Icon */
     , (1036230,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T11:00:56.7646203-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
