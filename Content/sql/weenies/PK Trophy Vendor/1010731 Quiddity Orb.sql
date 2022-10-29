DELETE FROM `weenie` WHERE `class_Id` = 1010731;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1010731, 'ace1010731-quiddityorb', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1010731,   1,      32768) /* ItemType - Caster */
     , (1010731,   3,         13) /* PaletteTemplate - Purple */
     , (1010731,   5,         50) /* EncumbranceVal */
     , (1010731,   8,         50) /* Mass */
     , (1010731,   9,   16777216) /* ValidLocations - Held */
     , (1010731,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (1010731,  18,          1) /* UiEffects - Magical */
     , (1010731,  19,         20) /* Value */
     , (1010731,  46,        512) /* DefaultCombatStyle - Magic */
     , (1010731,  52,          1) /* ParentLocation - RightHand */
     , (1010731,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1010731,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1010731,  94,         16) /* TargetType - Creature */
     , (1010731, 106,        250) /* ItemSpellcraft */
     , (1010731, 107,       1000) /* ItemCurMana */
     , (1010731, 108,       1000) /* ItemMaxMana */
     , (1010731, 109,        200) /* ItemDifficulty */
     , (1010731, 114,          1) /* Attuned - Attuned */
     , (1010731, 115,        200) /* ItemSkillLevelLimit */
     , (1010731, 150,        103) /* HookPlacement - Hook */
     , (1010731, 151,          2) /* HookType - Wall */
     , (1010731, 176,         16) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1010731,  11, True ) /* IgnoreCollisions */
     , (1010731,  13, True ) /* Ethereal */
     , (1010731,  14, True ) /* GravityStatus */
     , (1010731,  15, True ) /* LightsStatus */
     , (1010731,  19, True ) /* Attackable */
     , (1010731,  22, True ) /* Inscribable */
     , (1010731,  23, True ) /* DestroyOnSell */
     , (1010731,  69, False) /* IsSellable */
     , (1010731,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1010731,   5, -0.05000000074505806) /* ManaRate */
     , (1010731,  12,     0.5) /* Shade */
     , (1010731,  29,       1) /* WeaponDefense */
     , (1010731,  76,     0.5) /* Translucency */
     , (1010731, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1010731,   1, 'Quiddity Orb') /* Name */
     , (1010731,  15, 'A spellcasting orb empowered with an otherworldly energy.') /* ShortDesc */
     , (1010731,  16, 'A spellcasting orb pulsing with the mickle energies of the Virindi.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1010731,   1,   33557050) /* Setup */
     , (1010731,   3,  536870932) /* SoundTable */
     , (1010731,   6,   67111928) /* PaletteBase */
     , (1010731,   7,  268436041) /* ClothingBase */
     , (1010731,   8,  100671667) /* Icon */
     , (1010731,  22,  872415275) /* PhysicsEffectTable */
     , (1010731,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1010731,  36,  234881046) /* MutateFilter */
     , (1010731,  37,         16) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T02:10:39.3883976-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
