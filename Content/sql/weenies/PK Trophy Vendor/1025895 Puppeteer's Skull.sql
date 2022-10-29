DELETE FROM `weenie` WHERE `class_Id` = 1025895;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1025895, 'ace1025895-puppeteersskull', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1025895,   1,      32768) /* ItemType - Caster */
     , (1025895,   5,        125) /* EncumbranceVal */
     , (1025895,   8,         50) /* Mass */
     , (1025895,   9,   16777216) /* ValidLocations - Held */
     , (1025895,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1025895,  18,          1) /* UiEffects - Magical */
     , (1025895,  19,         20) /* Value */
     , (1025895,  46,        512) /* DefaultCombatStyle - Magic */
     , (1025895,  52,          1) /* ParentLocation - RightHand */
     , (1025895,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1025895,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1025895,  94,         16) /* TargetType - Creature */
     , (1025895, 106,        400) /* ItemSpellcraft */
     , (1025895, 107,       1400) /* ItemCurMana */
     , (1025895, 108,       1400) /* ItemMaxMana */
     , (1025895, 109,        100) /* ItemDifficulty */
     , (1025895, 150,        103) /* HookPlacement - Hook */
     , (1025895, 151,          2) /* HookType - Wall */
     , (1025895, 158,          1) /* WieldRequirements - Skill */
     , (1025895, 159,         34) /* WieldSkillType - WarMagic */
     , (1025895, 160,        330) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1025895,  11, True ) /* IgnoreCollisions */
     , (1025895,  13, True ) /* Ethereal */
     , (1025895,  14, True ) /* GravityStatus */
     , (1025895,  15, True ) /* LightsStatus */
     , (1025895,  19, True ) /* Attackable */
     , (1025895,  22, True ) /* Inscribable */
     , (1025895,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1025895,   5, -0.0333000011742115) /* ManaRate */
     , (1025895,  29,       1) /* WeaponDefense */
     , (1025895,  76, 0.20000000298023224) /* Translucency */
     , (1025895, 144, 0.15000000596046448) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1025895,   1, 'Puppeteer''s Skull') /* Name */
     , (1025895,  16, 'A skull with dark energies pouring from its eyes and mouth.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1025895,   1,   33558558) /* Setup */
     , (1025895,   3,  536870932) /* SoundTable */
     , (1025895,   8,  100675627) /* Icon */
     , (1025895,  22,  872415275) /* PhysicsEffectTable */
     , (1025895,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1025895,  28,       2998) /* Spell - Wrath of the Puppeteer */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (1025895,  1478,      2)  /* Aura of Hermetic Link Self IV */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T08:26:14.431292-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
