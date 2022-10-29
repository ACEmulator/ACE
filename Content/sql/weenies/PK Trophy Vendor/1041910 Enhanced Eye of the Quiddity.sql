DELETE FROM `weenie` WHERE `class_Id` = 1041910;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1041910, 'ace1041910-enhancedeyeofthequiddity', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1041910,   1,      32768) /* ItemType - Caster */
     , (1041910,   5,         50) /* EncumbranceVal */
     , (1041910,   9,   16777216) /* ValidLocations - Held */
     , (1041910,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (1041910,  18,          1) /* UiEffects - Magical */
     , (1041910,  19,         20) /* Value */
     , (1041910,  53,        101) /* PlacementPosition - Resting */
     , (1041910,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1041910,  94,         16) /* TargetType - Creature */
     , (1041910, 106,        400) /* ItemSpellcraft */
     , (1041910, 107,       1000) /* ItemCurMana */
     , (1041910, 108,       1000) /* ItemMaxMana */
     , (1041910, 109,         50) /* ItemDifficulty */
     , (1041910, 151,          6) /* HookType - Wall, Ceiling */
     , (1041910, 158,          2) /* WieldRequirements - RawSkill */
     , (1041910, 159,         34) /* WieldSkillType - WarMagic */
     , (1041910, 160,        330) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1041910,  11, True ) /* IgnoreCollisions */
     , (1041910,  13, True ) /* Ethereal */
     , (1041910,  14, True ) /* GravityStatus */
     , (1041910,  15, True ) /* LightsStatus */
     , (1041910,  19, True ) /* Attackable */
     , (1041910,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1041910,   5, -0.02500000037252903) /* ManaRate */
     , (1041910,  39, 0.800000011920929) /* DefaultScale */
     , (1041910, 144, 0.10000000149011612) /* ManaConversionMod */
     , (1041910, 152, 1.0800000429153442) /* ElementalDamageMod */
     , (1041910, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1041910,   1, 'Enhanced Eye of the Quiddity') /* Name */
     , (1041910,  16, 'An orb with a large purple eye in the middle.  Gazing at it makes you dizzy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1041910,   1,   33557115) /* Setup */
     , (1041910,   3,  536870932) /* SoundTable */
     , (1041910,   8,  100671692) /* Icon */
     , (1041910,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (1041910,   609,      2)  /* Life Magic Mastery Self V */
     , (1041910,  2249,      2)  /* Celcynd's Blessing */
     , (1041910,  2287,      2)  /* Nuhmudira's Blessing */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:37:17.9937358-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
