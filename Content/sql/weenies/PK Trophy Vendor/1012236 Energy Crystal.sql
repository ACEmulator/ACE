DELETE FROM `weenie` WHERE `class_Id` = 1012236;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1012236, 'ace1012236-energycrystal', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1012236,   1,      32768) /* ItemType - Caster */
     , (1012236,   5,        300) /* EncumbranceVal */
     , (1012236,   8,         50) /* Mass */
     , (1012236,   9,   16777216) /* ValidLocations - Held */
     , (1012236,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (1012236,  18,          1) /* UiEffects - Magical */
     , (1012236,  19,         20) /* Value */
     , (1012236,  33,          1) /* Bonded - Bonded */
     , (1012236,  46,        512) /* DefaultCombatStyle - Magic */
     , (1012236,  52,          1) /* ParentLocation - RightHand */
     , (1012236,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1012236,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1012236,  94,         16) /* TargetType - Creature */
     , (1012236, 106,        170) /* ItemSpellcraft */
     , (1012236, 107,       1000) /* ItemCurMana */
     , (1012236, 108,       1200) /* ItemMaxMana */
     , (1012236, 109,        180) /* ItemDifficulty */
     , (1012236, 150,        104) /* HookPlacement - XXXUnknown68 */
     , (1012236, 151,         11) /* HookType - Floor, Wall, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1012236,  11, True ) /* IgnoreCollisions */
     , (1012236,  13, True ) /* Ethereal */
     , (1012236,  14, True ) /* GravityStatus */
     , (1012236,  15, True ) /* LightsStatus */
     , (1012236,  19, True ) /* Attackable */
     , (1012236,  22, True ) /* Inscribable */
     , (1012236,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1012236,   5, -0.05000000074505806) /* ManaRate */
     , (1012236,  29,       1) /* WeaponDefense */
     , (1012236,  76,     0.5) /* Translucency */
     , (1012236, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1012236,   1, 'Energy Crystal') /* Name */
     , (1012236,  15, 'A strange, purple crystal.') /* ShortDesc */
     , (1012236,  16, 'A strange, purple crystal.  It has an odd aura around it, and you can see strange flickering shapes within.') /* LongDesc */
     , (1012236,  33, 'HouseDeedUltra') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1012236,   1,   33557374) /* Setup */
     , (1012236,   3,  536870932) /* SoundTable */
     , (1012236,   8,  100672184) /* Icon */
     , (1012236,  22,  872415275) /* PhysicsEffectTable */
     , (1012236,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1012236,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:08:37.6330818-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
