DELETE FROM `weenie` WHERE `class_Id` = 1011305;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1011305, 'ace1011305-staveofpalenqual', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1011305,   1,      32768) /* ItemType - Caster */
     , (1011305,   5,        200) /* EncumbranceVal */
     , (1011305,   8,        200) /* Mass */
     , (1011305,   9,   16777216) /* ValidLocations - Held */
     , (1011305,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1011305,  18,          1) /* UiEffects - Magical */
     , (1011305,  19,         20) /* Value */
     , (1011305,  33,          1) /* Bonded - Bonded */
     , (1011305,  46,        512) /* DefaultCombatStyle - Magic */
     , (1011305,  52,          1) /* ParentLocation - RightHand */
     , (1011305,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1011305,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1011305,  94,         16) /* TargetType - Creature */
     , (1011305, 106,        250) /* ItemSpellcraft */
     , (1011305, 107,       8544) /* ItemCurMana */
     , (1011305, 108,       8544) /* ItemMaxMana */
     , (1011305, 114,          1) /* Attuned - Attuned */
     , (1011305, 117,        600) /* ItemManaCost */
     , (1011305, 150,        103) /* HookPlacement - Hook */
     , (1011305, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1011305,  11, True ) /* IgnoreCollisions */
     , (1011305,  13, True ) /* Ethereal */
     , (1011305,  14, True ) /* GravityStatus */
     , (1011305,  19, True ) /* Attackable */
     , (1011305,  22, True ) /* Inscribable */
     , (1011305,  23, True ) /* DestroyOnSell */
     , (1011305,  69, False) /* IsSellable */
     , (1011305,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1011305,   5, -0.05000000074505806) /* ManaRate */
     , (1011305,  29,       1) /* WeaponDefense */
     , (1011305, 144, 0.07000000029802322) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1011305,   1, 'Stave of Palenqual') /* Name */
     , (1011305,  16, 'The Stave of Palenqual, an Aun Tumerok relic. This magic caster was given to the Aun xuta by spirits called the Deru. It is the embodiment of Marae Lassel''s spirit - a single great totem for the island as a whole. Three Tumerok fetishes are attached to this weapon; those of Siraluun, Storm, and Tonk.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1011305,   1,   33557232) /* Setup */
     , (1011305,   3,  536870932) /* SoundTable */
     , (1011305,   6,   67111919) /* PaletteBase */
     , (1011305,   8,  100671868) /* Icon */
     , (1011305,  22,  872415275) /* PhysicsEffectTable */
     , (1011305,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1011305,  28,       1836) /* Spell - Avalanche */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:08:01.2366415-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
