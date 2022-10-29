DELETE FROM `weenie` WHERE `class_Id` = 41910;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (41910, 'ace41910-enhancedeyeofthequiddity', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (41910,   1,      32768) /* ItemType - Caster */
     , (41910,   5,         50) /* EncumbranceVal */
     , (41910,   9,   16777216) /* ValidLocations - Held */
     , (41910,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (41910,  18,          1) /* UiEffects - Magical */
     , (41910,  19,         20) /* Value */
     , (41910,  53,        101) /* PlacementPosition - Resting */
     , (41910,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (41910,  94,         16) /* TargetType - Creature */
     , (41910, 106,        400) /* ItemSpellcraft */
     , (41910, 107,       1000) /* ItemCurMana */
     , (41910, 108,       1000) /* ItemMaxMana */
     , (41910, 109,         50) /* ItemDifficulty */
     , (41910, 151,          6) /* HookType - Wall, Ceiling */
     , (41910, 158,          2) /* WieldRequirements - RawSkill */
     , (41910, 159,         34) /* WieldSkillType - WarMagic */
     , (41910, 160,        330) /* WieldDifficulty */
     , (41910, 263,          1) /* ResistanceModifierType */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (41910,  11, True ) /* IgnoreCollisions */
     , (41910,  13, True ) /* Ethereal */
     , (41910,  14, True ) /* GravityStatus */
     , (41910,  15, True ) /* LightsStatus */
     , (41910,  19, True ) /* Attackable */
     , (41910,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (41910,   5, -0.02500000037252903) /* ManaRate */
     , (41910,  29, 1.1200000047683716) /* WeaponDefense */
     , (41910,  39, 0.800000011920929) /* DefaultScale */
     , (41910, 144, 0.10000000149011612) /* ManaConversionMod */
     , (41910, 152, 1.0800000429153442) /* ElementalDamageMod */
     , (41910, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (41910,   1, 'Enhanced Eye of the Quiddity') /* Name */
     , (41910,  16, 'An orb with a large purple eye in the middle.  Gazing at it makes you dizzy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (41910,   1,   33557115) /* Setup */
     , (41910,   3,  536870932) /* SoundTable */
     , (41910,   8,  100671692) /* Icon */
     , (41910,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (41910,   609,      2)  /* Life Magic Mastery Self V */
     , (41910,  2249,      2)  /* Celcynd's Blessing */
     , (41910,  2287,      2)  /* Nuhmudira's Blessing */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-24T14:48:26.2809753-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
